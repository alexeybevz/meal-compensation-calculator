using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MealCompensationViewModel : ViewModelBase
    {
        private string _compensationName;
        public string CompensationName
        {
            get
            {
                return _compensationName;
            }
            set
            {
                _compensationName = value;
                OnPropertyChanged(nameof(CompensationName));
            }
        }

        private decimal _compensationAmount;
        public decimal CompensationAmount
        {
            get
            {
                return _compensationAmount;
            }
            set
            {
                _compensationAmount = value;
                OnPropertyChanged(nameof(CompensationAmount));
            }
        }

        private string _compensationAmountText;
        public string CompensationAmountText
        {
            get
            {
                return _compensationAmountText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _compensationAmountText = "0";
                    OnPropertyChanged(nameof(CompensationAmountText));
                    CompensationAmount = 0;
                    return;
                }

                var regex = new Regex(@"^\d{1,3}$");
                var isParse = regex.IsMatch(value);

                if (isParse)
                {
                    _compensationAmountText = value;
                    CompensationAmount = decimal.Parse(_compensationAmountText);
                    OnPropertyChanged(nameof(CompensationAmountText));
                }
            }
        }

        private string _startTimeCompensationHour;
        public string StartTimeCompensationHour
        {
            get
            {
                return _startTimeCompensationHour;
            }
            set
            {
                _startTimeCompensationHour = FormatHours(_startTimeCompensationHour, value);
                OnPropertyChanged(nameof(StartTimeCompensationHour));
            }
        }

        private string _startTimeCompensationMinute;
        public string StartTimeCompensationMinute
        {
            get
            {
                return _startTimeCompensationMinute;
            }
            set
            {
                _startTimeCompensationMinute = FormatMinute(_startTimeCompensationMinute, value);
                OnPropertyChanged(nameof(StartTimeCompensationMinute));
            }
        }

        private string _endTimeCompensationHour;
        public string EndTimeCompensationHour
        {
            get
            {
                return _endTimeCompensationHour;
            }
            set
            {
                _endTimeCompensationHour = FormatHours(_endTimeCompensationHour, value);
                OnPropertyChanged(nameof(EndTimeCompensationHour));
            }
        }

        private string _endTimeCompensationMinute;
        public string EndTimeCompensationMinute
        {
            get
            {
                return _endTimeCompensationMinute;
            }
            set
            {
                _endTimeCompensationMinute = FormatMinute(_endTimeCompensationMinute, value);
                OnPropertyChanged(nameof(EndTimeCompensationMinute));
            }
        }

        public MealCompensationViewModel(string compensationName, MealCompensation mealCompensation)
        {
            CompensationName = compensationName;

            CompensationAmount = mealCompensation.Compensation;
            CompensationAmountText = mealCompensation.Compensation.ToString(CultureInfo.InvariantCulture);

            StartTimeCompensationHour = mealCompensation.StartTimeCompensation.ToString("hh");
            StartTimeCompensationMinute = mealCompensation.StartTimeCompensation.ToString("mm");

            EndTimeCompensationHour = mealCompensation.EndTimeCompensation.ToString("hh");
            EndTimeCompensationMinute = mealCompensation.EndTimeCompensation.ToString("mm");
        }

        public MealCompensation GetCurrentSettingsOfMealCompensation()
        {
            return new MealCompensation(
                CompensationAmount,
                new TimeSpan(int.Parse(StartTimeCompensationHour), int.Parse(StartTimeCompensationMinute), 0),
                new TimeSpan(int.Parse(EndTimeCompensationHour), int.Parse(EndTimeCompensationMinute), 0));
        }

        private string FormatHours(string previousValue, string newValue)
        {
            return FormatHourOrMinute(previousValue, newValue, new Regex(@"^(?:[0-1]?[0-9]|2[0-3])$"));
        }

        private string FormatMinute(string previousValue, string newValue)
        {
            return FormatHourOrMinute(previousValue, newValue, new Regex(@"^(?:[0-5]?\d)$"));
        }

        private string FormatHourOrMinute(string previousValue, string newValue, Regex regex)
        {
            if (string.IsNullOrEmpty(newValue) || newValue == "0")
                return "00";

            var firstTwo = new string(newValue.Take(2).ToArray());
            var isParse = regex.IsMatch(firstTwo);
            return isParse ? firstTwo : previousValue;
        }
    }
}