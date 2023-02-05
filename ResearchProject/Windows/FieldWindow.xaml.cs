using System;
using System.Windows;
using SkiaSharp.Views.Desktop;

namespace ResearchProject.Windows
{
    public partial class FieldWindow : Window
    {
        public EventHandler<SKPaintGLSurfaceEventArgs> PaintSurface;

        public FieldWindow()
        {
            InitializeComponent();

            SKGLControl.PaintSurface += (s, e) => PaintSurface?.Invoke(s, e);
        }

        public void Update() => SKGLControl.Invalidate();
    }
}
