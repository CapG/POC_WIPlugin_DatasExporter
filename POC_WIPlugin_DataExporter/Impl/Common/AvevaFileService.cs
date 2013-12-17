using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataExporter.Model;
using System.IO;

namespace DataExporter.Impl
{
    public class AvevaFileService : IFileService
    {
        private string _fullPath;

        public string FullPath
        {
            get { return _fullPath; }
        }

        public StreamWriter CreateAssetsFile( string path )
        {
            _fullPath = String.Format( @"{0}\{1}_assets.csv", path, DateTime.Now.ToString( "yyyyMMddhhmmss" ) );
            StreamWriter csvFile = new StreamWriter(
                _fullPath,
                true
            );

            csvFile.WriteLine( Resource.AssetsFileColumns );

            return csvFile;
        }

        public StreamWriter CreateScenariosFile( string path )
        {
            _fullPath = String.Format( @"{0}\{1}_scenarios.csv", path, DateTime.Now.ToString( "yyyyMMddhhmmss" ) );

            StreamWriter csvFile = new StreamWriter(
                _fullPath,
                true
            );

            csvFile.WriteLine( Resource.ScenariosFileColumns );

            return csvFile;
        }
    }
}
