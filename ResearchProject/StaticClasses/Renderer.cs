using SkiaSharp;

using ResearchProject.Models;

namespace ResearchProject.StaticClasses
{
    public static class Renderer
    {
        #region Colors

        private static readonly SKColor ColorWhite = SKColor.Parse("#ffffff");
        private static readonly SKColor ColorBlack = SKColor.Parse("#000000");

        private static readonly SKColor ColorLightBlue = SKColor.Parse("#caf0f8");
        private static readonly SKColor ColorDarkBlue = SKColor.Parse("#03045e");

        #endregion

        #region Paints

        private static readonly SKPaint PaintWhite = new() { Color = SKColor.Parse("#ffffff") };
        private static readonly SKPaint PaintBlack = new() { Color = SKColor.Parse("#000000") };

        private static readonly SKPaint PaintLightBlue = new() { Color = SKColor.Parse("#caf0f8") };
        private static readonly SKPaint PaintDarkBlue = new() { Color = SKColor.Parse("#03045e") };

        #endregion


        public static void RenderField(SKCanvas canvas, Field field)
        {
            canvas.Clear(ColorLightBlue);

            for (int columnNumber = 0; columnNumber < field.ColumnsNumber; columnNumber++)
            {
                for (int rowNumber = 0; rowNumber < field.RowsNumber; rowNumber++)
                {
                    var paint = field.Cells[columnNumber, rowNumber].IsAlive ? PaintDarkBlue : PaintLightBlue;

                    canvas.DrawCircle(
                        columnNumber * Settings.CellSize,
                        rowNumber * Settings.CellSize,
                        Settings.CellSize / 2,
                        paint
                    );
                }
            }
        }
    }
}
