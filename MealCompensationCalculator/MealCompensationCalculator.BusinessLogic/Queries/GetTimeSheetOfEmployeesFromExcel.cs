using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;

namespace MealCompensationCalculator.BusinessLogic.Queries
{
    public class GetTimeSheetOfEmployeesFromExcel : IGetTimeSheetOfEmployeesQuery
    {
        private readonly string _pathToFile;

        public GetTimeSheetOfEmployeesFromExcel(string pathToFile)
        {
            _pathToFile = pathToFile;
        }

        public async Task<TimeSheetOfEmployees> Execute()
        {
            TimeSheetOfEmployees timeSheetOfEmployees = null;
            await Task.Run(() =>
            {
                timeSheetOfEmployees = ParseExcelFile();
            });

            return timeSheetOfEmployees;
        }

        private TimeSheetOfEmployees ParseExcelFile()
        {
            var startPeriod = DateTime.MinValue;
            var endPeriod = DateTime.MinValue;
            var employeeTimeSheets = new List<EmployeeTimeSheet>();

            using (var workbook = new XLWorkbook(_pathToFile))
            {
                var ws = workbook.Worksheet(1);

                startPeriod = DateTime.Parse(ws.Cell(12, 51).GetString());
                endPeriod = DateTime.Parse(ws.Cell(12, 55).GetString());

                // Не смотрим строку с подписями с помощью -6
                var maxRowNumber = ws.LastRowUsed().RowNumber() - 6;
                var i = 21;

                while (i <= maxRowNumber)
                {
                    int seq;
                    var isParse = ws.Cell(i, 2).TryGetValue(out seq);

                    if (isParse)
                    {
                        var fullNameAndJobTitle = ws.Cell(i, 3).GetValue<string>();
                        var employeeNumber = ws.Cell(i, 5).GetValue<int>();

                        var employee = new EmployeeFromTimeSheet(employeeNumber, fullNameAndJobTitle);
                        var timeSheetDays = new List<TimeSheetDay>();

                        var startCol = 9;
                        var col = startCol;
                        var day = 1;
                        var halfMonth = 1;
                        var row = i;

                        while (col <= 38)
                        {
                            var scheduleOfWork = ws.Cell(row, col).GetString();
                            var shift = ws.Cell(row + 1, col).GetString();

                            if (!string.IsNullOrEmpty(scheduleOfWork))
                                timeSheetDays.Add(new TimeSheetDay(day, scheduleOfWork, shift));

                            col = MoveColToNextDay(col);
                            day++;

                            if (col == 38 && halfMonth == 1)
                            {
                                col = startCol;
                                row += 2;
                                halfMonth = 2;
                            }
                        }

                        employeeTimeSheets.Add(new EmployeeTimeSheet(employee, timeSheetDays.ToDictionary(x => x.Day, x => x)));

                        i += 4;
                    }
                    else
                    {
                        // Переходим через header
                        i += 7;
                    }
                }
            }

            return new TimeSheetOfEmployees(startPeriod, endPeriod, employeeTimeSheets);
        }

        private static int MoveColToNextDay(int col)
        {
            switch (col)
            {
                case 13:
                case 37:
                    return col + 1;
                case 34:
                    return col + 3;
                default:
                    return col + 2;
            }
        }
    }
}