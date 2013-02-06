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
    public class CreatetVersionHelper : BaseObject
    {
        public CreatetVersionHelper() : base() { }
        public CreatetVersionHelper(Session session) : base(session) { }
        public CreatetVersionHelper(Session session, VersionStates vs) : base(session) {
            VersionStateAssign = true;
            newVersionState = vs;
        }

        #region ПОЛЯ КЛАССА ДЛЯ ПОДДЕРЖКИ ПОРОЖДЕНИЯ ВЕРСИЙ

        private Dictionary<IVersionSupport, IVersionSupport> _ObjectClonePair;

        // Для указания какой статус назначать копиям объектов (если null, то не назначать)

        private bool VersionStateAssign = false;
        private VersionStates newVersionState;

        #endregion

        #region МЕТОДЫ ДЛЯ ПОДДЕРЖКИ ВЕРСИОННОСТИ

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IVersionSupport CopyProcessing(IVersionSupport vr) {
            if (vr == null) return null;

            IVersionSupport CopyOperationResult = null;

            // Список всех затронутых объектом vr объектов
            List<IVersionSupport> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

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
        public IVersionSupport CopyProcessing(IVersionSupport vr, VersionStates VState) {
            if (vr == null) return null;

            IVersionSupport CopyOperationResult = null;

            // Список всех затронутых объектом vr объектов
            List<IVersionSupport> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // Делаем копии всех объектов в списке
            _ObjectClonePair = GenerateCopyOfObjects(htDependenceObjectCollection);

            // Цикл по всем объектам пары с установкой заданного статуса
            foreach (IVersionSupport vrec in _ObjectClonePair.Values) {
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

                IVersionSupport vrPrev = ((IVersionSupport)_enumLink.Key);
                IVersionSupport vrNext = ((IVersionSupport)_enumLink.Value);

                // <<<<<<<<<<<<<<<<<<<<
                vrNext.PrevVersion = vrPrev;

                // LinkToCurrent просто переносим, т.к. она одна и та же у всех вообще версий (версионный инвариант)
                //vrNext.Current = vrPrev.Current;
                // >>>>>>>>>>>>>>>>>>>>>>

                // <<<<<<<<<<<<<<<<<<<<
                // Находим значение LinkToEditor из предыдущего версионного слоя
                IVersionSupport LinkToEditorPrev = vrPrev.LinkToEditor;

                // Находим его новую версию
                IVersionSupport LinkToEditorNext = null;
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
            //    IVersionSupport vrPrev = ((IVersionSupport)_enumLink.Key);

            //    // Находим значение LinkToEditor из предыдущего версионного слоя
            //    IVersionSupport LinkToEditorPrev = vrPrev.LinkToEditor;

            //    // Находим его новую версию
            //    IVersionSupport LinkToEditorNext = null;
            //    bool ExistsLinkToEditorNext = _ObjectClonePair.TryGetValue(LinkToEditorPrev, out LinkToEditorNext);

            //    if (ExistsLinkToEditorNext) {
            //        ((IVersionSupport)_enumLink.Value).LinkToEditor = LinkToEditorNext;
            //    }
            //    else {
            //        // Может потом привести к сообщению, что редактор не найден, когда попытается отредактировать данную
            //        // семантическую часть
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

            // Список всех затронутых объектом vr объектов
            List<IVersionSupport> htDependenceObjectCollection = GetDependenceObjectCollection(vr);

            // Цикл по всем объектам пары с установкой заданного статуса
            foreach (IVersionSupport vrec in htDependenceObjectCollection) {
                vrec.VersionState = VState;
            }

            return true;
        }


        /// <summary>
        /// Класс собирает все объекты, необходимые для создания версии
        /// </summary>
        /// <returns></returns>
        public List<IVersionSupport> GetDependenceObjectCollection(IVersionSupport vr) {

            if (vr == null) return null;

            List<IVersionSupport> htResult = new List<IVersionSupport>();

            // Список всех затронутых объектом vr объектов
            Dictionary<IVersionSupport, List<IVersionSupport>> htDependenceSourceObject = new Dictionary<IVersionSupport, List<IVersionSupport>>();

            // Вначале образуем коллекцию от начального объекта
            List<IVersionSupport> DependObjects = vr.GetDependentObjects();
            if (DependObjects != null && DependObjects.Count == 0) DependObjects = null;
            htDependenceSourceObject.Add(vr, DependObjects);

            if (DependObjects == null) { 
                htResult.Add(vr);
                return htResult;
            }

            int Position = 0;   // Позиция, с которой начинать перебор коллекции

            while (true) {
                Dictionary<IVersionSupport, List<IVersionSupport>> htWork = new Dictionary<IVersionSupport, List<IVersionSupport>>();   // Заготовки для елементов основной коллекции htDependenceSourceObject

                IDictionaryEnumerator _enum = htDependenceSourceObject.GetEnumerator();

                // Пропускаем обработанные позиции
                for (int i = 0; i < Position; i++) { _enum.MoveNext(); }

                // Цикл по ключам основной таблицы
                while (_enum.MoveNext()) {
                    List<IVersionSupport> ht = (List<IVersionSupport>)_enum.Value;  // Коллекция зависимых объектов

                    if (ht == null) continue;

                    // Перебираем все зависимые объекты в ht
                    foreach (IVersionSupport obj in ht) {
                        List<IVersionSupport> htTail = obj.GetDependentObjects();   // Коллекция зависимых объектов для зависимого объекта

                        //IVersionSupport objVers = (IVersionSupport)obj;
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
                    htDependenceSourceObject.Add((IVersionSupport)_enumDict.Key, (List<IVersionSupport>)_enumDict.Value);
                }
            }


            // Финальная коллекция
            IDictionaryEnumerator _enumResult = htDependenceSourceObject.GetEnumerator();
            while (_enumResult.MoveNext()) {
                htResult.Add((IVersionSupport)_enumResult.Key);
            }

            return htResult;
        }

        /// <summary>
        /// Метод генерирует копии объектов, заданных в коллекции
        /// </summary>
        /// <returns></returns>
        public Dictionary<IVersionSupport, IVersionSupport> GenerateCopyOfObjects(List<IVersionSupport> htSource) {

            // Составляем пары из объектов и их копий
            Dictionary<IVersionSupport, IVersionSupport> htPair = new Dictionary<IVersionSupport, IVersionSupport>();

            foreach (IVersionSupport vr in htSource) {
                IVersionSupport vrClone = vr.CreateCopyObjects();
                htPair.Add(vr, vrClone);
            }

            return htPair;
        }

        /// <summary>
        /// Прохождение по всем ссылкам и присваивание новых значений
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
        /// Прохождение по всем ссылкам и присваивание новых значений
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
        ///// Прохождение по всем ссылкам и присваивание новых значений статуса
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