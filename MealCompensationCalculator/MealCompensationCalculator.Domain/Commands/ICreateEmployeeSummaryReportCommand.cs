using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.Domain.Commands
{
    public interface ICreateEmployeeSummaryReportCommand
    {
        Task Execute(string pathToXlsxFile, DateTime startPeriod, DateTime endPeriod, IEnumerable<CompensationResult> compensationResults);
    }
}