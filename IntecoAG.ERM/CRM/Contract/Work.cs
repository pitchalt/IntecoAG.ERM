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
using DevExpress.Persistent.Base;


//*****************************************************************************************************//
// Из документации.
//*****************************************************************************************************//
// Stage – этап работ 
// - DateBegin, DateEnd – атрибуты задающие дату начала и окончания этапа посредством 
// CS.CRM.Contract..Time этот атрибут отличается от DateStart и DateStop, которые задают 
// абсолютные даты эти атрибуты должны автоматически заполняться через ввод DateBegin и DateEnd
//
// Каждый Stage создаёт два Event (начала и завершения этапа) для Contract. Эти даты модифицируются 
// автоматически
//*****************************************************************************************************//


namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс для поддержки структуры работ в этапах проекта
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmWork")]
    public partial class crmWork : Stage
    {
        public crmWork() : base() { }
        public crmWork(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {

            // Создание событий для контракта
            _EventBegin = new crmEvent(this.Session);
            _EventBegin.Contract = this.Contract;

            _EventEnd = new crmEvent(this.Session);
            _EventEnd.Contract = this.Contract;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Событие начала работы
        /// </summary>
        private CRM.Contract.crmEvent _EventBegin;
        public CRM.Contract.crmEvent EventBegin {
            get { return _EventBegin; }
            set {
                if (_EventBegin != value) {
                    SetPropertyValue("EventBegin", ref _EventBegin, value);
                }
            }
        }

        /// <summary>
        /// Событие конца работы
        /// </summary>
        private CRM.Contract.crmEvent _EventEnd;
        public CRM.Contract.crmEvent EventEnd {
            get { return _EventEnd; }
            set { if (_EventEnd != value) SetPropertyValue("EventEnd", ref _EventEnd, value); }
        }


        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
            Res = base.ToString() + ", период: " + this.EventBegin.ToString() + " - " + this.EventEnd.ToString();
            return Res;
        }

        #endregion

    }

}