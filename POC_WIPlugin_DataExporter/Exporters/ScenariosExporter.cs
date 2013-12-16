using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Renci.SshNet;
using SevenZip;

namespace DataExporter
{
    public class ScenarioExporter
    {
        private IExcelService _excelService;
        private IFTPSender _ftpSender;

        public ScenarioExporter()
        {
            _excelService = new AvevaExcelService();
            _ftpSender = new AvevaFTPSender();
        }

        public void ExportScenario( string[] args )
        {
            DataExporter.CurrentViewer.UI.ShowInformation(Resource.DecompressScenario);

            Decompress(args);

            string[] array = new string[args.Length];
            int k = 0;
            foreach( string dir in Directory.GetDirectories( Resource.TmpFolder ) )
            {
                array[k] = Directory.GetFiles(dir, "*.xml")[0];
                //k++;
            }

            Dictionary<string, List<string>> l_scenario = SerialiseXML(array);

            StreamWriter csvFile = _excelService.CreateFile(
                Path.Combine(
                    Resource.TmpFolder,
                    string.Format( Resource.ExcelName, Resource.ScenariosListFileName ) ),
                Resource.ScenarioFileColumns 
                );

            _excelService.WriteScenario( l_scenario, ref csvFile );

            _excelService.CloseFile(csvFile);

            foreach (string dir in Directory.GetDirectories(Resource.TmpFolder))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    try { File.Delete(file); }
                    catch (Exception e) { Console.Write(e.Message); }
                }
                try { Directory.Delete(dir); }
                catch (Exception e) { Console.Write(e.Message); }
            }

            _ftpSender.Send( Path.Combine(
                    Resource.TmpFolder, 
                    string.Format(
                        "{0}.csv", 
                        Resource.ScenariosListFileName
                    )), 
                    String.Format("{0}_scenario.csv", DateTime.Now.ToString("yyyyMMddhhmmss")));
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
                        SevenZipExtractor.SetLibraryPath( @"C:\Users\antmeuni\AppData\Local\Apps\COMOS\COMOS Walkinside 6.2 (64 bit)\7z.dll" );
                        using (var tmp = new SevenZipExtractor(args[i]))
                        {
                            for (int n = 0; n < tmp.ArchiveFileData.Count; n++)
                            {
                                if (tmp.ArchiveFileData[n].FileName.Contains(".xml"))
                                {
                                    tmp.ExtractFiles(Path.Combine(Resource.TmpFolder, i.ToString()), tmp.ArchiveFileData[n].Index);
                                }
                            }
                        }
                    }
                }
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
    }
}