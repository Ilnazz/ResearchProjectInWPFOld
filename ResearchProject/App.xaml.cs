using ResearchProject.Models;
using ResearchProject.StaticClasses;
using ResearchProject.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ResearchProject
{
    public partial class App : Application
    {
        private Field Field;
        private FieldWindow FieldWindow;

        public App()
        {
            Settings.Initialize();

            Field = new Field(
                Settings.ScreenWidth,
                Settings.ScreenHeight,
                Settings.CellSize,
                Settings.WrapField
            );

            Field.PopulateFieldRandomly(0.33);

            FieldWindow = new FieldWindow();
            FieldWindow.Show();

            FieldWindow.MouseMove += OnMouseMove;
            FieldWindow.KeyDown += OnKeyDown;
            FieldWindow.PaintSurface += (s, e) => Renderer.RenderField(e.Surface.Canvas, Field);

            Timer.TicksPerSecond = Settings.TimerTicksPerSecond;
            Timer.TickCallback += delegate
            {
                Field.Advance();
                FieldWindow.Update();
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

            if (Timer.IsEnabled == false)
                FieldWindow.Update();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Timer.Start();

            else if (e.Key == Key.Space) Timer.Stop();

            else if (e.Key == Key.Up) Timer.TicksPerSecond++;

            else if (e.Key == Key.Down) Timer.TicksPerSecond--;

            else if (e.Key == Key.Back)
            {
                // Reset field and session properties
            }
        }

        #endregion
    }
}
