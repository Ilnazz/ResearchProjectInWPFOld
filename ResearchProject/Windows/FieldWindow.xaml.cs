using System;
using System.Windows.Media;

using OpenTK;
using System.Windows;

using SkiaSharp.Views.Desktop;
using ResearchProject.StaticClasses;

namespace ResearchProject.Windows
{
    public partial class FieldWindow : Window
    {
        public EventHandler<SKPaintGLSurfaceEventArgs> PaintSurface;

        public FieldWindow()
        {
            InitializeComponent();

            //MinWidth = Settings.FieldMinSize.Width;
            //MinHeight = Settings.FieldMinSize.Height;

            WindowState = System.Windows.WindowState.Maximized;

            //if (Settings.MaximizeFieldAfterRun)
            //{
            //    WindowState = System.Windows.WindowState.Maximized;

            //    Width = Settings.ScreenFullSize.Width;
            //    Height = Settings.ScreenFullSize.Height;
            //}
            //else
            //{
            //    MaxWidth = Settings.FieldMaxSize.Width;
            //    MaxHeight = Settings.FieldMaxSize.Height;

            //    Width = Settings.FieldInitialSize.Width;
            //    Height = Settings.FieldInitialSize.Height;
            //}

            //if (Settings.IsFieldResizable == false)
            //    ResizeMode = ResizeMode.NoResize;

            //if (Settings.IsFieldTopMenuVisible == false)
            //    WindowStyle = WindowStyle.None;

            //Background = Brushes.Red;

            FieldCanvas.MaximumSize = new System.Drawing.Size(Settings.ScreenFullSize.Width, Settings.ScreenFullSize.Height);

            FieldCanvas.PaintSurface += (s, e) => PaintSurface?.Invoke(s, e);
        }

        public void Update() => FieldCanvas.Invalidate();

        //private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    Background = Brushes.Red;

        //    System.Drawing.Size fieldCanvasSize;
        //    if (WindowState != System.Windows.WindowState.Maximized)
        //        fieldCanvasSize = new System.Drawing.Size((int)Width - 16, (int)Height - 39);
        //    else
        //        fieldCanvasSize = new System.Drawing.Size((int)Width, (int)Height);

        //    FieldCanvas.MaximumSize = fieldCanvasSize;
        //}
    }
}
