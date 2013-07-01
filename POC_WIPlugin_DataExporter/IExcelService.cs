using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vrcontext.walkinside.sdk;
using System.IO;

namespace DataExporter
{
    public interface IExcelService
    {
        StreamWriter CreateFile( string path, string columns );
        void CloseFile( StreamWriter file );
        void WriteAssets( IVRBranch root, ref StreamWriter file );
        void WriteScenario( Dictionary<string, List<string>> l_scenario, ref StreamWriter file );
    }
}
