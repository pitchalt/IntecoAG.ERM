using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.CRM.Counters {

    // ����� ��������� ������� ���������������� ������� ���������� � ������� �����������.
    // ����� ������� ��������� �������, ����������, ����� �� �������� � �� ��� ��������� ���������. 
    // ��� ����� �� ��������� ��������� �����, �������������� ������ �� ��������� ������� 

    // ��� XPLiteObject �� ��������� �� ����������� Optimistic Locking

    [DefaultClassOptions] // �������� ��� ���������
    public class RegistrationLogISNGenerator : XPLiteObject {
        public RegistrationLogISNGenerator(Session session) : base(session) {
        }

        #region ���� ������

        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public int ISN;

        #endregion

        #region �������� ������

        #endregion

        #region ������

        public int ReserveNumber() {
            this.Session.BeginTransaction();
            //RegistrationLogISNGenerator regLogISN = new RegistrationLogISNGenerator(this.Session);
            //regLogISN.Save();
            this.Save();
            this.Session.FlushChanges();
            this.Session.CommitTransaction();
            return this.ISN;
        }

        #endregion

    }

}
