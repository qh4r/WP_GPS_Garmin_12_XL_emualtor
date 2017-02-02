using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Resources
{
    using System.IO;

    using Windows.Storage;

    public class Constants
    {
        public string DbPath { get; } = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "PositionsStore.sqlite"));

        public double R { get; } = 6378.16;
    }
}
