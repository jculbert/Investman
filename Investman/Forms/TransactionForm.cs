using Investman.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace Investman.Forms
{
    public partial class TransactionForm : BaseForm
    {
        private readonly Investman.Entities.Transaction transaction;

        public TransactionForm(Investman.Entities.Transaction _transaction)
        {
            transaction = _transaction;
            InitializeComponent();
            Text = "Transaction: " + transaction.account + " / " + transaction.symbol + " / " + transaction.id;

            CreateTransactionFields();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CreateTransactionFields()
        {
            var properties = typeof(Investman.Entities.Transaction).GetProperties();
            tableLayoutPanel.RowCount = 2 * properties.Length;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel.Padding = new Padding(10);

            int row = 0;
            foreach (var prop in properties)
            {
                // Don't show some fields 
                if (prop.Name == "id" || prop.Name == "account" || prop.Name == "symbol")
                    continue;

                // Create label
                var label = new Label
                {
                    Text = prop.Name,
                    Anchor = AnchorStyles.Left,
                    AutoSize = true,
                    Font = new Font(Font.FontFamily, 8), // Fixed: Create a new Font object with the desired size
                };

                // Create textbox
                var textBox = new TextBox
                {
                    Name = "textBox" + prop.Name,
                    //Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Anchor = AnchorStyles.Left,

                    Font = new Font(Font.FontFamily, 8),
                };

                if (prop.Name == "note")
                {
                    textBox.Multiline = true;
                    textBox.Height = 100; // Fixed height for multiline text box
                    textBox.Width = 300; // Fixed height for multiline text box
                }
                else
                {
                    textBox.Multiline = false;
                    textBox.Width = 125;
                }

                // Data binding (optional, for two-way binding)
                textBox.DataBindings.Add("Text", transaction, prop.Name, true, DataSourceUpdateMode.OnPropertyChanged);

                tableLayoutPanel.Controls.Add(label, 0, row++);
                tableLayoutPanel.Controls.Add(textBox, 0, row++);
            }

            // Add Save button
            //tableLayoutPanel.Controls.Add(buttonSave, 0, row++);
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            await PutTransaction();
        }

        private async Task PutTransaction()
        {
            var response = await httpClient.PutAsync($"transactions/{transaction.id}/",
                new StringContent(JsonSerializer.Serialize(transaction), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to save data.");
                return; // Ensure all code paths return a value
            }

            MessageBox.Show("Transaction updated successfully."); // Optional success message
            Close();
        }
    }
}
