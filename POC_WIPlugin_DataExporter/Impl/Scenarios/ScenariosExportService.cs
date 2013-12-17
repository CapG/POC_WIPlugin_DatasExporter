using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataExporter.Fuck.Model;
using DataExporter.Impl.Scenarios;
using DataExporter.Impl;
using System.IO;

namespace DataExporter.Fuck.Impl
{
    public class ScenariosExportService : IScenariosExportService
    {
        private AvevaFileService _fileService;
        private AvevaSftpSender _sender;
        private ScenariosSerializer _serializer;

        public ScenariosExportService()
        {
            _fileService = new AvevaFileService();
            _sender = new AvevaSftpSender();
            _serializer = new ScenariosSerializer();
        }

        public void ExportScenariosFrom( string path )
        {
            //Checks to do
            using( ZipExtractor extractor = new ZipExtractor( path ) )
            {
                extractor.Extract();

                IList<string> filesList = Directory.GetFiles( extractor.TmpDir, "*.xml" );

                IDictionary<string, List<string>> scenarios = _serializer.DeserializeFromXml( filesList );

                using( StreamWriter file = _fileService.CreateScenariosFile( Resource.TmpDir ) )
                {
                    _serializer.Serialize( scenarios, file );
                }

                _sender.Send( _fileService.FullPath );
            }
        }
    }
}
