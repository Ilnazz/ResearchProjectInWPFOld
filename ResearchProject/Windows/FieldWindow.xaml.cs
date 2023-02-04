using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
