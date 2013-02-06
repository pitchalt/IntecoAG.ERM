using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.FM.AVT {
    /// <summary>
    /// 
    /// </summary>
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "InvoiceType;Period;Number")]
    [Persistent("fmAVTInvoiceNumberGenerator")]
    public class fmCAVTInvoiceNumberGenerator : XPLiteObject {
        public fmCAVTInvoiceNumberGenerator(Session ses) : base(ses) { }
        //
        private Int32 _Number;
        private Int32 _NextNumber;
        private String _Period;
        private Guid _InvoiceType;
        private Guid _SourceObject;
        //
        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public Guid Oid;

        /// <summary>
        /// 
        /// </summary>
        [Indexed]
        public Guid SourceObject {
            get { return _SourceObject; }
            set { SetPropertyValue<Guid>("SourceObject", ref _SourceObject, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Number {
            get { return _Number; }
            set { SetPropertyValue<Int32>("Number", ref _Number, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 NextNumber {
            get { return _NextNumber; }
            set { SetPropertyValue<Int32>("NextNumber", ref _NextNumber, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(10)]
        public String Period {
            get { return _Period; }
            set { SetPropertyValue<String>("Period", ref _Period, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Indexed("Period", "Number", Unique = true)]
        public Guid InvoiceType {
            get { return _InvoiceType; }
            set { SetPropertyValue<Guid>("InvoiceType", ref _InvoiceType, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ses"></param>
        /// <param name="inv_type"></param>
        /// <param name="inv_date"></param>
        /// <returns></returns>
        public static Int32 GenerateNumber(Session ses, Guid object_oid, fmCAVTInvoiceType inv_type, DateTime inv_date, Int32 number) {
            String per = inv_date.ToString("yyyyMM");
            CriteriaOperator oper = CriteriaOperator.And(new BinaryOperator("InvoiceType", inv_type.Oid, BinaryOperatorType.Equal),
                                                         new BinaryOperator("Period", per, BinaryOperatorType.Equal),
                                                         new BinaryOperator("Number", -1, BinaryOperatorType.Equal));
#pragma warning disable 0162
            for (int probe = 0; probe < 5; probe++) {
#pragma warning restore 0162
                using (UnitOfWork uow = new UnitOfWork(ses.DataLayer)) {
                    fmCAVTInvoiceNumberGenerator ng = uow.FindObject<fmCAVTInvoiceNumberGenerator>(
                        PersistentCriteriaEvaluationBehavior.BeforeTransaction, oper);
                    if (ng == null) {
                        ng = new fmCAVTInvoiceNumberGenerator(uow) {
                            Period = per,
                            InvoiceType = inv_type.Oid,
                            Number = -1
                        };
                    }
                    fmCAVTInvoiceNumberGenerator num = null;
                    if (object_oid != Guid.Empty) {
                        num = uow.FindObject<fmCAVTInvoiceNumberGenerator>(
                            PersistentCriteriaEvaluationBehavior.BeforeTransaction,
                            new BinaryOperator("SourceObject", object_oid));
                    }
                    if (num == null) {
                        //
                        if (number != 0) {
                            if (number > ng.NextNumber)
                                ng.NextNumber = number;
                        }
                        else
                            number = ++ng.NextNumber;
                        num = new fmCAVTInvoiceNumberGenerator(uow) {
                            SourceObject = object_oid,
                            Period = per,
                            InvoiceType = inv_type.Oid,
                            Number = number
                        };
                    }
                    else {
                        if (number != 0 && number != num.Number)
                            throw new LockConflictException();
                        else
                            number = num.Number;
                    }
                    //
                    uow.CommitChanges();
                    //
                    //return ng.NextNumber;
                    //                    return inv_type.Prefix + per + ng.NextNumber.ToString("00000");
                    return number;
                }
            }
            throw new LockConflictException();
        }
    }
}
