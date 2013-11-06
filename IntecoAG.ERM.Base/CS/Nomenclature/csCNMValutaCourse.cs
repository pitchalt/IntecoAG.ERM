using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using DevExpress.Data.Filtering;
using IntecoAG.ERM.FM;

namespace IntecoAG.ERM.CS.Nomenclature {

    // Погашение заявок на оплату (не счетов), а счета прикреплены к заявкам и поэтому тоже погашаются, но могут быть погашены и заявки по прочим основаниям. 
    // Этот объект ассоциирует Заявки на оплату с соответствующими документами оплаты, снабжая ассоциацию признаками:
    // Ссылка на выписку; дата, сумма и валюта оплаты

    //[NavigationItem("Money")]
    [Persistent("csNMValutaCourse")]
    public class csCNMValutaCourse : csCComponent
    {
        public csCNMValutaCourse(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(csCNMValutaCourse);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private DateTime _CourseDate; // Дата курса валюты
        private csValuta _Valuta; // Валюта
        private Decimal _Course;   // Курс в формате 2.4

        // Вывести покзатель пересчёта
        #endregion
        
        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата курса валюты
        /// </summary>
        public DateTime CourseDate {
            get {
                return _CourseDate;
            }
            set {
                SetPropertyValue<DateTime>("CourseDate", ref _CourseDate, value.Date);
            }
        }

        /// <summary>
        /// Валюта
        /// </summary>
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public csValuta Valuta {
            get {
                return _Valuta;
            }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        /// <summary>
        /// Курс в формате 2.4
        /// </summary>
        public Decimal Course {
            get {
                return _Course;
            }
            set {
                SetPropertyValue<Decimal>("Course", ref _Course, (Decimal)Math.Truncate(value * 10000)/10000);
            }
        }

        [PersistentAlias("Valuta.ConversionCount")]
        public Decimal ConversionCount {
            get {
                return Valuta.ConversionCount;
            }
        }

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Кросс-курс валюты valuta1 относительновалюты valuta2 на дату courseDate, вычисленный через курсы для рубля,
        /// т.е. сколькоо valuta1 дают за 1 единицу valuta2
        /// </summary>
        /// <param name="courseDate"></param>
        /// <param name="valuta1"></param>
        /// <param name="valuta2"></param>
        /// <returns></returns>
        public static Decimal GetCrossCourceOnDate(Session session, DateTime courseDate, csValuta valuta1, csValuta valuta2) {

            if  (valuta1 == valuta2) return 1;
            
            if (valuta1 == null) {
                throw new Exception("Не задана валюта, курс которой определяется");
            }
            if (valuta2 == null) {
                throw new Exception("Не задана валюта, к курсу которой выполняется приведение");
            }

            Decimal crossCource = 1;

            Decimal conversionCount1 = 1;   // За сколько единиц валюты дают сумму в рублях, равную курсу
            Decimal course1 = 1;   // Курс на дату courseDate

            Decimal conversionCount2 = 1;   // За сколько единиц валюты дают сумму в рублях, равную курсу
            Decimal course2 = 1;   // Курс на дату courseDate

            XPQuery<csCNMValutaCourse> valutaCourses1 = new XPQuery<csCNMValutaCourse>(session, true);
            csCNMValutaCourse qValutaCourse1 = (from valutaCourse in valutaCourses1
                                                where valutaCourse.CourseDate.Date == courseDate
                                                   && valutaCourse.Valuta == valuta1
                                                select valutaCourse).FirstOrDefault();
            if (qValutaCourse1 != null) {
                conversionCount1 = qValutaCourse1.ConversionCount;   // За сколько единиц валюты дают сумму в рублях, равную курсу
                if (conversionCount1 == 0)
                    conversionCount1 = 1;
                course1 = qValutaCourse1.Course;   // Курс на дату courseDate.Date
            } else {
                throw new Exception("Exchage course not found. Currency: " + valuta1.Code + ", date: " + courseDate.ToString("dd.MM.yyyy"));
            }


            XPQuery<csCNMValutaCourse> valutaCourses2 = new XPQuery<csCNMValutaCourse>(session, true);
            csCNMValutaCourse qValutaCourse2 = (from valutaCourse in valutaCourses2
                                                where valutaCourse.CourseDate.Date == courseDate
                                                   && valutaCourse.Valuta == valuta2
                                                select valutaCourse).First();
            if (qValutaCourse2 != null) {
                conversionCount2 = qValutaCourse2.ConversionCount;   // За сколько единиц валюты дают сумму в рублях, равную курсу
                if (conversionCount2 == 0)
                    conversionCount2 = 1;
                course2 = qValutaCourse2.Course;   // Курс на дату courseDate.Date
            } else {
                throw new Exception("Exchage course not found. Currency: " + valuta2.Code + ", date: " + courseDate.ToString("dd.MM.yyyy"));
            }

            // Соглашение. Если conversionCount1 == 0, то считаем его равным 1, то же и с conversionCount2
            if (course1 == 0) {
                course1 = GetDefaultCourceRub(session, valuta1.Code);
            }
            if (course2 == 0) {
                course2 = GetDefaultCourceRub(session, valuta2.Code);
            }

            // Курс valuta1 к 1 рублю
            //Decimal normCource1 = Math.Round(course1 / conversionCount1, 4);
            Decimal normCource1 = course1 / conversionCount1;

            // Курс valuta2 к 1 рублю
            //Decimal normCource2 = Math.Round(course2 / conversionCount2, 4);
            Decimal normCource2 = course2 / conversionCount2;

            if (normCource2 == 0) {
                throw new Exception("Не задан курс валюты " + valuta1.Code + ", на дату " + courseDate.ToString("yyyy.MM.dd"));
            }
            //crossCource = Math.Round(normCource1 / normCource2, 4);
            crossCource = normCource1 / normCource2;

            return crossCource;
        }

        public static csCNMValutaCourse GetValutaCourseOnDate(Session session, DateTime courseDate, csValuta valuta) {
            // Курс с указанными параметрами
            XPQuery<csCNMValutaCourse> valutaCourses = new XPQuery<csCNMValutaCourse>(session, true);
            var query = from valutaCourse in valutaCourses
                                      where valutaCourse.CourseDate.Date == courseDate.Date
                                         && valutaCourse.Valuta == valuta
                                      select valutaCourse;
            if (query.Count() == 0) return null;
            return query.First();
        }

        public static Decimal GetDefaultCourceRub(Session session, String Code) {
            //fmCSettingsFinance settingsFinance
            XPQuery<fmCSettingsFinance> SettingsFinances = new XPQuery<fmCSettingsFinance>(session);
            Decimal defaultCourse = (from settingsFinance in SettingsFinances
                                     where settingsFinance.CurrencyCodeRub == Code
                                     select settingsFinance.CurrencyDefaultCourceRub).FirstOrDefault();
            return defaultCourse;
        }
        #endregion

    }

}
