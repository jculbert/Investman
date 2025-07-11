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

namespace Investman
{
    public partial class SymbolsForm : Form
    {
        private readonly HttpClient httpClient = new();

        public SymbolsForm()
        {
            InitializeComponent();

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
                Name = "Name",
                DataPropertyName = "name",
                HeaderText = "Name",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                DataPropertyName = "description",
                FillWeight = 30,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Price",
                DataPropertyName = "last_price",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Price_Date",
                DataPropertyName = "last_price_date",
                HeaderText = "Price Date",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Reviewed_Date",
                DataPropertyName = "reviewed_date",
                HeaderText = "Reviewed Date",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Review_Result",
                DataPropertyName = "review_result",
                HeaderText = "Review Result",
                FillWeight = 15,
            });

            dataGridView1.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "Details",
                FillWeight = 10,
                Text = "...",
                UseColumnTextForLinkValue = true,
            });

            var users = await GetData();
            dataGridView1.DataSource = users;
        }

        private async Task<List<Symbol>> GetData()
        {
            var response = await httpClient.GetAsync("symbols/");

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to retrieve data.");
                return new List<Symbol>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Symbol>>(json);
        }
        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Make sure the click is on the button column and not the header
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                MessageBox.Show($"Button clicked on row {row.Cells["Name"].Value}");
                //MDIParent parent = (MDIParent)MdiParent;
                //parent.ShowHoldings(row.Cells["Name"].Value.ToString());
            }
        }

    }
}
