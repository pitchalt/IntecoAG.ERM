#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

// === IntecoAG namespaces ===
using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.FM.Subject
{

    /// <summary>
    /// Класс AbstractSubject представляет базовую функциональность
    /// </summary>
    //[NonPersistent]
    [Persistent("fmAbstractSubject")]
    [DefaultProperty("Code")]
    public abstract partial class fmAbstractSubject : BaseObject, ITreeNode //IHDenormalization<fmAbstractSubject>
    {
        public fmAbstractSubject(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
//            CrossRefAdd(this);
        }

        
        #region ПОЛЯ КЛАССА
        private string _Code;
        private string _Name;
        private DateTime? _DateBegin;
        private DateTime? _DateEnd;
        [Persistent("IsClosed")]
        private Boolean _IsClosed;
        #endregion


        #region СВОЙСТВА КЛАССА
        [RuleRequiredField("fmAbstractSubject.RequiredCode", "Save")]
        [Size(15)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        [Size(70)]
        [VisibleInLookupListView(true)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }
        [RuleRequiredField("fmAbstractSubject.DateBegin", "Save")]
        public DateTime? DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime?>("DateBegin", ref _DateBegin, value); }
        }
        public DateTime? DateEnd {
            get { return _DateEnd; }
            set {
                SetPropertyValue<DateTime?>("DateEnd", ref _DateEnd, value);
                if (!IsLoading) {
                    IsClosedUpdate();
                }
            }
        }
        [PersistentAlias("_IsClosed")]
        public Boolean IsClosed {
            get { return _IsClosed; }
        }
        protected void IsClosedUpdate() {
            _IsClosed = DateEnd != null;
            OnChanged("IsClosed");
        }
        [Action(PredefinedCategory.Edit, Caption = "ReOpen", ToolTip = "ReOpen order")]
        public void ReOpen() {
            DateEnd = null;
        }
        #endregion

        [Browsable(false)]
        public abstract System.ComponentModel.IBindingList Children {
            get;
        }

        [Browsable(false)]
        public abstract ITreeNode Parent {
            get;
        }

        #region ITreeNode
        System.ComponentModel.IBindingList ITreeNode.Children {
            get { return Children; }
        }

        string ITreeNode.Name {
            get { return Name; }
        }

        ITreeNode ITreeNode.Parent {
            get { return Parent; }
        }
        #endregion

        /*
        #region Разузлование дерева:  TopAbstractSubject - SubAbstractSubjects

        // Ассоциация список аналитик нижнего уровня
        fmAbstractSubject _TopAbstractSubject;
        [Association("SubAbstractSubjects"), Aggregated]
        public fmAbstractSubject TopAbstractSubject {
            get { return _TopAbstractSubject; }
            set {
                if (IsLoading) {
                    _TopAbstractSubject = value;
                }
                else {
                    List<fmAbstractSubject> fd = new List<fmAbstractSubject>(this.FullDownLevel);
                    foreach (var downobj in fd)
                        downobj.CrossRefRemove(downobj);
                    SetPropertyValue<fmAbstractSubject>("TopAbstractSubject", ref _TopAbstractSubject, value);
                    foreach (var downobj in fd)
                        downobj.CrossRefAdd(downobj);
                }
            }
        }

        [Association("SubAbstractSubjects", typeof(fmAbstractSubject))]
        public XPCollection<fmAbstractSubject> SubAbstractSubjects {
            get { return GetCollection<fmAbstractSubject>("SubAbstractSubjects"); }
        }



        #region Ассоциация отражающая полную прошивку дерева в соответствии с иерархией
        
        [Association("AbstractSubjectCrossRef", typeof(AbstractSubject), UseAssociationNameAsIntermediateTableName = true)]
        public IList<AbstractSubject> FullDownLevel {
            get { return GetCollection<AbstractSubject>("FullDownLevel"); }
        }

        [Association("AbstractSubjectCrossRef", typeof(AbstractSubject), UseAssociationNameAsIntermediateTableName = true)]
        public IList<AbstractSubject> FullUpLevel {
            get { return GetCollection<AbstractSubject>("FullUpLevel"); }
        }
        

        #region Правая сторона связи
        [Association("AbstractSubjectSelfRef-UpAbstractSubjects")]
        protected IList<AbstractSubjectSelfRef> RightLinks {
            get {
                return GetList<AbstractSubjectSelfRef>("RightLinks");
            }
        }
        [ManyToManyAlias("RightLinks", "AbstractSubject")]
        public IList<fmAbstractSubject> FullUpLevel {
            get { return GetList<fmAbstractSubject>("FullUpLevel"); }
        }
        #endregion

        #region Левая сторона связи
        [Association("AbstractSubjectSelfRef-DownAbstractSubjects")]
        protected IList<AbstractSubjectSelfRef> LeftLinks {
            get {
                return GetList<AbstractSubjectSelfRef>("LeftLinks");
            }
        }
        [ManyToManyAlias("LeftLinks", "AbstractSubject")]
        public IList<fmAbstractSubject> FullDownLevel {
            get { return GetList<fmAbstractSubject>("FullDownLevel"); }
        }
        #endregion

        #endregion


        #region Методы для рекурсивного изменения связи AbstractSubjecteCrossRef в случаях изменения иерархии
        protected void CrossRefAdd(fmAbstractSubject obj) {
            FullDownLevel.Add(obj);
            if (TopAbstractSubject != null)
                TopAbstractSubject.CrossRefAdd(obj);
        }

        protected void CrossRefRemove(fmAbstractSubject obj) {
            if (obj != this)
                FullDownLevel.Remove(obj);
            if (TopAbstractSubject != null)
                TopAbstractSubject.CrossRefRemove(obj);
        }
        #endregion

        #region Разузлование дерева: реализация интерфейса

        // Ассоциация список аналитик нижнего уровня
        public fmAbstractSubject UpLevel {
            get { return TopAbstractSubject; }
            set { TopAbstractSubject = value; }
        }

        public IList<fmAbstractSubject> DownLevel {
            get { return SubAbstractSubjects; }
        }

        #endregion

        #endregion
*/

    }

/*
    /// <summary>
    /// Связывающий класс
    /// </summary>
    public class AbstractSubjectSelfRef : BaseObject
    {
        public AbstractSubjectSelfRef(Session session) : base(session) { }

        private fmAbstractSubject _UpAbstractSubject;
        [Association("AbstractSubjectSelfRef-UpAbstractSubjects")]
        public fmAbstractSubject UpAbstractSubject {
            get { return _UpAbstractSubject; }
            set { SetPropertyValue("UpAbstractSubject", ref _UpAbstractSubject, value); }
        }

        private fmAbstractSubject _DownAbstractSubject;
        [Association("AbstractSubjectSelfRef-DownAbstractSubjects")]
        public fmAbstractSubject DownAbstractSubject {
            get { return _DownAbstractSubject; }
            set { SetPropertyValue("DownAbstractSubject", ref _DownAbstractSubject, value); }
        }
    }
*/

}