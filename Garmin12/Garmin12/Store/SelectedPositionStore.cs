using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Store
{
    using GalaSoft.MvvmLight;

    using Garmin12.Models;

    public class SelectedPositionStore : ViewModelBase
    {
        public event Action<PositionEntity> SelectionChanged;

        private PositionEntity selectedPosition;

        public SelectedPositionStore()
        {
            this.SelectedPosition = null;
        }

        public PositionEntity SelectedPosition
        {
            get
            {
                return this.selectedPosition;
            }
            set
            {
                this.Set(ref this.selectedPosition, value);
                this.RaisePropertyChanged(() => this.IsPositionSelected);
                this.SelectionChanged?.Invoke(value);
            }
        }

        public bool IsPositionSelected => this.SelectedPosition != null;
    }
}
