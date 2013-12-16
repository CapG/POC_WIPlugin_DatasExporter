using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.IO;

namespace DataExporter
{
    public class AvevaFTPSender : IFTPSender
    {
        private SftpClient _client;

        public AvevaFTPSender()
        {
            _client = new SftpClient( "194.2.93.194", "Administrateur", "AdminPOCNUC01" );
        }

        public void Send( string sendedFilePath, string newFileName )
        {
            _client.Connect();
            using( FileStream filestream = File.OpenRead( sendedFilePath ) )
            {
                _client.UploadFile( filestream, "/" + newFileName, null );
                _client.Disconnect();
            }
        }
    }
}
