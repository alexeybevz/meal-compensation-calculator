using System;
using System.IO;
using System.Threading.Tasks;
using MealCompensationCalculator.BusinessLogic.Commands;
using MealCompensationCalculator.BusinessLogic.Queries;
using MealCompensationCalculator.BusinessLogic.Services.CompensationCalculator;
using MealCompensationCalculator.Domain.Models;
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

            Task<TotalPayOfEmployees> totalPayOfEmployeesServiceTask = totalPayOfEmployeesService.Execute();
            Task<TimeSheetOfEmployees> timeSheetOfEmployeesServiceTask = timeSheetOfEmployeesService.Execute();

            await Task.WhenAll(totalPayOfEmployeesServiceTask, timeSheetOfEmployeesServiceTask);

            var totalPayOfEmployees = totalPayOfEmployeesServiceTask.Result;
            var timeSheetOfEmployees = timeSheetOfEmployeesServiceTask.Result;

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var compensationResults = await compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            var summaryReportPath = Path.Combine(pathToOutputDirectory, $"Сводный отчет ({totalPayOfEmployees.StartPeriod.ToShortDateString()} - {totalPayOfEmployees.EndPeriod.ToShortDateString()}) на {DateTime.Now.ToShortDateString()}.xlsx");
            var summaryReportService = new CreateEmployeeSummaryReportToExcelCommand(dayEveningCompensation);
            var summaryReportServiceTask = summaryReportService.Execute(summaryReportPath, totalPayOfEmployees.StartPeriod, totalPayOfEmployees.EndPeriod, compensationResults);

            var mistakesReportPath = Path.Combine(pathToOutputDirectory, $"Отчет по несоответствиям ({totalPayOfEmployees.StartPeriod.ToShortDateString()} - {totalPayOfEmployees.EndPeriod.ToShortDateString()}) на {DateTime.Now.ToShortDateString()}.xlsx");
            var mistakesReportService = new CreateEmployeePaymentsMistakesReportToExcelCommand(dayCompensation, dayEveningCompensation);
            var mistakesReportServiceTask = mistakesReportService.Execute(mistakesReportPath, compensationResults);

            await Task.WhenAll(summaryReportServiceTask, mistakesReportServiceTask);
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