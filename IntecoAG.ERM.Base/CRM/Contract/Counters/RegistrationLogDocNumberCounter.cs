using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CRM.Counters {

    // Класс реализует счётчик последовательных номеров документов в журнале регистрации.
    // Чтобы счётчик корректно работал, необходимо, чтобы он сохранял в БД своё изменение мгновенно. 
    // Для этого он вычисляет последний номер, подготавливает запись со следующим номером 

    // Тип XPLiteObject по умолчанию не поддерживет Optimistic Locking

    //[DefaultClassOptions] // Временно для просмотра
    [Persistent("crmRegistrationLogDocNumberCounter")]
    public class RegistrationLogDocNumberCounter : XPLiteObject {

        public RegistrationLogDocNumberCounter(Session session)
            : base(session) {
        }

        public RegistrationLogDocNumberCounter(Session session, int year, hrmDepartment department)
            : base(session) {
                Year = year;
                Department = department;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public int Oid;

        #endregion


        #region СВОЙСТВА КЛАССА

        private int _DocNumber;
        public int DocNumber {
            get { return _DocNumber; }
            set { SetPropertyValue<int>("DocNumber", ref _DocNumber, value); }
        }

        private int _Year;
        public int Year {
            get { return _Year; }
            set { SetPropertyValue<int>("year", ref _Year, value); }
        }

        private hrmDepartment _Department;
        public hrmDepartment Department {
            get { return _Department; }
            set { SetPropertyValue<hrmDepartment>("Department", ref _Department, value); }
        }

        private int _StartValue;
        public int StartValue {
            get { return _StartValue; }
            set { SetPropertyValue<int>("StartValue", ref _StartValue, value); }
        }

        private int _Step;
        public int Step {
            get { return _Step; }
            set { SetPropertyValue<int>("Step", ref _Step, value); }
        }

        #endregion


        #region МЕТОДЫ

        public int ReserveDocNumber() {

            using (UnitOfWork uow = new UnitOfWork(this.Session.DataLayer)) {
                RegistrationLogDocNumberCounter rldn = uow.GetObjectByKey<RegistrationLogDocNumberCounter>(this.Oid);
                rldn.Reload();
                rldn.DocNumber++;
                uow.CommitChanges();
                return rldn.DocNumber;
            }

            //this.DocNumber++;
            //this.Save();
            //this.Session.FlushChanges();
            //return this.DocNumber;


            /*
            this.Session.BeginTransaction();

            // Быстро получаем Oid записи
            this.Save();
            this.Session.FlushChanges();

            int RecordOid = this.Oid;

            
            // Подсчитываем, сколько записей с заданным годом и подразделением имеетт с Oid <= RecordOid. Это и есть номер внутри фрагмента.
            // Этот номер, во-1-х, вставляем в запись и сохраняем, а во-2-х, возвразаем из метода
            int docNumber = 0;

            IObjectSpace objectSpace = new ObjectSpace((UnitOfWork)(this.Session));
            CollectionSource csb = new CollectionSource(objectSpace, typeof(RegistrationLogDocNumberGenerator), true, CollectionSourceMode.Normal);

            CriteriaOperator criteriaYear = new BinaryOperator("year", year, BinaryOperatorType.Equal);
            csb.Criteria.Add("year", criteriaYear);

            CriteriaOperator criteriaDepartment = null;
            if (Department != null) {
                criteriaDepartment = new BinaryOperator("Department", Department, BinaryOperatorType.Equal);
                csb.Criteria.Add("Department", criteriaDepartment);
            }

            CriteriaOperator criteriaOid = new BinaryOperator("Oid", year, BinaryOperatorType.LessOrEqual);
            csb.Criteria.Add("Oid", criteriaOid);

            csb.Reload();
            docNumber = csb.GetCount();

            this.DocNumber = docNumber;
            this.Save();
            this.Session.FlushChanges();

            this.Session.CommitTransaction();

            return docNumber;
            */

            /*
            // Смотрим, какой наибольший номер есть в базе
            //CriteriaOperator criteria = null;
            //criteria = new GroupOperator();
            //((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            CriteriaOperator criteriaYear = new BinaryOperator("year", year, BinaryOperatorType.Equal);
            //((GroupOperator)criteria).Operands.Add(criteriaYear);

            //CriteriaOperator criteriaDocNumberDepartment = null;
            //if (!string.IsNullOrEmpty(DocNumberDepartment)) {
            //    criteriaDocNumberDepartment = new BinaryOperator("DocNumberDepartment", DocNumberDepartment, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteria).Operands.Add(criteriaDocNumberDepartment);
            //}

            CriteriaOperator criteriaDepartment = null;
            if (Department != null) {
                criteriaDepartment = new BinaryOperator("Department", Department, BinaryOperatorType.Equal);
                //((GroupOperator)criteria).Operands.Add(criteriaDepartment);
            }

            //SortProperty[] sorting = { new SortProperty("Oid", DevExpress.Xpo.DB.SortingDirection.Descending) };

            IObjectSpace objectSpace = new ObjectSpace((UnitOfWork)(this.Session));

            CollectionSource csb = new CollectionSource(objectSpace, typeof(RegistrationLogDocNumberGenerator), true, CollectionSourceMode.Normal);
            csb.Criteria.Add("year", criteriaYear);
            csb.Criteria.Add("Department", criteriaDepartment);
            csb.Sorting.Add(new SortProperty("Oid", DevExpress.Xpo.DB.SortingDirection.Descending));
            csb.TopReturnedObjects = 1;



            //return this.Oid;
            */



/*
            //XPCollection<crmContractRegistrationLog> crl = new XPCollection<crmContractRegistrationLog>(this.Session, criteriaISN, sorting);
            XPCollection<crmContractRegistrationLog> crl = new XPCollection<crmContractRegistrationLog>(this.Session, criteria, sorting);
            crl.TopReturnedObjects = 1;
            if (!crl.IsLoaded) crl.Load();

            if (crl.Count > 0) {
                ContractRegistrationLogConditionalLastRecord = crl[0];
                return crl[0].ISN;
            } else {
                return 0;
            }
*/




            //this.Session.BeginTransaction();
            //RegistrationLogISNGenerator regLogISN = new RegistrationLogISNGenerator(this.Session);
            //regLogISN.Save();
            //this.Session.FlushChanges();
            //this.Session.CommitTransaction();
            //return regLogISN.ISN;
        }

        #endregion

    }

}
