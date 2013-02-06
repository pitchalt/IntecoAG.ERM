using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;
//
namespace IntecoAG.ERM.FM.AVT {
    /// <summary>
    /// Строка счета-фактуры
    /// </summary>
    [Persistent("fmAVTInvoiceLine")]
    public class fmCAVTInvoiceLine : csCComponent {
        public fmCAVTInvoiceLine(Session ses) : base(ses) { }
        //
        private String _NomenclatureText;
        private csMeasurement _Count;
        private Decimal _Price;
        private Decimal _Cost;
        private csNDSRate _AVTRate;
        private Decimal _AVTSumm;
        [Persistent("SummAll")]
        private Decimal _SummAll;
        //private csCountry _Country;
        //private String _GTDNumber;
        private fmCAVTInvoiceVersion _AVTInvoiceVersion;
        private fmCAVTInvoiceLine _Source;
        //
        /// <summary>
        /// Версия Счета-Фактуры
        /// </summary>
        [Browsable(false)]
        [Association("fmAVTInvoiceVersion-fmAVTInvoiceLine")]
        public fmCAVTInvoiceVersion AVTInvoiceVersion {
            get { return _AVTInvoiceVersion; }
            set { SetPropertyValue<fmCAVTInvoiceVersion>("AVTInvoiceVersion", ref _AVTInvoiceVersion, value); }
        }
        /// <summary>
        /// Номенклатура
        /// </summary>
        [Size(250)]
        public String NomenclatureText {
            get { return _NomenclatureText; }
            set { SetPropertyValue<String>("NomenclatureText", ref _NomenclatureText, value); }
        }
        /// <summary>
        /// Кол-во
        /// </summary>
        public csMeasurement Count {
            get { return _Count; }
            set { 
                SetPropertyValue<csMeasurement>("Count", ref _Count, value);
                if (!IsLoading) {
                    Cost = Count.Count * Price;
                }
            }
        }
        /// <summary>
        /// Цена
        /// </summary>
        public Decimal Price {
            get { return _Price; }
            set { 
                SetPropertyValue<Decimal>("Price", ref _Price, value);
                if (!IsLoading) {
                    Cost = Count.Count * Price;
                }
            }
        }
        /// <summary>
        /// Стоимость
        /// </summary>
        public Decimal Cost {
            get { return _Cost; }
            set { 
                SetPropertyValue<Decimal>("Cost", ref _Cost, value);
                if (!IsLoading) {
                    if (AVTRate != null) {
                        if (AVTRate.Denominator != 0)
                            AVTSumm = Cost * AVTRate.Numerator / AVTRate.Denominator;
                    }
                    else
                        UpdateFields();
                }
            }
        }
        /// <summary>
        /// Сумма НДС
        /// </summary>
        public Decimal AVTSumm {
            get { return _AVTSumm; }
            set { 
                SetPropertyValue<Decimal>("AVTSumm", ref _AVTSumm, value);
                if (!IsLoading)
                    UpdateFields();
            }
        }
        /// <summary>
        /// Ставка НДС
        /// </summary>
        public csNDSRate AVTRate {
            get { return _AVTRate; }
            set { 
                SetPropertyValue<csNDSRate>("AVTRate", ref _AVTRate, value);
                if (AVTRate != null) {
                    if (AVTRate.Denominator != 0)
                        AVTSumm = Cost * AVTRate.Numerator / AVTRate.Denominator;
                }
            }
        }
        /// <summary>
        /// Сумма всего
        /// </summary>
        [PersistentAlias("_SummAll")]
        public Decimal SummAll {
            get { return _SummAll; }
//            set { SetPropertyValue<Decimal>("AVTSumm", ref _AVTSumm, value); }
        }
        //
        void UpdateFields() {
            _SummAll = Cost + AVTSumm;
            OnChanged("SummAll");
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public fmCAVTInvoiceLine Source {
            get { return _Source; }
            set {
                SetPropertyValue<fmCAVTInvoiceLine>("Source", ref _Source, value);
            }
        }

    }
}
