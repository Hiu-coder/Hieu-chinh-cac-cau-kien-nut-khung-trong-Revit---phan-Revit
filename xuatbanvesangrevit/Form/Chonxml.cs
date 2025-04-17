using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xuatbanvesangrevit
{
    public partial class Chonxml: System.Windows.Forms.Form
    {
        
        public Chonxml()
        {
            InitializeComponent();
        }
        //public static string filePath;
        public void btnXml_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //filePath = dlg.FileName;              
            }

        }
    }
}
