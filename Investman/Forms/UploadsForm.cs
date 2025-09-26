using Investman.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Investman.Forms
{
    public partial class UploadsForm : BaseForm, IAddable
    {
        private BindingList<Upload>? uploads;

        public UploadsForm()
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
                Name = "ID",
                DataPropertyName = "id",
                FillWeight = 20,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                DataPropertyName = "date",
                FillWeight = 20,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "File_Name",
                DataPropertyName = "file_name",
                HeaderText = "File",
                FillWeight = 30,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Num_Transactions",
                DataPropertyName = "num_transactions",
                HeaderText = "Num Trans",
                FillWeight = 20,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Result",
                DataPropertyName = "result",
                FillWeight = 10,
            });

            dataGridView1.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "Content",
                FillWeight = 10,
                Text = "...",
                UseColumnTextForLinkValue = true,
            });

            uploads = await GetData();
            dataGridView1.DataSource = uploads;
        }

        private async Task<BindingList<Upload>> GetData()
        {
            var response = await httpClient.GetAsync("uploads/");

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to retrieve data.");
                return new BindingList<Upload>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BindingList<Upload>>(json);
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Make sure the click is on the button column and not the header
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                //var row = dataGridView1.Rows[e.RowIndex];
                //MessageBox.Show($"Button clicked on row {row.Cells["Name"].Value}");
                mainForm.ShowUpload(uploads[e.RowIndex].id);
            }
        }

        public async void Add()
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                Title = "Select a file to upload"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog.FileName;
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileContent, "file", Path.GetFileName(filePath));

                // Add other form fields if needed:
                // content.Add(new StringContent("value"), "fieldName");

                var response = await httpClient.PostAsync("uploads/", content);
                if (response.IsSuccessStatusCode)
                {
                    //MessageBox.Show("File uploaded successfully.");
                }
                else
                {
                    MessageBox.Show("File upload failed.");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var upload = JsonSerializer.Deserialize<Upload>(json);
                uploads.Add(upload);

                this.mainForm.ShowUpload(upload.id);
            }
        }
    }
}
