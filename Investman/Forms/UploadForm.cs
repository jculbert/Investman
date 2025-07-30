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
    public partial class UploadForm : BaseForm
    {
        private readonly int id;
        private Upload upload;

        public UploadForm(int id)
        {
            this.id = id;
            InitializeComponent();
            Load += _Load;
        }

        private async Task<Upload> GetData()
        {
            var response = await httpClient.GetAsync("uploads/" + id + "/");

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to retrieve data.");
                return new Upload();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Upload>(json);
        }

        private async void _Load(object? sender, EventArgs e)
        {
            upload = await GetData();
            upload.content = upload.content.Replace("\n", "\r\n"); // Need this for the textbox

            var properties = typeof(Upload).GetProperties();
            tableLayoutPanel.RowCount = 2 * properties.Length;
            tableLayoutPanel.ColumnCount = 1;
            //tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //tableLayoutPanel.Padding = new Padding(10);

            Text = "Upload: " + id;

            int row = 0;
            foreach (var prop in properties)
            {
                if (prop.Name == "id")
                    continue; // Skip the 'id' property

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

                if (prop.Name == "content")
                {
                    textBox.Multiline = true;
                    textBox.ScrollBars = ScrollBars.Both;
                    textBox.WordWrap = false;
                    textBox.Height = 300; // Fixed height for multiline text box
                    textBox.Width = 700; // Fixed height for multiline text box
                }
                else if (prop.Name == "notes")
                {
                    textBox.Multiline = true;
                    textBox.ScrollBars = ScrollBars.Both;
                    textBox.WordWrap = false;
                    textBox.Height = 100; // Fixed height for multiline text box
                    textBox.Width = 700; // Fixed height for multiline text box
                }
                else
                {
                    textBox.Multiline = false;
                    textBox.Width = 125;
                }

                // Data binding (optional, for two-way binding)
                textBox.DataBindings.Add("Text", upload, prop.Name, true, DataSourceUpdateMode.OnPropertyChanged);

                tableLayoutPanel.Controls.Add(label, 0, row++);
                tableLayoutPanel.Controls.Add(textBox, 0, row++);
            }
        }
    }
}
