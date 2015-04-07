using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("FmTaxRuVatДекларацияНДСДокумент")]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    public abstract class ДекларацияНДСДокумент : ДекларацияДокумент { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).

        //private String _Код;
        //[Size(16)]
        ////        [Custom("Caption", "Код")]
        //public String Код {
        //    get { return _Код; }
        //    set {
        //        if (!IsLoading) OnChanging("Код", value);
        //        SetPropertyValue<String>("Код", ref _Код, value);
        //    }
        //}

        private ДекларацияНДС _ДекларацияНДС;
        [Association("ДекларацияНДС-ДекларацияНДСДокумент")]
        public ДекларацияНДС ДекларацияНДС {
            get { return _ДекларацияНДС; }
            set {
                if (!IsLoading) OnChanging("ДекларацияНДС", value);
                SetPropertyValue<ДекларацияНДС>("ДекларацияНДС", ref _ДекларацияНДС, value);
            }
        }


        public ДекларацияНДСДокумент(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case "ДекларацияНДС":
                    Декларация = ДекларацияНДС;
                    break;
            }
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
