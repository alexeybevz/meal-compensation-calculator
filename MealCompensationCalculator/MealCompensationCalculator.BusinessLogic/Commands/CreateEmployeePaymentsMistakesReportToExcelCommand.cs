using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClosedXML.Excel;
using MealCompensationCalculator.Domain.Commands;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.BusinessLogic.Commands
{
    public class CreateEmployeePaymentsMistakesReportToExcelCommand : ICreateEmployeePaymentsMistakesReportCommand
    {
        public async Task Execute(string pathToXlsxFile, IEnumerable<CompensationResult> compensationResults)
        {
            await Task.Run(() =>
            {
                CreateReport(pathToXlsxFile, compensationResults);
            });
        }

        private void CreateReport(string pathToXlsxFile, IEnumerable<CompensationResult> compensationResults)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Style.Font.FontName = "Calibri";
                workbook.Style.Font.FontSize = 11;

                var ws = workbook.Worksheets.Add("Несоответствия");

                CreateWs(ws, compensationResults);

                workbook.SaveAs(pathToXlsxFile);
            }
        }

        private void CreateWs(IXLWorksheet ws, IEnumerable<CompensationResult> compensationResults)
        {
            var columnNames = new[]
            {
                "Таб. №",
                "Сотрудник",
                "Подразделение",
                "Дата заказа",
                "Стоимость питания",
                "График",
                "Смена"
            };

            CreateTitle(ws, columnNames.Length);
            CreateHeader(ws, columnNames);
            CreateBody(ws, compensationResults);
            PostFormatSheet(ws);
        }

        private void CreateTitle(IXLWorksheet ws, int columnNamesLength)
        {
            var range = ws.Range(ws.Cell(1, 1), ws.Cell(1, columnNamesLength));
            range.Merge();
            range.Style.Alignment.WrapText = true;
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Row(1).Height = 30;

            ws.Cell(1, 1).SetValue(
                $"Отчет по несоответствиям. Записываются сотрудники, которые были в столовой вне времени действия компенсации или не в соответствии с табелем. Подготовлен: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        }

        private void CreateHeader(IXLWorksheet ws, string[] columnNames)
        {
            var row = 2;
            var i = 1;

            foreach (var column in columnNames)
            {
                ws.Cell(row, i).Value = column;
                ws.Cell(row, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(row, i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(row, i).Style.Font.Bold = true;
                ws.Cell(row, i).Style.Alignment.WrapText = true;
                i++;
            }

            FormatHeader(ws, row, columnNames.Length);
        }

        private void FormatHeader(IXLWorksheet ws, int headerRow, int countHeaderColumns)
        {
            var row = headerRow;
            var col = countHeaderColumns;

            ws.Range(ws.Cell(row, 1), ws.Cell(row, col)).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            ws.Range(ws.Cell(row, 1), ws.Cell(row, col)).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            ws.Range(ws.Cell(row, 1), ws.Cell(row, col)).Style.Fill.BackgroundColor = XLColor.LightGray;
            ws.Range(ws.Cell(row, 1), ws.Cell(row, col)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(ws.Cell(row, 1), ws.Cell(row, col)).Style.Font.Bold = true;
        }

        private void CreateBody(IXLWorksheet ws, IEnumerable<CompensationResult> compensationResults)
        {
            var row = 3;

            foreach (var compensationResult in compensationResults)
            {
                foreach (var pay in compensationResult.EmployeePayments.Payments)
                {
                    var compensationTimeSheetDay = compensationResult.CompensationByDays[pay.TransactionDateTime.Day];
                    if (compensationTimeSheetDay.Compensation > 0)
                        continue;

                    var col = 1;

                    ws.Cell(row, col++).SetValue(compensationResult.EmployeePayments.Employee.EmployeeNumber);
                    ws.Cell(row, col++).SetValue(compensationResult.EmployeePayments.Employee.FullName);
                    ws.Cell(row, col++).SetValue(compensationResult.EmployeePayments.Employee.Department);

                    ws.Cell(row, col++).SetValue(pay.TransactionDateTime);
                    ws.Cell(row, col++).SetValue(pay.Cost);

                    ws.Cell(row, col++).SetValue(compensationTimeSheetDay.ScheduleOfWork);
                    ws.Cell(row, col).SetValue(compensationTimeSheetDay.Shift);

                    row++;
                }
            }
        }

        private void PostFormatSheet(IXLWorksheet ws)
        {
            var numberLastColumn = ws.LastColumnUsed().ColumnNumber();
            var numberLastRow = ws.LastRowUsed().RowNumber();

            var range = ws.Range(2, 1, numberLastRow, numberLastColumn);
            range.SetAutoFilter();

            range = ws.Range(3, 1, numberLastRow, numberLastColumn);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(2);
        }
    }
}