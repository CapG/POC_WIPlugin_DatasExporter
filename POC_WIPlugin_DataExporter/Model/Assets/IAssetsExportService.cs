using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vrcontext.walkinside.sdk;

namespace DataExporter.Model
{
    public interface IAssetsExportService
    {
        void ExportAllAssets();
        void ExportAsset( IVRBranch branch );
        void ExportAssets( IEnumerable<IVRBranch> branches );
    }
}
