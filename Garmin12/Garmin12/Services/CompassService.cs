using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Services
{
    using Windows.Devices.Sensors;

    using Garmin12.Models;

    public class CompassService
    {
        public event Action<CompassData> OnCompassReading;
        private Compass compass;

        public CompassService()
        {
            this.compass = Compass.GetDefault();
            if (this.compass != null)
            {
                this.compass.ReportInterval = 1000;
                this.compass.ReadingChanged += this.OnReadingChanged;
            }
        }

        private void OnReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            this.OnCompassReading?.Invoke(
                new CompassData(args.Reading.HeadingTrueNorth ?? args.Reading.HeadingMagneticNorth));

        }
    }
}
