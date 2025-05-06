using System;
using System.IO;
using System.Threading.Tasks;
using MealCompensationCalculator.BusinessLogic.Commands;
using MealCompensationCalculator.BusinessLogic.Queries;
using MealCompensationCalculator.BusinessLogic.Services.CompensationCalculator;
using MealCompensationCalculator.WPF.ViewModels;
using Microsoft.Win32;

namespace MealCompensationCalculator.WPF.Commands
{
    public class ExecuteMealCompensationCalculatorCommand : AsyncCommandBase
    {
        private readonly ConfigViewModel _configViewModel;

        public ExecuteMealCompensationCalculatorCommand(ConfigViewModel configViewModel)
        {
            _configViewModel = configViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            var vm = _configViewModel.DayCompensationViewModel;
            var dayCompensation = vm.GetCurrentSettingsOfMealCompensation();

            vm = _configViewModel.DayEveningCompensationViewModel;
            var dayEveningCompensation = vm.GetCurrentSettingsOfMealCompensation();

            var pathToOutputDirectory = _configViewModel.ReportLocationViewModel.PathToReport;

            var pathOfExcelFileWithTotalPayOfEmployees = GetPathOfExcelFileWithTotalPayOfEmployees();
            var totalPayOfEmployeesService = new GetTotalPayOfEmployeesFromExcel(pathOfExcelFileWithTotalPayOfEmployees);

            var pathOfExcelFileWithTimeSheetOfEmployees = GetPathOfExcelFileWithTimeSheetOfEmployees();
            var timeSheetOfEmployeesService = new GetTimeSheetOfEmployeesFromExcel(pathOfExcelFileWithTimeSheetOfEmployees);

            var timeSheetOfEmployees = await timeSheetOfEmployeesService.Execute();
            var totalPayOfEmployees = await totalPayOfEmployeesService.Execute();

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var compensationResults = await compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            var path1 = Path.Combine(pathToOutputDirectory, "результат.xlsx");
            var service1 = new CreateEmployeeSummaryReportToExcelCommand(dayEveningCompensation);
            await service1.Execute(path1, totalPayOfEmployees.StartPeriod, totalPayOfEmployees.EndPeriod, compensationResults);

            var path2 = Path.Combine(pathToOutputDirectory, "несоответствия.xlsx");
            var service2 = new CreateEmployeePaymentsMistakesReportToExcelCommand(dayCompensation, dayEveningCompensation);
            await service2.Execute(path2, compensationResults);
        }

        private string GetPathOfExcelFileWithTotalPayOfEmployees()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите Excel файл с отчетом по сотрудникам за месяц";
            openFileDialog.Filter = "Excel Files|*.xlsx;";
            if (openFileDialog.ShowDialog() != true)
                throw new ArgumentException("Не выбран файл с отчетом по сотрудникам за месяц");

            return openFileDialog.FileName;
        }

        private string GetPathOfExcelFileWithTimeSheetOfEmployees()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите Excel файл с табелем учета рабочего времени сотрудников";
            openFileDialog.Filter = "Excel Files|*.xlsx;";
            if (openFileDialog.ShowDialog() != true)
                throw new ArgumentException("Не выбран файл с с табелем учета рабочего времени сотрудников");

            return openFileDialog.FileName;
        }
    }
}