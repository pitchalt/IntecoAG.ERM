using System;
using System.ComponentModel;
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
    public class csCNMCourseEditor : csCComponent
    {
        public csCNMCourseEditor(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(csCNMCourseEditor);
            this.CID = Guid.NewGuid();

            CourseDate = DateTime.MinValue;
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
            get {
                return _CourseDate;
            }
            set {
                DateTime value_Old = _CourseDate;
                SetPropertyValue<DateTime>("CourseDate", ref _CourseDate, value.Date);
                if (!IsLoading) {
                    CreateValutaCourceCollection();
                }
                if (!IsLoading && value_Old != value.Date) {
                    CriteriaOperator criteria = new BinaryOperator(new OperandProperty("CourseDate"), new ConstantValue(CourseDate), BinaryOperatorType.Equal);
                    _CourseDayTable.Criteria = criteria;

                    /*
                    //_CourseDayTable.Reload();
                    //if (!_CourseDayTable.IsLoaded) {
                    _CourseDayTable.Load();
                    //}

                    _CourseDayTable.Criteria = null;
                    _CourseDayTable.Filter = criteria;
                    */

                    FillValutaCourceCollection(this.CourseDate);
                    OnChanged("CourseDayTable");
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

        public void CreateValutaCourceCollection() {
            if (_CourseDayTable == null) {
                _CourseDayTable = new XPCollection<csCNMValutaCourse>(PersistentCriteriaEvaluationBehavior.InTransaction, this.Session, null);
                _CourseDayTable.LoadingEnabled = true;

                SortProperty[] mSortings = { new SortProperty("Valuta.Code", SortingDirection.Ascending) };
                SortingCollection sortCol = new SortingCollection(mSortings);

                CriteriaOperator criteria = new BinaryOperator(new OperandProperty("CourseDate"), new ConstantValue(CourseDate), BinaryOperatorType.Equal);
                _CourseDayTable.Criteria = criteria;

                _CourseDayTable.Sorting = sortCol;
            }
        }

        public void FillValutaCourceCollection(DateTime courceDate) {
            if (courceDate == DateTime.MinValue)
                return;

            XPQuery<csCNMValutaCourse> valutaCourses = new XPQuery<csCNMValutaCourse>(Session, true);
            var queryValutaCourses = from valutaCourse in valutaCourses
                                     where valutaCourse.CourseDate == courceDate
                                     select valutaCourse;
            foreach (var valutaCourse in queryValutaCourses) {
                if (!_CourseDayTable.Contains<csCNMValutaCourse>(valutaCourse)) {
                    _CourseDayTable.Add(valutaCourse);
                }
            }

            // Дополнение коллекции теми валютами, которые в ней отсутствуют, а в справочнике валют имеются.
            XPQuery<csValuta> currencies = new XPQuery<csValuta>(Session, true);
            var queryCurrencies = from currency in currencies
                                  select currency;
            foreach (var currency in queryCurrencies) {
                bool isFound = false;
                foreach (csCNMValutaCourse vCourse in _CourseDayTable) {
                    if (vCourse.Valuta == currency) {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound) {
                    csCNMValutaCourse newRow = new csCNMValutaCourse(Session);
                    newRow.Valuta = currency;
                    newRow.CourseDate = CourseDate.Date;
                    newRow.Course = 0;

                    _CourseDayTable.Add(newRow);
                }
            }
        }

        #endregion

    }

}
