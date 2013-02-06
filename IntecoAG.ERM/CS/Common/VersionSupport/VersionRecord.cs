using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;
using DevExpress.Xpo.Metadata;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// ����� �������� ����� ����������� ������������� ������ ������� �������
    /// </summary>
    [NonPersistent]
    public abstract partial class VersionRecord : BaseObject, IVersionSupport
    {
        protected VersionRecord(Session session): base(session) { }
        protected VersionRecord(Session session, VersionStates state): base(session) {
            this.VersionState = state;
            this.VersionAfterConstruction();
        }
        public virtual void VersionAfterConstruction() {             
        }

        [Browsable(false)]
        public new bool IsLoading {
            get { return base.IsLoading || ((IVersionSupport)this).IsProcessCloning; }
        }

        #region ���� ������ ��� ��������� ������������
        private Boolean _IsProcessCloning;
        [NonPersistent]
        Boolean IVersionSupport.IsProcessCloning { 
            get { return _IsProcessCloning; }
            set { SetPropertyValue<Boolean>("IsProcessCloning", ref _IsProcessCloning, value); }
        }
        /// <summary>
        /// PrevVersion - ������ �� ���������� ������ (null � ������ ���������� �������)
        /// </summary>
        protected IVersionSupport _PrevVersion;
        [Browsable(false)]
        public IVersionSupport PrevVersion {
            get { return _PrevVersion; }
            set { SetPropertyValue("PrevVersion", ref _PrevVersion, value); }
        }


        /// <summary>
        /// VersionState - ������ ������
        /// </summary>
        protected VersionStates _VersionState;
        [Browsable(false)]
        public VersionStates VersionState {
            get { return _VersionState; }
            set {
                try {
                    SetPropertyValue<VersionStates>("VersionState", ref _VersionState, value);
                }
                catch (Exception e) {
                    String s = e.ToString();
                }
            }
        }


        /// <summary>
        /// IsCurrent - ������� ������� ������ (�� �� �� �����, ��� CURRENT ��� VERSION_CURRENT)
        /// </summary>
        protected bool _IsCurrent;
        [Browsable(false)]
        public bool IsCurrent {
            get { return _IsCurrent; }
            set { SetPropertyValue("IsCurrent", ref _IsCurrent, value); }
        }


        /// <summary>
        /// IsOfficial - ������� ���������� �������� ������ ������
        /// </summary>
        protected bool _IsOfficial;
        [Browsable(false)]
        public bool IsOfficial {
            get { return _IsOfficial; }
            set { SetPropertyValue("IsOfficial", ref _IsOfficial, value); }
        }


        ///// <summary>
        ///// Current - ������ �� ������ � ��������� VersionState = CURRENT
        ///// </summary>
        //protected IVersionSupport _Current;
        ////[Browsable(false)]
        //public IVersionSupport Current {
        //    get { return _Current; }
        //    set { SetPropertyValue<IVersionSupport>("Current", ref _Current, value); }
        //}


        /// <summary>
        /// LinkToEditor - ������ �� ��������, ������� � ��������� ��� ��������������� ������ ������������� ���������
        /// </summary>
        protected IVersionSupport _LinkToEditor;
        [Browsable(false)]
        public IVersionSupport LinkToEditor {
            get { return _LinkToEditor; }
            set { SetPropertyValue("LinkToEditor", ref _LinkToEditor, value); }
        }

        /// <summary>
        /// MainObject - ������ �� �������� ������, ��� �������� ��������� ��� ������
        /// </summary>
        //protected object _MainObject;
        [Browsable(false)]
        public virtual object MainObject {
            get { return null; }
            //set { SetPropertyValue<object>("MainObject", ref _MainObject, value); }
        }

        #endregion

        #region ������ ��� ��������� ������������

        public virtual List<IVersionSupport> GetFirstDependentList(IVersionSupport sourceObj, List<IVersionSupport> dependentObjectList, VersionHelper vHelper) { 
            return new List<IVersionSupport>();
        }

        public virtual IVersionSupport CreateNewVersion(IVersionSupport sourceObj, VersionHelper vHelper) {
            IVersionSupport newVersionObj = vHelper.CreateNewVersion(sourceObj, vHelper);
            return newVersionObj;
        }

        public virtual Dictionary<IVersionSupport, IVersionSupport> GenerateCopyOfObjects(List<IVersionSupport> list, Session ssn, IVersionSupport sourceObj) {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.GenerateCopyOfObjects(list, ssn, sourceObj);
        }

        public virtual object CopyForVersion(IXPSimpleObject source) {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CopyForVersion(source);
        }

        public virtual void SetVersionState(Dictionary<IVersionSupport, IVersionSupport> dict, VersionStates versionState) {
            VersionHelper vHelper = new VersionHelper(this.Session);
            vHelper.SetVersionState(dict, versionState);
        }

        public virtual void SetVersionStateExt(IVersionSupport obj, VersionStates vs) {
            VersionHelper vHelper = new VersionHelper(this.Session);
            vHelper.SetVersionStateExt(obj, vs);
        }

        public virtual List<IVersionSupport> GetVersionedStrate(IVersionSupport sourceObj, VersionHelper vHelper) {
            return vHelper.GetVersionedStrate(sourceObj, vHelper);
        }

        public void AddObjectToList(IVersionSupport obj, List<IVersionSupport> dependentObjectList) {
            VersionHelper.AddObjectToList(obj, dependentObjectList);
        }

        #endregion

    }

}