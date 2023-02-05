using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;

using ILGPU.Runtime;
using ILGPU;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.OpenCL;
using System.IO;
using System.Diagnostics;

namespace ResearchProject.Models
{
    public class Field
    {
        public readonly Cell[,] Cells;
        public readonly float CellSize;

        public readonly int ColumnsNumber;
        public readonly int RowsNumber;

        private readonly Random _randomNumberGenerator = new Random();

        public Field(int width, int height, float cellSize, bool wrap = true)
        {
            CellSize = cellSize;

            ColumnsNumber = width / (int)cellSize;
            RowsNumber = height / (int)cellSize;

            Cells = new Cell[ColumnsNumber, RowsNumber];

            for (int columnNumber = 0; columnNumber < ColumnsNumber; columnNumber++)
                for (int rowNumber = 0; rowNumber < RowsNumber; rowNumber++)
                    Cells[columnNumber, rowNumber] = new Cell();

            ConnectCellsWithNeighbours(wrap);
        }

        private void ConnectCellsWithNeighbours(bool wrap)
        {
            for (int columnNumber = 0; columnNumber < ColumnsNumber; columnNumber++)
            {
                for (int rowNumber = 0; rowNumber < RowsNumber; rowNumber++)
                {
                    bool isLeftEdge = (columnNumber == 0),
                         isRightEdge = (columnNumber == ColumnsNumber - 1),
                         isTopEdge = (rowNumber == 0),
                         isBottomEdge = (rowNumber == RowsNumber - 1);
                     
                    var isEdge = isLeftEdge | isRightEdge | isTopEdge | isBottomEdge;

                    // We skip, because cells near field edges are always dead (if wrap is false)
                    if (wrap == false && isEdge)
                        continue;

                    int leftNeighbourColumnNumber = isLeftEdge ? ColumnsNumber - 1 : columnNumber - 1,
                        rightNeighbourColumnNumber = isRightEdge ? 0 : columnNumber + 1,
                        topNeighbourRowNumber = isTopEdge ? RowsNumber - 1 : rowNumber - 1,
                        bottomNeighbourRowNumber = isBottomEdge ? 0 : rowNumber + 1;

                    // Left neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[leftNeighbourColumnNumber, rowNumber]);
                    
                    // Right neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[rightNeighbourColumnNumber, rowNumber]);
                    
                    // Top neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[columnNumber, topNeighbourRowNumber]);
                    
                    // Bottom neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[columnNumber, bottomNeighbourRowNumber]);
                    
                    // Left-top neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[leftNeighbourColumnNumber, topNeighbourRowNumber]);

                    // Right-top neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[rightNeighbourColumnNumber, topNeighbourRowNumber]);

                    // Left-bottom neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[leftNeighbourColumnNumber, bottomNeighbourRowNumber]);

                    // Right-bottom neighbour
                    Cells[columnNumber, rowNumber].Neighbours.Add(Cells[rightNeighbourColumnNumber, bottomNeighbourRowNumber]);
                }
            }
        }

        public void PopulateFieldRandomly(double liveDensity)
        {
            foreach (var cell in Cells)
                cell.IsAlive = _randomNumberGenerator.NextDouble() < liveDensity;

            ConnectCellsWithNeighbours(true);

            var cellsArray = Cells.Cast<Cell>().ToList();

            cellStructArray = new CellStruct[cellsArray.Count];

            for (int i = 0; i < cellsArray.Count; i++)
            {
                var currentCell = cellsArray[i];
                Cell n1 = currentCell.Neighbours[0],
                    n2 = currentCell.Neighbours[1],
                    n3 = currentCell.Neighbours[2],
                    n4 = currentCell.Neighbours[3],
                    n5 = currentCell.Neighbours[4],
                    n6 = currentCell.Neighbours[5],
                    n7 = currentCell.Neighbours[6],
                    n8 = currentCell.Neighbours[7];

                cellStructArray[i] = new CellStruct
                {
                    IsAlive = (byte)(currentCell.IsAlive ? 1 : 0),
                    IsAliveNext = (byte)(currentCell.IsAliveNext ? 1 : 0),
                    Neighbour1Index = cellsArray.IndexOf(n1),
                    Neighbour2Index = cellsArray.IndexOf(n2),
                    Neighbour3Index = cellsArray.IndexOf(n3),
                    Neighbour4Index = cellsArray.IndexOf(n4),
                    Neighbour5Index = cellsArray.IndexOf(n5),
                    Neighbour6Index = cellsArray.IndexOf(n6),
                    Neighbour7Index = cellsArray.IndexOf(n7),
                    Neighbour8Index = cellsArray.IndexOf(n8),
                };
            }

            // Initialize ILGPU.
            Context context = Context.CreateDefault();
            Accelerator accelerator = context.GetPreferredDevice(preferCPU: false)
                                      .CreateAccelerator(context);

            // Load the data.
            MemoryBuffer1D<CellStruct, Stride1D.Dense> deviceData = accelerator.Allocate1D(cellStructArray);

            // load / precompile the kernel
            Action<Index1D, ArrayView<CellStruct>> loadedKernel =
                accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<CellStruct>>(SomeKernel);

            // finish compiling and tell the accelerator to start computing the kernel
            loadedKernel((int)deviceData.Length, deviceData.View);

            // wait for the accelerator to be finished with whatever it's doing
            // in this case it just waits for the kernel to finish.
            accelerator.Synchronize();

            // moved output data from the GPU to the CPU for output to console
            //cellStructArray = deviceData.GetAsArray1D();
            deviceData.CopyToCPU(cellStructArray);

            accelerator.Dispose();
            context.Dispose();
        }

        static void SomeKernel(Index1D i, ArrayView<CellStruct> cs)
        {
            int aliveNeighboursNumber = 0;
            
            var currentCell = cs[i];

            if (cs[(int)currentCell.Neighbour1Index].IsAlive == 1) aliveNeighboursNumber++;
            if (cs[(int)currentCell.Neighbour2Index].IsAlive == 1) aliveNeighboursNumber++;
            if (cs[(int)currentCell.Neighbour3Index].IsAlive == 1) aliveNeighboursNumber++;
            if (cs[(int)currentCell.Neighbour4Index].IsAlive == 1) aliveNeighboursNumber++;
            if (cs[(int)currentCell.Neighbour5Index].IsAlive == 1) aliveNeighboursNumber++;
            if (cs[(int)currentCell.Neighbour6Index].IsAlive == 1) aliveNeighboursNumber++;
            if (cs[(int)currentCell.Neighbour7Index].IsAlive == 1) aliveNeighboursNumber++;
            if (cs[(int)currentCell.Neighbour8Index].IsAlive == 1) aliveNeighboursNumber++;

            if (currentCell.IsAlive == 1)
                currentCell.IsAliveNext = (byte)(aliveNeighboursNumber == 2 || aliveNeighboursNumber == 3 ? 1 : 0);
            else
                currentCell.IsAliveNext = (byte)(aliveNeighboursNumber == 3 ? 1 : 0);
        }

        public void Advance()
        {
            sw.Restart();

            // Initialize ILGPU.
            Context context = Context.CreateDefault();
            Accelerator accelerator = context.GetPreferredDevice(preferCPU: false)
                                      .CreateAccelerator(context);

            // Load the data.
            MemoryBuffer1D<CellStruct, Stride1D.Dense> deviceData = accelerator.Allocate1D(cellStructArray);

            // load / precompile the kernel
            Action<Index1D, ArrayView<CellStruct>> loadedKernel =
                accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<CellStruct>>(SomeKernel);

            // finish compiling and tell the accelerator to start computing the kernel
            loadedKernel((int)deviceData.Length, deviceData.View);

            // wait for the accelerator to be finished with whatever it's doing
            // in this case it just waits for the kernel to finish.
            accelerator.Synchronize();

            // moved output data from the GPU to the CPU for output to console
            //cellStructArray = deviceData.GetAsArray1D();
            deviceData.CopyToCPU(cellStructArray);

            accelerator.Dispose();
            context.Dispose();

            sw.Stop();
            File.AppendAllText(filePath, $"determining: {sw.Elapsed.TotalSeconds} seconds\n");
            //sw.Restart();
            //foreach (var c in Cells) c.Advance();
            //sw.Stop();
            //File.AppendAllText(filePath, $"advancing: {sw.Elapsed.TotalSeconds} seconds\n\n");
        }
        
        private struct CellStruct
        {
            public byte IsAlive;
            public byte IsAliveNext;

            public int Neighbour1Index, Neighbour2Index, Neighbour3Index, Neighbour4Index, Neighbour5Index, Neighbour6Index, Neighbour7Index, Neighbour8Index;
        }

        private CellStruct[] cellStructArray;

        string filePath = @"C:\Users\Ильназ\Desktop\Текстовый документ.txt";
        
        Stopwatch sw = new();
    }
}