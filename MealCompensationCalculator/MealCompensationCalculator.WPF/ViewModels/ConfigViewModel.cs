using MealCompensationCalculator.WPF.Stores;

namespace MealCompensationCalculator.WPF.ViewModels
{
    public class ConfigViewModel : ViewModelBase
    {
        private readonly ConfigStore _configStore;
        private MealCompensationViewModel _dayCompensationViewModel;
        private MealCompensationViewModel _dayEveningCompensationViewModel;
        private ReportLocationViewModel _reportLocationViewModel;

        public MealCompensationViewModel DayCompensationViewModel
        {
            get { return _dayCompensationViewModel; }
            private set
            {
                _dayCompensationViewModel = value;
                OnPropertyChanged(nameof(DayCompensationViewModel));
            }
        }

        public MealCompensationViewModel DayEveningCompensationViewModel
        {
            get { return _dayEveningCompensationViewModel; }
            private set
            {
                _dayEveningCompensationViewModel = value;
                OnPropertyChanged(nameof(DayEveningCompensationViewModel));
            }
        }

        public ReportLocationViewModel ReportLocationViewModel
        {
            get { return _reportLocationViewModel; }
            private set
            {
                _reportLocationViewModel = value;
                OnPropertyChanged(nameof(ReportLocationViewModel));
            }
        }

        public ConfigViewModel(ConfigStore configStore)
        {
            _configStore = configStore;
            _configStore.ConfigLoaded += ConfigStoreOnConfigLoaded;
        }

        private void ConfigStoreOnConfigLoaded()
        {
            var timeSpanStringFormat = @"hh\:mm";

            DayCompensationViewModel = new MealCompensationViewModel("Дневная компенсация", _configStore.Config.DayCompensation);
            DayEveningCompensationViewModel = new MealCompensationViewModel("Дневная и вечерняя компенсация", _configStore.Config.DayEveningCompensation);
            ReportLocationViewModel = new ReportLocationViewModel()
            {
                PathToReport = _configStore.Config.PathToSaveReports
            };
        }

        protected override void Dispose()
        {
            _configStore.ConfigLoaded -= ConfigStoreOnConfigLoaded;
            base.Dispose();
        }
    }
}