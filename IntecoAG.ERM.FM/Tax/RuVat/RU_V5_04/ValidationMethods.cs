using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Xml;
//using System.Xml.Linq;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {
    public static class ValidationMethods {
        public static DateTime _DATE_NULL = new DateTime(1900, 1, 1);
        public const UInt16 _UINT16_NULL = 0;
        public const Decimal _DECIMAL_NULL = 0;

        public static String DateToValidFormat(DateTime date) {
            return date.ToString("dd.MM.yyyy");
            //int days = date.Day;
            //int months = date.Month;
            //int year = date.Year;
                //(days < 10 ? "0" + days.ToString() : days.ToString()) + "."
                //+ (months < 10 ? "0" + months.ToString() : months.ToString()) + "."
                //+ year.ToString();
        }

        public static XmlAttribute DateAttribute(XmlDocument doc, DateTime date, String attribute_name) {
            XmlAttribute result = doc.CreateAttribute(attribute_name);
            String str_date = date.ToShortDateString();
            result.Value = str_date;
            return result;
        }

        /// <summary>
        /// Проверяет на одновременную заданность\незаданность объекта и даты
        /// </summary>
        public static bool VerifyPropertyAndItsDate(object obj, DateTime date) {
            return obj == null && date == ValidationMethods._DATE_NULL || obj != null && date != ValidationMethods._DATE_NULL;
        }
    }
}
