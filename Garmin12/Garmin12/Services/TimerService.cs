using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Services
{
    using System.Threading;

    public class TimerService
    {
        public event Action TimerUpdate;
        private Timer timer;

        public TimerService()
        {
            this.timer = new Timer(state => TimerUpdate?.Invoke(), null, 0, 500);
        }
}
}
