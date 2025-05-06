using System;
using System.IO;
using MealCompensationCalculator.BusinessLogic.Commands;
using MealCompensationCalculator.BusinessLogic.Queries;
using MealCompensationCalculator.BusinessLogic.Services.CompensationCalculator;
using MealCompensationCalculator.Domain.Models;
using Xunit;

namespace MealCompensationCalculator.IntegrationTest
{
    public class CreateEmployeeSummaryReportToExcelCommandTest
    {
        [Fact]
        public async void Execute()
        {
            var timeSheetOfEmployeesService = new GetTimeSheetOfEmployeesFromExcel(PathToFilesStore.PathToTimeSheetOfEmployees);
            var timeSheetOfEmployees = await timeSheetOfEmployeesService.Execute();

            var totalPayOfEmployeesService = new GetTotalPayOfEmployeesFromExcel(PathToFilesStore.PathToTotalPayOfEmployees);
            var totalPayOfEmployees = await totalPayOfEmployeesService.Execute();

            var dayCompensation = new MealCompensation(70, new TimeSpan(11, 0, 0), new TimeSpan(14, 0, 0));
            var dayEveningCompensation = new MealCompensation(110, new TimeSpan(16, 30, 0), new TimeSpan(17, 30, 0));

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var compensationResults = await compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            var guid = Guid.NewGuid().ToString();
            var outputDirectory = Directory.CreateDirectory(Path.Combine(PathToFilesStore.PathToTestDataDirectory, guid)).FullName;

            var path = Path.Combine(outputDirectory, "сводный отчет.xlsx");
            var service = new CreateEmployeeSummaryReportToExcelCommand(dayEveningCompensation);
            await service.Execute(path, totalPayOfEmployees.StartPeriod, totalPayOfEmployees.EndPeriod, compensationResults);
        }
    }
}