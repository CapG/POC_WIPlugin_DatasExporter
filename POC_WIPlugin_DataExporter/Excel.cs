﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vrcontext.walkinside.sdk;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Web;

namespace DataExporter
{
    public class ExcelService
    {
        public static StreamWriter csvFile = null;

        public static StreamWriter OpenExcelFile(string path, string architecture)
        {
            if (File.Exists(path))
            {
                try { File.Delete(path); }
                catch (Exception e) { DataExporter.CurrentViewer.UI.StatusMessage.Text = e.Message; }
            }
            try
            {
                csvFile = new StreamWriter(path, true);
                csvFile.Write(architecture);
                csvFile.Write("\r\n");
            }
            catch (Exception e)
            {
                DataExporter.CurrentViewer.UI.StatusMessage.Text = e.Message;
            }
            return csvFile;
        }

        public static void CloseExcelFile(StreamWriter csvFile)
        {
            try
            {
                csvFile.Flush();
                csvFile.Close();
            }
            catch (Exception e) { DataExporter.CurrentViewer.UI.StatusMessage.Text = e.Message; }
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
            csvFile.Write(string.Format(Resource.UrlRefScenario, project, scenario));
            csvFile.Write("\r\n");
        }
    }
}
