using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Investman.Forms
{
    public partial class BaseForm : Form
    {
        protected readonly HttpClient httpClient = new();

        public BaseForm()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri(Properties.Settings.Default.BaseURL);

        }
    }
}
