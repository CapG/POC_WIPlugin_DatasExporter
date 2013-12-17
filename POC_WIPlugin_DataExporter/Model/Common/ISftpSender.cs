using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataExporter.Model
{
    public interface ISftpSender
    {
        void Send( string path );
    }
}
