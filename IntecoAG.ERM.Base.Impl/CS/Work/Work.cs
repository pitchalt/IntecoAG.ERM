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

// === IntecoAG namespaces ===
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.CS.Work {

    /// <summary>
    /// Класс Work представляет базовую функциональность
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("csWork")]
    public abstract class csWork : BaseObject //, IHDenormalization<csWork>, ITreeNode
    {
        public csWork(Session session) : base(session) { }
//        public csWork(Session session, VersionStates state) : base(session, state) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
//            CrossRefAdd(this);
        }

        
        #region ПОЛЯ КЛАССА
        public static string Delimiter = ".";
        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата начала события
        /// </summary>
        private DateTime _DateBegin;
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }

        /// <summary>
        /// Дата конца события
        /// </summary>
        private DateTime _DateEnd;
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        }
        /// <summary>
        /// Code
        /// </summary>
        [Size(10)]
        private string _Code;
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        /// <summary>
        /// Name - описание
        /// </summary>
        [Size(70)]
        private string _Name;
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false)]
        private string _Description;
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }


        /// <summary>
        /// Current - Ссылка на версию с признаком VersionState = CURRENT
        /// </summary>
        protected csWork _Current;
        public csWork Current {
            get { return _Current; }
            set { SetPropertyValue<csWork>("Current", ref _Current, value); }
        }

        #endregion

/*
        #region Разузлование дерева:  TopWork - SubWorks

        // Ассоциация список аналитик нижнего уровня
        csWork _TopWork;
        [Browsable(false)]
        [Association("SubWorks")]
        public virtual csWork TopWork {
            get { return _TopWork; }
            set {
                if (IsLoading) {
                    _TopWork = value;
                }
                else {
                    List<csWork> fd = new List<csWork>(this.FullDownLevel);
                    foreach (var downobj in fd)
                        downobj.CrossRefRemove(downobj);
                    SetPropertyValue<csWork>("TopWork", ref _TopWork, value);
                    foreach (var downobj in fd)
                        downobj.CrossRefAdd(downobj);
                }
            }
        }

        [Association("SubWorks", typeof(csWork))]
        [Browsable(false)]
        public virtual XPCollection<csWork> SubWorks {
            get { return GetCollection<csWork>("SubWorks"); }
        }



        #region Ассоциация отражающая полную прошивку дерева в соответствии с иерархией
        [Browsable(false)]
        [Association("csWorkCrossRef", typeof(csWork), UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<csWork> FullDownLevel {
            get { return GetCollection<csWork>("FullDownLevel"); }
        }
        [Browsable(false)]
        [Association("csWorkCrossRef", typeof(csWork), UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<csWork> FullUpLevel {
            get { return GetCollection<csWork>("FullUpLevel"); }
        }
        
        #endregion


        #region Методы для рекурсивного изменения связи WorkCrossRef в случаях изменения иерархии
        protected void CrossRefAdd(csWork obj) {
            FullDownLevel.Add(obj);
            if (TopWork != null)
                TopWork.CrossRefAdd(obj);
        }

        protected void CrossRefRemove(csWork obj) {
            if (obj != this)
                FullDownLevel.Remove(obj);
            if (TopWork != null)
                TopWork.CrossRefRemove(obj);
        }
        #endregion

        #region IHDenormalization<csWork> Members

        csWork  IHDenormalization<csWork>.UpLevel
        {
            get { return TopWork; }
            set { TopWork = value; }
        }

        IList<csWork>  IHDenormalization<csWork>.DownLevel
        {
            get { return SubWorks; }
        }

        IList<csWork>  IHDenormalization<csWork>.FullDownLevel
        {
            get { return FullDownLevel; }
        }

        IList<csWork>  IHDenormalization<csWork>.FullUpLevel
        {
            get { return FullUpLevel; }
        }

        #endregion

        #endregion


        #region МЕТОДЫ

        #endregion



        #region ITreeNode Members

        IBindingList ITreeNode.Children {
            get { return SubWorks; }
        }

        string ITreeNode.Name {
            get { return Code; }
        }

        ITreeNode ITreeNode.Parent {
            get { return TopWork; }
        }

        #endregion
 */ 
    }

/*
    /// <summary>
    /// Связывающий класс
    /// </summary>
    public class WorkSelfRef : BaseObject
    {
        public WorkSelfRef(Session session) : base(session) { }

        private csWork _UpWork;
        [Association("WorkSelfRef-UpWorks")]
        public csWork UpWork {
            get { return _UpWork; }
            set { SetPropertyValue("UpWork", ref _UpWork, value); }
        }

        private csWork _DownWork;
        [Association("WorkSelfRef-DownWorks")]
        public csWork DownWork {
            get { return _DownWork; }
            set { SetPropertyValue("DownWork", ref _DownWork, value); }
        }
    }
*/
}