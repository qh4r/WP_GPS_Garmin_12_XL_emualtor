using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garmin12.Resources;

namespace Garmin12.Services
{
    using System.Collections.ObjectModel;

    using Windows.UI.Core;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Threading;

    using Garmin12.Models;

    public class DataService : ViewModelBase
    {
        private readonly Constants constants;

        private PositionsStore store;

        private ObservableCollection<PositionEntity> positionsList;

        public DataService(PositionsStore positionsStore, Constants constants)
        {
            this.store = positionsStore;
            this.constants = constants;
            this.store.InitializeConnection(constants.DbPath);       
        }    

        ObservableCollection<PositionEntity> PositionsList
        {
            get
            {
                return this.positionsList
                       ?? (this.positionsList =
                           new ObservableCollection<PositionEntity>(
                               this.store.FetchPositions() ?? new List<PositionEntity>()));
            }
            set
            {
                this.Set(ref this.positionsList, value);
            }
        }

        public ObservableCollection<PositionEntity> SortedPositions
            => new ObservableCollection<PositionEntity>(this.PositionsList.OrderBy(x => x.Id));

        public async void SavePosition(string name, GpsPosition location)
        {
            var serializedPosition = new PositionEntity
                                         {
                                             Latitude = location.Latitude,
                                             Longitude = location.Longitude,
                                             Name = name
                                         };
            var id = this.store.InsertPosition(serializedPosition);
            serializedPosition.Id = id;
            await DispatcherHelper.RunAsync(
                () =>
                    {
                        this.PositionsList.Add(serializedPosition);
                        this.RaisePropertyChanged(() => this.SortedPositions);
                    });
        }

        public async void DeletePositon(PositionEntity position)
        {
            await Task.Run(
                () =>
                    {
                        this.store.DeletePosition((int)position.Id);
                    }).ContinueWith(
                        async x =>
                            {
                                await DispatcherHelper.RunAsync(
                                    () =>
                                        {
                                            this.PositionsList.Remove(position);
                                            this.RaisePropertyChanged(() => this.SortedPositions);
                                        });
                            });
        }
    }
}
