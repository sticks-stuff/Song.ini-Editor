using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                //DataTable dt = Form1.dt;
                DataTable dt2 = Form1.dt;
                if (dt2.Columns.Contains("File name") == false)
                {
                    dt2.Columns.Add("File name");
                }
                Console.WriteLine(openFileDialog1.FileNames[0]);
                Console.WriteLine(openFileDialog1.FileNames[1]);
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
                            if (data[0].Contains("=")) {
                                var inputName = data[0].Split(" = ".ToCharArray()).First();
                                var inputData = data[0].Substring(data[0].LastIndexOf("=") + 1);
                                Console.WriteLine(inputName);
                                if (dt2.Columns.Contains(inputName) == false)
                                {
                                    dt2.Columns.Add(inputName);
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
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }
    }
}
