using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using vrcontext.walkinside.sdk;

namespace DataExporter.Model
{
    public interface IBranchesSerializer
    {
        void Serialize( StreamWriter file );
        void Serialize( IVRBranch branch, StreamWriter file );
        void Serialize( IEnumerable<IVRBranch> branches, StreamWriter file );
    }
}
