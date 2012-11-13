namespace Services.Pipeline.StateControl
{
    using System;
    using System.Timers;

    public class ServiceTimer
    {
        private readonly Timer timer;

        internal ServiceTimer(TimeSpan timeSpan)
        {
            if (this.timer == null)
            {
                this.timer = new Timer
                {
                    AutoReset = true,
                    Interval = timeSpan.TotalMilliseconds
                };
            }
        }

        public double Interval
        {
            get
            {
                return this.timer.Interval;
            }
        }

        public static ServiceTimer FromMinutes(int minutes)
        {
            return new ServiceTimer(TimeSpan.FromMinutes(minutes));
        }

        public static ServiceTimer FromSeconds(int seconds)
        {
            return new ServiceTimer(TimeSpan.FromSeconds(seconds));
        }

        public static implicit operator Timer(ServiceTimer serviceTimer)
        {
            return serviceTimer.timer;
        }

        public ServiceTimer Execute(Action action)
        {
            if (action != null)
            {
                this.timer.Elapsed += delegate { action.Invoke(); };
            }
            return this;
        }

        public ServiceTimer Start()
        {
            this.timer.Start();
            return this;
        }

        public ServiceTimer Stop()
        {
            this.timer.Stop();
            return this;
        }
    }
}
