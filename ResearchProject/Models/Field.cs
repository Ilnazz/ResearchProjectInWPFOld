using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public void Advance()
        {
            foreach (var cell in Cells)
                cell.DetermineNextLiveState();

            foreach (var cell in Cells)
                cell.Advance();
        }
    }
}
