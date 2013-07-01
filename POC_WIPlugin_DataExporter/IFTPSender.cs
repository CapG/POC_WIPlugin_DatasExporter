using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataExporter
{
    public interface IFTPSender
    {
        void Send( string sendedFilePath, string newFileName );
    }
}
