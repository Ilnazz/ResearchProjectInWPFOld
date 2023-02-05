using System.ComponentModel;
using System.Security.AccessControl;
using System.Windows;

namespace ResearchProject.StaticClasses
{
    public static class Settings
    {
        #region Field

        #region Dimensions

        public static int FieldMinWidth = 800;
        public static int FieldMinHeight = 600;

        // If false, then set field initial width and height to values you want
        public static bool SetFieldMaxSizeToScreenSize = true;

        public static int FieldMaxWidth = 1920;
        public static int FieldMaxHeight = 1080;

        // If false, then set field initial width and height to values you want
        public static bool SetFieldInitialSizeToScreenSize = true;

        public static int FieldInitialWidth = 1280;
        public static int FieldInitialHeight = 720;

        public static bool IsFieldResizable = true;

        public static bool IsFieldWindowHasBorder = false;

        #endregion

        public static bool WrapField = true;

        #region Cells

        public static float CellSize = 2;

        public static float RandomPopulationDensity = 0.0625f;

        #endregion

        #endregion

        #region Timer

        public static int TimerInitialTicksPerSecond = 60;

        public static int TimerMaxTicksPerSecond = 144;
        public static int TimerMinTicksPerSecond = 1;

        #endregion

        #region Runtime computing properties

        #endregion

        public static void Initialize()
        {

            double w1 = SystemParameters.FullPrimaryScreenWidth, // 1920
                h1 = SystemParameters.FullPrimaryScreenHeight; // 1009

            // Constant
            double w2 = SystemParameters.PrimaryScreenWidth, // 1920
                h2 = SystemParameters.PrimaryScreenHeight; //1080

            double w3 = SystemParameters.MaximizedPrimaryScreenWidth, // 1936
                h3 = SystemParameters.MaximizedPrimaryScreenHeight; // 1048

            // Constant
            double w4 = SystemParameters.VirtualScreenWidth, // 1920
                h4 = SystemParameters.VirtualScreenHeight; // 1080

            double w5 = SystemParameters.WorkArea.Width, // 1920
                h5 = SystemParameters.WorkArea.Height; // 1032

        }
    }
}
