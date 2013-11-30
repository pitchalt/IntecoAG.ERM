using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CRM.Contract.Forms {

    /// <summary>
    /// Общие свойства регистрационных форм для сделок
    /// </summary>
    [NonPersistent]
    public class crmCommonRegistrationForm : BaseObject {

//        static string backColorRequired = "";

        public crmCommonRegistrationForm(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    this.OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }
            KindOfDeal = Deal.KindOfDeal.DEAL_WITH_STAGE;
//            backColorRequired = Color.FromArgb(245, 255, 255).ToString();
        }

        #region PROPERTY

        /// <summary>
        /// Curator
        /// </summary>
        private hrmDepartment _CuratorDepartment;
        [RuleRequiredField("crmCommonRegistrationForm.CuratorDepartment.Required", "Next")]
        public hrmDepartment CuratorDepartment {
            get { return _CuratorDepartment; }
            set { SetPropertyValue<hrmDepartment>("CuratorDepartment", ref _CuratorDepartment, value); }
        }

        //
        protected KindOfDeal _KindOfDeal;
        [RuleRequiredField("crmCommonRegistrationForm.KindOfDeal.Required", "Next")]
        public KindOfDeal KindOfDeal {
            get { return _KindOfDeal; }
            set { SetPropertyValue<KindOfDeal>("KindOfDeal", ref _KindOfDeal, value); }
        }

        protected DateTime _DateRegistration = DateTime.Now;
        public DateTime DateRegistration {
            get { return _DateRegistration; }
        }

        protected DateTime _DateBegin;
        [RuleRequiredField("crmCommonRegistrationForm.DateBegin.Required", "Next")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { 
                SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value);
                if (!IsLoading) {
                    if (this.DateEnd < value) {
                        this.DateEnd = value;
                    }
                }
            }
        }

        protected DateTime _DateEnd;
        [RuleRequiredField("crmCommonRegistrationForm.DateEnd.Required", "Next")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { 
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
                if (!IsLoading) {
                    if (this.DateBegin > value) {
                        this.DateBegin = value;
                    }
                    if (this.DateFinish < value) {
                        this.DateFinish = value;
                    }
                }
            }
        }

        protected DateTime _DateFinish;
        [RuleRequiredField("crmCommonRegistrationForm.DateFinish.Required", "Next")]
        public DateTime DateFinish {
            get { return _DateFinish; }
            set { 
                SetPropertyValue<DateTime>("DateFinish", ref _DateFinish, value);
                if (this.DateEnd > value) {
                    this.DateEnd = value;
                }
            }
        }

        //
        protected crmContractCategory _Category;
        [RuleRequiredField("crmCommonRegistrationForm.Category.Required", "Next")]
        public crmContractCategory Category {
            get { return _Category; }
            set { SetPropertyValue<crmContractCategory>("Category", ref _Category, value); }
        }

        // Описание документа
        private string _DescriptionShort;
        [RuleRequiredField("crmCommonRegistrationForm.DescriptionShort.Required", "Next")]
        public string DescriptionShort {
            get { return _DescriptionShort; }
            set {
                SetPropertyValue<string>("DescriptionShort", ref _DescriptionShort, value);
            }
        }


        //[Appearance("crmContractDocument.Number.Require.Caption", AppearanceItemType = "LayoutItem", BackColor = "Red", FontColor = "Black", FontStyle = System.Drawing.FontStyle, Criteria = "isnull(Number)")]
        //[Appearance("crmContractDocument.Number.Require.Field", BackColor = "Red", FontColor = "Black", Criteria = "isnull(Number)")]

        //[Appearance("Number.Caption.Italic", AppearanceItemType.LayoutItem, "FontStyle = 'Italic'", FontStyle = FontStyle.Bold.Italic)]
        //[Appearance("Number.Caption.Regular", AppearanceItemType.LayoutItem, "FontStyle = 'Regular'", FontStyle = FontStyle.Bold.Regular)]
        //[Appearance("Number.Caption.Strikeout", AppearanceItemType.LayoutItem, "FontStyle = 'Strikeout'", FontStyle = FontStyle.Bold.Strikeout)]
        //[Appearance("Number.Caption.Underline", AppearanceItemType.LayoutItem, "FontStyle = 'Underline'", FontStyle = FontStyle.Bold.Underline)]
        //[Appearance("Number.Caption.BackColor.Red", AppearanceItemType.LayoutItem, "Severity = 'Severe'", BackColor = "Red", FontColor = "Black", Priority = 1)]
        //[Appearance("Number.Caption.Blue", AppearanceItemType.LayoutItem, "Priority = 'Low'", FontColor = "Blue")]
        //[Appearance("Number.Caption.FontClor.Red", AppearanceItemType.LayoutItem, "Priority = 'High'", FontColor = "Red")]
        //[RuleRequiredField("crmContractDocument.Number.Required.Immediate", "Immediate")]

        
        // Сумма
        protected decimal _Price;
        [RuleRequiredField("crmCommonRegistrationForm.Price.Required", "Next")]
        public decimal Price {
            get { return _Price; }
            set {
                SetPropertyValue<decimal>("Price", ref _Price, value);
            }
        }
        
        /// <summary>
        /// csValuta
        /// </summary>
        private csValuta _Valuta;
        [RuleRequiredField("crmCommonRegistrationForm.Valuta.Required", "Next")]
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        //// Подразделение
        //protected hrmDepartment _Department;
        //[RuleRequiredField("crmCommonRegistrationForm.Department.Required", "Next")]
        //[Appearance("crmCommonRegistrationForm.Department.Caption", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        //public hrmDepartment Department {
        //    get { return _Department; }
        //    set {
        //        SetPropertyValue<hrmDepartment>("Department", ref _Department, value);
        //    }
        //}
        
        //
        private crmIParty _OurParty;
        [RuleRequiredField("crmCommonRegistrationForm.OurParty.Required", "Next")]
        public crmIParty OurParty {
            get { return _OurParty; }
            set { SetPropertyValue<crmIParty>("OurParty", ref _OurParty, value); }
        }
        
        private PartyRole _OurRole;
        [RuleRequiredField("crmCommonRegistrationForm.OurRole.Required", "Next")]
        [ImmediatePostData]
        public PartyRole OurRole {
            get { return _OurRole; }
            set {
                _OurRole = value;
                OnChanged("OurRole");
                OnChanged("PartnerRole");
            }
        }
        
        private crmIParty _PartnerParty;
        [RuleRequiredField("crmCommonRegistrationForm.PartnerParty.Required", "Next")]
        public crmIParty PartnerParty {
            get { return _PartnerParty; }
            set { SetPropertyValue<crmIParty>("PartnerParty", ref _PartnerParty, value); }
        }

        public PartyRole PartnerRole {
            get {
                if (OurRole == PartyRole.CUSTOMER)
                    return PartyRole.SUPPLIER;
                else
                    return PartyRole.CUSTOMER;
            }
        }

        public virtual bool ApplyTopAppearance {
            get { return false; }
        }

        #endregion

        #region METHODS

        public crmContractDeal RegisterDeal() {
            crmContractDeal rd;
            switch (KindOfDeal ){
//                case KindOfDeal.DEAL_LONG_SERVICE:
//                    rd = new crmDealLongService(this.Session);
//                    break;
                case KindOfDeal.DEAL_WITH_STAGE:
                    rd = new crmDealWithStage(this.Session);
                    break;
                case KindOfDeal.DEAL_WITHOUT_STAGE:
                    rd = new crmDealWithoutStage(this.Session);
                    break;
                default:
                    throw new ArgumentException("Unknown Kind Of Deal", "KindOfDeal");
            }
            //rd.DateRegistration = this.DateRegistration;
            rd.CuratorDepartment = this.CuratorDepartment;
            rd.Category = this.Category;
            rd.Current.DateBegin = this.DateBegin;
            rd.Current.DateEnd = this.DateEnd;
            rd.Current.DateFinish = this.DateFinish;
            rd.Current.DescriptionShort = this.DescriptionShort;
            rd.Current.Price = this.Price;
            rd.Current.Valuta = this.Valuta;
            if (this.OurRole == PartyRole.CUSTOMER) {
                rd.Customer = this.OurParty.Party;
                rd.Supplier = this.PartnerParty.Party;
            } else {
                rd.Supplier = this.OurParty.Party;
                rd.Customer = this.PartnerParty.Party;
            }
            return rd;
        }

        #endregion

    }
}
