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

namespace WIExample
{
    class IVRExportAsset
    {
        public void ExportToExcel()
        {
            WIPlugin.currentViewer.UI.ShowInformation(Resource.CreateExcel);

            if (Directory.Exists(Resource.DirectoryTmp))
            {
                StreamWriter csvFile = Excel.OpenEXCEL(Path.Combine(Resource.DirectoryTmp, string.Format(Resource.ExcelName, Resource.FileNameAssetList)), Resource.TitleColumnExcelAsset);

                ListenerAsset(WIPlugin.branch, ref csvFile);

                Excel.CloseExcel(csvFile);

                WIPlugin.currentViewer.UI.ShowInformation(Resource.MoveFile);

                sendSFTP(Path.Combine(Resource.DirectoryTmp, string.Format(Resource.ExcelName, Resource.FileNameAssetList)), String.Format("{0}_assets.csv", DateTime.Now ));
                //File.Move(Path.Combine(Resource.DirectoryTmp, string.Format(Resource.ExcelName, Resource.FileNameAssetList)), Path.Combine(Resource.DirectoryDestination, string.Format(Resource.ExcelName, Resource.FileNameAssetList)));
                //SendTCP(Path.Combine(Resource.DirectoryTmp, string.Format(Resource.ExcelName, "test")));
            }
        }

        private void sendSFTP(string filePath, string fileName)
        {
            SftpClient sftp = new SftpClient("194.2.93.194", "userece", "AdminPOCNUC01");
            sftp.Connect();
            using (FileStream filestream = File.OpenRead(filePath))
            {
                sftp.UploadFile(filestream, "/"+fileName, null);
                sftp.Disconnect();
            }
        }

        private void ListenerAsset(IVRBranch branch, ref StreamWriter csvFile)
        {
            WIPlugin.currentViewer.UI.SetPercentage((int) branch.ID / WIPlugin.nbBranch);
            WIPlugin.currentViewer.UI.ShowInformation(string.Format(Resource.ExportBranch, branch.Name));

            Excel.WriteAssetToExcel(branch.Name,
                branch.Project.Name,
                branch.OBB.Position.X.ToString(),
                branch.OBB.Position.Y.ToString(),
                branch.OBB.Position.Z.ToString());

            foreach (IVRBranch br in branch.Children)
            {
                ListenerAsset(br, ref csvFile);
            }
        }

        private void SendTCP(string file)
        {
            TcpClient client = null;
            NetworkStream netstream = null;
            try
            {
                client = new TcpClient();

                client.Connect(new IPEndPoint(IPAddress.Parse("194.2.93.194"), 1111));
                netstream = client.GetStream();
                 
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    long fileSize = fs.Length;
                    long sum = 0;
                    int count = 0;
                    byte[] data = new byte[1024];
                    while (sum < fileSize)
                    {
                        count = fs.Read(data, 0, data.Length);
                        netstream.Write(data, 0, count);
                        sum += count;
                    }
                    netstream.Flush();
                }
            }
            catch (ObjectDisposedException exObj)
            {
                WIPlugin.currentViewer.UI.StatusMessage.Text = exObj.Message;
            }
            catch (InvalidOperationException exIn)
            {
                WIPlugin.currentViewer.UI.StatusMessage.Text = exIn.Message;
            }
            catch (Exception ex)
            {
                WIPlugin.currentViewer.UI.StatusMessage.Text = ex.Message;
            }
            finally
            {
                 netstream.Close();
                 client.Close();

                 WIPlugin.currentViewer.UI.StatusMessage.Text = "Finish export asset data";
            }
        }
    }
}
