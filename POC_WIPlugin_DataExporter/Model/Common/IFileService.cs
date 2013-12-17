using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataExporter.Model
{
    public interface IFileService
    {
        string FullPath { get; }
        StreamWriter CreateAssetsFile( string path );
        StreamWriter CreateScenariosFile( string path );
    }
}
