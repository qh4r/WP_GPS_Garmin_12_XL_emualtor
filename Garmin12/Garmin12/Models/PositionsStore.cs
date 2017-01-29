using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Models
{
    using SQLite;

    public class PositionsStore
    {
        private SQLiteConnection positionsConnection;

        public bool Initialized { get; private set; } = false;

        public PositionsStore()
        {
            
        }

        public bool InitializeConnection(string dbPath)
        {
            try
            {
                if (!this.CheckFileExists(dbPath).Result)
                {
                    using (this.positionsConnection = new SQLiteConnection(dbPath))
                    {
                        this.positionsConnection.CreateTable<PositionEntity>();
                    }
                }
                this.Initialized = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
