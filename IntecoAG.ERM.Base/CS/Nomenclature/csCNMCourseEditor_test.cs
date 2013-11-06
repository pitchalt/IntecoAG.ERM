using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CS.Nomenclature {

    // Редактор курсов валют

    [NavigationItem("Money")]
    [NonPersistent]
    //[Persistent("csNMCourseEditor")]
    public class csCNMCourseEditor : XPLiteObject
    {
        public csCNMCourseEditor(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
//            this.ComponentType = typeof(csCNMCourseEditor);
//            this.CID = Guid.NewGuid();

            _CourseDate = default(DateTime);
            _CourseDayTable = new XPCollection<csCNMValutaCourse>(PersistentCriteriaEvaluationBehavior.InTransaction, this.Session, null);
        }

        #region ПОЛЯ КЛАССА

        private DateTime _CourseDate; // Дата, на которую определяются курсы валют
        private XPCollection<csCNMValutaCourse> _CourseDayTable;   // Таблица курсов на указанную дату

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата, на которую определяются курсы валют
        /// </summary>
        [ImmediatePostData(true)]
        //[Browsable(false)]
        public DateTime CourseDate {
            get { return _CourseDate; }
            set {
                DateTime old = _CourseDate;
                if (old != value.Date) {
                    _CourseDate = value.Date;
                    if (!IsLoading) {
                        FillValutaCourceCollection();
                        CriteriaOperator criteria = new BinaryOperator(new OperandProperty("CourseDate"), new ConstantValue(CourseDate), BinaryOperatorType.Equal);
                        CourseDayTable.Criteria = criteria;
                        CourseDayTable.Reload();
                        OnChanged("CourseDate", old, value);
                    }
                }
            }
        }

        /// <summary>
        /// Таблица курсов всех имеющихся валют на CourseDate
        /// </summary>
        public XPCollection<csCNMValutaCourse> CourseDayTable {
            get {
                return _CourseDayTable;
            }
        }

        #endregion

        #region МЕТОДЫ

        public void FillValutaCourceCollection() {
            FillValutaCourceCollection(CourseDate);
            //if (_CourseDayTable == null) {
            //    _CourseDayTable = new XPCollection<csCNMValutaCourse>(PersistentCriteriaEvaluationBehavior.InTransaction, this.Session, null);
            //    _CourseDayTable.LoadingEnabled = true;

            //    SortProperty[] mSortings = { new SortProperty("Valuta.Code", SortingDirection.Ascending) };
            //    SortingCollection sortCol = new SortingCollection(mSortings);

            //    CriteriaOperator criteria = new BinaryOperator(new OperandProperty("CourseDate"), new ConstantValue(CourseDate), BinaryOperatorType.Equal);
            //    _CourseDayTable.Criteria = criteria;

            //    _CourseDayTable.Sorting = sortCol;
            //}
        }

        public void FillValutaCourceCollection(DateTime date) {
            if (date == default(DateTime))
                return;
            Dictionary<csValuta, csCNMValutaCourse> curs = new Dictionary<csValuta, csCNMValutaCourse>();
            XPQuery<csCNMValutaCourse> valutaCourses = new XPQuery<csCNMValutaCourse>(Session, true);
            var queryValutaCourses = from valutaCourse in valutaCourses
                                     where valutaCourse.CourseDate == date
                                     select valutaCourse;
            foreach (var valutaCourse in queryValutaCourses) {
                curs[valutaCourse.Valuta] = valutaCourse;
            }

            // Дополнение коллекции теми валютами, которые в ней отсутствуют, а в справочнике валют имеются.
            XPQuery<csValuta> currencies = new XPQuery<csValuta>(Session, true);
            var queryCurrencies = from currency in currencies
                                  select currency;
            foreach (var currency in queryCurrencies) {
                if (!curs.ContainsKey(currency)) {
                    csCNMValutaCourse newRow = new csCNMValutaCourse(Session);
                    newRow.Valuta = currency;
                    newRow.CourseDate = date;
                    newRow.Course = 0;
                }
            }
        }

        #endregion

    }

}
