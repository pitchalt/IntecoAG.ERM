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
using DevExpress.Xpo;

// === IntecoAG namespaces ===
using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.FM.Subject
{

    /// <summary>
    /// Класс AbstractSubject представляет базовую функциональность
    /// </summary>
    [Persistent("csAbstractSubject"), OptimisticLocking(false)]
    public abstract partial class AbstractSubject : XPObject, IHDenormalization<AbstractSubject>
    {
        public AbstractSubject() : base() { }
        public AbstractSubject(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            CrossRefAdd(this);
        }

        
        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion


        #region Разузлование дерева:  TopAbstractSubject - SubAbstractSubjects

        // Ассоциация список аналитик нижнего уровня
        AbstractSubject _TopAbstractSubject;
        [Association("SubAbstractSubjects"), Aggregated]
        public AbstractSubject TopAbstractSubject {
            get { return _TopAbstractSubject; }
            set {
                if (IsLoading) {
                    _TopAbstractSubject = value;
                }
                else {
                    List<AbstractSubject> fd = new List<AbstractSubject>(this.FullDownLevel);
                    foreach (var downobj in fd)
                        downobj.CrossRefRemove(downobj);
                    SetPropertyValue<AbstractSubject>("TopAbstractSubject", ref _TopAbstractSubject, value);
                    foreach (var downobj in fd)
                        downobj.CrossRefAdd(downobj);
                }
            }
        }

        [Association("SubAbstractSubjects", typeof(AbstractSubject))]
        public XPCollection<AbstractSubject> SubAbstractSubjects {
            get { return GetCollection<AbstractSubject>("SubAbstractSubjects"); }
        }



        // Ассоциация отражающая полную прошивку дерева в соответствии с иерархией
        [Association("AbstractSubjectCrossRef", typeof(AbstractSubject), UseAssociationNameAsIntermediateTableName = true)]
        public IList<AbstractSubject> FullDownLevel {
            get { return GetCollection<AbstractSubject>("FullDownLevel"); }
        }

        [Association("AbstractSubjectCrossRef", typeof(AbstractSubject), UseAssociationNameAsIntermediateTableName = true)]
        public IList<AbstractSubject> FullUpLevel {
            get { return GetCollection<AbstractSubject>("FullUpLevel"); }
        }



        // Методы для рекурсивного изменения связи AbstractSubjectCrossRef в случаях изменения иерархии
        protected void CrossRefAdd(AbstractSubject obj) {
            FullDownLevel.Add(obj);
            if (TopAbstractSubject != null)
                TopAbstractSubject.CrossRefAdd(obj);
        }

        protected void CrossRefRemove(AbstractSubject obj) {
            if (obj != this)
                FullDownLevel.Remove(obj);
            if (TopAbstractSubject != null)
                TopAbstractSubject.CrossRefRemove(obj);
        }

        #region Разузлование дерева: реализация интерфейса

        // Ассоциация список аналитик нижнего уровня
        public AbstractSubject UpLevel {
            get { return TopAbstractSubject; }
            set { TopAbstractSubject = value; }
        }

        public IList<AbstractSubject> DownLevel {
            get { return SubAbstractSubjects; }
        }

        #endregion

        #endregion

    }

}