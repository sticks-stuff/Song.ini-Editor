﻿using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Song.ini_Editor
{
    public partial class Form1 : Form
    {

        public static DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = -1;
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Filter = "Song.ini Files|song.ini|All files (*.*)|*.*";
            this.openFileDialog1.FileName = "song.ini";

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                //DataTable dt = Form1.dt;
                DataTable dt2 = Form1.dt;
                if (dt2.Columns.Contains("File name") == false)
                {
                    dt2.Columns.Add("File name");
                    dt2.Columns[0].ReadOnly = true;
                }
                try
                {
                    foreach (string file in openFileDialog1.FileNames) 
                    { 
                        dt2.Rows.Add(file);

                        string text = File.ReadAllText(file);
                        size = text.Length;
                        foreach (string line in File.ReadAllLines(file))
                        {
                            string[] data = line.Split('\n');
                            //Console.WriteLine(data[0]);
                            if (data[0].Contains("=")) 
                            {
                                var inputName = data[0].Split(" = ".ToCharArray()).First();
                                var inputData = data[0].Substring(data[0].IndexOf("=", data[0].IndexOf("=")) + 1);
                                Console.WriteLine(inputName);
                                if (dt2.Columns.Contains(inputName) == false)
                                {
                                    dt2.Columns.Add(inputName.Trim());
                                }
                                Console.WriteLine(inputData);
                                dt2.Rows[dt2.Rows.Count - 1][inputName] = inputData.Trim();
                            }
                        }
                    }
                }
                catch (IOException)
                {
                }
                dataGridView1.DataSource = dt2;
            }
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Shift)
            {
                dataGridView1.ClearSelection();
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, 0];
                dataGridView1[e.ColumnIndex, i].Selected = true;
            }
        }

        private int _keyValue;
        private Boolean _checkKeyValue = false;
        private int value;

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (_checkKeyValue)
            {
                _checkKeyValue = false;

                if (value != -1)
                {
                    cell.Value = _keyValue;
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 1)
            {
                _checkKeyValue = true;
                _keyValue = (int)e.KeyValue;
                dataGridView1.BeginEdit(false);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            foreach (DataGridViewCell cells in dataGridView1.SelectedCells)
            {
                dataGridView1.Rows[cells.RowIndex].Cells[cells.ColumnIndex].Value = cell.Value;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            const string message = "WARNING!!\nTHIS WILL EDIT ALL SONG.INIS YOU HAVE IMPORTED\nAS THIS IS A PRERELEASE BUILD THERE MAY BE BUGS THAT WILL BREAK ALL YOUR SONG.INIS\nTHERE IS NO WAY TO UNDO!!!\nDo you wish to continue?";
            const string caption = "WARNING!!";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                DataTable dt2 = Form1.dt;
                Console.WriteLine("asdasdasd.");
                for (int count = 0; count < dataGridView1.Rows.Count; count++)
                {
                    string fileName = dataGridView1.Rows[count].Cells[0].Value.ToString();
                    Console.WriteLine(fileName);
                    var newFile = new StringBuilder();

                    string[] file = File.ReadAllLines(fileName);
                    newFile.Append("[Song]" + "\r\n");
                    foreach (string line in file)
                    {
                        string[] data = line.Split('\n');
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            string header = dataGridView1.Columns[i].HeaderText;
                            if (data[0].Contains(header))
                            {
                                string toReplace = dt2.Rows[count][header].ToString();
                                //Console.WriteLine("\"" + toReplace.Trim() + "\"");
                                //Console.WriteLine("\"" + data[0].Substring(data[0].IndexOf(" = ", data[0].IndexOf(" = ")) + 2).Trim() + "\"");
                                if (data[0].Substring(data[0].IndexOf("=", data[0].IndexOf("=")) + 1).Trim() != toReplace.Trim())
                                {
                                    Console.WriteLine("Replacing " + (data[0].Substring(data[0].IndexOf("=", data[0].IndexOf("=")) + 1)) + " in " + header + " with " + toReplace);
                                    newFile.Append(header + " = " + toReplace.Trim() + "\r\n");
                                    continue;
                                }
                                else 
                                { 
                                    newFile.Append(line + "\r\n"); 
                                }
                            }
                        }
                    }
                    File.WriteAllText(fileName, newFile.ToString());
                }
            }
        }
    }
}
