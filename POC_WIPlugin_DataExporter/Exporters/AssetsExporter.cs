using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vrcontext.walkinside.sdk;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Web;
using Renci.SshNet;
using System.Security.Cryptography;

namespace DataExporter
{
    public class AssetsExporter
    {
        private IExcelService _excelService;
        private IFTPSender _ftpSender;

        public AssetsExporter()
        {
            _excelService = new AvevaExcelService();
            _ftpSender = new AvevaFTPSender();
        }

        public void ExportToExcel()
        {
            DataExporter.CurrentViewer.UI.ShowInformation( Resource.FileCreated );

            if ( Directory.Exists( Resource.TmpFolder ) )
            {
                StreamWriter csvFile = _excelService.CreateFile(
                    Path.Combine( Resource.TmpFolder, string.Format( "{0}.csv", Resource.AssetsListFileName ) ),
                    Resource.AssetsFileColumns );

                _excelService.WriteAssets( DataExporter.Branch, ref csvFile );

                _excelService.CloseFile( csvFile );

                _ftpSender.Send(Path.Combine(
                    Resource.TmpFolder,
                    string.Format(
                        Resource.ExcelName,
                        Resource.AssetsListFileName
                        )),
                    String.Format( "{0}_assets.csv", DateTime.Now.ToString( "yyyyMMddhhmmss" ) ) );
            }
        }
    }
}
