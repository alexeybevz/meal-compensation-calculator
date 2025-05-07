using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClosedXML.Excel;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;

namespace MealCompensationCalculator.BusinessLogic.Queries
{
    public class GetTotalPayOfEmployeesFromExcel : IGetTotalPayOfEmployeesQuery
    {
        private readonly string _pathToFile;

        public GetTotalPayOfEmployeesFromExcel(string pathToFile)
        {
            _pathToFile = pathToFile;
        }

        public async Task<TotalPayOfEmployees> Execute()
        {
            TotalPayOfEmployees totalPayOfEmployees = null;
            await Task.Run(() =>
            {
                totalPayOfEmployees = ParseExcelFile();
            });

            return totalPayOfEmployees;
        }

        private TotalPayOfEmployees ParseExcelFile()
        {
            var startPeriod = DateTime.MinValue;
            var endPeriod = DateTime.MinValue;
            var employeePayments = new List<EmployeePayments>();

            using (var workbook = new XLWorkbook(_pathToFile))
            {
                var ws = workbook.Worksheet(1);

                var titleReport = ws.Cell(1, 1).GetString();
                var startIndex = titleReport.IndexOf("(", StringComparison.Ordinal);
                var endIndex = titleReport.IndexOf(")", StringComparison.Ordinal);

                if (startIndex > -1 && endIndex > -1)
                {
                    var periodString = titleReport.Substring(startIndex + 1, endIndex - 1 - startIndex);
                    var periodStringSplit = periodString.Split('-');
                    startPeriod = DateTime.Parse(periodStringSplit[0]);
                    endPeriod = DateTime.Parse(periodStringSplit[1]);
                }

                // Не смотрим строку с итогами с помощью -1
                var maxRowNumber = ws.LastRowUsed().RowNumber() - 1;
                var i = 4;

                var employee = new Employee(
                    ws.Cell(i, 2).GetValue<int>(),
                    ws.Cell(i, 3).GetValue<string>(),
                    ws.Cell(i, 4).GetValue<string>());

                i++;

                var payments = new List<Payment>();

                while (i <= maxRowNumber)
                {
                    if (string.IsNullOrEmpty(ws.Cell(i, 1).GetValue<string>()))
                    {
                        var dateString = ws.Cell(i, 3).GetValue<string>().Replace(" - ", " ");
                        var date = DateTime.Parse(dateString);

                        decimal cost;
                        ws.Cell(i, 5).TryGetValue(out cost);

                        decimal costCashLess;
                        ws.Cell(i, 7).TryGetValue(out costCashLess);

                        decimal costCash;
                        ws.Cell(i, 8).TryGetValue(out costCash);

                        var pay = new Payment(date, cost, costCashLess, costCash);
                        payments.Add(pay);
                    }
                    else
                    {
                        employeePayments.Add(new EmployeePayments(employee, new List<Payment>(payments)));
                        payments.Clear();

                        employee = new Employee(
                            ws.Cell(i, 2).GetValue<int>(),
                            ws.Cell(i, 3).GetValue<string>(),
                            ws.Cell(i, 4).GetValue<string>());
                    }

                    i++;
                }

                employeePayments.Add(new EmployeePayments(employee, new List<Payment>(payments)));
            }

            return new TotalPayOfEmployees(startPeriod, endPeriod, employeePayments);
        }
    }
}