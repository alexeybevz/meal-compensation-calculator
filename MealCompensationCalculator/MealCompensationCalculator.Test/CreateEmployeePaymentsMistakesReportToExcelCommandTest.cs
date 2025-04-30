using System;
using MealCompensationCalculator.BusinessLogic.Commands;
using MealCompensationCalculator.BusinessLogic.Queries;
using MealCompensationCalculator.BusinessLogic.Services.CompensationCalculator;
using MealCompensationCalculator.Domain.Models;
using Xunit;

namespace MealCompensationCalculator.Test
{
    public class CreateEmployeePaymentsMistakesReportToExcelCommandTest
    {
        [Fact]
        public async void Execute()
        {
            var path = @"C:\Users\abevz\Desktop\табель.xlsx";
            var timeSheetOfEmployeesService = new GetTimeSheetOfEmployeesFromExcel(path);
            var timeSheetOfEmployees = await timeSheetOfEmployeesService.Execute();

            path = @"C:\Users\abevz\Desktop\отчет по сотрудникам октябрь.xlsx";
            var totalPayOfEmployeesService = new GetTotalPayOfEmployeesFromExcel(path);
            var totalPayOfEmployees = await totalPayOfEmployeesService.Execute();

            var dayCompensation = new MealCompensation(70, new TimeSpan(11, 0, 0), new TimeSpan(14, 0, 0));
            var dayEveningCompensation = new MealCompensation(110, new TimeSpan(16, 30, 0), new TimeSpan(17, 30, 0));

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var compensationResults = compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            path = @"C:\Users\abevz\Desktop\несоответствия.xlsx";
            var service = new CreateEmployeePaymentsMistakesReportToExcelCommand(dayCompensation, dayEveningCompensation);
            await service.Execute(path, compensationResults);
        }
    }
}