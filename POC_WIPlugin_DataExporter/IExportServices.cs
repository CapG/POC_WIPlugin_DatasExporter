using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataExporter
{
    public interface IExportServices
    {
        void ExportAssets();
        void ExportScenarios( string[] scenarios );
    }
}
