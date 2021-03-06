﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garmin12.Resources;

namespace Garmin12.Services
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using Windows.UI.Core;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Threading;

    using Garmin12.Models;
    using Garmin12.Store;

    public class DataService : ViewModelBase
    {
        private readonly Constants constants;

        private readonly SelectedPositionStore selectedPositionStore;

        private PositionsStore store;

        private ObservableCollection<PositionEntity> positionsList;

        private string nameFilter;

        public DataService(PositionsStore positionsStore, Constants constants, SelectedPositionStore selectedPositionStore)
        {
            this.store = positionsStore;
            this.constants = constants;
            this.selectedPositionStore = selectedPositionStore;
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

        public ObservableCollection<PositionEntity> FilteredPositions
            => new ObservableCollection<PositionEntity>(this.PositionsList
                .Where(x => string.IsNullOrWhiteSpace(this.NameFilter) 
                || x.Name.ToLower().Contains(this.NameFilter.ToLower())).OrderBy(x => x.Id));

        public string NameFilter
        {
            get
            {
                return this.nameFilter;
            }
            set
            {
                this.Set(ref this.nameFilter, value);
                this.RaisePropertyChanged(() => this.FilteredPositions);
            }
        }

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
                        this.RaisePropertyChanged(() => this.FilteredPositions);
                    });
        }

        public async void UpdatePosition(long id, string name, GpsPosition location)
        {
            var serializedPosition = new PositionEntity
            {
                Id = id,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Name = name
            };
            await Task.Run(
                () =>
                {
                    var updatedPosition = this.store.UpdatePosition(serializedPosition);
                    var toSubstitute = this.PositionsList.FirstOrDefault(x => x.Id == updatedPosition.Id);
                    var index = this.PositionsList.IndexOf(toSubstitute);                    
                    return new
                    {
                        index,
                        updatedPosition
                    };
                }).ContinueWith(
                        async result =>
                        {
                            await DispatcherHelper.RunAsync(
                                () =>
                                {
                                    this.PositionsList.RemoveAt(result.Result.index);
                                    this.PositionsList.Insert(result.Result.index, result.Result.updatedPosition);
                                    this.selectedPositionStore.SelectedPosition = result.Result.updatedPosition;
                                    this.RaisePropertyChanged(() => this.FilteredPositions);
                                });
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
                                            this.RaisePropertyChanged(() => this.FilteredPositions);                                            
                                        });
                            });
        }
    }
}
