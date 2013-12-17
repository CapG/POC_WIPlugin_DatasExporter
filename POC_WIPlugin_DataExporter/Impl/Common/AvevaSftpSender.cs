using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataExporter.Model;
using Renci.SshNet;
using System.IO;

namespace DataExporter.Impl
{
    public class AvevaSftpSender : ISftpSender
    {
        private SftpClient _client;

        public AvevaSftpSender()
        {
            _client = new SftpClient( "194.2.93.194", "Administrateur", "AdminPOCNUC01" );
        }

        public void Send( string path )
        {
            _client.Connect();
            using( FileStream filestream = File.OpenRead( path ) )
            {
                _client.UploadFile( filestream, path.Substring( path.LastIndexOf( @"\" ) ) );
                _client.Disconnect();
            }
        }
    }
}
