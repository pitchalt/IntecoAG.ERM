using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base.General;
using System.ComponentModel;

namespace IntecoAG.ERM.CS
{
    ///// <summary>
    ///// Назначение.
    ///// </summary>
    //[NavigationItem]
    //public abstract partial class TreeStructure : BaseObject, ITreeNode
    //{

        [NavigationItem]
        public abstract class TreeStructure : BaseObject, ITreeNode
        {
            private string name;

            protected abstract ITreeNode Parent { get; }
            protected abstract IBindingList Children  { get; }

            public TreeStructure(Session session) : base(session) { }
            public string Name {
                get { return name; }
                set { SetPropertyValue("Name", ref name, value); }
            }

            #region ITreeNode
            IBindingList ITreeNode.Children {
                get { return Children; }
            }
            string ITreeNode.Name {
                get { return Name; }
            }
            ITreeNode ITreeNode.Parent {
                get { return Parent; }
            }
            #endregion
        }





    //    protected abstract ITreeNode Parent { get; }
    //    protected abstract IBindingList Children { get; }
    //    public TreeStructure(Session session) : base(session) { }

    //    ///// <summary>
    //    ///// Наименование
    //    ///// </summary>
    //    //private string _Code;
    //    //public string Code {
    //    //    get { return _Code; }
    //    //    set { SetPropertyValue("Code", ref _Code, value); }
    //    //}

    //    /// <summary>
    //    /// Код
    //    /// </summary>
    //    private string _Code;
    //    public string Code {
    //        get { return _Code; }
    //        set { SetPropertyValue("Code", ref _Code, value); }
    //    }

    //    #region ITreeNode
    //    IBindingList ITreeNode.Children {
    //        get { return Children; }
    //    }
    //    string ITreeNode.Code {
    //        get { return Code; }
    //    }
    //    ITreeNode ITreeNode.Parent {
    //        get { return Parent; }
    //    }
    //    #endregion
    //}

}