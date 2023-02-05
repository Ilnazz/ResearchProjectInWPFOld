using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using ResearchProject.Models;
using ResearchProject.Windows;
using ResearchProject.StaticClasses;

namespace ResearchProject
{
    public partial class App : Application
    {
        private readonly Field Field;
        private readonly FieldWindow FieldWindow;

        public App()
        {
            Settings.Initialize();

            Field = new Field(
                Settings.ScreenWidth,
                Settings.ScreenHeight,
                Settings.CellSize,
                Settings.WrapField
            );

            Field.PopulateFieldRandomly(Settings.RandomPopulationDensity);

            FieldWindow = new FieldWindow();
            FieldWindow.Show();

            FieldWindow.MouseMove += OnMouseMove;
            FieldWindow.KeyDown += OnKeyDown;
            FieldWindow.PaintSurface += (s, e) => Renderer.RenderField(e.Surface.Canvas, Field);

            var filePath = @"C:\Users\Ильназ\Desktop\Текстовый документ.txt";

            var sw = new Stopwatch();

            ImprovedTimer.TicksPerSecond = Settings.TimerTicksPerSecond;
            ImprovedTimer.Tick += delegate
            {
                sw.Restart();
                Field.Advance();
                sw.Stop();
                System.IO.File.AppendAllText(filePath, $"calc: {sw.Elapsed.TotalSeconds} sec\n");

                sw.Restart();
                FieldWindow.Update();
                sw.Stop();
                System.IO.File.AppendAllText(filePath, $"rendering: {sw.Elapsed.TotalSeconds} sec\n\n");
            };
        }

        #region Simple/basic keyboard control panel

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            bool isLeftButtonPressed = e.LeftButton == MouseButtonState.Pressed,
                 isRightButtonPressed = e.RightButton == MouseButtonState.Pressed;

            if (isLeftButtonPressed == false && isRightButtonPressed == false)
                return;

            int mouseX = (int)e.GetPosition(FieldWindow).X,
                mouseY = (int)e.GetPosition(FieldWindow).Y;

            int cellColumnNumber = mouseX / (int)Settings.CellSize,
                cellRowNumber = mouseY / (int)Settings.CellSize;

            if (isLeftButtonPressed)
                Field.Cells[cellColumnNumber, cellRowNumber].IsAlive = true;
            else
                Field.Cells[cellColumnNumber, cellRowNumber].IsAlive = false;

            if (ImprovedTimer.IsEnabled == false)
                FieldWindow.Update();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ImprovedTimer.Start();

            else if (e.Key == Key.Space) ImprovedTimer.Stop();

            else if (e.Key == Key.Up) ImprovedTimer.TicksPerSecond++;

            else if (e.Key == Key.Down) ImprovedTimer.TicksPerSecond--;

            else if (e.Key == Key.Back)
            {
                // Reset field and session properties
            }
        }

        #endregion
    }
}
