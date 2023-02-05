using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Threading;

namespace ResearchProject.StaticClasses
{
    public static class ImprovedTimer
    {
        #region Constants

        public const int MinTicksPerSecond = 1;
        public const int MaxTicksPerSecond = 1000;

        #endregion

        private static DispatcherTimer _realTimer = new();

        private static IDictionary<EventHandler, EventHandler> _wrappedTickCallbacks = new Dictionary<EventHandler, EventHandler>();

        private static bool _isTickCallbackExecuting;

        public static event EventHandler Tick
        {
            add {
                var realCallBack = value;

                void wrappedTickCallback(object? state, EventArgs e)
                {
                    if (_isTickCallbackExecuting == true)
                        return;
                    _isTickCallbackExecuting = true;

                    realCallBack?.Invoke(state, e);

                    _isTickCallbackExecuting = false;
                }

                _realTimer.Tick += wrappedTickCallback;
                _wrappedTickCallbacks.Add(realCallBack, wrappedTickCallback);
            }

            remove {
                if (_wrappedTickCallbacks.TryGetValue(value, out var realCallBack))
                {
                    _realTimer.Tick -= realCallBack;
                    _wrappedTickCallbacks.Remove(realCallBack);
                }
            }
        }

        public static bool IsEnabled => _realTimer.IsEnabled;

        private static int _ticksPerSecond;
        public static int TicksPerSecond
        {
            get => _ticksPerSecond;
            set
            {
                if (value < MinTicksPerSecond || value > MaxTicksPerSecond)
                    return;
                
                var wasEnabled = _realTimer.IsEnabled;

                if (_realTimer.IsEnabled) _realTimer.Stop();

                _ticksPerSecond = value;
                _realTimer.Interval = TimeSpan.FromMilliseconds(1000 / _ticksPerSecond);

                if (wasEnabled) _realTimer.Start();
            }
        }

        public static void Start() => _realTimer.Start();
        public static void Stop() => _realTimer.Stop();
    }
}
