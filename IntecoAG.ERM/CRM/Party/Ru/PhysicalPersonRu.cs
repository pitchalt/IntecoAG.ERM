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
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("crmPartyPhysicalPersonRu")]
    public partial class crmPhysicalPersonRu : crmPhysicalPerson
    {
        public crmPhysicalPersonRu(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// INN
        /// </summary>
//        private string _INN;
//        public string INN {
//            get { return _INN; }
//            set { SetPropertyValue("INN", ref _INN, value); }
//        }

        #endregion


        #region ������

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
//        public override string ToString()
//        {
//            return Name; //+ ". " + ((_PhysicalPerson == null) ? "" : _PhysicalPerson.Name);
//        }

        #endregion

    }

}