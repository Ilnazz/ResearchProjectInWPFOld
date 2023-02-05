using System.Timers;
using System.Collections.Generic;

namespace ResearchProject.StaticClasses
{
    public static class ImprovedTimer
    {
        // We are using System.Timers.Timer (nor System.Threading.Timer, nor System.Threading.DispatcherTimer), because of this timer works well without freezings
        private static System.Timers.Timer _realTimer = new();

        private static IDictionary<ElapsedEventHandler, ElapsedEventHandler> _wrappedTickCallbacks = new Dictionary<ElapsedEventHandler, ElapsedEventHandler>();

        private static object _locker = new();

        public static event ElapsedEventHandler Tick
        {
            add {
                var realCallback = value;

                void wrappedTickCallback(object? s, ElapsedEventArgs e)
                {
                    lock (_locker)
                        realCallback?.Invoke(s, e);
                }

                _realTimer.Elapsed += wrappedTickCallback;
                _wrappedTickCallbacks.Add(realCallback, wrappedTickCallback);
            }

            remove {
                if (_wrappedTickCallbacks.TryGetValue(value, out var realCallBack))
                {
                    _realTimer.Elapsed -= realCallBack;
                    _wrappedTickCallbacks.Remove(realCallBack);
                }
            }
        }

        public static bool IsEnabled => _realTimer.Enabled;

        private static int _ticksPerSecond;
        public static int TicksPerSecond
        {
            get => _ticksPerSecond;
            set
            {
                if (value < Settings.TimerMinTicksPerSecond || value > Settings.TimerMaxTicksPerSecond)
                    return;
                
                var wasEnabled = _realTimer.Enabled;

                if (_realTimer.Enabled) _realTimer.Stop();

                _ticksPerSecond = value;
                _realTimer.Interval = 1000 / _ticksPerSecond;

                if (wasEnabled) _realTimer.Start();
            }
        }

        public static void Start() => _realTimer.Start();
        public static void Stop() => _realTimer.Stop();
    }
}
