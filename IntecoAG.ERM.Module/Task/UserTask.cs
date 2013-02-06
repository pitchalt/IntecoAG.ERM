using System;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.Metadata;


namespace IntecoAG.ERM.Module {

    /// <summary>
    /// �����, ����������� ����������� ������������ � ���� ����������� ������-������
    /// � ��������� ������ (2011-06-23) �������������� ������ �� ����� TIn � TOut ������ �� ������� ����������� ����� XAF'��
    /// TIn � TOut - �������� � ��������� ��������� (���� �������, ��������� �������� ������������� � �.�.).
    /// TIn � TOut - ������������� � XML ����. ����������� �� ���������� (Delay)
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    //[DefaultClassOptions]
    [Persistent]
    public partial class UserTask : BaseUserTask {

        public UserTask(Session session)
            : base(session) {
        }


        #region ��������

        /// <summary>
        /// ����������� ������ � ������ �������������, ��������, "IntecoAG.ERM.TaskInvoice" 
        /// </summary>
        public override string Name {
            get { return this.GetType().ToString(); }
        }

        static string _Subject = "New Invoice Subject";
        public override string Subject {
            get { return _Subject; }
        }

        static string _Description = "New Invoice Description";
        public override string Description {
            get { return _Description; }
        }

        #endregion

    }

}
