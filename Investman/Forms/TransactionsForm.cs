using Investman.Entities;
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

namespace Investman.Forms
{
    public partial class TransactionsForm : Form
    {
        private readonly HttpClient httpClient = new();
        private readonly string accountName;
        private readonly string symbolName;
        private List<Transaction> transactions;

        public TransactionsForm(string _accountName, string _symbolName)
        {
            InitializeComponent();
            accountName = _accountName;
            symbolName = _symbolName;
            Load += _Load;

            httpClient.BaseAddress = new Uri(Properties.Settings.Default.BaseURL);
        }
        private async void _Load(object? sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

              dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                DataPropertyName = "date",
                FillWeight = 12,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Type",
                DataPropertyName = "type",
                FillWeight = 8,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                DataPropertyName = "quantity",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Price",
                DataPropertyName = "price",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Amount",
                DataPropertyName = "amount",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Capital_Return",
                DataPropertyName = "capital_return",
                HeaderText = "Cap Ret",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Capital_Gain",
                DataPropertyName = "capital_gain",
                HeaderText = "Cap Gain",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ACB",
                DataPropertyName = "acb",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Delete",
                FillWeight = 10,
                Text = "Del",
                UseColumnTextForButtonValue = true,
            });

            dataGridView1.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "Details",
                FillWeight = 10,
                Text = "...",
                UseColumnTextForLinkValue = true,
            });

            transactions = await GetData();
            dataGridView1.DataSource = transactions;
        }

        private async Task<List<Transaction>> GetData()
        {
            Uri uri = new Uri("transactions/?account=" + Uri.EscapeDataString(accountName)
                + "&symbol=" + Uri.EscapeDataString(symbolName), UriKind.Relative);
            var response = await httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to retrieve data.");
                return new List<Transaction>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Transaction>>(json);
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                //MessageBox.Show($"Button clicked on row {row.Cells["Name"].Value}");
                MDIParent parent = (MDIParent)MdiParent;
                parent.ShowTransaction(transactions[e.RowIndex]);
            }
        }
    }
}
