using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace NAVObjectCompareWinClient.Helpers
{
    public class DataGridViewHelper
    {
        // Light Blue
        private static Color _colorColumnEqualA = Color.FromArgb(222, 236, 249);
        private static Color _colorColumnNotEqualA = Color.FromArgb(236, 236, 249);

        // Light Green
        private static Color _colorColumnEqualB = Color.FromArgb(222, 249, 236);
        private static Color _colorColumnNotEqualB = Color.FromArgb(236, 249, 236);

        public static void SetRowColors(DataGridView dataGridView, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow dataGridViewRow = dataGridView.Rows[e.RowIndex];
                DataRowView dataRowView = (DataRowView)dataGridViewRow.DataBoundItem;
                bool equal = (bool)dataRowView["Equal"];

                if (e.ColumnIndex > -1)
                    SetColumnColors(equal, dataGridView, dataGridViewRow, e);
            }
        }

        private static void SetColumnColors(bool equal, DataGridView dataGridView, DataGridViewRow dataGridViewRow, DataGridViewCellPaintingEventArgs e)
        {
            Color colorColumnA;
            Color colorColumnB;

            if (equal)
            {
                colorColumnA = _colorColumnEqualA;
                colorColumnB = _colorColumnEqualB;
            }
            else
            {
                colorColumnA = _colorColumnNotEqualA;
                colorColumnB = _colorColumnNotEqualB;
            }

            DataGridViewColumn dataGridViewColumn = dataGridView.Columns[e.ColumnIndex];
            switch (dataGridViewColumn.DataPropertyName)
            {
                case "StringDateA":
                    e.CellStyle.BackColor = colorColumnA;
                    break;
                case "StringTimeA":
                    e.CellStyle.BackColor = colorColumnA;
                    break;
                case "VersionListA":
                    e.CellStyle.BackColor = colorColumnA;
                    break;
                case "NoOfLinesA":
                    e.CellStyle.BackColor = colorColumnA;
                    break;
                case "StringDateB":
                    e.CellStyle.BackColor = colorColumnB;
                    break;
                case "StringTimeB":
                    e.CellStyle.BackColor = colorColumnB;
                    break;
                case "VersionListB":
                    e.CellStyle.BackColor = colorColumnB;
                    break;
                case "NoOfLinesB":
                    e.CellStyle.BackColor = colorColumnB;
                    break;
            }


            if (!equal)
                dataGridViewRow.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;
        }
    }
}
