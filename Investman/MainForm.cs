using Investman.Entities;
using Investman.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Investman
{
    public partial class MainForm : Form
    {
        private int childFormNumber = 0;

        public MainForm()
        {
            InitializeComponent();
            toolStripButtonAdd.Visible = false;
            toolStripButtonSave.Visible = false;

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl_DrawItem;
            tabControl.MouseDown += TabControl_MouseDown;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged; // Add this line

            BaseForm childForm = new AccountsForm();
            childForm.mainForm = this;
            childForm.Text = "Accounts";

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            tabControl.TabPages[0].Text = childForm.Text;
            tabControl.TabPages[0].Controls.Add(childForm);

            childForm.Show();
        }

        public void ShowHoldings(string accountName)
        {
            BaseForm childForm = new HoldingsForm(accountName);
            ShowNewForm(childForm);
        }

        public void ShowTransactions(string accountName, string symbolName)
        {
            BaseForm childForm = new TransactionsForm(accountName, symbolName);
            ShowNewForm(childForm);
        }

        public void ShowTransaction(Transaction transaction)
        {
            BaseForm childForm = new TransactionForm(transaction);
            ShowNewForm(childForm);
        }

        public void ShowSymbols()
        {
            BaseForm childForm = new SymbolsForm();
            ShowNewForm(childForm);
        }

        public void ShowUploads()
        {
            BaseForm childForm = new UploadsForm();
            ShowNewForm(childForm);
        }

        public void ShowUpload(int id)
        {
            BaseForm childForm = new UploadForm(id);
            ShowNewForm(childForm);
        }

        public void ShowSymbol(Symbol symbol)
        {
            BaseForm childForm = new SymbolForm(symbol);
            ShowNewForm(childForm);
        }

        private void ShowNewForm(BaseForm childForm)
        {
            childForm.MdiParent = this;
            childForm.mainForm = this;
            childForm.Dock = DockStyle.Fill;

            var tabPage = new TabPage(childForm.Text);
            tabPage.Controls.Add(childForm);
            tabControl.TabPages.Add(tabPage);
            childForm.Show();
            tabControl.SelectedTab = tabPage;
        }

        private void symbolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSymbols();
        }

        private void uploadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUploads();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabRect = tabControl.GetTabRect(e.Index);
            var tabText = tabControl.TabPages[e.Index].Text;

            bool isSelected = (e.Index == tabControl.SelectedIndex);

            // Draw background
            using (Brush backBrush = new SolidBrush(isSelected ? Color.LightSkyBlue : tabControl.BackColor))
            {
                e.Graphics.FillRectangle(backBrush, tabRect);
            }

            // Draw border for selected tab
            if (isSelected)
            {
                using (Pen borderPen = new Pen(Color.DodgerBlue, 2))
                {
                    e.Graphics.DrawRectangle(borderPen, tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
                }
            }

            // Draw tab text (bold if selected)
            Font tabFont = isSelected
                ? new Font(tabControl.Font, FontStyle.Bold)
                : tabControl.Font;
            TextRenderer.DrawText(
                e.Graphics,
                tabText,
                tabFont,
                new Rectangle(tabRect.X + 2, tabRect.Y + 2, tabRect.Width - 20, tabRect.Height - 4),
                tabControl.ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter
            );

            // Draw close button ("X")
            int x = tabRect.Right - 18;
            int y = tabRect.Top + (tabRect.Height - 16) / 2;
            Rectangle closeRect = new Rectangle(x, y, 16, 16);
            e.Graphics.DrawRectangle(Pens.Gray, closeRect);
            e.Graphics.DrawString("X", tabControl.Font, Brushes.Black, x + 2, y + 1);

            // Dispose bold font if created
            if (isSelected)
                tabFont.Dispose();
        }

        private void TabControl_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                Rectangle tabRect = tabControl.GetTabRect(i);
                Rectangle closeRect = new Rectangle(tabRect.Right - 18, tabRect.Top + (tabRect.Height - 16) / 2, 16, 16);

                if (closeRect.Contains(e.Location))
                {
                    tabControl.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected tab page
            var selectedTab = tabControl.SelectedTab;
            if (selectedTab == null || selectedTab.Controls.Count == 0)
            {
                toolStripButtonAdd.Visible = false;
                return;
            }

            // Assume the first control is your child form
            var childForm = selectedTab.Controls[0] as BaseForm;
            if (childForm is UploadsForm || childForm is TransactionsForm)
                toolStripButtonAdd.Visible = true;
            else
                toolStripButtonAdd.Visible = false;

            if (childForm is TransactionForm)
                toolStripButtonSave.Visible = true;
            else
                toolStripButtonSave.Visible = false;
        }
    }
}
