using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    [VisibleInReports]
    [DC.DomainComponent]
    public interface IBookPay20144 {
        IList<IBookPay20144Record> Records { get; }
    }

    [NavigationItem("AVT")]
    [VisibleInReports]
    [Persistent("fmAVTBookVAT")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Party;BookVATType;Period;Number")]
    public class fmCAVTBookVAT : csCCodedComponent, IBookPay20144 {

        public enum fmCAVTBookVATType { 
            PAY_MAIN = 1,
            BAY_MAIN = 2,
            PAY_ADD  = 5,
            BAY_ADD  = 6,
            PAY_2014 = 14,
            BAY_2014 = 15
        }

        public fmCAVTBookVAT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private crmCParty _Party;
        private String _Period;
        private UInt16 _NextNumber;
        private UInt16 _Number;
        private fmCAVTBookVATType _BookVATType;
        //private Boolean _IsClosed;

        [Aggregated]
        [Association("fmAVTBookVAT-fmAVTBookVATRecords")]
        public XPCollection<fmCAVTBookVATRecord> BookVATRecords {
            get { return GetCollection<fmCAVTBookVATRecord>("BookVATRecords"); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Indexed("BookVATType;Period;Number", Unique = true)]
        [RuleRequiredField]
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }
        /// <summary>
        /// Тип книги
        /// </summary>
        [RuleRequiredField]
        public fmCAVTBookVATType BookVATType {
            get { return _BookVATType; }
            set { SetPropertyValue<fmCAVTBookVATType>("BookVATType", ref _BookVATType, value); }
        }
        /// <summary>
        /// Номер книги по порядку
        /// </summary>
        [RuleRequiredField(TargetCriteria = "BookVATType = 'PAY_ADD' or BookVATType = 'BAY_ADD'")]
        public UInt16 Number {
            get { return _Number; }
            set { SetPropertyValue<UInt16>("Number", ref _Number, value); }
        }
        /// <summary>
        /// Следующий номер
        /// </summary>
        public UInt16 NextNumber {
            get { return _NextNumber; }
            set { SetPropertyValue<UInt16>("NextNumber", ref _NextNumber, value); }
        }
        /// <summary>
        /// Период YYYYK
        /// </summary>
        [Size(5)]
        [RuleRequiredField]
        public String Period {
            get { return _Period; }
            set { SetPropertyValue<String>("Period", ref _Period, value); }
        }
        /// <summary>
        /// Год
        /// </summary>
        [NonPersistent]
        public String PeriodYYYY {
            get { return Period != null && Period.Length == 5 ? Period.Substring(0, 4) : String.Empty; }
        }
        /// <summary>
        /// Квартал
        /// </summary>
        [NonPersistent]
        public String PeriodKV {
            get { return Period != null && Period.Length == 5 ? Period.Substring(4, 1) : String.Empty; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DatePeriodStart {
            get {
                switch (PeriodKV) { 
                    case "1":
                        return new DateTime(Int32.Parse(PeriodYYYY), 1, 1);
                    case "2":
                        return new DateTime(Int32.Parse(PeriodYYYY), 4, 1);
                    case "3":
                        return new DateTime(Int32.Parse(PeriodYYYY), 7, 1);
                    case "4":
                        return new DateTime(Int32.Parse(PeriodYYYY), 10, 1);
                    default:
                        return default(DateTime);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DatePeriodStop {
            get {
                switch (PeriodKV) {
                    case "1":
                        return new DateTime(Int32.Parse(PeriodYYYY), 3, 31);
                    case "2":
                        return new DateTime(Int32.Parse(PeriodYYYY), 6, 30);
                    case "3":
                        return new DateTime(Int32.Parse(PeriodYYYY), 9, 30);
                    case "4":
                        return new DateTime(Int32.Parse(PeriodYYYY), 12, 31);
                    default:
                        return default(DateTime);
                }
            }
        }
        [Action()]
        public void ReNumber() {
            using (IObjectSpace os = CommonMethods.FindObjectSpaceByObject(this).CreateNestedObjectSpace()) {
                fmCAVTBookVAT book = os.GetObject<fmCAVTBookVAT>(this);
                fmCAVTBookVATLogic.ReNumber(os, book);
                os.CommitChanges();
            }
        }
        
        [Action()]
        public void Clear() {
            using (IObjectSpace os = CommonMethods.FindObjectSpaceByObject(this).CreateNestedObjectSpace()) {
                fmCAVTBookVAT book = os.GetObject<fmCAVTBookVAT>(this);
                fmCAVTBookVATLogic.Clear(os, book);
                os.CommitChanges();
            }
        }

        public IBookPay20144 BookPay20144 {
            get { return this; }
        }

        public IList<IBookPay20144Record> Records {
            get { 
                return new ListConverter<IBookPay20144Record, fmCAVTBookVATRecord>(BookVATRecords); 
            }
        }
    }

}