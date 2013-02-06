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
    public class CreateObjectVersionHelper : BaseObject
    {
        public CreateObjectVersionHelper() : base() { }
        public CreateObjectVersionHelper(Session session) : base(session) { }

        #region ���� ������ ��� ��������� ���������� ������

        private Dictionary<VersionRecord, VersionRecord> _ObjectClonePair;

        #endregion

        #region ������ ��� ��������� ������������

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public VersionRecord CopyProcessing(VersionRecord vr) {
            if (vr == null) return null;

            VersionRecord CopyOperationResult = null;

            // ������ ���� ���������� �������� vr ��������
            List<VersionRecord> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

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
        public VersionRecord CopyProcessing(VersionRecord vr, VersionStates VState) {
            if (vr == null) return null;

            VersionRecord CopyOperationResult = null;

            // ������ ���� ���������� �������� vr ��������
            List<VersionRecord> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // ������ ����� ���� �������� � ������
            _ObjectClonePair = GenerateCopyOfObjects(htDependenceObjectCollection);

            // ���� �� ���� �������� ���� � ���������� ��������� �������
            foreach (VersionRecord vrec in _ObjectClonePair.Values) {
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

                VersionRecord vrPrev = ((VersionRecord)_enumLink.Key);
                VersionRecord vrNext = ((VersionRecord)_enumLink.Value);

                // <<<<<<<<<<<<<<<<<<<<
                vrNext.PrevVersion = vrPrev;

                // LinkToCurrent ������ ���������, �.�. ��� ���� � �� �� � ���� ������ ������ (���������� ���������)
                vrNext.Current = vrPrev.Current;
                // >>>>>>>>>>>>>>>>>>>>>>

                // <<<<<<<<<<<<<<<<<<<<
                // ������� �������� LinkToEditor �� ����������� ����������� ����
                VersionRecord LinkToEditorPrev = vrPrev.LinkToEditor;

                // ������� ��� ����� ������
                VersionRecord LinkToEditorNext = null;
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
            //    VersionRecord vrPrev = ((VersionRecord)_enumLink.Key);

            //    // ������� �������� LinkToEditor �� ����������� ����������� ����
            //    VersionRecord LinkToEditorPrev = vrPrev.LinkToEditor;

            //    // ������� ��� ����� ������
            //    VersionRecord LinkToEditorNext = null;
            //    bool ExistsLinkToEditorNext = _ObjectClonePair.TryGetValue(LinkToEditorPrev, out LinkToEditorNext);

            //    if (ExistsLinkToEditorNext) {
            //        ((VersionRecord)_enumLink.Value).LinkToEditor = LinkToEditorNext;
            //    }
            //    else {
            //        // ����� ����� �������� � ���������, ��� �������� �� ������, ����� ���������� ��������������� ������
            //        // ������������� �����
            //        ((VersionRecord)_enumLink.Value).LinkToEditor = null;
            //    }
            //}
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SetStatusProcessing(VersionRecord vr, VersionStates VState) {
            if (vr == null) return true;

            // ������ ���� ���������� �������� vr ��������
            List<VersionRecord> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // ���� �� ���� �������� ���� � ���������� ��������� �������
            foreach (VersionRecord vrec in htDependenceObjectCollection) {
                vrec.VersionState = VState;
            }

            return true;
        }


        /// <summary>
        /// ����� �������� ��� �������, ����������� ��� �������� ������
        /// </summary>
        /// <returns></returns>
        public List<VersionRecord> GetDependenceObjectCollection(VersionRecord vr) {

            if (vr == null) return null;

            List<VersionRecord> htResult = new List<VersionRecord>();

            // ������ ���� ���������� �������� vr ��������
            Dictionary<VersionRecord, List<VersionRecord>> htDependenceSourceObject = new Dictionary<VersionRecord, List<VersionRecord>>();

            // ������� �������� ��������� �� ���������� �������
            List<VersionRecord> DependObjects = vr.GetDependenceObjects();
            if (DependObjects != null && DependObjects.Count == 0) DependObjects = null;
            htDependenceSourceObject.Add(vr, DependObjects);

            if (DependObjects == null) { 
                htResult.Add(vr);
                return htResult;
            }

            int Position = 0;   // �������, � ������� �������� ������� ���������

            while (true) {
                Dictionary<VersionRecord, List<VersionRecord>> htWork = new Dictionary<VersionRecord, List<VersionRecord>>();   // ��������� ��� ��������� �������� ��������� htDependenceSourceObject

                IDictionaryEnumerator _enum = htDependenceSourceObject.GetEnumerator();

                // ���������� ������������ �������
                for (int i = 0; i < Position; i++) { _enum.MoveNext(); }

                // ���� �� ������ �������� �������
                while (_enum.MoveNext()) {
                    List<VersionRecord> ht = (List<VersionRecord>)_enum.Value;  // ��������� ��������� ��������

                    if (ht == null) continue;

                    // ���������� ��� ��������� ������� � ht
                    foreach (VersionRecord obj in ht) {
                        List<VersionRecord> htTail = obj.GetDependenceObjects();   // ��������� ��������� �������� ��� ���������� �������

                        //VersionRecord objVers = (VersionRecord)obj;
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
                    htDependenceSourceObject.Add((VersionRecord)_enumDict.Key, (List<VersionRecord>)_enumDict.Value);
                }
            }


            // ��������� ���������
            IDictionaryEnumerator _enumResult = htDependenceSourceObject.GetEnumerator();
            while (_enumResult.MoveNext()) {
                htResult.Add((VersionRecord)_enumResult.Key);
            }

            return htResult;
        }

        /// <summary>
        /// ����� ���������� ����� ��������, �������� � ���������
        /// </summary>
        /// <returns></returns>
        public Dictionary<VersionRecord, VersionRecord> GenerateCopyOfObjects(List<VersionRecord> htSource) {

            // ���������� ���� �� �������� � �� �����
            Dictionary<VersionRecord, VersionRecord> htPair = new Dictionary<VersionRecord, VersionRecord>();

            foreach (VersionRecord vr in htSource) {
                VersionRecord vrClone = vr.CreateCopyObject();
                htPair.Add(vr, vrClone);
            }

            return htPair;
        }

        /// <summary>
        /// ����������� �� ���� ������� � ������������ ����� ��������
        /// </summary>
        /// <param name="htSource"></param>
        /// <returns></returns>
        private void ResetObjectLink(Dictionary<VersionRecord, VersionRecord> htSource) {

            if (htSource == null) return;

            foreach (VersionRecord vr in htSource.Values) {
                vr.SetReferences(this);
            }
        }

        /// <summary>
        /// ����������� �� ���� ������� � ������������ ����� ��������
        /// </summary>
        /// <param name="htSource"></param>
        /// <returns></returns>
        private void ResetObjectLink(Dictionary<VersionRecord, VersionRecord> htSource, VersionStates VersionSate) {

            if (htSource == null) return;

            foreach (VersionRecord vr in htSource.Values) {
                vr.SetReferences(this, VersionSate);
            }
        }

        ///// <summary>
        ///// ����������� �� ���� ������� � ������������ ����� �������� �������
        ///// </summary>
        ///// <param name="htSource"></param>
        ///// <returns></returns>
        //public void SetStates(Dictionary<VersionRecord, VersionRecord> htSource, VersionStates VersionState) {

        //    if (htSource == null) return;

        //    foreach (VersionRecord vr in htSource.Values) {
        //        vr.SetVersionState(this, VersionState);
        //    }
        //}


        public VersionRecord GetCopyObject(VersionRecord vr) {
            VersionRecord vrOut = null;
            if (vr == null) return null;
            if (_ObjectClonePair.ContainsKey(vr)) {
                _ObjectClonePair.TryGetValue(vr, out vrOut);
            }
            return vrOut;
        }

        #endregion

    }

}