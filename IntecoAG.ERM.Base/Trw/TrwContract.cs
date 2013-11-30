using System;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.Trw {

    [NavigationItem("Trw")]
    [Persistent("TrwContract")]
    [DefaultProperty("Name")]
    public class TrwContract : BaseObject {

        public String TrwNumber;
        public DateTime TrwDate;
        public TrwContractSuperType ContractSuperType;

        private fmCSubject _Subject;
        public fmCSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading && value != null) {
                    TrwNumber = "D" + value.TrwCode;
                    TrwDate = value.DateBegin;
                }
            }
        }

        public String Name {
            get {
                return TrwNumber + " " + TrwDate.ToString("D");
            }
        }

        private csValuta _ObligationValuta;
        public csValuta ObligationValuta {
            get { return _ObligationValuta; }
            set { SetPropertyValue<csValuta>("ObligationValuta", ref _ObligationValuta, value); }
        }

        public TrwContract(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }


    }

}
