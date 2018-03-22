using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Group8_Homework5_SDI_Application
{
    public partial class MainForm : Form
    {
        private DocumentFont document;
        private bool needSaving = false;
        private string FileName {get; set;}

        public MainForm()
        {
            InitializeComponent();
            document = new DocumentFont();
            updateTextBox();
            updateStatusLabel();
        }

        private void oathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OathDialog oath = new OathDialog())
            {
                oath.ShowDialog(this);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog about = new AboutDialog();
            about.Show();
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog dlg = new FontDialog())
            {
                dlg.Font = document.Font;
                dlg.ShowColor = true;
                dlg.Color = document.Color;
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    document.Font = dlg.Font;
                    document.Color = dlg.Color;
                    document.Text = this.textBox.Text;
                    updateTextBox();
                    updateStatusLabel();
                }
            }
        }

        private void updateTextBox()
        {
            this.textBox.Font = document.Font;
            this.textBox.ForeColor = document.Color;
            this.textBox.Text = document.Text;
        }

        private void saveDocument()
        {
            document.Font = this.textBox.Font;
            document.Color = this.textBox.ForeColor;
            document.Text = this.textBox.Text;
        }

        public void Serialize(string fileName)
        {
            using (Stream stream =
                new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, document);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream stream =
                new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                IFormatter formatter = new BinaryFormatter();
                document = (DocumentFont)formatter.Deserialize(stream);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Deserialize(dlg.FileName);
                }
            }
            updateTextBox();
            updateStatusLabel();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileName == null)
                saveAsToolStripMenuItem_Click(sender, e);
            else
            {
                saveDocument();
                Serialize(FileName);
            }
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDocument();
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Serialize(FileName = dlg.FileName);
                }
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyToClipboard(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, textBox.SelectedText);
        }

        private void cutFromClipboard(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, textBox.SelectedText);
            textBox.SelectedText = "";
        }

        private void pasteFromClipboard(object sender, EventArgs e)
        {
            textBox.Paste((String)Clipboard.GetData(DataFormats.Text)); 
        }

        private void updateStatusLabel()
        {
            this.toolStripStatusLabel.Text = "Current Font: " + textBox.Font.Size + " " 
                + textBox.Font.Name + " " + textBox.ForeColor.Name;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (needSaving)
            {
                DialogResult confirmExit = MessageBox.Show("You have unsaved changes. Do you want to save them before quitting?", "Exit Application?", MessageBoxButtons.YesNoCancel);

               //e.Cancel = (confirmExit == DialogResult.Cancel);

                if(confirmExit == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }

                else if(confirmExit == DialogResult.No)
                {
                    //do nothing and go quietly
                }
                
                else
                {
                    //User made mistake and wants to cancel
                    e.Cancel = true;
                }
            }
            else
            {
                DialogResult confirmExit = MessageBox.Show("Would you like to quit?", "Exit Application?", MessageBoxButtons.OKCancel);

                e.Cancel = (confirmExit == DialogResult.Cancel);
            }                
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            needSaving = true;
        }

        private void createNewFile() {

            MainForm newMainForm = new MainForm();
            newMainForm.Show();

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createNewFile();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            createNewFile();
        }
    }
}
