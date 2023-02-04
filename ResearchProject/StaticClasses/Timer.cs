using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ResearchProject.StaticClasses
{
    public static class Timer
    {
        #region Constants

        public const int MinTicksPerSecond = 1;
        public const int MaxTicksPerSecond = 1000;

        #endregion

        private static System.Timers.Timer _innerTimer = new();

        private static ElapsedEventHandler _tickCallback;
        public static ElapsedEventHandler TickCallback
        {
            get => _tickCallback;
            set
            {
                if (value == null)
                    return;
                _innerTimer.Elapsed += value;
            }
        }

        public static bool IsEnabled => _innerTimer.Enabled;

        private static int _ticksPerSecond;
        public static int TicksPerSecond
        {
            get => _ticksPerSecond;
            set
            {
                if (value < MinTicksPerSecond || value > MaxTicksPerSecond)
                    return;
                
                var wasEnabled = _innerTimer.Enabled;

                if (_innerTimer.Enabled) _innerTimer.Stop();

                _ticksPerSecond = value;
                _innerTimer.Interval= _ticksPerSecond;

                if (wasEnabled) _innerTimer.Start();
            }
        }

        public static void Start() => _innerTimer.Start();
        public static void Stop() => _innerTimer.Stop();
    }
}
