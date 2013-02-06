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

namespace IntecoAG.ERM.CS.Work {

    /// <summary>
    /// Класс Work представляет базовую функциональность
    /// </summary>
    [Persistent("csWork"), OptimisticLocking(false)]
    public abstract class Work : XPObject, IHDenormalization<Work>
    {
        public Work() : base() { }
        public Work(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            CrossRefAdd(this);
        }

        
        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата начала работы
        /// </summary>
        private DateTime _DateStart;
        public DateTime DateStart {
            get { return _DateStart; }
            set { if (_DateStart != value) SetPropertyValue("DateStart", ref _DateStart, value); }
        }

        /// <summary>
        /// Дата конца работы
        /// </summary>
        private DateTime _DateStop;
        public DateTime DateStop {
            get { return _DateStop; }
            set { if (_DateStop != value) SetPropertyValue("DateStop", ref _DateStop, value); }
        }

        #endregion


        #region Разузлование дерева:  TopWork - SubWorks

        // Ассоциация список аналитик нижнего уровня
        Work _TopWork;
        [Association("SubWorks"), Aggregated]
        public Work TopWork {
            get { return _TopWork; }
            set {
                if (IsLoading) {
                    _TopWork = value;
                }
                else {
                    List<Work> fd = new List<Work>(this.FullDownLevel);
                    foreach (var downobj in fd)
                        downobj.CrossRefRemove(downobj);
                    SetPropertyValue<Work>("TopWork", ref _TopWork, value);
                    foreach (var downobj in fd)
                        downobj.CrossRefAdd(downobj);
                }
            }
        }

        [Association("SubWorks", typeof(Work))]
        public XPCollection<Work> SubWorks {
            get { return GetCollection<Work>("SubWorks"); }
        }



        // Ассоциация отражающая полную прошивку дерева в соответствии с иерархией
        [Association("WorkCrossRef", typeof(Work), UseAssociationNameAsIntermediateTableName = true)]
        public IList<Work> FullDownLevel {
            get { return GetCollection<Work>("FullDownLevel"); }
        }

        [Association("WorkCrossRef", typeof(Work), UseAssociationNameAsIntermediateTableName = true)]
        public IList<Work> FullUpLevel {
            get { return GetCollection<Work>("FullUpLevel"); }
        }



        // Методы для рекурсивного изменения связи WorkCrossRef в случаях изменения иерархии
        protected void CrossRefAdd(Work obj) {
            FullDownLevel.Add(obj);
            if (TopWork != null)
                TopWork.CrossRefAdd(obj);
        }

        protected void CrossRefRemove(Work obj) {
            if (obj != this)
                FullDownLevel.Remove(obj);
            if (TopWork != null)
                TopWork.CrossRefRemove(obj);
        }

        #region Разузлование дерева: реализация интерфейса

        // Ассоциация список аналитик нижнего уровня
        public Work UpLevel {
            get { return TopWork; }
            set { TopWork = value; }
        }

        public IList<Work> DownLevel {
            get { return SubWorks; }
        }

        #endregion

        #endregion

    }

}