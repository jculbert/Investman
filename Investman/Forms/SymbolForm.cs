using Investman.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
//using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Investman.Forms
{
    public partial class SymbolForm : Form
    {
        private readonly Symbol symbol;
        private readonly HttpClient httpClient = new();

        public SymbolForm(Symbol symbol)
        {
            this.symbol = symbol;
            InitializeComponent();
            httpClient.BaseAddress = new Uri(Properties.Settings.Default.BaseURL);

            labelTitle.Text = "Symbol: " + symbol.name;

            CreateSymbolFields();

        }
        private void CreateSymbolFields()
        {
            var properties = typeof(Symbol).GetProperties();
            tableLayoutPanel.RowCount = 2 * properties.Length;
            tableLayoutPanel.ColumnCount = 2;
            //tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //tableLayoutPanel.Padding = new Padding(10);

            int row = 0;
            foreach (var prop in properties)
            {
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

                if (prop.Name == "notes")
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
                textBox.DataBindings.Add("Text", symbol, prop.Name, true, DataSourceUpdateMode.OnPropertyChanged);

                tableLayoutPanel.Controls.Add(label, 0, row++);
                tableLayoutPanel.Controls.Add(textBox, 0, row++);
            }

            // Add Save button
            tableLayoutPanel.Controls.Add(buttonSave, 0, row++);
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            //var response = await httpClient.PutAsJsonAsync($"symbols/{symbol.name}/", symbol);
            var response = await httpClient.PutAsync($"symbols/{symbol.name}/",
                new StringContent(JsonSerializer.Serialize(symbol), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Symbol updated successfully.");
                Close();
            }
            else
            {
                MessageBox.Show("Failed to update symbol: " + response.ReasonPhrase);
            }
        }
    }
}
