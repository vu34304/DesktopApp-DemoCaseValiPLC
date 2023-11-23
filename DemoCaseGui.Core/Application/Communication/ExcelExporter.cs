using DemoCaseGui.Core.Application.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Communication
{
    public class ExcelExporter : IExcelExporter
    {
        private const string reportTemplatePath = @"./ReportTemplate.xlsx";
        private const int reportTemplateStartRow = 4;

        public void ExportReport(string filePath, IEnumerable<FilterEntry> filters)
        {
            XSSFWorkbook workBook;
            using (FileStream file = new FileStream(reportTemplatePath, FileMode.Open, FileAccess.Read))
            {
                workBook = new XSSFWorkbook(file);
            }

            ISheet sheet = workBook.GetSheet("Sheet1");
            var orderedFilter = filters.OrderBy(x => x.Time).ToList();

            var currentRowIndex = reportTemplateStartRow;
            var currentRowCount = 1;
            
            XSSFCellStyle defaultStyle = (XSSFCellStyle)workBook.CreateCellStyle();
            defaultStyle.WrapText = true;
            defaultStyle.Alignment = HorizontalAlignment.Left;
            defaultStyle.VerticalAlignment = VerticalAlignment.Bottom;
            defaultStyle.BorderBottom = BorderStyle.Thin;
            defaultStyle.BorderTop = BorderStyle.Thin;
            defaultStyle.BorderLeft = BorderStyle.Thin;
            defaultStyle.BorderRight = BorderStyle.Thin;

            foreach (var filter in orderedFilter)
            {
                var currentRow = sheet.GetRow(currentRowIndex);
                if (currentRow == null)
                {
                    sheet.CreateRow(currentRowIndex);
                    currentRow = sheet.GetRow(currentRowIndex);
                    var cell0 = currentRow.CreateCell(0); cell0.CellStyle = defaultStyle;
                    var cell1 = currentRow.CreateCell(1); cell1.CellStyle = defaultStyle;
                    var cell2 = currentRow.CreateCell(2); cell2.CellStyle = defaultStyle;
                    var cell3 = currentRow.CreateCell(3); cell3.CellStyle = defaultStyle;
                }
                currentRow.GetCell(0).SetCellValue(currentRowCount);
                currentRow.GetCell(1).SetCellValue(filter.Name);
                currentRow.GetCell(2).SetCellValue(filter.Value);
                currentRow.GetCell(3).SetCellValue(filter.Time.ToString("dd-MM-yyyy hh:mm:ss"));
                currentRowCount++;
                currentRowIndex++;
            }

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workBook.Write(fs);
            }
        }
    }
}
