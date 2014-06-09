using System;
using System.Collections.Generic;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.Cost {

    [Persistent("FmCostTask")]
    public abstract class FmCostTask : csCComponent, ITreeNode {

        [Size(16)]
        public String Code;

        public virtual String CodeFull {
            get {
                if (UpTask != null) {
                    return UpTask.CodeFull + "." + Code;
                }
                else
                    return Code;
            }
        }

//        [Size(60)]
//        public String Name;

        [Association("FmCostTaskList-FmCostTask")]
        public FmCostTaskList UpTask;

        public FmCostTask(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        protected virtual String TreeName {
            get {
                return Code;
            }
        }
        protected virtual ITreeNode TreeParent {
            get {
                return UpTask;
            }
        }
        protected abstract IBindingList TreeChildren { get; }

        String ITreeNode.Name {
            get { return Code; }
        }

        ITreeNode ITreeNode.Parent {
            get { return TreeParent; }
        }

        IBindingList ITreeNode.Children {
            get { return TreeChildren; }
        }
    }

}
