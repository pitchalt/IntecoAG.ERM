using System;
using DevExpress.Xpo;
using System.Collections;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// Вспомогательный класс для создания версионного слоя документов
    /// </summary>
    //[DefaultClassOptions]
    [NonPersistent]
    public class CreateObjectVersionHelper : BaseObject
    {
        public CreateObjectVersionHelper() : base() { }
        public CreateObjectVersionHelper(Session session) : base(session) { }

        #region ПОЛЯ КЛАССА ДЛЯ ПОДДЕРЖКИ ПОРОЖДЕНИЯ ВЕРСИЙ

        private Dictionary<VersionRecord, VersionRecord> _ObjectClonePair;

        #endregion

        #region МЕТОДЫ ДЛЯ ПОДДЕРЖКИ ВЕРСИОННОСТИ

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public VersionRecord CopyProcessing(VersionRecord vr) {
            if (vr == null) return null;

            VersionRecord CopyOperationResult = null;

            // Список всех затронутых объектом vr объектов
            List<VersionRecord> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // Делаеим копии всех объектов в списке
            _ObjectClonePair = GenerateCopyOfObjects(htDependenceObjectCollection);

            // Переустановка ссылок на предыдущий версионный слой, редакторы и главную текущую запись
            ResetLinks();

            // Переустанавливаем ссылки в коллекции htDependenceTargetObject
            ResetObjectLink(_ObjectClonePair);
            //ResetObjectLink(_ObjectClonePair, vr.VersionState);
            // Находим копию для передачи ея как результата

            //// Переустанавливаем статусы на те же, что и для текущего объекта, htDependenceTargetObject
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

            // Список всех затронутых объектом vr объектов
            List<VersionRecord> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // Делаем копии всех объектов в списке
            _ObjectClonePair = GenerateCopyOfObjects(htDependenceObjectCollection);

            // Цикл по всем объектам пары с установкой заданного статуса
            foreach (VersionRecord vrec in _ObjectClonePair.Values) {
                vrec.VersionState = VState;
            }

            // Переустановка ссылок на предыдущий версионный слой, редакторы и главную текущую запись
            ResetLinks();

            // Переустанавливаем ссылки в коллекции htDependenceTargetObject
            ResetObjectLink(_ObjectClonePair);
            //ResetObjectLink(_ObjectClonePair, VersionState);
            // Находим копию для передачи ея как результата

            //// Переустанавливаем статусы на те же, что и для текущего объекта, htDependenceTargetObject
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
        /// ПЕРЕУСТАНОВКИ ПОЛЕЙ PrevVersion, LinkToEditor, LinkToCurrent в новой версии
        /// Если для некоторой семантической части был редактор, то для новой версии этой семантичской части
        /// будет новая версия редактора, который есть версионная копия предыдущего редактора
        /// </summary>
        private void ResetLinks() {

            // Цикл по всем объектам пары с установкой полей PrevVersion, LinkToCurrent и  LinkToEditor (у новой версии редактор устанавливается в соответствующий из новой версии)
            IDictionaryEnumerator _enumLink = _ObjectClonePair.GetEnumerator();
            while (_enumLink.MoveNext()) {

                VersionRecord vrPrev = ((VersionRecord)_enumLink.Key);
                VersionRecord vrNext = ((VersionRecord)_enumLink.Value);

                // <<<<<<<<<<<<<<<<<<<<
                vrNext.PrevVersion = vrPrev;

                // LinkToCurrent просто переносим, т.к. она одна и та же у всех вообще версий (версионный инвариант)
                vrNext.Current = vrPrev.Current;
                // >>>>>>>>>>>>>>>>>>>>>>

                // <<<<<<<<<<<<<<<<<<<<
                // Находим значение LinkToEditor из предыдущего версионного слоя
                VersionRecord LinkToEditorPrev = vrPrev.LinkToEditor;

                // Находим его новую версию
                VersionRecord LinkToEditorNext = null;
                if (LinkToEditorPrev == null) {
                    // Может потом привести к сообщению, что редактор не найден, когда попытается отредактировать данную
                    // семантическую часть
                    vrNext.LinkToEditor = null;
                }
                else {
                    bool ExistsLinkToEditorNext = _ObjectClonePair.TryGetValue(LinkToEditorPrev, out LinkToEditorNext);

                    if (ExistsLinkToEditorNext) {
                        vrNext.LinkToEditor = LinkToEditorNext;
                    }
                    else {
                        // Может потом привести к сообщению, что редактор не найден, когда попытается отредактировать данную
                        // семантическую часть
                        vrNext.LinkToEditor = null;
                    }
                }
                // >>>>>>>>>>>>>>>>>>>>>>
            
            }

            //// Цикл по всем объектам пары с установкой поля LinkToEditor (у новой версии редактор устанавливается в соответствующий из новой версии)
            //while (_enumLink.MoveNext()) {
            //    VersionRecord vrPrev = ((VersionRecord)_enumLink.Key);

            //    // Находим значение LinkToEditor из предыдущего версионного слоя
            //    VersionRecord LinkToEditorPrev = vrPrev.LinkToEditor;

            //    // Находим его новую версию
            //    VersionRecord LinkToEditorNext = null;
            //    bool ExistsLinkToEditorNext = _ObjectClonePair.TryGetValue(LinkToEditorPrev, out LinkToEditorNext);

            //    if (ExistsLinkToEditorNext) {
            //        ((VersionRecord)_enumLink.Value).LinkToEditor = LinkToEditorNext;
            //    }
            //    else {
            //        // Может потом привести к сообщению, что редактор не найден, когда попытается отредактировать данную
            //        // семантическую часть
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

            // Список всех затронутых объектом vr объектов
            List<VersionRecord> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // Цикл по всем объектам пары с установкой заданного статуса
            foreach (VersionRecord vrec in htDependenceObjectCollection) {
                vrec.VersionState = VState;
            }

            return true;
        }


        /// <summary>
        /// Класс собирает все объекты, необходимые для создания версии
        /// </summary>
        /// <returns></returns>
        public List<VersionRecord> GetDependenceObjectCollection(VersionRecord vr) {

            if (vr == null) return null;

            List<VersionRecord> htResult = new List<VersionRecord>();

            // Список всех затронутых объектом vr объектов
            Dictionary<VersionRecord, List<VersionRecord>> htDependenceSourceObject = new Dictionary<VersionRecord, List<VersionRecord>>();

            // Вначале образуем коллекцию от начального объекта
            List<VersionRecord> DependObjects = vr.GetDependenceObjects();
            if (DependObjects != null && DependObjects.Count == 0) DependObjects = null;
            htDependenceSourceObject.Add(vr, DependObjects);

            if (DependObjects == null) { 
                htResult.Add(vr);
                return htResult;
            }

            int Position = 0;   // Позиция, с которой начинать перебор коллекции

            while (true) {
                Dictionary<VersionRecord, List<VersionRecord>> htWork = new Dictionary<VersionRecord, List<VersionRecord>>();   // Заготовки для елементов основной коллекции htDependenceSourceObject

                IDictionaryEnumerator _enum = htDependenceSourceObject.GetEnumerator();

                // Пропускаем обработанные позиции
                for (int i = 0; i < Position; i++) { _enum.MoveNext(); }

                // Цикл по ключам основной таблицы
                while (_enum.MoveNext()) {
                    List<VersionRecord> ht = (List<VersionRecord>)_enum.Value;  // Коллекция зависимых объектов

                    if (ht == null) continue;

                    // Перебираем все зависимые объекты в ht
                    foreach (VersionRecord obj in ht) {
                        List<VersionRecord> htTail = obj.GetDependenceObjects();   // Коллекция зависимых объектов для зависимого объекта

                        //VersionRecord objVers = (VersionRecord)obj;
                        // Проверяем, содержится ли данный объект в коллекции htDependenceSourceObject.
                        // Если не содержится, то добавляем в рабочую коллекцию
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

                // Передвигаем указатель начала перебора (чтобы не перебирать уже перебранное)
                Position += 1;

                // Переносим содержимое htWork в htDependenceSourceObject
                IDictionaryEnumerator _enumDict = htWork.GetEnumerator();
                while (_enumDict.MoveNext()) {
                    htDependenceSourceObject.Add((VersionRecord)_enumDict.Key, (List<VersionRecord>)_enumDict.Value);
                }
            }


            // Финальная коллекция
            IDictionaryEnumerator _enumResult = htDependenceSourceObject.GetEnumerator();
            while (_enumResult.MoveNext()) {
                htResult.Add((VersionRecord)_enumResult.Key);
            }

            return htResult;
        }

        /// <summary>
        /// Метод генерирует копии объектов, заданных в коллекции
        /// </summary>
        /// <returns></returns>
        public Dictionary<VersionRecord, VersionRecord> GenerateCopyOfObjects(List<VersionRecord> htSource) {

            // Составляем пары из объектов и их копий
            Dictionary<VersionRecord, VersionRecord> htPair = new Dictionary<VersionRecord, VersionRecord>();

            foreach (VersionRecord vr in htSource) {
                VersionRecord vrClone = vr.CreateCopyObject();
                htPair.Add(vr, vrClone);
            }

            return htPair;
        }

        /// <summary>
        /// Прохождение по всем ссылкам и присваивание новых значений
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
        /// Прохождение по всем ссылкам и присваивание новых значений
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
        ///// Прохождение по всем ссылкам и присваивание новых значений статуса
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