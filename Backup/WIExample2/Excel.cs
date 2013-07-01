using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vrcontext.walkinside.sdk;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Web;

namespace WIExample
{
    class Excel
    {
        public static StreamWriter csvFile = null;

        public static StreamWriter OpenEXCEL(string path, string architecture)
        {
            if (File.Exists(path))
            {
                try { File.Delete(path); }
                catch (Exception e) { WIPlugin.currentViewer.UI.StatusMessage.Text = e.Message; }
            }
            try
            {
                csvFile = new StreamWriter(path, true);
                csvFile.Write(architecture);
                csvFile.Write("\r\n");
            }
            catch (Exception e)
            {
                WIPlugin.currentViewer.UI.StatusMessage.Text = e.Message;
            }
            return csvFile;
        }

        public static void CloseExcel(StreamWriter csvFile)
        {
            try
            {
                csvFile.Flush();
                csvFile.Close();
            }
            catch (Exception e) { WIPlugin.currentViewer.UI.StatusMessage.Text = e.Message; }
        }

        public static void WriteAssetToExcel(string name, string project, string x, string y, string z)
        {
            // Tag
            if (name.StartsWith("/"))
                csvFile.Write(name.Substring(1));
            else
                csvFile.Write(name);
            csvFile.Write(";");
            // Project
            csvFile.Write(project);
            csvFile.Write(";");
            // Position
            csvFile.Write(x);
            csvFile.Write(";");
            csvFile.Write(y);
            csvFile.Write(";");
            csvFile.Write(z);
            csvFile.Write(";");
            // Lien
            if (name.StartsWith("/"))
                csvFile.Write(string.Format(Resource.UrlLinkTag,project,name.Substring(1)));
            else
                csvFile.Write(string.Format(Resource.UrlLinkTag,project,name));
            csvFile.Write("\r\n");
        }

        public static void WriteScenarioToExcel(string name, string project, string scenario)
        {
            // Tag
            if (name.StartsWith("/"))
                csvFile.Write(name.Substring(1));
            else
                csvFile.Write(name);
            csvFile.Write(";");
            // Project
            csvFile.Write(project);
            csvFile.Write(";");
            // Scenario
            csvFile.Write(scenario);
            csvFile.Write(";");
            // Lien
            csvFile.Write(string.Format(Resource.UrlLinkScenario, project, scenario));
            csvFile.Write("\r\n");
        }
    }
}
