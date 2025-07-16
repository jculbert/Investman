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
using System.Transactions;
using System.Windows.Forms;
using Transaction = Investman.Entities.Transaction;

namespace Investman.Forms
{
    public partial class TransactionsForm : BaseForm
    {
        private readonly string accountName;
        private readonly string symbolName;
        private BindingList<Transaction> transactions;

        public TransactionsForm(string _accountName, string _symbolName)
        {
            InitializeComponent();
            accountName = _accountName;
            symbolName = _symbolName;
            Load += _Load;
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

        private async Task<BindingList<Transaction>> GetData()
        {
            Uri uri = new Uri("transactions/?account=" + Uri.EscapeDataString(accountName)
                + "&symbol=" + Uri.EscapeDataString(symbolName), UriKind.Relative);
            var response = await httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to retrieve data.");
                return new BindingList<Transaction>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BindingList<Transaction>>(json);
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                Transaction transaction = transactions[e.RowIndex];
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
                {
                    // Handle the link click, show transaction details
                    mainForm.ShowTransaction(transaction);
                }
                else if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    // Handle the delete click
                    DeleteTransaction(transaction);
                }
            }
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(accountName, symbolName);

            var response = await httpClient.PostAsync($"transactions/",
                new StringContent(JsonSerializer.Serialize(transaction), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to create transaction.");
                return; // Ensure all code paths return a value
            }

            //MessageBox.Show("Transaction created successfully."); // Optional success message

            var json = await response.Content.ReadAsStringAsync();
            transaction = JsonSerializer.Deserialize<Transaction>(json);
            transactions.Add(transaction);

            MainForm parent = (MainForm)MdiParent;
            parent.ShowTransaction(transaction);
        }

        private async void DeleteTransaction(Transaction transaction)
        {
            var result = MessageBox.Show("Delete transaction?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            var response = await httpClient.DeleteAsync($"transactions/{transaction.id}/");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to delete transaction.");
                return; // Ensure all code paths return a value
            }

            //MessageBox.Show("Transaction deleted successfully."); // Optional success message
            // Refresh the data grid view
            //transactions = await GetData();
            //dataGridView1.DataSource = transactions;
            transactions.Remove(transaction);
        }
    }
}
