using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.FM {
    public class UpdaterVaultaCourse {

        public UpdaterVaultaCourse() {}

        public UpdaterVaultaCourse(Session session) {
            Ssn = session;
        }

        private Session Ssn;
        private DateTime StartDate = new DateTime(2012, 4, 1);
        private DateTime EndDate = new DateTime(2012, 5, 6);
        private XPQuery<csValuta> valutas = null;

        #region Массивы курсов валют

        private Decimal[] USD = {29.3282m,
                                29.3282m,
                                29.3479m,
                                29.2944m,
                                29.4285m,
                                29.4303m,
                                29.4606m,
                                29.4606m,
                                29.4606m,
                                29.6358m,
                                29.6359m,
                                29.8033m,
                                29.5690m,
                                29.4711m,
                                29.4711m,
                                29.4711m,
                                29.7614m,
                                29.6368m,
                                29.4978m,
                                29.5122m,
                                29.5214m,
                                29.5214m,

                                29.5214m,
                                29.4880m,
                                29.4549m,
                                29.2962m,
                                29.2770m,
                                29.4234m,
                                29.3627m,
                                29.3627m,
                                29.3627m,
                                29.3627m,
                                29.3708m,
                                29.4630m,
                                29.5937m,
                                29.8075m};

        private Decimal[] EUR = {39.1707m,
                                39.1707m,
                                39.1677m,
                                39.0846m,
                                38.8368m,
                                38.7097m,
                                38.5138m,
                                38.5138m,
                                38.5138m,
                                38.7192m,
                                38.8349m,
                                39.0781m,
                                38.8507m,
                                38.8134m,
                                38.8134m,
                                38.8134m,
                                38.7374m,
                                38.8509m,
                                38.6716m,
                                38.7230m,
                                38.8118m,
                                38.8118m,
                                
                                38.8118m,
                                38.8386m,
                                38.7950m,
                                38.6593m,
                                38.7393m,
                                38.7477m,
                                38.9203m,
                                38.9203m,
                                38.9203m,
                                38.9203m,
                                38.8223m,
                                38.7350m,
                                38.9157m,
                                39.0001m};

        private Decimal[] KZT = {19.8371m,
                                19.8371m,
                                19.8210m,
                                19.7715m,
                                19.8653m,
                                19.8531m,
                                19.9442m,
                                19.9442m,
                                19.9442m,
                                20.0601m,
                                20.0765m,
                                20.1687m,
                                20.0169m,
                                19.9621m,
                                19.9621m,
                                19.9621m,
                                20.1731m,
                                20.0791m,
                                19.9694m,
                                19.9724m,
                                19.9563m,
                                19.9563m,

                                19.9563m,
                                19.9506m,
                                19.9491m,
                                19.8229m,
                                19.7891m,
                                19.9008m,
                                19.8712m,
                                19.8712m,
                                19.8712m,
                                19.8712m,
                                19.8344m,
                                19.9189m,
                                20.0045m,
                                20.1579m};

        private Decimal[] RUB = {1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,

                                1m,
                                1m,
                                1m,
                                1,
                                1,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m,
                                1m};

        #endregion

        public void FixValutaCourseEtc() {
            valutas = new XPQuery<csValuta>(Ssn);

            // Коды валют
            var query = from valuta in valutas
                        select valuta;
            foreach (var valuta in query) {
                if (valuta.Code.ToUpper() == "EUR")
                    valuta.CodeCurrencyValue = "978";
                if (valuta.Code.ToUpper() == "KZT")
                    valuta.CodeCurrencyValue = "398";
                if (valuta.Code.ToUpper() == "RUB")
                    valuta.CodeCurrencyValue = "810";
                if (valuta.Code.ToUpper() == "USD")
                    valuta.CodeCurrencyValue = "840";
                valuta.Save();
            }
            Ssn.CommitTransaction();

            // Курсы валют
            FillCurrencyOnDateInterval(StartDate, EndDate);

            Ssn.CommitTransaction();
        }


        private void FillCurrencyOnDateInterval(DateTime StartDate, DateTime EndDate) {
            int i = 0;
            while (StartDate.AddDays((double)i) <= EndDate) {
                DateTime checkDate = StartDate.AddDays((double)i);

                FillCurrencyOnDate("USD", USD, checkDate, i);
                FillCurrencyOnDate("EUR", EUR, checkDate, i);
                FillCurrencyOnDate("KZT", KZT, checkDate, i);
                FillCurrencyOnDate("RUB", RUB, checkDate, i);

                i++;
            }
        }

        private void FillCurrencyOnDate(String CurrencyCode, Decimal[] mCourse, DateTime checkDate, Int32 i) {
            csValuta valuta = GetValutaByCode(CurrencyCode);
            if (valuta == null)
                return;

            XPQuery<csCNMValutaCourse> CurrencyCourses = new XPQuery<csCNMValutaCourse>(Ssn);
            var query = from currencyCourse in CurrencyCourses
                        where currencyCourse.Valuta == valuta
                           && currencyCourse.CourseDate == checkDate
                        select currencyCourse;
            if (query.Count() > 0) {
                foreach (var currency in query) {
                    currency.Course = mCourse[i];
                    break;
                }
            } else {
                CreateCurrencyCourse(valuta, checkDate, mCourse[i]);
            }
        }

        private void CreateCurrencyCourse(csValuta valuta, DateTime courseDate, Decimal Course) {
            csCNMValutaCourse valutaCourse = new csCNMValutaCourse(Ssn);
            valutaCourse.Valuta = valuta;
            valutaCourse.CourseDate = courseDate;
            valutaCourse.Course = Course;
            valutaCourse.Save();
        }

        private csValuta GetValutaByCode(String code) {
            return (from item in valutas
                    where item.Code == code
                    select item).FirstOrDefault();
        }

    }
}
