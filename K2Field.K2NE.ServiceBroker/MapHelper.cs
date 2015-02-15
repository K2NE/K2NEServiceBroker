using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Field.K2NE.ServiceBroker
{
    /// <summary>
    /// MapHelper class is a simple static helper class that's used to handle supportive
    /// methods on the TypeMappings class. The TypeMappings are use dto see what simple types we support.
    /// </summary>
    public static class MapHelper
    {
        #region Private Field And Filling method
        private static TypeMappings _map = null;

       
        private static Dictionary<SoType, string> soType2SystemTypeMapping = new Dictionary<SoType, string>() {
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

        private static TypeMappings CreateTypeMappings()
        {
            TypeMappings map = new TypeMappings();
            map.Add(typeof(System.Int16), SoType.Number);
            map.Add(typeof(System.Int32), SoType.Number);
            map.Add(typeof(System.Int64), SoType.Number);
            map.Add(typeof(System.UInt16), SoType.Number);
            map.Add(typeof(System.UInt32), SoType.Number);
            map.Add(typeof(System.UInt64), SoType.Number);
            map.Add(typeof(System.Boolean), SoType.YesNo);
            map.Add(typeof(System.Char), SoType.Text);
            map.Add(typeof(System.DateTime), SoType.DateTime);
            map.Add(typeof(System.Decimal), SoType.Decimal);
            map.Add(typeof(System.Single), SoType.Decimal);
            map.Add(typeof(System.Double), SoType.Decimal);
            map.Add(typeof(System.Guid), SoType.Guid);
            map.Add(typeof(System.Byte), SoType.File);
            map.Add(typeof(System.SByte), SoType.File);
            map.Add(typeof(System.String), SoType.Text);

            map.Add(typeof(Nullable<System.Int16>), SoType.Number);
            map.Add(typeof(Nullable<System.Int32>), SoType.Number);
            map.Add(typeof(Nullable<System.Int64>), SoType.Number);
            map.Add(typeof(Nullable<System.UInt16>), SoType.Number);
            map.Add(typeof(Nullable<System.UInt32>), SoType.Number);
            map.Add(typeof(Nullable<System.UInt64>), SoType.Number);
            map.Add(typeof(Nullable<System.Boolean>), SoType.YesNo);
            map.Add(typeof(Nullable<System.Char>), SoType.Text);
            map.Add(typeof(Nullable<System.DateTime>), SoType.DateTime);
            map.Add(typeof(Nullable<System.Decimal>), SoType.Decimal);
            map.Add(typeof(Nullable<System.Single>), SoType.Decimal);
            map.Add(typeof(Nullable<System.Double>), SoType.Decimal);
            map.Add(typeof(Nullable<System.Guid>), SoType.Guid);
            map.Add(typeof(Nullable<System.Byte>), SoType.File);
            map.Add(typeof(Nullable<System.SByte>), SoType.File);

            return map;
        }
        #endregion Private Field And Filling method

        #region Public properties
        /// <summary>
        /// Returns the TypeMappings that this object contains.
        /// </summary>
        public static TypeMappings Map
        {
            get
            {
                if (_map == null)
                {
                    _map = CreateTypeMappings();
                }
                return _map;
            }
        }
        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Retrieves the SOType for the given .NET Type.
        /// </summary>
        public static SoType GetSoTypeByType(Type type)
        {
            return Map[type.FullName.ToLower()];
        }


        /// <summary>
        /// Retrieve the .NET Type (typeof(Type).toString()) for a given SOType.
        /// </summary>
        /// <param name="soType">the SOType</param>
        /// <returns>A typeof(T).toString() for the given SOType.</returns>
        public static string GetTypeBySoType(SoType soType)
        {
            return soType2SystemTypeMapping[soType];
        }

        /// <summary>
        /// Checks to see if the type is in the map. If it is, than this must be a simple type.
        /// </summary>
        public static bool IsSimpleMapableType(Type type)
        {
            if (Map.Contains(type.FullName.ToLower()))
            {
                return true;
            }
            return false;

        }
        #endregion Public methods

    
    }
}
