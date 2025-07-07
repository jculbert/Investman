using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Investman
{
    public partial class HoldingsForm : Form
    {
        private readonly HttpClient httpClient = new();
        private readonly string accountName;

        public HoldingsForm(string _accountName)
        {
            InitializeComponent();
            accountName = _accountName;
            Load += _Load;

            httpClient.BaseAddress = new Uri(Properties.Settings.Default.BaseURL);
        }
        private async void _Load(object? sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            dataGridView1.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "Symbol",
                DataPropertyName = "symbol_name",
                HeaderText = "Symbol",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                DataPropertyName = "symbol_description",
                HeaderText = "Description",
                FillWeight = 20,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                DataPropertyName = "quantity",
                HeaderText = "Quantity",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Amount",
                DataPropertyName = "amount",
                HeaderText = "Amount",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Reviewed Date",
                DataPropertyName = "symbol_reviewed_date",
                HeaderText = "Reviewed Date",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Reviewed Result",
                DataPropertyName = "symbol_review_result",
                HeaderText = "Reviewed Result",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "Transactions",
                HeaderText = "Transactions",
                FillWeight = 10,
                Text = "...",
                UseColumnTextForLinkValue = true,
            });

            List<Holding> holdings = await GetData();
            // Flatten the list of Holding into a list of HoldingView as required by the DataGridView
            var flatholdings = holdings.Select(c => new HoldingView(c)).ToList();
            dataGridView1.DataSource = flatholdings;
        }

        private async Task<List<Holding>> GetData()
        {
            Uri uri = new Uri("holdings/?account=" + Uri.EscapeDataString(accountName), UriKind.Relative);
            var response = await httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to retrieve data.");
                return new List<Holding>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Holding>>(json);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure the click is on the button column and not the header
            //if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            //{
                //var row = dataGridView1.Rows[e.RowIndex];
                //MessageBox.Show($"Button clicked on row {row.Cells["Name"].Value}");
                //MDIParent parent = (MDIParent)MdiParent;
                //parent.ShowHoldings(row.Cells["Name"].Value.ToString());
            //}
        }
    }
}
