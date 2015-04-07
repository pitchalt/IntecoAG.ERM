using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("FmTaxRuVatПериодБУ")]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    [Appearance(null, AppearanceItemType.ViewItem, "ДатаС > DefaultDate", TargetItems = "ДатаПериода", Enabled = false)]
    public class ПериодБУ : Период { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
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

        [Association("ПериодБУ-Операция"), DevExpress.Xpo.Aggregated]
        public XPCollection<Операция> Операции {
            get { return GetCollection<Операция>("Операции"); }
        }

        public ПериодБУ(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Налог = this.Session.FindObject<Налог>(new BinaryOperator("Код", "БУ"), true);
            if (Налог == null) {
                Налог = new Налог(this.Session);
                Налог.КодSet("БУ");
            }
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
            Int32 month = date.Month;
            return Налог.Код + ' ' + date.Year + " " + month.ToString("00");
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "ДатаПериода":
                    TimeBorderSet(ДатаПериода);
                    break;
                case "ДатаС":
                    DateTime date = ДатаС;
                    Int32 month = date.Month;
                    КодУст(Налог.Код + '.' + date.Year + '.' + month.ToString("00"));
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
            ДатаСSet(new DateTime(year, month, 1));
            ДатаПоSet(new DateTime(year, month, 1).AddMonths(1).AddDays(-1));
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
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
