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

namespace DataExporter
{
    public partial class ExportScenarioView : VRForm
    {
        private string[] _exportList;

        public ExportScenarioView()
        {
            InitializeComponent();
        }

        private void folderPath_Click( object sender, EventArgs e )
        {
            if( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                _exportList = Directory.GetFiles( folderBrowserDialog1.SelectedPath, "*.7z" );
                listView1.Clear();
                foreach( string name in _exportList )
                {
                    listView1.Items.Add( name.Substring( folderBrowserDialog1.SelectedPath.Length + 1 ) );
                }
            }
        }

        private void generate_Click( object sender, EventArgs e )
        {
            Generate.Enabled = false;

            ScenarioExporter process = new ScenarioExporter();

            string[] s = null;
            for( int i = 0; i < listView1.Items.Count; i++ )
                s[i] = listView1.Items[i].Text;

            process.ExportScenario( s );

            Generate.Enabled = true;
        }

        private void cancel_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void exportScenarioView_Load( object sender, EventArgs e )
        {
            _exportList = Directory.GetFiles( Resource.ScenarioDirectory, "*.7z" );
            listView1.Clear();
            foreach( string name in _exportList )
            {
                listView1.Items.Add( name.Substring( Resource.ScenarioDirectory.Length + 1 ) );
            }
        }
    }
}
