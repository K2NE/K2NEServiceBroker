using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Field.K2NE.ServiceObjects
{
    public class Helper
    {
        //TODO: Move this to Maphelper?
        private static Dictionary<SoType, string> _SoType2SystemTypeMapping = new Dictionary<SoType, string>() {
            {SoType.AutoGuid, typeof(Guid).ToString() },
            {SoType.Autonumber, typeof(int).ToString() },
            {SoType.DateTime, typeof(DateTime).ToString() },
            {SoType.Decimal, typeof(Decimal).ToString() },
            {SoType.Default, typeof(string).ToString() },
            {SoType.File, typeof(byte[]).ToString() },
            {SoType.Guid, typeof(Guid).ToString() },
            {SoType.HyperLink, typeof(string).ToString() },
            {SoType.Image, typeof(byte[]).ToString() },
            {SoType.Memo, typeof(string).ToString() },
            {SoType.MultiValue, typeof(string).ToString() },
            {SoType.Number, typeof(int).ToString() },
            {SoType.Text, typeof(string).ToString() },
            {SoType.Xml, typeof(string).ToString() },
            {SoType.YesNo, typeof(bool).ToString() }
        };
        /// <summary>
        /// Creates a system name from the given name. 
        /// 
        /// This would remove spaces and other fancy characters.
        /// </summary>
        /// <param name="name">Name to change</param>
        /// <returns>The system name</returns>
        public static string MakeSystemName(string name)
        {
            return name.Replace(" ", "");
        }


        /// <summary>
        /// Method that adds a space before a captical letter, this makes CamelCasing more readable.
        /// Examples:
        /// CamelCasing => Camel Casing
        /// This isSparta => This is Sparta
        /// </summary>
        /// <param name="name">A camelcased name.</param>
        /// <returns></returns>
        public static string AddSpaceBeforeCaptialLetter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            StringBuilder newText = new StringBuilder(name.Length + 10);
            newText.Append(name[0]);
            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]) && name[i - 1] != ' ')
                {
                    if (i + 1 < name.Length && !char.IsUpper(name[i + 1]))
                    {
                        newText.Append(' ');
                    }
                    if (i - 1 > 0 && !char.IsUpper(name[i - 1]) && newText[newText.Length - 1] != ' ')
                    {
                        newText.Append(' ');
                    }
                }
                newText.Append(name[i]);
            }
            return newText.ToString();
        }

        /// <summary>
        /// See <see cref="CreateSpecificProperty"/>.
        /// This method however simply uses AddSpaceBeforeCapitalLetter(name) for it's displayname.
        /// </summary>
        public static Property CreateProperty(string name, SoType type, string description)
        {
            return CreateSpecificProperty(name, AddSpaceBeforeCaptialLetter(name), description, type);
        }


        public static Property CreateProperty(string name, string description, SoType type)
        {
            return CreateSpecificProperty(name, AddSpaceBeforeCaptialLetter(name), description, type);
        }


        /// <summary>
        /// Create an instance of a Service Object property
        /// This method allows you to set the displayname specifically.
        /// </summary>
        /// <param name="name">DisplayName of the property</param>
        /// <param name="displayName">The displayname to use.</param>
        /// <param name="description">A short description.</param>
        /// <param name="type">SMO Type of the property.</param>
        /// <returns>The property</returns>
        public static Property CreateSpecificProperty(string name, string displayName, string description, SoType type)
        {
            Property property = new Property();
            property.Name = name;
            property.SoType = type;
            property.Type = _SoType2SystemTypeMapping[type];
            property.MetaData.DisplayName = displayName;
            property.MetaData.Description = description;
            return property;

        }

        /// <summary>
        /// Creates a service object method with the given name, description and Methodtype
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="methodType"></param>
        /// <returns></returns>
        public static Method CreateMethod(string name, string description, MethodType methodType)
        {
            Method m = new Method();
            m.Name = name;
            m.Type = methodType;
            m.MetaData.DisplayName = AddSpaceBeforeCaptialLetter(name);
            m.MetaData.Description = description;
            return m;
        }

        /// <summary>
        /// Create a service object with a name and description.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static ServiceObject CreateServiceObject(string name, string description)
        {
            ServiceObject so = new ServiceObject();
            so.Name = name;
            so.MetaData.DisplayName = AddSpaceBeforeCaptialLetter(name);
            so.MetaData.Description = description;
            so.Active = true;
            return so;
        }

    }
}
