using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataExporter.Model.Scenarios;
using SevenZip;
using System.IO;

namespace DataExporter.Impl.Scenarios
{
    public class ZipExtractor : IZipExtractor, IDisposable
    {
        private string _zipPath;
        private string _tmpDir;

        public ZipExtractor( string zipPath )
        { 
            //checks to do !!!!!
            _zipPath = String.Format( @"{0}\{1}", Resource.ScenarioDirectory, zipPath );
            _tmpDir = Resource.ScenariosFileTmpDir;
        }

        public string TmpDir
        {
            get { return _tmpDir; }
        }

        public void Extract()
        {
            //Checks to do !!!!!

            using( SevenZipExtractor extractor = new SevenZipExtractor( _zipPath ) )
            {
                for( int n = 0; n < extractor.ArchiveFileData.Count; n++ )
                {
                    if( extractor.ArchiveFileData[n].FileName.Contains( ".xml" ) )
                    {
                        extractor.ExtractFiles( _tmpDir, extractor.ArchiveFileData[n].Index );
                    }
                }
            }
        }

        public void Dispose()
        {
            Array.ForEach( Directory.GetFiles( Resource.ScenariosFileTmpDir ), File.Delete );
        }
    }
}
