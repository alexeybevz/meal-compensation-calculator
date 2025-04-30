using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using MealCompensationCalculator.Domain.Commands;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.BusinessLogic.Commands
{
    public class CreateEmployeeSummaryReportToExcelCommand : ICreateEmployeeSummaryReportCommand
    {
        private readonly MealCompensation _dayEveningCompensation;

        public CreateEmployeeSummaryReportToExcelCommand(MealCompensation dayEveningCompensation)
        {
            _dayEveningCompensation = dayEveningCompensation;
        }

        public async Task Execute(string pathToXlsxFile, DateTime startPeriod, DateTime endPeriod, IEnumerable<CompensationResult> compensationResults)
        {
            await Task.Run(() =>
            {
                CreateReport(pathToXlsxFile, startPeriod, endPeriod, compensationResults);
            });
        }

        private void CreateReport(string pathToXlsxFile, DateTime startPeriod, DateTime endPeriod, IEnumerable<CompensationResult> compensationResults)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Style.Font.FontName = "Calibri";
                workbook.Style.Font.FontSize = 11;

                var ws = workbook.Worksheets.Add("Сводный отчет");

                CreateWs(ws, startPeriod, endPeriod, compensationResults);

                workbook.SaveAs(pathToXlsxFile);
            }
        }

        private void CreateWs(IXLWorksheet ws, DateTime startPeriod, DateTime endPeriod, IEnumerable<CompensationResult> compensationResults)
        {
            var columnNames = new[]
            {
                "№ п/п",
                "Таб. №",
                "Сотрудники",
                "Подразделение",
                "Стоимость питания",
                "Компенсация за период",
                "К уплате за период",
                "Наличный расчет за период",
                "Дополнительная дотация"
            };

            CreateTitle(ws, startPeriod, endPeriod, columnNames.Length);
            CreateHeader(ws, columnNames);
            CreateBody(ws, compensationResults, columnNames.Length);
            PostFormatSheet(ws);
        }

        private void CreateTitle(IXLWorksheet ws, DateTime startPeriod, DateTime endPeriod, int countColumns)
        {
            var range = ws.Range(ws.Cell(1, 1), ws.Cell(1, countColumns));
            range.Merge();
            range.Style.Font.Bold = true;
            range.Style.Font.FontSize = 12;
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell(1, 1).SetValue($"ИП Валиев Р.И. - сводный отчет ({startPeriod:dd.MM.yyyy} - {endPeriod:dd.MM.yyyy}), подготовлен: {DateTime.Now:dd.MM.yyyy HH:mm:ss}, денежная единица - р.");
        }

        private void CreateBody(IXLWorksheet ws, IEnumerable<CompensationResult> compensationResults, int countColumns)
        {
            var seq = 1;
            var row = 4;

            decimal totalCompensation = 0;
            decimal totalCost = 0;
            decimal totalCostCash = 0;

            foreach (var compensationResult in compensationResults)
            {
                var col = 1;

                ws.Cell(row, col++).Value = seq++;
                ws.Cell(row, col++).Value = compensationResult.EmployeePayments.Employee.EmployeeNumber;
                ws.Cell(row, col++).Value = compensationResult.EmployeePayments.Employee.FullName;
                ws.Cell(row, col++).Value = compensationResult.EmployeePayments.Employee.Department;

                var cost = compensationResult.EmployeePayments.Payments.Sum(x => x.Cost);
                totalCost += cost;
                ws.Cell(row, col++).Value = cost;

                totalCompensation += compensationResult.TotalCompensation;
                ws.Cell(row, col++).Value = compensationResult.TotalCompensation;
                col++;

                var costCash = compensationResult.EmployeePayments.Payments.Sum(x => x.CostCash);
                totalCostCash += costCash;
                ws.Cell(row, col).Value = costCash;

                var isExistsAtLeastOneEveningVisit = compensationResult.EmployeePayments.Payments.Any(x =>
                    _dayEveningCompensation.IsDateFallsToCompensationPeriod(x.TransactionDateTime));

                if (isExistsAtLeastOneEveningVisit)
                    ws.Range(ws.Cell(row, 1), ws.Cell(row, countColumns)).Style.Fill.BackgroundColor = XLColor.LightBlue;

                row++;
            }

            ws.Cell(row, 3).Style.Font.Bold = true;
            ws.Cell(row, 3).Value = "Итого";

            ws.Cell(row, 5).Style.Font.Bold = true;
            ws.Cell(row, 5).Value = totalCost;

            ws.Cell(row, 6).Style.Font.Bold = true;
            ws.Cell(row, 6).Value = totalCompensation;

            ws.Cell(row, 8).Style.Font.Bold = true;
            ws.Cell(row, 8).Value = totalCostCash;

            ws.Range(ws.Cell(row, 1), ws.Cell(row, countColumns)).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            ws.Range(ws.Cell(row, 1), ws.Cell(row, countColumns)).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
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

        private void CreateHeader(IXLWorksheet ws, string[] columnNames)
        {
            var row = 3;
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

        private void PostFormatSheet(IXLWorksheet ws)
        {
            var numberLastColumn = ws.LastColumnUsed().ColumnNumber();
            var numberLastRow = ws.LastRowUsed().RowNumber();

            var range = ws.Range(3, 1, numberLastRow, numberLastColumn);
            range.SetAutoFilter();

            range = ws.Range(4, 1, numberLastRow, numberLastColumn);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(3);

            ws.Column(5).Style.NumberFormat.SetFormat("#,##0.00");
            ws.Column(6).Style.NumberFormat.SetFormat("#,##0.00");
            ws.Column(8).Style.NumberFormat.SetFormat("#,##0.00");
        }
    }
}