using System;
using System.Threading.Tasks;
using System.Windows;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.WPF.Stores
{
    public class ConfigStore
    {
        private MealCompensation _dayCompensation;
        private MealCompensation _dayEveningCompensation;
        private string _pathToSaveReports;

        public MealCompensation DayCompensation => _dayCompensation;
        public MealCompensation DayEveningCompensation => _dayEveningCompensation;
        public string PathToSaveReports => _pathToSaveReports;

        public event Action ConfigLoaded;

        public async Task Load()
        {
            // Чтение конфиг файла
            MessageBox.Show("Чтение конфиг файла...");
        }
    }
}