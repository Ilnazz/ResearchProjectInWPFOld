using ResearchProject.Models;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchProject.StaticClasses
{
    public static class Renderer
    {
        #region Colors

        private static readonly SKColor ColorWhite = SKColor.Parse("#ffffff");
        private static readonly SKColor ColorBlack = SKColor.Parse("#000000");

        #endregion

        #region Paints

        private static readonly SKPaint PaintWhite = new() { Color = SKColor.Parse("#ffffff") };
        private static readonly SKPaint PaintBlack = new() { Color = SKColor.Parse("#000000") };

        #endregion

        public static void RenderField(SKCanvas canvas, Field field)
        {
            canvas.Clear(ColorWhite);

            for (int columnNumber = 0; columnNumber < field.ColumnsNumber; columnNumber++)
            {
                for (int rowNumber = 0; rowNumber < field.RowsNumber; rowNumber++)
                {
                    var paint = field.Cells[columnNumber, rowNumber].IsAlive ? PaintBlack : PaintWhite;

                    canvas.DrawRect(
                        columnNumber * Settings.CellSize,
                        rowNumber * Settings.CellSize,
                        Settings.CellSize,
                        Settings.CellSize,
                        paint
                    );
                }
            }
        }
    }
}
