using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace ResearchProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum CellState
        {
            Dead, Alive
        }

        #region Proeprties

        #region Constants

        private const float CELL_SIZE = 4f; // width and height of one cell

        #endregion

        private int FieldColumnsNumber; // cells number by X axis
        private int FieldRowsNumber; // cells number by Y axis

        private CellState[,] PreviousGeneration;
        private CellState[,] CurrentGeneration;

        #region Paints

        private readonly SKPaint PaintWhite = new() { Color = SKColor.Parse("#ffffff") };
        private readonly SKPaint PaintBlack = new() { Color = SKColor.Parse("#000000") };

        #endregion

        public int CurrentGenerationNumber = 0;

        public int EvolutionsPerSecond = 10;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            InitializeField();
            
            InitializeTimer();

            RenderField();
        }

        #region Field

        private void InitializeField()
        {
            FieldColumnsNumber = (int)(Screen.PrimaryScreen.Bounds.Width / CELL_SIZE);
            FieldRowsNumber = (int)(Screen.PrimaryScreen.Bounds.Height / CELL_SIZE);

            PreviousGeneration = new CellState[FieldRowsNumber, FieldColumnsNumber];
            CurrentGeneration = new CellState[FieldRowsNumber, FieldColumnsNumber];


            PopulateFieldRandomly(0.125);
        }
        
        private void RenderField() => SKGLControl.Invalidate();
        
        public void FieldRenderer(object sender, SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(SKColor.Parse("#ffffff"));

            for (int rowNumber = 0; rowNumber < FieldRowsNumber; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < FieldColumnsNumber; columnNumber++)
                {
                    var paint = CurrentGeneration[rowNumber, columnNumber] == CellState.Dead ? PaintWhite : PaintBlack;

                    e.Surface.Canvas.DrawCircle(columnNumber * CELL_SIZE, rowNumber * CELL_SIZE, CELL_SIZE/2, paint);
                }
            }

            SKGLControl.SwapBuffers();
        }

        private void PopulateFieldRandomly(double density)
        {
            var rand = new Random();

            for (int rowNumber = 0; rowNumber < FieldRowsNumber; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < FieldColumnsNumber; columnNumber++)
                {
                    CurrentGeneration[rowNumber, columnNumber]
                        = density >= rand.NextDouble() ? CellState.Alive : CellState.Dead;
                }
            }
        }

        #endregion

        #region Evolution

        private void EvoluteCurrentGeneration()
        {
            // remembering cell states of previous generation
            for (int rowNumber = 0; rowNumber < FieldRowsNumber; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < FieldColumnsNumber; columnNumber++)
                {
                    PreviousGeneration[rowNumber, columnNumber] = CurrentGeneration[rowNumber, columnNumber];
                }
            }

            // calculating cell states
            for (int rowNumber = 0; rowNumber < FieldRowsNumber; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < FieldColumnsNumber; columnNumber++)
                {
                    var aliveNeighbours = CountAliveNeighboursOfCell(rowNumber, columnNumber);

                    if (aliveNeighbours == 3)
                        CurrentGeneration[rowNumber, columnNumber] = CellState.Alive;

                    else if (PreviousGeneration[rowNumber, columnNumber] == CellState.Alive && (aliveNeighbours < 2 || aliveNeighbours > 3))
                        CurrentGeneration[rowNumber, columnNumber] = CellState.Dead;
                }
            }

            CurrentGenerationNumber++;
        }

        private int CountAliveNeighboursOfCell(int cellRowNumber, int cellColumnNumber)
        {
            var aliveNeighboursNumber = 0;

            // left neighbour
            if (cellColumnNumber > 0 && PreviousGeneration[cellRowNumber, cellColumnNumber - 1] == CellState.Alive)
                aliveNeighboursNumber++;

            // right neighbour
            if (cellColumnNumber < FieldColumnsNumber - 1 && PreviousGeneration[cellRowNumber, cellColumnNumber + 1] == CellState.Alive)
                aliveNeighboursNumber++;

            // top neighbour
            if (cellRowNumber > 0 && PreviousGeneration[cellRowNumber - 1, cellColumnNumber] == CellState.Alive)
                aliveNeighboursNumber++;

            // bottom neighbour
            if (cellRowNumber < FieldRowsNumber - 1 && PreviousGeneration[cellRowNumber + 1, cellColumnNumber] == CellState.Alive)
                aliveNeighboursNumber++;

            // top left neighbour
            if (cellRowNumber > 0 && cellColumnNumber > 0 && PreviousGeneration[cellRowNumber - 1, cellColumnNumber - 1] == CellState.Alive)
                aliveNeighboursNumber++;

            // top right neighbour
            if (cellRowNumber > 0 && cellColumnNumber < FieldColumnsNumber - 1 && PreviousGeneration[cellRowNumber - 1, cellColumnNumber + 1] == CellState.Alive)
                aliveNeighboursNumber++;

            // bottom left neighbour
            if (cellRowNumber < FieldRowsNumber - 1 && cellColumnNumber > 0 && PreviousGeneration[cellRowNumber + 1, cellColumnNumber - 1] == CellState.Alive)
                aliveNeighboursNumber++;

            // bottom right neighbour
            if (cellRowNumber < FieldRowsNumber - 1 && cellColumnNumber < FieldColumnsNumber - 1 && PreviousGeneration[cellRowNumber + 1, cellColumnNumber + 1] == CellState.Alive)
                aliveNeighboursNumber++;

            return aliveNeighboursNumber;
        }

        #endregion

        #region Timer

        private DispatcherTimer Timer;

        private void InitializeTimer()
        {
            Timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1000 / EvolutionsPerSecond)
            };

            Timer.Tick += delegate { TimerTickCallback(); };
        }

        private void TimerTickCallback()
        {
            EvoluteCurrentGeneration();

            RenderField();
        }

        private void UpdateTimerTicksPerSecond(int ticksPerSecond)
        {
            var timerWasEnabled = Timer.IsEnabled;

            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / EvolutionsPerSecond);

            if (timerWasEnabled) Timer.Start();
        }

        private void IncreaseEvolutionSpeed() => UpdateTimerTicksPerSecond(++EvolutionsPerSecond);

        private void DecreaseEvolutionSpeed() { if (EvolutionsPerSecond > 1) EvolutionsPerSecond--; }

        #endregion

        #region Simple/basic keyboard control panel

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bool isLeftButtonPressed = e.LeftButton == MouseButtonState.Pressed,
                 isRightButtonPressed = e.RightButton == MouseButtonState.Pressed;

            if (isLeftButtonPressed == false && isRightButtonPressed == false)
                return;

            int mouseX = (int)e.GetPosition(this).X,
                mouseY = (int)e.GetPosition(this).Y;

            int cellColumnNumber = mouseX / (int)CELL_SIZE,
                cellRowNumber = mouseY / (int)CELL_SIZE;

            CellState newCellState;
            if (isLeftButtonPressed)
                newCellState = CellState.Alive;
            else
                newCellState = CellState.Dead;

            CurrentGeneration[cellRowNumber, cellColumnNumber] = newCellState;

            if (Timer.IsEnabled == false)
                RenderField();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) StartEvolution();
            else if (e.Key == Key.Space) StopEvolution();
            else if (e.Key == Key.Back) StopEvolutionAndResetPopulation();
            else if (e.Key == Key.Up) IncreaseEvolutionSpeed();
            else if (e.Key == Key.Down) DecreaseEvolutionSpeed();
        }

        private void StartEvolution() => Timer.Start();

        private void StopEvolution() => Timer.Stop();

        private void StopEvolutionAndResetPopulation()
        {
            StopEvolution();


        }

        #endregion
    }
}
