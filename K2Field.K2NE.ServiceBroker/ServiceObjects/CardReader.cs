using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    public class CardReader : ServiceObjectBase
    {
        public CardReader(K2NEServiceBroker broker)
            : base(broker)
        {
        }


        public override List<SourceCode.SmartObjects.Services.ServiceSDK.Objects.ServiceObject> DescribeServiceObjects()
        {

            ServiceObject rfidHelper = Helper.CreateServiceObject("RFID Helper", "RFID Helper class to read/write the RFID XML");

            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.DateOfBirth, SoType.DateTime, "Date of birth"));
            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.DonorId, SoType.Text, "Donor ID as string"));
            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.FirstName, SoType.Text, "First name"));
            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.LastName, SoType.Text, "Last Name"));
            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.Sex, SoType.Text, "Sex"));
            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.City, SoType.Text, "City"));
            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.RfidXMLInput, SoType.Memo, "XML of the RFID Card"));
            rfidHelper.Properties.Create(Helper.CreateProperty(Constants.SOProperties.CardReader.RfidXMLOutput, SoType.Memo, "XML of the RFID Card"));


            Method getFromXML = Helper.CreateMethod(Constants.Methods.CardReader.GetFromXML, "Read the details from the XML file", MethodType.Read);
            getFromXML.ReturnProperties.Add(Constants.SOProperties.CardReader.DateOfBirth);
            getFromXML.ReturnProperties.Add(Constants.SOProperties.CardReader.DonorId);
            getFromXML.ReturnProperties.Add(Constants.SOProperties.CardReader.FirstName);
            getFromXML.ReturnProperties.Add(Constants.SOProperties.CardReader.LastName);
            getFromXML.ReturnProperties.Add(Constants.SOProperties.CardReader.Sex);
            getFromXML.ReturnProperties.Add(Constants.SOProperties.CardReader.City);
            getFromXML.ReturnProperties.Add(Constants.SOProperties.CardReader.RfidXMLOutput);
            getFromXML.InputProperties.Add(Constants.SOProperties.CardReader.RfidXMLInput);
            getFromXML.Validation.RequiredProperties.Add(Constants.SOProperties.CardReader.RfidXMLInput);
            rfidHelper.Methods.Create(getFromXML);


            Method updateXML = Helper.CreateMethod(Constants.Methods.CardReader.UpdateXML, "Update the details from the XML file", MethodType.Read);
            updateXML.InputProperties.Add(Constants.SOProperties.CardReader.DonorId);
            updateXML.InputProperties.Add(Constants.SOProperties.CardReader.City);
            updateXML.InputProperties.Add(Constants.SOProperties.CardReader.RfidXMLInput);
            updateXML.ReturnProperties.Add(Constants.SOProperties.CardReader.RfidXMLOutput);
            updateXML.Validation.RequiredProperties.Add(Constants.SOProperties.CardReader.DonorId);
            rfidHelper.Methods.Create(updateXML);


            return new List<ServiceObject>() { rfidHelper };


        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.CardReader.UpdateXML:
                    UpdateXML();
                    break;
                case Constants.Methods.CardReader.GetFromXML:
                    GetFromXML();
                    break;
            }
        }



        private void UpdateXML()
        {

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            string xml = base.GetStringProperty(Constants.SOProperties.CardReader.RfidXMLInput, true);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            Dictionary<string, Dictionary<string, string>> files = ReadXml(xmlDoc);
            
            if (string.Compare(
                    base.GetStringProperty(Constants.SOProperties.CardReader.DonorId, true),
                    files["A1F0"]["CD"],
                    true) != 0)
            {
                throw new ApplicationException("DonorID does not match");

            }
/*            props["CD"] = base.GetStringProperty(Constants.SOProperties.CardReader.DonorId, true);

            string dateOfBirth = base.GetStringProperty(Constants.SOProperties.CardReader.DateOfBirth, false);
            if (!string.IsNullOrEmpty(dateOfBirth)) {
                DateTime d = DateTime.MinValue;
                if (DateTime.TryParse(dateOfBirth, out d)) {
                    props["C3"] = d.ToString("dd.MM.yyyy");
                }
            }
            string firstname = base.GetStringProperty(Constants.SOProperties.CardReader.FirstName, false);
            if (!string.IsNullOrEmpty(firstname))
            {
                props["C1"] = firstname;
            }

            string lastname = base.GetStringProperty(Constants.SOProperties.CardReader.LastName, false);
            if (!string.IsNullOrEmpty(lastname))
            {
                props["C0"] = lastname;
            }

            string sex = base.GetStringProperty(Constants.SOProperties.CardReader.Sex, false);
            if (!string.IsNullOrEmpty(sex))
            {
                props["C4"] = firstname;
            }
            
            XmlNode node = xmlDoc.SelectSingleNode("/drk-bsd/application[@nr='1']/file[@nr='0']");
            if (node != null)
            {
                StringBuilder str = new StringBuilder();
                foreach (KeyValuePair<string, string> prop in props)
                {
                    str.Append(prop.Key);
                    str.Append(prop.Value.Length.ToString("X2"));
                    str.Append(Encode(prop.Value).ToUpper());
                }
                while (str.Length < node.InnerText.Length)
                {
                    str.Append("0");
                }
                node.InnerText = str.ToString();
            }
 */

            string city = base.GetStringProperty(Constants.SOProperties.CardReader.City, true);
            if (!string.IsNullOrEmpty(city))
            {
                Dictionary<string, string> props = files["A1F1"];
                props["CB"] = city;


                XmlNode node = xmlDoc.SelectSingleNode("/drk-bsd/application[@nr='1']/file[@nr='1']");
                if (node != null)
                {
                    StringBuilder str = new StringBuilder();
                    foreach (KeyValuePair<string, string> prop in props)
                    {
                        str.Append(prop.Key);
                        str.Append(prop.Value.Length.ToString("X2"));
                        str.Append(Encode(prop.Value).ToUpper());
                    }
                    while (str.Length < node.InnerText.Length)
                    {
                        str.Append("0");
                    }
                    node.InnerText = str.ToString();
                }
            }

            DataRow dr = results.NewRow();

            /*
             * MemoryStream memoryStream = new MemoryStream();
// initialize xmlWriterSettings as above...
 
XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
// call the same operations on the xmlWriter as above...
 
string xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());
*/
            using ( MemoryStream memoryStream = new MemoryStream())
            {
               
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = false;
                settings.IndentChars = "  ";
                settings.Indent = true;
                using (XmlWriter xmlTextWriter = XmlWriter.Create(memoryStream, settings))
                {
                    xmlDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    
                    dr[Constants.SOProperties.CardReader.RfidXMLOutput] = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            results.Rows.Add(dr);
        }

        private void GetFromXML()
        {
            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            string xml = base.GetStringProperty(Constants.SOProperties.CardReader.RfidXMLInput, true);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            Dictionary<string, Dictionary<string, string>> files = ReadXml(xmlDoc);

            Dictionary<string, string> props = files["A1F0"];

            DataRow dr = results.NewRow();
            DateTime d = DateTime.MinValue;
            if (DateTime.TryParse(props["C3"], out d))
            {
                dr[Constants.SOProperties.CardReader.DateOfBirth] = d;
            }

            dr[Constants.SOProperties.CardReader.DonorId] = props["CD"];
            dr[Constants.SOProperties.CardReader.FirstName] = props["C1"];
            dr[Constants.SOProperties.CardReader.LastName] = props["C0"];
            dr[Constants.SOProperties.CardReader.Sex] = props["C4"];
            dr[Constants.SOProperties.CardReader.RfidXMLOutput] = xml;

            props = files["A1F1"];
            dr[Constants.SOProperties.CardReader.City] = props["CB"];

            results.Rows.Add(dr);
        }







        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }


        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }


        private string Decode(string str)
        {
            str = str.Replace(" ", "");

            byte[] data = StringToByteArray(str);
            return Encoding.UTF8.GetString(data);
        }

        private string Encode(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return ByteArrayToString(data);
        }




        private Dictionary<string, Dictionary<string, string>> ReadXml(XmlDocument xml)
        {


            Dictionary<string, Dictionary<string, string>> files = new Dictionary<string, Dictionary<string, string>>();

            foreach (XmlNode application in xml.SelectNodes("/drk-bsd/application"))
            {
                foreach (XmlNode file in application.ChildNodes)
                {
                    //Console.WriteLine("A:{0};File:{1}", application.Attributes["nr"].Value, file.Attributes["nr"].Value);

                    //Console.WriteLine("File: {0}", file.InnerText);

                    string codedFile = file.InnerText.Replace(" ", "");

                    Dictionary<string, string> values = new Dictionary<string, string>();
                    int start = 0;
                    while (start + 4 <= codedFile.Length)
                    {
                        string key = codedFile.Substring(start, 2);
                        string len = codedFile.Substring(start + 2, 2);
                        if (key == "00" && len == "00")
                        {
                            break;
                        }
                        UInt32 chars = Convert.ToUInt32(len, 16) * 2;
                        //Console.WriteLine("Length: {0} - {1}", chars,(int)chars);
                        string val = codedFile.Substring(start + 4, (int)chars);
                        if (!values.ContainsKey(key))
                        {
                            values.Add(key, Decode(val));
                        }
                        start = (int)chars + start + 4;
                        //Console.WriteLine("{0} : {1}", key, Decode(val));
                    }
                    files.Add(string.Format("A{0}F{1}", application.Attributes["nr"].Value, file.Attributes["nr"].Value), values);
                }
            }
            return files;
        }
    }


}
