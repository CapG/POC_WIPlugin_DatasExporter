using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vrcontext.walkinside.sdk;
using System.IO;
using System.Xml;

namespace WIExample
{
    class IVRExport
    {
        protected static int ListenerAsset(IVRBranch branch, string branchParentName, int rank, ref XmlTextWriter myXmlTextWriter)
        {
            string elementXML = string.Empty;
            string branchName = getName(branch.Name);

            if (!branchName.Contains("CIVI") && !branchName.Contains("STRU"))
            {
                rank++;

                if (!branchName.Contains(branchParentName) || rank < 5)
                {
                    switch (rank)
                    {
                        case 1:
                            elementXML = "Project";
                            myXmlTextWriter.WriteStartElement(elementXML);
                            myXmlTextWriter.WriteAttributeString("ID", branchName);
                            break;
                        case 2:
                            elementXML = "Area";
                            myXmlTextWriter.WriteStartElement(elementXML);
                            myXmlTextWriter.WriteAttributeString("Name", branchName);
                            break;
                        case 3:
                        case 4:
                            elementXML = "Tags by type";
                            myXmlTextWriter.WriteStartElement(elementXML);
                            myXmlTextWriter.WriteAttributeString("Type", branchName);
                            break;
                        default:
                            elementXML = "Tag";
                            myXmlTextWriter.WriteStartElement(elementXML);
                            myXmlTextWriter.WriteElementString("Name", branchName);
                            myXmlTextWriter.WriteElementString("Position", branch.OBB.Position.ToString());
                            break;
                    }

                    if (rank < 5)
                    {
                        foreach (IVRBranch br in branch.Children)
                        {
                            rank = ListenerAsset(br, branchName, rank, ref myXmlTextWriter);
                        }
                    }
                }
                rank--;
                myXmlTextWriter.WriteEndElement();
            }
            return rank;
        }

        public static void ExportToXML(string path)
        {

            String filename = "C:\\Users\\saad\\Desktop\\VRModel.xml";
            FileStream myFileStream = new FileStream(filename, FileMode.OpenOrCreate);
            XmlTextWriter myXmlTextWriter = new XmlTextWriter(myFileStream, System.Text.Encoding.UTF8);

            myXmlTextWriter.Formatting = Formatting.Indented;
            myXmlTextWriter.WriteStartDocument(false);

            int rank = 0;

            myXmlTextWriter.WriteStartElement("Context");
            myXmlTextWriter.WriteAttributeString("ID", "ECE");

            rank = ListenerAsset(WIPlugin.root, string.Empty, rank, ref myXmlTextWriter);

            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();
        }

        protected static string getName(string name)
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
