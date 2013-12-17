using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataExporter.Model.Scenarios;
using System.IO;
using System.Xml;

namespace DataExporter.Impl.Scenarios
{
    public class ScenariosSerializer : IScenariosSerializer
    {
        public IDictionary<string, List<string>> DeserializeFromXml( IEnumerable<string> paths )
        {
            //To do :
            //+ Check arguments
            //+ Check xml reading

            Dictionary<string, List<string>> scenarios = new Dictionary<string, List<string>>();

            foreach( string file in paths )
            {
                StreamReader stream = new StreamReader( file );
                
                using( XmlReader reader = XmlReader.Create( stream ) )
                {
                    reader.MoveToContent();
                    while( reader.Read() )
                    {
                        while( reader.ReadToFollowing( "scenario" ) )
                        {
                            string scenarioName = string.Empty;
                            List<string> l_equipmentName = new List<string>();

                            if( reader.NodeType == XmlNodeType.Element )
                            {
                                if( reader.MoveToAttribute( "Name" ) )
                                {
                                    scenarioName = reader.Value;
                                }

                                while( reader.ReadToFollowing( "item" ) )
                                {
                                    if( reader.NodeType == XmlNodeType.Element )
                                    {
                                        if( reader.MoveToAttribute( "BranchName" ) )
                                        {
                                            l_equipmentName.Add( reader.Value );
                                        }
                                    }
                                }
                            }
                            scenarios.Add( scenarioName, l_equipmentName );
                        }
                    }
                    reader.Close();
                    stream.Close();
                }
            }
            return scenarios;
        }

        public void Serialize( IDictionary<string, List<string>> scenarios, StreamWriter file )
        {
            //To do
            //+ Checks

            foreach( var data in scenarios )
            {
                string scenarioName = data.Key;
                for( int i = 0; i < data.Value.Count; i++ )
                {
                    file.WriteLine( String.Format(
                        "{0};{1};{2};{3}",
                        data.Value[i],
                        "VRModelECE",
                        scenarioName,
                        string.Format( Resource.UrlRefScenario, "VRModelECE", scenarioName ) ) );
                }
            }    
        }
    }
}
