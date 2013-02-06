using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;

using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.SystemModule;

namespace IntecoAG.ERM.Module {

    public static class CommonMethods {

        #region Методы загрузки View

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        public static void ShowConcreteDetailViewInWindow(Frame frame, IObjectSpace objectSpace, string DetailViewID, object currentObject, TargetWindow tw) {
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, currentObject);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            svp.TargetWindow = tw;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }

        #endregion



        #region МЕТОДЫ СЕРИАЛИЗАЦИИ

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string UTF8ByteArrayToString(byte[] characters) {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(string pXmlString) {
            string tmpXmlString = (string.IsNullOrEmpty(pXmlString)) ? "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" : pXmlString;
            //if (string.IsNullOrEmpty(pXmlString)) return "";
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(tmpXmlString);
            return byteArray;
        }

        /// <summary>
        /// Serialize an object into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T obj) {
            try {
                string xmlString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(T));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlString = UTF8ByteArrayToString(memoryStream.ToArray()); return xmlString;
            } catch {
                return string.Empty;
            }
        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string xml) {
            string tmpXmlString = (string.IsNullOrEmpty(xml)) ? "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" : xml;
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(tmpXmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (T)xs.Deserialize(memoryStream);
        }

        #endregion

        #region РЕФЛЕКСИЯ ДЛЯ СВОЙСТВ ОБЪЕКТОВ
        public static object getProperty(object containingObject, string propertyName) {
            return containingObject.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, containingObject, null);
        }

        public static void setProperty(object containingObject, string propertyName, object newValue) {
            containingObject.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, containingObject, new object[] { newValue });
        }

        public static object getField(object containingObject, string propertyName) {
            return containingObject.GetType().InvokeMember(propertyName, BindingFlags.GetField, null, containingObject, null);
        }

        public static void setField(object containingObject, string fieldName, object newValue) {
            containingObject.GetType().InvokeMember(fieldName, BindingFlags.SetField, null, containingObject, new object[] { newValue });
        }


        /// <summary>
        /// Извлечение значения из свойства или поля объекта
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="FieldOrPropertyName"></param>
        public static object getFieldOrProperty(object Object, string FieldOrPropertyName) {
            Type ObjType = Object.GetType();

            PropertyInfo prop = ObjType.GetProperty(FieldOrPropertyName);
            FieldInfo fld = ObjType.GetField(FieldOrPropertyName);

            if (prop != null) {
                return getProperty(Object, FieldOrPropertyName);
            } else if (fld != null) {
                return getField(Object, FieldOrPropertyName);
                //} else {
                //    throw new Exception("Не найдено поле или свойство с названием " + FieldOrPropertyName);
            }
            return null;
        }

        #endregion

        #region WORKING WITH ENUM
        
        /// <summary>
        /// Получение значения атрибута Description для Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value) {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        // Для большей понятности преднеазначения функции
        public static string GetMessage(Enum value) {
            return GetEnumDescription(value);
        }

        #endregion
    }
}
