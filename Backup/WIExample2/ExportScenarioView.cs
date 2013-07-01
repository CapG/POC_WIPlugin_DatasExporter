using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using vrcontext.walkinside.sdk;

namespace WIExample
{
    public partial class ExportScenarioView : VRForm
    {
        string[] array1 = null;

        public ExportScenarioView()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void folderPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.label2.Text = folderBrowserDialog1.SelectedPath;
                array1 = null;
                array1 = Directory.GetFiles(this.label2.Text, "*.7z");
                listView1.Clear();
                foreach (string name in array1)
                {
                    listView1.Items.Add(name.Substring(this.label2.Text.Length + 1));
                }
            }
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            Generate.Enabled = false;

            IVRExportScenario process = new IVRExportScenario();
            process.ExportScenario(array1);

            Generate.Enabled = true;

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ExportScenarioView.ActiveForm.Close();
        }

        private void ExportScenarioView_Load(object sender, EventArgs e)
        {
            this.label2.Text = Resource.ScenarioDirectory;
            array1 = null;
            array1 = Directory.GetFiles(this.label2.Text, "*.7z");
            listView1.Clear();
            foreach (string name in array1)
            {
                listView1.Items.Add(name.Substring(this.label2.Text.Length + 1));
            }
        }
    }
}
