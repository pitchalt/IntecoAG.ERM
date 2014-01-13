using System;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.Trw {

    [Persistent("TrwRefCacheFlow")]
    [NavigationItem("Trw")]
    [DefaultProperty("Code")]
    public class TrwRefCashFlow : csCComponent, ITreeNode {

        private String _Code;
        [Size(32)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        private String _Name;
        [Size(256)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        private String _NameFull;
        [Size(256)]
        [VisibleInLookupListView(true)]
        [VisibleInListView(true)]
        public String NameFull {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }

        private Boolean _IsSelectabled;
        [VisibleInLookupListView(true)]
        public Boolean IsSelectabled {
            get { return _IsSelectabled; }
            set { SetPropertyValue<Boolean>("IsSelectabled", ref _IsSelectabled, value); }
        }

        private TrwRefCashFlow _TopRef;
        [Association("TrwRefCacheFlow-Childs")]
        public TrwRefCashFlow TopRef {
            get { return _TopRef; }
            set { SetPropertyValue<TrwRefCashFlow>("TopRef", ref _TopRef, value); }
        }

        [Aggregated]
        [Association("TrwRefCacheFlow-Childs")]
        public XPCollection<TrwRefCashFlow> Childs {
            get {
                return GetCollection<TrwRefCashFlow>("Childs");
            }
        }

        public TrwRefCashFlow(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Удалять нельзя");
        }

        String ITreeNode.Name {
            get { return Code; }
        }
        ITreeNode ITreeNode.Parent {
            get { return TopRef; }
        }
        private IBindingList _Children;
        IBindingList ITreeNode.Children {
            get {
                if (_Children == null)
                    _Children = new BindingList<TrwRefCashFlow>(Childs);
                return _Children;
            }
        }

    }

}
