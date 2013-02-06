using System.Reflection;
using System.Xml;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;

namespace IntecoAG.ERM.CRM.Contract {
    class Common {

    // Полный путь к файлу конфигурации (в случае его задания)
    public static string SettingsFilePath = System.IO.Directory.GetCurrentDirectory() + "\\ParameterSettings.xml";   // Environment.CurrentDirectory;

    public static Session session0;
    //public static Session session1;
    //public static Session session2;
    //public static Session session3;

    public static InMemoryDataStore dataStore;
    //public static DataCacheNode cacheNode1;
    //public static DataCacheNode cacheNode2;
    
    // //////public static SimpleDataLayer dataLayer;
    public static IDataLayer dataLayer;
        
    //public static SimpleDataLayer dataLayer2;

    public static bool CheckValidationRule = false;


    #region Организация соединения и БД

        public static void PrepareDB() {

            string _ConnectionString = Common.GetConfigParam("ConnectionString");

            if (_ConnectionString == "InMemory") {
                // Create the data store and a DataCacheRoot
                dataStore = new InMemoryDataStore(AutoCreateOption.DatabaseAndSchema);

                /*
                // ---------------------------------
                // Образец с применением кэшей
                DataCacheRoot cacheRoot = new DataCacheRoot((IDataStoreForTests)dataStore);

                // Create two DataCacheNodes for the same DataCacheRoot
                cacheNode1 = new DataCacheNode(cacheRoot);
                //cacheNode2 = new DataCacheNode(cacheRoot);

                // Create two data layers and two sessions
                dataLayer1 = new SimpleDataLayer(cacheNode1);
                //dataLayer2 = new SimpleDataLayer(cacheNode2);
                session1 = new Session(dataLayer1);
                //session2 = new Session(dataLayer2);
                // ---------------------------------
                */

                // Образец без создания кэшей
                dataLayer = new SimpleDataLayer(dataStore);
                //session1 = new Session(dataLayer1);

                XpoDefault.DataLayer = dataLayer as IDataLayer;
                XpoDefault.Session = null;

            } else if (_ConnectionString == "DefaultAccess") {
                session0 = new Session();
                //dataStore = session0.;
                dataLayer = session0.DataLayer as SimpleDataLayer;
            } else {
                // Испольуется полученная строка подключения
                IDataLayer dl = XpoDefault.GetDataLayer(_ConnectionString, AutoCreateOption.None);
                dataLayer = dl;
                XpoDefault.DataLayer = dl;
                XpoDefault.Session = null;
            }

            CheckValidationRule = (Common.GetConfigParam("CheckValidationRule") == "Y") ? true : false;
        }

    #endregion

    #region РЕФЛЕКСИЯ ДЛЯ СВОЙСТВ ОБЪЕКТОВ
    public static object getProperty(object containingObject, string propertyName) {
        return containingObject.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, containingObject, null);
    }

    public static void setProperty(object containingObject, string propertyName, object newValue) {
        containingObject.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, containingObject, new object[] { newValue });
    }

    public static void setField(object containingObject, string fieldName, object newValue) {
        containingObject.GetType().InvokeMember(fieldName, BindingFlags.SetField, null, containingObject, new object[] { newValue });
    }



    //PropertyInfo[] propertyInfos;
    //propertyInfos = typeof(TestRegion).GetProperties(BindingFlags.GetProperty | BindingFlags.Public |
    //                                              BindingFlags.Static);
    //// sort properties by name
    //Array.Sort(propertyInfos,
    //        delegate(PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
    //        { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

    //// write property names
    //foreach (PropertyInfo propertyInfo in propertyInfos) {
    //    Console.WriteLine(propertyInfo.Name);
    //}


    #endregion

    #region Чтение конфигурационных параметров

    /// <summary>
    /// Получение значения параметра из ServiceSettings.xml
    /// </summary>
    /// <param name="ParamName">Название параметра конфигурации</param>
    /// <returns>Значение параметра конфигурации</returns>
    /// <remarks></remarks>
    //[DebuggerStepThrough]
    public static string GetConfigParam(string ParamName) {
        string ParamValue = null;

        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        xmlDoc.Load(SettingsFilePath);

        XmlNode appSettings;
        if (xmlDoc.HasChildNodes) {
            appSettings = xmlDoc.GetElementsByTagName("appSettings")[0];
        } else {
            return ParamValue;
        }
        if (appSettings == null || !appSettings.HasChildNodes) return null;
        foreach (XmlNode xnode in appSettings) {
            if (xnode.Name == ParamName) return xnode.InnerText;
        }
        
        return ParamValue;
    }

        #endregion
    }
}
