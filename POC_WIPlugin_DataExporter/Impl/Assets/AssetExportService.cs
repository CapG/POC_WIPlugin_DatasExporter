using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataExporter.Model;
using System.IO;
using vrcontext.walkinside.sdk;

namespace DataExporter.Impl
{
    public class AssetsExportService : IAssetsExportService
    {
        private IFileService _fileService;
        private ISftpSender _sFtpSender;
        private IBranchesSerializer _branchesSerializer;

        public AssetsExportService()
        {
            _fileService = new AvevaFileService();
            _sFtpSender = new AvevaSftpSender();
            _branchesSerializer = new BranchesSerializer();
        }

        public void ExportAllAssets()
        {
            using( StreamWriter file = _fileService.CreateAssetsFile( Resource.TmpDir ) )
            {
                _branchesSerializer.Serialize( file );
            }
            _sFtpSender.Send( _fileService.FullPath );
        }

        public void ExportAsset( IVRBranch branch )
        {
            throw new NotImplementedException();
        }

        public void ExportAssets( IEnumerable<IVRBranch> branches )
        {
            throw new NotImplementedException();
        }
    }
}
