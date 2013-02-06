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

using IntecoAG.ERM.CS.Work;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� ContractWork, �������������� ���� ����� �� ��������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmContractWork")]
    public partial class ContractWork : Work, IVersionSupport
    {
        public ContractWork() : base() { }
        public ContractWork(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        ///// <summary>
        ///// Description - ��������
        ///// </summary>
        //private string _Description;
        //public string Description {
        //    get { return _Description; }
        //    set { SetPropertyValue("Description", ref _Description, value); }
        //}

        #endregion


        #region ������

        ///// <summary>
        ///// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion



        #region ���� ������ ��� ��������� ������������

        /// <summary>
        /// PrevVersion - ������ �� ���������� ������ (null � ������ ���������� �������)
        /// </summary>
        protected IVersionSupport _PrevVersion;
        public IVersionSupport PrevVersion {
            get { return _PrevVersion; }
            set { SetPropertyValue("PrevVersion", ref _PrevVersion, value); }
        }


        /// <summary>
        /// VersionState - ������ ������
        /// </summary>
        protected VersionStates _VersionState;
        public VersionStates VersionState {
            get { return _VersionState; }
            set { SetPropertyValue("VersionState", ref _VersionState, value); }
        }


        /// <summary>
        /// IsCurrent - ������� ������� ������ (�� �� �� �����, ��� CURRENT ��� VERSION_CURRENT)
        /// </summary>
        protected bool _IsCurrent;
        public bool IsCurrent {
            get { return _IsCurrent; }
            set { SetPropertyValue("IsCurrent", ref _IsCurrent, value); }
        }


        /// <summary>
        /// IsOfficial - ������� ���������� �������� ������ ������
        /// </summary>
        protected bool _IsOfficial;
        public bool IsOfficial {
            get { return _IsOfficial; }
            set { SetPropertyValue("IsOfficial", ref _IsOfficial, value); }
        }


        /// <summary>
        /// Current - ������ �� ������ � ��������� VersionState = CURRENT
        /// </summary>
        protected IVersionSupport _Current;
        public IVersionSupport Current {
            get { return _Current; }
            set { SetPropertyValue<IVersionSupport>("Current", ref _Current, value); }
        }


        /// <summary>
        /// LinkToEditor - ������ �� ��������, ������� � ��������� ��� ��������������� ������ ������������� ���������
        /// </summary>
        protected IVersionSupport _LinkToEditor;
        public IVersionSupport LinkToEditor {
            get { return _LinkToEditor; }
            set { SetPropertyValue("LinkToEditor", ref _LinkToEditor, value); }
        }

        #endregion

        #region ������ ��� ��������� ������������

        public List<IVersionSupport> GetDependenceObjects() {
            List<IVersionSupport> htDependenceObjects = new List<IVersionSupport>();
            
            //if (LinkToEditor != null) {
            //    htDependenceObjects.Add(this.LinkToEditor);
            //}

            //if (UpLevel != null) {
            //    htDependenceObjects.Add(this.UpLevel);
            //}

            //if (TopWork != null) {
            //    htDependenceObjects.Add(this.TopWork);
            //}

            //if (SubWorks != null) {
            //    if (!SubWorks.IsLoaded) SubWorks.Load();
            //    foreach (Work cd in SubWorks) {
            //        htDependenceObjects.Add(cd);
            //    }
            //}
            
            return htDependenceObjects;
        }

        public IVersionSupport CreateCopyObjects() {
            ContractWork objCopy = new ContractWork(this.Session);

            ContractWork scv = this;

            objCopy.DateStart = scv.DateStart;
            objCopy.DateStop = scv.DateStop;
            objCopy.Current = scv.Current;

            objCopy.UpLevel = scv.UpLevel;
            objCopy.TopWork = scv.TopWork;
            objCopy.IsCurrent = false;
            objCopy.IsOfficial = false;
            objCopy.LinkToEditor = scv.LinkToEditor;
            objCopy.VersionState = scv.VersionState;

            return (IVersionSupport)objCopy;
        }

        public void SetReferences(CreatetVersionHelper CopyObjectHelper) {
            //this._Current = (IVersionSupport)CopyObjectHelper.GetCopyObject(_Current);
            //this._LinkToEditor = (IVersionSupport)CopyObjectHelper.GetCopyObject(_LinkToEditor);
        }

        public void SetReferences(CreatetVersionHelper CopyObjectHelper, VersionStates VersionState) {
            //this._Current = (IVersionSupport)CopyObjectHelper.GetCopyObject(_Current);
            //this._Current.VersionState = VersionState;

            //this._LinkToEditor = (IVersionSupport)CopyObjectHelper.GetCopyObject(_LinkToEditor);
            //this._LinkToEditor.VersionState = VersionState;

            this.VersionState = VersionState;
        }

        ///// <summary>
        ///// ��. �������� � ����������
        ///// </summary>
        ///// <param name="CopyObjectHelper"></param>
        ///// <param name="VersionState"></param>
        //public abstract void SetVersionState(CreateVersionHelper CopyObjectHelper, VersionStates VersionState);

        /// <summary>
        /// ��. �������� � ����������
        /// </summary>
        /// <param name="htObjects"></param>
        public void SetVersionAsCurrent() { }

        ///// <summary>
        ///// ��. �������� � ����������
        ///// </summary>
        ///// <param name="htObjects"></param>
        //public abstract void SetLinkToCurrent(VersionRecord vr);

        #endregion

    }

}