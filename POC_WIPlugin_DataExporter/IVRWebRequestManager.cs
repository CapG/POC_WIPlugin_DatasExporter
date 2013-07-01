using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vrcontext.walkinside.sdk;
using System.IO;
using System.Windows.Forms;


namespace DataExporter
{
    class IVRWebRequestManager
    {
        public static string AnalysisWebRequest(Dictionary<string, string> HTTPRequest)
        {
            string error = string.Empty;

            foreach (var data in HTTPRequest)
            {
                switch (data.Key)
                {
                    case "Project":
                        error = ProjectRequest(data.Value);
                        break;
                    case "Scenario":
                        error = ScenarioRequest(data.Value);
                        break;
                    case "Tag":
                        error = TagRequest(data.Value);
                        break;
                    default:
                        error = "Web Request unavailable";
                        break;
                }
            }
            return error;
        }

        protected static string ProjectRequest(string projectName)
        {
            string error = string.Empty;
            string filePath;

            IVRProject currentProject = DataExporter.currentViewer.ProjectManager.CurrentProject;
            if (currentProject == null || currentProject.Name.CompareTo(projectName) != 0)
            {
                filePath = RecursiveSearchFile(projectName + ".vrp", Resource.VRModelDirectory);
                if (filePath.CompareTo(string.Empty) == 0)
                {
                    error = "Project not found";
                    return error;
                }
                DataExporter.currentViewer.ProjectManager.LoadProject(filePath, out error);
            }
            return error;
        }

        protected static string ScenarioRequest(string scenarioName)
        {
            string error = string.Empty;
            IVRScriptManager scriptManager = DataExporter.currentViewer.ProjectManager.CurrentProject.ScriptManager;
            IVRScript script = scriptManager.Scripts.FirstOrDefault(var => var.Name == scenarioName);
            if (script == null)
            {
                string filePath = RecursiveSearchFile(scenarioName+".7z", Resource.ScenarioDirectory);
                if (filePath != string.Empty)
                {
                    script = DataExporter.currentViewer.ProjectManager.CurrentProject.ScriptManager.AddScript(filePath, false); 
                }
                else
                {
                    // TODO Mettre un message scenario not found
                    DataExporter.currentViewer.UI.ShowInformation("scenario not found");
                }
            }
            if (!script.IsRunning)
            {
                DataExporter.currentViewer.UI.MainForm.Invoke((MethodInvoker)delegate { script.Launch(); });
            }
            return error;
        }

        protected static string TagRequest(string tagName)
        {
            string error = string.Empty;
            IVRBranch branch = DataExporter.currentViewer.ProjectManager.CurrentProject.BranchManager.GetBranchesByType(0)[0];
            try
            {
                branch = RecursiveSearchBranch(branch, tagName);
                branch.JumpTo(0);
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return error;
        }

        protected static IVRBranch RecursiveSearchBranch(IVRBranch branch, string tagName)
        {
            IVRBranch branchSelect = branch;
            if (!branch.Name.Contains(tagName))
            {
                foreach (IVRBranch br in branchSelect.Children)
                {
                    branchSelect = RecursiveSearchBranch(br, tagName);
                    if (branchSelect.Name.Contains(tagName))
                    {
                        return branchSelect;
                    }
                }
            }
            return branchSelect;
        }

        protected static string RecursiveSearchFile(string fileName, string directoryName)
        {
            string[] filePath = null;
            filePath = Directory.GetFiles(directoryName, fileName, SearchOption.AllDirectories);
            for (int i = 0; i < filePath.Length; i++)
            {
                if (filePath[i].Contains(fileName))
                    return filePath[i];
            }
            return string.Empty;
        }
    }
}
