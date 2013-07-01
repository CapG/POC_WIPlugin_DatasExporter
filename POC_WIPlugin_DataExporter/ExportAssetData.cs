using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using vrcontext.walkinside.sdk;

namespace WIExample
{
    public partial class ExportAssetData : VRForm
    {
        IVRBranch branch = null;

        public ExportAssetData()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            branch = SDKViewer.ProjectManager.CurrentProject.BranchManager.GetBranchesByType(0)[0];
            //SDKViewer.UI.StatusMessage.Text
            //SDKViewer.UI.ProgressBar
            button1.Enabled = false;

            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = branch.Children.Count();
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            //string destination = "\\" + hostname.Text + ":" + port.Text + "\\";
                                                                            // TODO - Ici mettre le chemin scpécific à ce fichier 
            string destination = "C:\\Users\\saad\\Desktop\\";
            if (Directory.Exists(destination))
            {
                status.Text = "Start extraction";

                ExportToEXCEL(destination);

                status.Text = "Finish";
            }
            else
            {
                status.Text = "Error: path not found";
            }
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public void ExportToEXCEL(string path)
        {
            path += "test.csv";
            try
            {
                StreamWriter csvFile = new StreamWriter(path, true);

                csvFile.Write("TagName;Project;X;Y;Z;UrlLink\r\n");

                int rank = 0;
                rank = ListenerAsset(branch, string.Empty, rank, ref csvFile);

                csvFile.Write("\r\n");
                csvFile.Flush();
                csvFile.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        protected int ListenerAsset(IVRBranch branch, string branchParentName, int rank, ref StreamWriter csvFile)
        {
            string branchName = getName(branch.Name);
            status.Text = branchName;

            if (!branchName.Contains("CIVI") && !branchName.Contains("STRU"))
            {
                rank++;

                if (!branchName.Contains(branchParentName) || rank < 5)
                {
                    switch (rank)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            break;
                        default:
                            // Tag
                            csvFile.Write(branchName);
                            csvFile.Write(";");
                            // Project
                            csvFile.Write(branch.Project.Name);
                            csvFile.Write(";");
                            // Position
                            csvFile.Write(branch.OBB.Position.X);
                            csvFile.Write(";");
                            csvFile.Write(branch.OBB.Position.Y);
                            csvFile.Write(";");
                            csvFile.Write(branch.OBB.Position.Z);
                            csvFile.Write(";");
                            // Lien
                            csvFile.Write("http://localhost:5050/WVRequest?Project=" + branch.Project.Name + "&Tag=" + branchName.Substring(1));
                            csvFile.Write("\r\n");
                            break;
                    }

                    if (rank < 5)
                    {
                        foreach (IVRBranch br in branch.Children)
                        {
                            rank = ListenerAsset(br, branchName, rank, ref csvFile);
                        }
                    }
                }
                rank--;
            }
            return rank;
        }

        protected string getName(string name)
        {
            string result = name;
            int index = result.IndexOf('/');
            if (index == -1)
                return result;
            result = result.Substring(index, result.Length - index);
            return result;
        }
    }
}
