using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{
    [NonPersistent]   // ����� ����������� ������� �� �������� � ���� ���������
    public class ObjectCollection : BaseObject //XPObject
    {
        public ObjectCollection(Session session) : base(session) { }

        
        #region ���� ������

        /// <summary>
        /// ���
        /// </summary>
        private string _Code;
        [DisplayName("5-���")]
        [Size(5)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        #endregion
    }

}