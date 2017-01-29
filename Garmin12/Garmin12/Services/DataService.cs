using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garmin12.Resources;

namespace Garmin12.Services
{
    using Garmin12.Models;

    public class DataService
    {
        private readonly PositionsStore positionsStore;

        private readonly Constants constants;

        public DataService(PositionsStore positionsStore, Constants constants)
        {
            this.positionsStore = positionsStore;
            this.constants = constants;
            this.positionsStore.InitializeConnection(constants.DbPath);
        }
    }
}
