using System;
using DevExpress.Xpo;
using System.Collections;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// ��������������� ����� ��� �������� ����������� ���� ����������
    /// </summary>
    //[DefaultClassOptions]
    [NonPersistent]
    public class CreatetVersionHelper : BaseObject
    {
        public CreatetVersionHelper() : base() { }
        public CreatetVersionHelper(Session session) : base(session) { }
        public CreatetVersionHelper(Session session, VersionStates vs) : base(session) {
            VersionStateAssign = true;
            newVersionState = vs;
        }

        #region ���� ������ ��� ��������� ���������� ������

        private Dictionary<IVersionSupport, IVersionSupport> _ObjectClonePair;

        // ��� �������� ����� ������ ��������� ������ �������� (���� null, �� �� ���������)

        private bool VersionStateAssign = false;
        private VersionStates newVersionState;

        #endregion

        #region ������ ��� ��������� ������������

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IVersionSupport CopyProcessing(IVersionSupport vr) {
            if (vr == null) return null;

            IVersionSupport CopyOperationResult = null;

            // ������ ���� ���������� �������� vr ��������
            List<IVersionSupport> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // ������� ����� ���� �������� � ������
            _ObjectClonePair = GenerateCopyOfObjects(htDependenceObjectCollection);

            // ������������� ������ �� ���������� ���������� ����, ��������� � ������� ������� ������
            ResetLinks();

            // ����������������� ������ � ��������� htDependenceTargetObject
            ResetObjectLink(_ObjectClonePair);
            //ResetObjectLink(_ObjectClonePair, vr.VersionState);
            // ������� ����� ��� �������� �� ��� ����������

            //// ����������������� ������� �� �� ��, ��� � ��� �������� �������, htDependenceTargetObject
            //SetStates(_ObjectClonePair, vr.VersionState);

            bool ExistCopy = _ObjectClonePair.TryGetValue(vr, out CopyOperationResult);

            if (ExistCopy) {
                return CopyOperationResult;
            }
            else {
                return null;
            }

            //return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IVersionSupport CopyProcessing(IVersionSupport vr, VersionStates VState) {
            if (vr == null) return null;

            IVersionSupport CopyOperationResult = null;

            // ������ ���� ���������� �������� vr ��������
            List<IVersionSupport> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // ������ ����� ���� �������� � ������
            _ObjectClonePair = GenerateCopyOfObjects(htDependenceObjectCollection);

            // ���� �� ���� �������� ���� � ���������� ��������� �������
            foreach (IVersionSupport vrec in _ObjectClonePair.Values) {
                vrec.VersionState = VState;
            }

            // ������������� ������ �� ���������� ���������� ����, ��������� � ������� ������� ������
            ResetLinks();

            // ����������������� ������ � ��������� htDependenceTargetObject
            ResetObjectLink(_ObjectClonePair);
            //ResetObjectLink(_ObjectClonePair, VersionState);
            // ������� ����� ��� �������� �� ��� ����������

            //// ����������������� ������� �� �� ��, ��� � ��� �������� �������, htDependenceTargetObject
            //SetStates(_ObjectClonePair, vr.VersionState);

            bool ExistCopy = _ObjectClonePair.TryGetValue(vr, out CopyOperationResult);

            if (ExistCopy) {
                return CopyOperationResult;
            }
            else {
                return null;
            }

            //return null;
        }

        /// <summary>
        /// ������������� ����� PrevVersion, LinkToEditor, LinkToCurrent � ����� ������
        /// ���� ��� ��������� ������������� ����� ��� ��������, �� ��� ����� ������ ���� ������������ �����
        /// ����� ����� ������ ���������, ������� ���� ���������� ����� ����������� ���������
        /// </summary>
        private void ResetLinks() {

            // ���� �� ���� �������� ���� � ���������� ����� PrevVersion, LinkToCurrent �  LinkToEditor (� ����� ������ �������� ��������������� � ��������������� �� ����� ������)
            IDictionaryEnumerator _enumLink = _ObjectClonePair.GetEnumerator();
            while (_enumLink.MoveNext()) {

                IVersionSupport vrPrev = ((IVersionSupport)_enumLink.Key);
                IVersionSupport vrNext = ((IVersionSupport)_enumLink.Value);

                // <<<<<<<<<<<<<<<<<<<<
                vrNext.PrevVersion = vrPrev;

                // LinkToCurrent ������ ���������, �.�. ��� ���� � �� �� � ���� ������ ������ (���������� ���������)
                //vrNext.Current = vrPrev.Current;
                // >>>>>>>>>>>>>>>>>>>>>>

                // <<<<<<<<<<<<<<<<<<<<
                // ������� �������� LinkToEditor �� ����������� ����������� ����
                IVersionSupport LinkToEditorPrev = vrPrev.LinkToEditor;

                // ������� ��� ����� ������
                IVersionSupport LinkToEditorNext = null;
                if (LinkToEditorPrev == null) {
                    // ����� ����� �������� � ���������, ��� �������� �� ������, ����� ���������� ��������������� ������
                    // ������������� �����
                    vrNext.LinkToEditor = null;
                }
                else {
                    bool ExistsLinkToEditorNext = _ObjectClonePair.TryGetValue(LinkToEditorPrev, out LinkToEditorNext);

                    if (ExistsLinkToEditorNext) {
                        vrNext.LinkToEditor = LinkToEditorNext;
                    }
                    else {
                        // ����� ����� �������� � ���������, ��� �������� �� ������, ����� ���������� ��������������� ������
                        // ������������� �����
                        vrNext.LinkToEditor = null;
                    }
                }
                // >>>>>>>>>>>>>>>>>>>>>>
            
            }

            //// ���� �� ���� �������� ���� � ���������� ���� LinkToEditor (� ����� ������ �������� ��������������� � ��������������� �� ����� ������)
            //while (_enumLink.MoveNext()) {
            //    IVersionSupport vrPrev = ((IVersionSupport)_enumLink.Key);

            //    // ������� �������� LinkToEditor �� ����������� ����������� ����
            //    IVersionSupport LinkToEditorPrev = vrPrev.LinkToEditor;

            //    // ������� ��� ����� ������
            //    IVersionSupport LinkToEditorNext = null;
            //    bool ExistsLinkToEditorNext = _ObjectClonePair.TryGetValue(LinkToEditorPrev, out LinkToEditorNext);

            //    if (ExistsLinkToEditorNext) {
            //        ((IVersionSupport)_enumLink.Value).LinkToEditor = LinkToEditorNext;
            //    }
            //    else {
            //        // ����� ����� �������� � ���������, ��� �������� �� ������, ����� ���������� ��������������� ������
            //        // ������������� �����
            //        ((IVersionSupport)_enumLink.Value).LinkToEditor = null;
            //    }
            //}
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SetStatusProcessing(IVersionSupport vr, VersionStates VState) {
            if (vr == null) return true;

            // ������ ���� ���������� �������� vr ��������
            List<IVersionSupport> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // ���� �� ���� �������� ���� � ���������� ��������� �������
            foreach (IVersionSupport vrec in htDependenceObjectCollection) {
                vrec.VersionState = VState;
            }

            return true;
        }


        /// <summary>
        /// ����� �������� ��� �������, ����������� ��� �������� ������
        /// </summary>
        /// <returns></returns>
        public List<IVersionSupport> GetDependenceObjectCollection(IVersionSupport vr) {

            if (vr == null) return null;

            List<IVersionSupport> htResult = new List<IVersionSupport>();

            // ������ ���� ���������� �������� vr ��������
            Dictionary<IVersionSupport, List<IVersionSupport>> htDependenceSourceObject = new Dictionary<IVersionSupport, List<IVersionSupport>>();

            // ������� �������� ��������� �� ���������� �������
            List<IVersionSupport> DependObjects = vr.GetDependentObjects();
            if (DependObjects != null && DependObjects.Count == 0) DependObjects = null;
            htDependenceSourceObject.Add(vr, DependObjects);

            if (DependObjects == null) { 
                htResult.Add(vr);
                return htResult;
            }

            int Position = 0;   // �������, � ������� �������� ������� ���������

            while (true) {
                Dictionary<IVersionSupport, List<IVersionSupport>> htWork = new Dictionary<IVersionSupport, List<IVersionSupport>>();   // ��������� ��� ��������� �������� ��������� htDependenceSourceObject

                IDictionaryEnumerator _enum = htDependenceSourceObject.GetEnumerator();

                // ���������� ������������ �������
                for (int i = 0; i < Position; i++) { _enum.MoveNext(); }

                // ���� �� ������ �������� �������
                while (_enum.MoveNext()) {
                    List<IVersionSupport> ht = (List<IVersionSupport>)_enum.Value;  // ��������� ��������� ��������

                    if (ht == null) continue;

                    // ���������� ��� ��������� ������� � ht
                    foreach (IVersionSupport obj in ht) {
                        List<IVersionSupport> htTail = obj.GetDependentObjects();   // ��������� ��������� �������� ��� ���������� �������

                        //IVersionSupport objVers = (IVersionSupport)obj;
                        // ���������, ���������� �� ������ ������ � ��������� htDependenceSourceObject.
                        // ���� �� ����������, �� ��������� � ������� ���������
                        if (!htDependenceSourceObject.ContainsKey(obj) && !htWork.ContainsKey(obj)) {
                            if (htTail == null || htTail.Count == 0) {
                                htWork.Add(obj, null);
                            }
                            else {
                                htWork.Add(obj, htTail);
                            }
                        }
                    }
                }

                if (htWork.Count == 0) break;

                // ����������� ��������� ������ �������� (����� �� ���������� ��� �����������)
                Position += 1;

                // ��������� ���������� htWork � htDependenceSourceObject
                IDictionaryEnumerator _enumDict = htWork.GetEnumerator();
                while (_enumDict.MoveNext()) {
                    htDependenceSourceObject.Add((IVersionSupport)_enumDict.Key, (List<IVersionSupport>)_enumDict.Value);
                }
            }


            // ��������� ���������
            IDictionaryEnumerator _enumResult = htDependenceSourceObject.GetEnumerator();
            while (_enumResult.MoveNext()) {
                htResult.Add((IVersionSupport)_enumResult.Key);
            }

            return htResult;
        }

        /// <summary>
        /// ����� ���������� ����� ��������, �������� � ���������
        /// </summary>
        /// <returns></returns>
        public Dictionary<IVersionSupport, IVersionSupport> GenerateCopyOfObjects(List<IVersionSupport> htSource) {

            // ���������� ���� �� �������� � �� �����
            Dictionary<IVersionSupport, IVersionSupport> htPair = new Dictionary<IVersionSupport, IVersionSupport>();

            foreach (IVersionSupport vr in htSource) {
                IVersionSupport vrClone = vr.CreateCopyObjects();
                htPair.Add(vr, vrClone);
            }

            return htPair;
        }

        /// <summary>
        /// ����������� �� ���� ������� � ������������ ����� ��������
        /// </summary>
        /// <param name="htSource"></param>
        /// <returns></returns>
        private void ResetObjectLink(Dictionary<IVersionSupport, IVersionSupport> htSource) {

            if (htSource == null) return;

            foreach (IVersionSupport vr in htSource.Values) {
                vr.SetReferences(this);
            }
        }

        /// <summary>
        /// ����������� �� ���� ������� � ������������ ����� ��������
        /// </summary>
        /// <param name="htSource"></param>
        /// <returns></returns>
        private void ResetObjectLink(Dictionary<IVersionSupport, IVersionSupport> htSource, VersionStates VersionSate) {

            if (htSource == null) return;

            foreach (IVersionSupport vr in htSource.Values) {
                vr.SetReferences(this, VersionSate);
            }
        }

        ///// <summary>
        ///// ����������� �� ���� ������� � ������������ ����� �������� �������
        ///// </summary>
        ///// <param name="htSource"></param>
        ///// <returns></returns>
        //public void SetStates(Dictionary<IVersionSupport, IVersionSupport> htSource, VersionStates VersionState) {

        //    if (htSource == null) return;

        //    foreach (IVersionSupport vr in htSource.Values) {
        //        vr.SetVersionState(this, VersionState);
        //    }
        //}


        public IVersionSupport GetCopyObject(IVersionSupport vr) {
            IVersionSupport vrOut = null;
            if (vr == null) return null;
            if (_ObjectClonePair.ContainsKey(vr)) {
                _ObjectClonePair.TryGetValue(vr, out vrOut);
                if (VersionStateAssign) vrOut.VersionState = newVersionState;
            }
            return vrOut;
        }

        #endregion

    }

}