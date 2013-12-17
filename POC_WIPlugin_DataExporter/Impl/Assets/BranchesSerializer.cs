using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataExporter.Model;
using System.IO;
using vrcontext.walkinside.sdk;

namespace DataExporter.Impl
{
    public class BranchesSerializer : IBranchesSerializer
    {
        //Serializes all branches recursively
        public void Serialize( StreamWriter file )
        {
            serializeRecursively( DataExporter.Root, file );
        }

        //Serializes "branch" argument
        public void Serialize( IVRBranch branch, StreamWriter file )
        {
            string name = branch.Name;
            if( name.StartsWith( "/" ) )
            {
                name = name.Substring( 1 );
            }

            file.WriteLine(
                String.Format( "{0};{1};{2};{3};{4};{5}",
                    name,
                    branch.Project.Name,
                    branch.OBB.Position.X.ToString(),
                    branch.OBB.Position.Y.ToString(),
                    branch.OBB.Position.Z.ToString(),
                    string.Format( Resource.UrlRefTag, branch.Project.Name, name )
                )
            );
        }

        //Serializes all branches in "branches" argument
        public void Serialize( IEnumerable<IVRBranch> branches, StreamWriter file )
        {
            foreach( IVRBranch branch in branches )
            {
                Serialize( branch, file );
            }
        }

        private void serializeRecursively( IVRBranch branch, StreamWriter file )
        {
            DataExporter.CurrentViewer.UI.SetPercentage( ( int )branch.ID / DataExporter.NbBranches );
            DataExporter.CurrentViewer.UI.ShowInformation( string.Format( Resource.ExportBranch, branch.Name ) );

            Serialize( branch, file );

            foreach( IVRBranch subBranch in branch.Children )
            {
                serializeRecursively( subBranch, file );
            }
        }
    }
}
