using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
//using DevExpress.Persistent.Base.General;
//using System.ComponentModel;
//using DevExpress.Data.Filtering;
//using System.Collections.Generic;
//using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// ����� �������� ����� ����������� ������������� ������ ������� �������
    /// </summary>
    //[DefaultClassOptions]
    [NonPersistent]
    public abstract class BaseRecord : BaseObject
    {
        public BaseRecord() : base() { }
        public BaseRecord(Session session) : base(session) { }
        public BaseRecord(Session session, Guid StrateIndex) : base(session) { this.StrateIndex = StrateIndex; }

        public override void AfterConstruction() {
            base.AfterConstruction();

            _CreationDateTime = DateTime.Now;
        }


        #region ���� ������ ��� ��������� ��������� ����������

        /// <summary>
        /// CreationDateTime - ���� �������� ������
        /// </summary>
        protected DateTime _CreationDateTime;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public DateTime CreationDateTime {
            get { return _CreationDateTime; }
            set { SetPropertyValue("CreationDateTime", ref _CreationDateTime, value); }
        }

        /// <summary>
        /// StrateIndex - ������ ����������
        /// </summary>
        protected Guid _StrateIndex;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public Guid StrateIndex {
            get { return _StrateIndex; }
            set { SetPropertyValue("StrateIndex", ref _StrateIndex, value); }
        }

        #endregion

    }

}