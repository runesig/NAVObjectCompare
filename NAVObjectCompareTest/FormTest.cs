using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAVObjectCompare;

namespace NAVObjectCompareTest
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            NAVObjectCompare.Compare compare = new Compare(@"C:\temp\Objects\VehicleAreaA.txt", @"C:\temp\Objects\VehicleAreaB.txt");
            compare.RunCompare();
        }
    }
}
