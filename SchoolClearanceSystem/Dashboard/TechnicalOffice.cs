using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class TechnicalOffice : BaseOfficeForm
    {
        public TechnicalOffice()
        {
            InitializeComponent();
            this.CurrentOffice = "Technical Office";
        }

        private void TechnicalOffice_Load(object sender, EventArgs e) => LoadGridData();
    }
        

        }
    
