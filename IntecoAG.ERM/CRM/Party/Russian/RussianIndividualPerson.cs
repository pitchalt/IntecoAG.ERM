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
using System.ComponentModel;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// ����� RussianIndividualPerson, �������������� �������������� ������� ����������� ��� ��������� (�������) ��������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmRussianIndividualPerson")]
    public partial class RussianIndividualPerson : LegalPerson
    {
        public RussianIndividualPerson(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// INN
        /// </summary>
        private string _INN;
        public string INN {
            get { return _INN; }
            set { SetPropertyValue("INN", ref _INN, value); }
        }

        /// <summary>
        /// OGRIP
        /// </summary>
        private string _OGRIP;
        public string OGRIP {
            get { return _OGRIP; }
            set { SetPropertyValue("OGRIP", ref _OGRIP, value); }
        }


        /// <summary>
        /// PhysicalPerson
        /// </summary>
        private PhysicalPerson _PhysicalPerson;
        public PhysicalPerson PhysicalPerson {
            get { return _PhysicalPerson; }
            set { SetPropertyValue("PhysicalPerson", ref _PhysicalPerson, value); }
        }

        #endregion


        #region ������

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ". " + ((_PhysicalPerson == null) ? "" : _PhysicalPerson.Name);
        }

        #endregion

    }

}