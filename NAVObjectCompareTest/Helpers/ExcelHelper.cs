using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace NAVObjectCompareWinClient.Helpers
{
    public class ExcelHelper
    {
        public static int _startCol = 1;
        public static int _startRow = 1;

        public static void ExportDataGridToExcel(DataGridView dataGridView)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

                int startCol = _startCol;
                int startRow = _startRow;

                WriteHeaders(dataGridView, worksheet, startCol, ref startRow);

                WriteContent(dataGridView, worksheet, startCol, startRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static void WriteContent(DataGridView dataGridView, Excel.Worksheet worksheet, int startCol, int startRow)
        {
            // Write DataGridView Content
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[startRow + i, startCol + j];
                    myRange.Value2 = dataGridView[j, i].Value == null ? string.Empty : dataGridView[j, i].Value;
                }
            }
        }

        private static void WriteHeaders(DataGridView dataGridView, Excel.Worksheet worksheet, int startCol, ref int startRow)
        {
            //Write Headers
            for (int j = 0; j < dataGridView.Columns.Count; j++)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[startRow, startCol + j];
                myRange.Value2 = dataGridView.Columns[j].HeaderText;
            }

            startRow++;
        }
    }
}
