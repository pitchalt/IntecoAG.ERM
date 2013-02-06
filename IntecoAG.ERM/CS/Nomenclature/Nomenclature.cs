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

namespace IntecoAG.ERM.CS.Nomenclature
{
    /// <summary>
    /// ����� ������������, ���������� ���������� ������ (���������, ������, �������)
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("csNomenclature")]
    public abstract partial class Nomenclature : BaseObject, ICategorizedItem
    {
        public Nomenclature() : base() { }
        public Nomenclature(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// ��� ���� ������������
        /// </summary>
        private string _Code;
        public string Code {
            get { return _Code; }
            set { if (_Code != value) SetPropertyValue("Code", ref _Code, value); }
        }

        /// <summary>
        /// ������������ ���� ������������
        /// </summary>
        private string _Name;
        public string Name {
            get { return _Name; }
            set { if (_Name != value) SetPropertyValue("Name", ref _Name, value); }
        }


        #endregion


        #region ������

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
            Res = this.Code + " " + this.Name;
            return Res;
        }

        #endregion


        public csNomenclatureType Category {
            get {
                return NomenclatureType;
            }
        }

        ITreeNode ICategorizedItem.Category {
            get {
                return NomenclatureType;
            }
            set {
                NomenclatureType = (csNomenclatureType) value;
            }
        }
    }

}