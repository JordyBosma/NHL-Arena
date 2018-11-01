using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLogic
{
    public class GameTimer : System.Timers.Timer
    {
        private DateTime dueTime;

        public GameTimer(int interval) : base(interval)
        {
            this.Elapsed += this.ElapsedAction;
        }

        protected new void Dispose()
        {
            this.Elapsed -= this.ElapsedAction;
            base.Dispose();
        }

        public double TimeLeft
        {
            get
            {
                return (this.dueTime - DateTime.Now).TotalSeconds;
            }
        }

        public new void Start()
        {
            this.dueTime = DateTime.Now.AddSeconds(this.Interval);
            base.Start();
        }

        private void ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.AutoReset)
            {
                this.dueTime = DateTime.Now.AddSeconds(this.Interval);
            }
        }
    }
}
