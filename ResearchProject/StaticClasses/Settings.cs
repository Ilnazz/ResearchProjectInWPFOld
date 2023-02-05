namespace ResearchProject.StaticClasses
{
    public static class Settings
    {
        #region Properties with default values

        public static float CellSize = Properties.Settings.Default.CellSize;

        public static float RandomPopulationDensity = Properties.Settings.Default.RandomPopulationDensity;

        public static bool WrapField = Properties.Settings.Default.WrapField;

        public static int TimerTicksPerSecond = Properties.Settings.Default.TimerTicksPerSecond;

        #endregion
        
        #region Runtime computing properties

        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        #endregion

        public static void Initialize()
        {
            ScreenWidth = (int)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            ScreenHeight = (int)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        }
    }
}
