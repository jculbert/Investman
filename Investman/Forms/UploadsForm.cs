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
    public partial class UploadsForm : Form
    {
        private readonly HttpClient httpClient = new();
        private BindingList<Upload> uploads;

        public UploadsForm()
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
                MDIParent parent = (MDIParent)MdiParent;
                parent.ShowUpload(uploads[e.RowIndex].id);
            }
        }
    }
}
