using System.ComponentModel;
using System.Drawing;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace ResearchProject.StaticClasses
{
    public static class Settings
    {
        #region Screen dimensions

        public static readonly Size ScreenFullSize
            = Screen.PrimaryScreen.Bounds.Size;

        public static readonly Size ScreenWorkingAreaSize
            = Screen.PrimaryScreen.WorkingArea.Size;

        #endregion

        #region Field

        #region Dimensions

        //public static Size FieldMinSize = new(1280, 720);

        //public static Size FieldMaxSize = ScreenFullSize;

        //public static Size FieldInitialSize = ScreenFullSize;

        // Field max size will be ignored
        //public static bool MaximizeFieldAfterRun = false;

        #endregion

        //public static bool IsFieldResizable = true;

        //public static bool IsFieldTopMenuVisible = false;

        public static bool WrapField = false;

        #region Cells

        public static float CellSize = 10;

        public static float RandomPopulationDensity = 0.0625f;

        #endregion

        #endregion

        #region Timer

        public static int TimerInitialTicksPerSecond = 30;

        public static int TimerMaxTicksPerSecond = 144;
        public static int TimerMinTicksPerSecond = 1;

        #endregion

        #region Runtime computing properties

        #endregion

        public static void Initialize() { }
    }
}
