using System;
using System.Windows;
using System.Windows.Media;

using OpenTK;

using SkiaSharp.Views.Desktop;

namespace ResearchProject.Windows
{
    public partial class FieldWindow : Window
    {
        public EventHandler<SKPaintGLSurfaceEventArgs> PaintSurface;

        public FieldWindow()
        {
            InitializeComponent();

            Width = 1000;
            Height = 600;
            Background = Brushes.Red;

            // Magic numbers:
            int fieldCanvasWidth = (int)Width - 16,
                fieldCanvasHeight = (int)Height - 39;

            FieldCanvas.MaximumSize = new System.Drawing.Size(fieldCanvasWidth, fieldCanvasHeight);

            FieldCanvas.PaintSurface += (s, e) => PaintSurface?.Invoke(s, e);
        }

        public void Update() => FieldCanvas.Invalidate();
    }
}
