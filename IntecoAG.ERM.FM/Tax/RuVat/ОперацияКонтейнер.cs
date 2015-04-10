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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [NavigationItem("Налоги")]
    [Persistent("FmTaxRuVatОперацияКонтейнер")]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, "Налогоплательщик;Код")]
    public abstract class ОперацияКонтейнер : BaseEntity { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        private Налогоплательщик _Налогоплательщик;
        [RuleRequiredField]
//        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public Налогоплательщик Налогоплательщик {
            get { return _Налогоплательщик; }
            set {
                if (!IsLoading) OnChanging("Налогоплательщик", value);
                SetPropertyValue<Налогоплательщик>("Налогоплательщик", ref _Налогоплательщик, value);
            }
        }
        private String _Код;
//        [RuleRequiredField(TargetCriteria = "ПериодБУ == null")]
        [RuleRequiredField]
        [Size(32)]
        public String Код {
            get { return _Код; }
            set {
                if (!IsLoading) OnChanging("Код", value);
                SetPropertyValue<String>("Код", ref _Код, value);
            }
        }
        //
        private ПериодНДС _ПериодНДС;
        //[RuleRequiredField(TargetCriteria = "ПериодБУ == null")]
        public ПериодНДС ПериодНДС {
            get { return _ПериодНДС; }
            set {
                if (!IsLoading) OnChanging("ПериодНДС", value);
                SetPropertyValue<ПериодНДС>("ПериодНДС", ref _ПериодНДС, value);
            }
        }

        private ПериодБУ _ПериодБУ;
        //[RuleRequiredField(TargetCriteria = "ПериодНДС == null")]
        public ПериодБУ ПериодБУ {
            get { return _ПериодБУ; }
            set {
                if (!IsLoading) OnChanging("ПериодБУ", value);
                SetPropertyValue<ПериодБУ>("ПериодБУ", ref _ПериодБУ, value);
            }
        }

        [Association("ОперацияКонтейнер-Операция"), DevExpress.Xpo.Aggregated]
        public XPCollection<Операция> Операции {
            get { return GetCollection<Операция>("Операции"); }
        }

        public ОперацияКонтейнер(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
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
