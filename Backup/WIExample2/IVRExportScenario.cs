using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SevenZip;
using System.Xml;
using Renci.SshNet;

namespace WIExample
{
    class IVRExportScenario
    {
        public void ExportScenario(string[] args)
        {
            WIPlugin.currentViewer.UI.ShowInformation(Resource.DecompressScenario);
            Decompress(args);

            string[] array = new string[args.Length];
            int k = 0;
            foreach (string dir in Directory.GetDirectories(Resource.DirectoryTmp))
            {
                array[k] = Directory.GetFiles(dir, "*.xml")[0];
                k++;
            }
            Dictionary<string, List<string>> l_scenario = SerialiseXML(array);

            StreamWriter csvFile = Excel.OpenEXCEL(Path.Combine(Resource.DirectoryTmp, string.Format(Resource.ExcelName, Resource.FileNameScenarioList)), Resource.TitleColumnExcelScenario);

            ListenerScenario(l_scenario, ref csvFile);

            Excel.CloseExcel(csvFile);

            foreach (string dir in Directory.GetDirectories(Resource.DirectoryTmp))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    try { File.Delete(file); }
                    catch (Exception e) { Console.Write(e.Message); }
                }
                try { Directory.Delete(dir); }
                catch (Exception e) { Console.Write(e.Message); }
            }
            sendSFTP(Path.Combine(Resource.DirectoryTmp, string.Format(Resource.ExcelName, Resource.FileNameScenarioList)), String.Format("{0}_scenario.csv", DateTime.Now.ToString("yyyyMMddhhmmss")));
            //File.Move(Path.Combine(Resource.DirectoryTmp, string.Format(Resource.ExcelName, Resource.FileNameScenarioList)), Path.Combine(Resource.DirectoryDestination, string.Format(Resource.ExcelName, Resource.FileNameScenarioList)));
        }

        private void Decompress(string[] args)
        {
            //If no file name is specified, write usage text.
           
            if (args.Length == 0)
            {
                Console.WriteLine("error");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (File.Exists(args[i]))
                    {
                        SevenZipExtractor.SetLibraryPath(@"C:\Users\thlacroi\AppData\Local\Apps\COMOS\COMOS Walkinside 6.2 (64 bit)\7z.dll");
                        using (var tmp = new SevenZipExtractor(args[i]))
                        {
                            for (int n = 0; n < tmp.ArchiveFileData.Count; n++)
                            {
                                if (tmp.ArchiveFileData[n].FileName.Contains(".xml"))
                                {
                                    tmp.ExtractFiles(Path.Combine(Resource.DirectoryTmp, i.ToString()), tmp.ArchiveFileData[n].Index);
                                }
                            }
                        }
                    }
                }
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

        private Dictionary<string, List<string>> SerialiseXML(string[] args)
        {
            Dictionary<string, List<string>> l_scenario = new Dictionary<string, List<string>>();

            if (args.Length == 0)
            {
                Console.WriteLine("error");
            }
            foreach (string file in args)
            {
                StreamReader stream = new StreamReader(file);
                XmlReader reader = XmlReader.Create(stream);
                using (reader)
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        while (reader.ReadToFollowing("scenario"))
                        {
                            string scenarioName = string.Empty;
                            List<string> l_equipmentName = new List<string>();

                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    scenarioName = reader.Value;
                                }
                                while (reader.ReadToFollowing("item"))
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        if (reader.MoveToAttribute("BranchName"))
                                        {
                                            l_equipmentName.Add(reader.Value);
                                        }
                                    }
                                }
                            }
                            l_scenario.Add(scenarioName, l_equipmentName);
                        }
                    }
                    reader.Close();
                    stream.Close();
                }
            }
            return l_scenario;
        }

        private void ListenerScenario(Dictionary<string, List<string>> l_scenario, ref StreamWriter csvFile)
        {
            foreach (var data in l_scenario)
            {
                string scenarioName = data.Key;
                for (int i = 0; i < data.Value.Count; i++)
                {
                    Excel.WriteScenarioToExcel(data.Value[i], "ModelECE" ,scenarioName);
                }
            }
        }
    }
}