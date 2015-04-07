using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("FmTaxRuVatПериодНДС")]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    [Appearance(null, AppearanceItemType.ViewItem,  "ДатаС > DefaultDate", TargetItems = "ДатаПериода", Enabled = false)]
    [Appearance(null, AppearanceItemType.ViewItem, null, TargetItems = "ДекларацияНДС;КнигаПродаж;КнигаПокупок", Enabled = false)]
    public class ПериодНДС : Период { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).

        private static DateTime _DefaultDate = new DateTime(1900, 01, 01);

        [Browsable(false)]
        public DateTime DefaultDate {
            get {
                return _DefaultDate;
            }
        }

        private DateTime _ДатаПериода;
        public DateTime ДатаПериода {
            get { return _ДатаПериода; }
            set {
                if (!IsLoading) OnChanging("ДатаПериода", value);
                SetPropertyValue<DateTime>("ДатаПериода", ref _ДатаПериода, value);
            }
        }

        [Association("ПериодНДС-Книга"), DevExpress.Xpo.Aggregated]
        public XPCollection<Книга> Книги {
            get { return GetCollection<Книга>("Книги"); }
        }

        [Association("ПериодНДС-ДекларацияНДС"), DevExpress.Xpo.Aggregated]
        public XPCollection<ДекларацияНДС> ДекларацииНДС { 
            get { return GetCollection<ДекларацияНДС>("ДекларацииНДС"); } 
        }

        [Association("ПериодНДС-Операция"), DevExpress.Xpo.Aggregated]
        public XPCollection<Операция> Операции {
            get { return GetCollection<Операция>("Операции"); }
        }

        private ДекларацияНДС _ДекларацияНДС;
//        [Custom("AllowEdit", "false")]
        public ДекларацияНДС ДекларацияНДС {
            get { return _ДекларацияНДС; }
            set {
                if (!IsLoading) OnChanging("ДекларацияНДС", value);
                SetPropertyValue<ДекларацияНДС>("ДекларацияНДС", ref _ДекларацияНДС, value);
            }
        }

        private КнигаПродаж _КнигаПродаж;
//        [Custom("AllowEdit", "false")]
        public КнигаПродаж КнигаПродаж {
            get { return _КнигаПродаж; }
            set {
                if (!IsLoading) OnChanging("КнигаПродаж", value);
                SetPropertyValue<КнигаПродаж>("КнигаПродаж", ref _КнигаПродаж, value);
            }
        }

        private КнигаПокупок _КнигаПокупок;
//        [Custom("AllowEdit", "false")]
        public КнигаПокупок КнигаПокупок {
            get { return _КнигаПокупок; }
            set {
                if (!IsLoading) OnChanging("КнигаПокупок", value);
                SetPropertyValue<КнигаПокупок>("КнигаПокупок", ref _КнигаПокупок, value);
            }
        }

        public ПериодНДС(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Налог = this.Session.FindObject<Налог>(new BinaryOperator("Код", "НДС"), true);
            if (Налог == null) {
                Налог = new Налог(this.Session);
                Налог.КодSet("НДС");
            }
            ДекларацияНДС = new ДекларацияНДС(this.Session);
            ДекларацииНДС.Add(ДекларацияНДС);
            КнигаПродаж = new КнигаПродаж(this.Session);
            Книги.Add(КнигаПродаж);
            КнигаПокупок = new КнигаПокупок(this.Session);
            Книги.Add(КнигаПокупок);
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
        public override void OnChanging(string propertyName, object newValue) {
            base.OnChanging(propertyName, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "ДатаПериода":
                    TimeBorderCheck((DateTime)newValue);
                    break;
            }
        }

        public override string ToString() {
            DateTime date = ДатаС;
            Int32 quart = (date.Month - 1) / 3 + 1;
            return Налог.Код + ' ' + date.Year + " кв. " + quart;
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "ДатаПериода":
                    TimeBorderSet((DateTime)newValue);
                    break;
                case "ДатаС":
                    DateTime date = (DateTime)newValue;
                    Int32 quart = (date.Month - 1) / 3 + 1;
                    КодУст(Налог.Код + '.' + date.Year + '.' + quart);
                    break;
            }
        }
        
        public void TimeBorderCheck(DateTime date) {
            Int32 year = date.Year;
            Int32 month = date.Month;
            Int32 quoter = (month - 1) / 3;
            if (date < new DateTime(2006, 01, 01))
                throw new ArgumentOutOfRangeException("Дата слишком маленькая");
            if (new DateTime(year, quoter * 3 + 1, 1) > DateTime.Now.AddMonths(3))
                throw new ArgumentOutOfRangeException("Дата слишком большая");
        }

        public void TimeBorderSet(DateTime date) {
            TimeBorderCheck(date);
            Int32 year = date.Year;
            Int32 month = date.Month;
            Int32 quoter = (month - 1) / 3;
            ДатаСSet(new DateTime(year, quoter * 3 + 1, 1));
            ДатаПоSet(new DateTime(year, (quoter + 1) * 3, 1).AddMonths(1).AddDays(-1));
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
//        [ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
//        [Custom("EditMask", "### ### ### ##0.000000")]

        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (http://documentation.devexpress.com/#Xaf/CustomDocument2619).
        //    this.PersistentProperty = "Paid";
        //}
    }
}
