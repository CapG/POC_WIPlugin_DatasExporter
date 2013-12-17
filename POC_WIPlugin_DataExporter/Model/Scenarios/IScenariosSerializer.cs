using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataExporter.Model.Scenarios
{
    public interface IScenariosSerializer
    {
        IDictionary<string, List<string>> DeserializeFromXml( IEnumerable<string> paths );
        void Serialize( IDictionary<string, List<string>> scenarios, StreamWriter file );
    }
}
