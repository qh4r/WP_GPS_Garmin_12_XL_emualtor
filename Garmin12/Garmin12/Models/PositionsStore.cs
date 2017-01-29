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
        public bool Initialized { get; private set; } = false;

        public string DbPath { get; private set; }

        public PositionsStore()
        {

        }

        public bool InitializeConnection(string dbPath)
        {
            this.DbPath = dbPath;
            try
            {
                if (!this.CheckFileExists(dbPath).Result)
                {
                    using (var positionsConnection = new SQLiteConnection(dbPath))
                    {
                        positionsConnection.CreateTable<PositionEntity>();
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

        public List<PositionEntity> FetchPositions()
        {
            using (var db = new SQLiteConnection(this.DbPath))
            {
                var positions = db.Table<PositionEntity>().ToList<PositionEntity>();
                return positions;
            }
        }

        public int InsertPosition(PositionEntity newcontact)
        {
            using (var db = new SQLiteConnection(this.DbPath))
            {
               return db.Insert(newcontact);
            }
        }

        public void DeletePosition(int id)
        {
            using (var db = new SQLiteConnection(this.DbPath))
            {
                var position = db.Query<PositionEntity>("select * from PositionEntity where Id =" + id).FirstOrDefault();
                if (position != null)
                {
                    db.Delete(position);
                }
            }
        }
    }
}
