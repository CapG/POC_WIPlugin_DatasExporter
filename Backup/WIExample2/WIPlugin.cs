using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using vrcontext.walkinside.sdk;
using System.Threading;

namespace WIExample
{
    /// <summary>
    /// A basic example of a working plugin for walkinside, which creates an additional item in the Walkinside View Menu.
    /// </summary>
    /// <remarks>
    /// This module does not do anything really. The module creates an item in the view menu of Walkinside also called Plugin Menu.
    /// </remarks>
    public class WIPlugin : IVRPlugin
    {
        internal static VRPluginDescriptor pDescriptor = new VRPluginDescriptor(VRPluginType.Unknown, 1, "", "19/01/2009", "Export to AVEVA", "Export scenario and asset data and active HTTP server", "Vrcontext_SDK");
        /// <summary>
        /// Get the plugin descriptor of this walkinside module without creating a plugin instance of type WIPlugin.
        /// </summary>
        static public VRPluginDescriptor GetDescriptor
        {
            get
            {
                return pDescriptor;
            }
        }

        /// <summary>
        /// Get the plugin descriptor of this walkinside module object of type WIPlugin.
        /// </summary>
        public VRPluginDescriptor Description
        {
            get
            {
                return pDescriptor;
            }
        }

        ToolStripMenuItem m_ToolStripItem1 = null;
        ToolStripItem m_ToolStripItem2 = null;

        public static IVRViewerSdk currentViewer = null;
        public static IVRBranch branch = null;
        public static int nbBranch = int.MaxValue;

        Thread processThread;

        /// <summary>
        /// The method called by walkinside, immediatly after the plugin assembly is loaded in walkinside.
        /// </summary>
        /// <param name="viewer">The context of the viewer when plugin is created.</param>
        /// <returns>True if creation of plugin succeeded.</returns>
        public bool CreatePlugin(IVRViewerSdk viewer)
        {
            currentViewer = viewer;

            // Export scenario
            m_ToolStripItem1 = viewer.UI.PluginMenu.DropDownItems.Add(Resource.TitleExportScenarioMenu) as ToolStripMenuItem;
            viewer.UI.RegisterVRFormWithMenu(Keys.NoName, m_ToolStripItem1, typeof(ExportScenarioView));
            // Export asset
            m_ToolStripItem2 = viewer.UI.PluginMenu.DropDownItems.Add(Resource.TitleExportAssetMenu);
            m_ToolStripItem2.Click += new EventHandler(m_ToolStripItem_Click);
            viewer.ProjectManager.OnProjectOpen += new VRProjectEventHandler(ProjectManager_OnProjectOpen);
            // HTTP Server
            currentViewer.HTTPServer.Start();
            currentViewer.HTTPServer.AddEvent("WVRequest", new VRHTTPEventHandler(OnReceiveWebRequest));
            return true;
        }

        static void ProjectManager_OnProjectOpen(object sender, VRProjectEventArgs e)
        {
            if (e.Project.ProjectManager.CurrentProject.Name != "startup")
            {
                IVRBranch[] rootarray = e.Project.ProjectManager.CurrentProject.BranchManager.GetBranchesByType(0);
                if (rootarray.Length != 0)
                {
                    branch = rootarray[0];
                    nbBranch = rootarray.Length;
                }
            }
        }

        /// <summary>
        /// The event handler when a user clicks my menu item called "Example 2"
        /// </summary>
        void m_ToolStripItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(Resource.TextExportAsset, Resource.TitleExportAssetMenu, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                IVRExportAsset process = new IVRExportAsset();
                processThread = new Thread(process.ExportToExcel);
                processThread.IsBackground = true;
                processThread.Start();
                while (!processThread.IsAlive);

                Thread.Sleep(1);
                processThread.Join();
            }
        }

        void OnReceiveWebRequest(object sender, VRCollectionEventArgs args)
        {
            string error = string.Empty;
            Dictionary<string, string> l_RequestHTTP = new Dictionary<string, string>();
            for (int i = 0; i < args.Values.Count; i++)
            {
                l_RequestHTTP.Add(args.Values.AllKeys[i], args.Values[i]);
                //WIPlugin.currentViewer.UI.StatusMessage.Text = "Analyse web request: " + args.Values.AllKeys[i] + " = " + args.Values[i];
            }
            error = IVRWebRequestManager.AnalysisWebRequest(l_RequestHTTP);
            if (error.CompareTo(string.Empty) != 0)
            {
                //WIPlugin.currentViewer.UI.StatusMessage.Text = error;
            }
            return;
        }

        /// <summary>
        /// The method called by walkinside, just before the plugin is removed from walkinside environment.
        /// </summary>
        /// <param name="viewer">The context of the viewer when plugin is created.</param>
        /// <returns>True if destruction of plugin succeeded.</returns>
        public bool DestroyPlugin(IVRViewerSdk viewer)
        {
            // Export scenario
            m_ToolStripItem2.Click -= m_ToolStripItem_Click;
            viewer.UI.PluginMenu.DropDownItems.Remove(m_ToolStripItem2);
            m_ToolStripItem2 = null;
            // Export asset
            viewer.ProjectManager.OnProjectOpen -= new VRProjectEventHandler(ProjectManager_OnProjectOpen);
            viewer.UI.UnregisterVRFORM(m_ToolStripItem1, typeof(ExportScenarioView));
            viewer.UI.PluginMenu.DropDownItems.Remove(m_ToolStripItem1);
            m_ToolStripItem1 = null;
            // HTTP server
            currentViewer.HTTPServer.RemoveEvent("WVRequest", new VRHTTPEventHandler(OnReceiveWebRequest));
            currentViewer.HTTPServer.Stop();

            branch = null;
            return true;
        }
    }
}
