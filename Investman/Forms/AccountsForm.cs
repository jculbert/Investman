using Investman.Entities;
using Investman.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net.Http;
using System.Text.Json;

namespace Investman
{
    public partial class AccountsForm : BaseForm
    {
        public AccountsForm()
        {
            InitializeComponent();
            Load += _Load;
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
                FillWeight = 55,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Currency",
                DataPropertyName = "currency",
                HeaderText = "Currency",
                FillWeight = 35,
            });

            dataGridView1.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "Holdings",
                HeaderText = "Holdings",
                FillWeight = 10,
                Text = "...",
                UseColumnTextForLinkValue = true,
            });

            var users = await GetData();
            dataGridView1.DataSource = users;
        }

        private async Task<List<Account>> GetData()
        {
            var response = await httpClient.GetAsync("accounts/");

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to retrieve data.");
                return new List<Account>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Account>>(json);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MainForm parent = (MainForm)MdiParent;
            parent.ShowHoldings("blart");
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Make sure the click is on the button column and not the header
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                //MessageBox.Show($"Button clicked on row {row.Cells["Name"].Value}");
                mainForm.ShowHoldings(row.Cells["Name"].Value.ToString());
            }
        }
    }
}
