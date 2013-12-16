using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using vrcontext.walkinside.sdk;

namespace DataExporter
{
    public class AvevaExcelService : IExcelService
    {
        public void WriteAssets(IVRBranch root, ref StreamWriter file)
        {
            DataExporter.CurrentViewer.UI.SetPercentage((int) root.ID / DataExporter.NbBranches);
            DataExporter.CurrentViewer.UI.ShowInformation(string.Format(Resource.ExportBranch, root.Name));

            string name = root.Name;
            if( name.StartsWith( "/" ) )
                name = name.Substring( 1 );

            file.Write( String.Format("{0};{1};{2};{3};{4};{5}\r\n",
                name,
                root.Project.Name,
                root.OBB.Position.X.ToString(),
                root.OBB.Position.Y.ToString(),
                root.OBB.Position.Z.ToString(),
                string.Format( Resource.UrlRefTag, root.Project.Name, name )
                ));

            foreach( IVRBranch branch in root.Children )
            {
                WriteAssets( branch, ref file );
            }
        }

        public void WriteScenario( Dictionary<string, List<string>> l_scenario, ref StreamWriter file )
        {
            foreach( var data in l_scenario )
            {
                string scenarioName = data.Key;
                for( int i = 0; i < data.Value.Count; i++ )
                {
                    file.Write( "{0};{1};{2};{3}\r\n",
                        data.Value[i],
                        "VRModelECE",
                        scenarioName,
                        string.Format( Resource.UrlRefScenario, "VRModelECE", scenarioName ) );
                }
            }    
        }

        public StreamWriter CreateFile( string path, string columns )
        {
            if( File.Exists( path ) )
            {
                try { File.Delete( path ); }
                catch( Exception e ) { DataExporter.CurrentViewer.UI.StatusMessage.Text = e.Message; }
            }

            StreamWriter csvFile = new StreamWriter( path, true );
            csvFile.Write( columns );
            csvFile.Write( "\r\n" );

            return csvFile;
        }

        public void CloseFile( StreamWriter file )
        {
            try
            {
                file.Flush();
                file.Close();
            }
            catch( Exception e ) { DataExporter.CurrentViewer.UI.StatusMessage.Text = e.Message; }
        }
    }
}
