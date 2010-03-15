using System;
using System.Xml;

namespace MiniDeluxe
{
    public static class CheckForUpdate
    {
        private const String ProgramName = "MiniDeluxe";

        public static String CheckForXMLUpdate(String updateXMLURL)
        {
            // this code was borrowed from:
            // http://themech.net/2008/05/adding-check-for-update-option-in-csharp/

            XmlTextReader reader = null;
            Version newVersion = null;
            string url = "";

            try
            {                
                reader = new XmlTextReader(updateXMLURL);
                reader.MoveToContent();
                string elementName = "";
                if ((reader.NodeType == XmlNodeType.Element) &&
                    (reader.Name == ProgramName))
                {
                    while (reader.Read())
                    {                        
                        if (reader.NodeType == XmlNodeType.Element)
                            elementName = reader.Name;
                        else
                        {                            
                            if ((reader.NodeType == XmlNodeType.Text) &&
                                (reader.HasValue))
                            {                                
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        url = reader.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (reader != null) reader.Close();
            }        
       
            if(url != null && newVersion != null)
            {
                Version currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                if(currentVersion.CompareTo(newVersion) < 0)
                {
                    return url;
                }
            }

            return String.Empty;
        }
    }
}
