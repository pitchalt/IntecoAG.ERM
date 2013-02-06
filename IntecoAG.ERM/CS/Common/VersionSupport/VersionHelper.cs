using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Xpo.Metadata;
using System.Collections;

namespace IntecoAG.ERM.CS {

    public class VersionHelper {

        Session sourceSession;
        IXPSimpleObject source = null;

        // Свойства, которые будут игнорироваться или назначаться специальным образом при создании новой версии
        string ignoredProperties = "LinkToEditor, IsOfficial, IsCurrent, VersionState, PrevVersion";


        public VersionHelper(Session source) {
            this.sourceSession = source;
        }


        /// <summary>
        /// Создание нового версионного слоя. Возвращает главный объект слоя (например, для контракта объект из
        /// слоя типа crmComplexContractVersion)
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public virtual IVersionSupport CreateNewVersion(IVersionSupport sourceObj, VersionHelper vHelper) {
            source = sourceObj as IXPSimpleObject;
            if (source == null) return null;

            // Алгоритм.
            // 1. Вычисляется версионный слой sourceVersionStrate
            // 2. Создаётся массив пар зависмых объектов и их клонов - dict
            // 3. Все вхождения подобъектов в исходный версионный слой, говоря приблизительно, заменяются на их клоны согласно dict
            // 4. У нового версионного слоя у всех входящих в него версионных объектов проставляется статус VERSION_PROJECT

            // Версионный слой от sourceObj - это версионный слой, из которого будет делаться новый версионный слой
            // Замечание. Свм объект по построению входит в свой версионный слой.
            List<IVersionSupport> sourceVersionStrate = GetVersionedStrate(sourceObj, vHelper);

            // Создаём словарь с копиями объектов из newVersionStrate
            Dictionary<IVersionSupport, IVersionSupport> dict = GenerateCopyOfObjects(sourceVersionStrate, source.Session, sourceObj);

            // Проставляем статус и некоторые другие свойства
            SetVersionState(dict, VersionStates.VERSION_PROJECT);

            // Меняем свойства на версионные
            ResetVersionProperty(vHelper.sourceSession, ignoredProperties, dict);

            // На выходе - копия основного объекта новой версии
            return dict[sourceObj];
        }


        #region Разбивка CreateNewVersion на две части (чтобы можно было внести исправления в dict)
        /*
        public virtual Dictionary<IVersionSupport, IVersionSupport> CreateNewVersionStep1(IVersionSupport sourceObj, VersionHelper vHelper) {
            source = sourceObj as IXPSimpleObject;
            if (source == null) return null;

            // Алгоритм.
            // 1. Вычисляется версионный слой sourceVersionStrate
            // 2. Создаётся массив пар зависмых объектов и их клонов - dict
            // 3. Все вхождения подобъектов в исходный версионный слой, говоря приблизительно, заменяются на их клоны согласно dict
            // 4. У нового версионного слоя у всех входящих в него версионных объектов проставляется статус VERSION_PROJECT

            // Версионный слой от sourceObj - это версионный слой, из которого будет делаться новый версионный слой
            // Замечание. Свм объект по построению входит в свой версионный слой.
            List<IVersionSupport> sourceVersionStrate = GetVersionedStrate(sourceObj, vHelper);

            // Создаём словарь с копиями объектов из newVersionStrate
            Dictionary<IVersionSupport, IVersionSupport> dict = GenerateCopyOfObjects(sourceVersionStrate, source.Session, sourceObj);

            return dict;
        }


        public virtual IVersionSupport CreateNewVersionStep2(IVersionSupport sourceObj, Dictionary<IVersionSupport, IVersionSupport> dict, VersionHelper vHelper) {

            // Проставляем статус и некоторые другие свойства
            SetVersionState(dict, VersionStates.VERSION_PROJECT);

            // Меняем свойства на версионные
            ResetVersionProperty(vHelper.sourceSession, ignoredProperties, dict);

            // На выходе - копия основного объекта новой версии
            return dict[sourceObj];
        }
        */
        #endregion



        /// <summary>
        /// Нерекурсивный метод создания версионного слоя по заданному объекту
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <param name="vHelper"></param>
        /// <returns></returns>
        public virtual List<IVersionSupport> GetVersionedStrate(IVersionSupport sourceObj, VersionHelper vHelper) {

            // Коллекция проверенных
            List<IVersionSupport> processedList = new List<IVersionSupport>();

            List<IVersionSupport> vStrate = new List<IVersionSupport>() { sourceObj };

            //List<IVersionSupport>  vStrate = GetFirstDependentList(sourceObj, vHelper);

            int count;
            for (;;) {
                count = vStrate.Count;

                List<IVersionSupport> tempCol = new List<IVersionSupport>();
                foreach ( IVersionSupport obj in vStrate ) {
                    if (processedList.Contains(obj)) continue;
                    List<IVersionSupport> list = GetFirstDependentList(obj, vHelper);
                    foreach (IVersionSupport elem in list) AddObjectToList(elem, tempCol);
                    AddObjectToList(obj, processedList);
                }

                foreach (IVersionSupport elem in tempCol) {
                    AddObjectToList(elem, vStrate);
                }

                if (count == vStrate.Count) break;
            }

            return vStrate;
        }


        /// <summary>
        /// Снятие непосредственно прилегающих к объекту версионныъ объектов
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <param name="dependentObjectList"></param>
        /// <param name="vHelper"></param>
        /// <returns></returns>
        public List<IVersionSupport> GetFirstDependentList(IVersionSupport sourceObj, VersionHelper vHelper) {

            List<IVersionSupport> ResList = new List<IVersionSupport>();
            if (sourceObj == null) return ResList;

            IXPSimpleObject sourceObject = (IXPSimpleObject)sourceObj;
            XPClassInfo sourceClassInfo = sourceObject.ClassInfo;   // sourceSession.GetClassInfo(sourceObject);

            foreach (XPMemberInfo m in sourceClassInfo.PersistentProperties) {
                if (m is DevExpress.Xpo.Metadata.Helpers.ServiceField || m.IsKey) continue;
                if (m.ReferenceType != null) {
                    IVersionSupport ob = m.GetValue(sourceObj) as IVersionSupport;
                    if (ob != null) AddObjectToList(ob, ResList);
                }
            }

            foreach (XPMemberInfo m in sourceClassInfo.CollectionProperties) {
                //if (m.HasAttribute(typeof(AggregatedAttribute))) {
                XPBaseCollection colSource = (XPBaseCollection)m.GetValue(sourceObj);
                foreach (IXPSimpleObject obj in colSource) {
                    if (obj is IVersionSupport) AddObjectToList((IVersionSupport)obj, ResList);
                }
                //}
            }

            return ResList;
        }


        /// <summary>
        /// Метод генерирует копии (не клоны!) объектов, заданных в коллекции.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<IVersionSupport, IVersionSupport> GenerateCopyOfObjects(List<IVersionSupport> list, Session ssn, IVersionSupport sourceObj) {
            // Составляем пары из объектов и их копий
            Dictionary<IVersionSupport, IVersionSupport> dict = new Dictionary<IVersionSupport, IVersionSupport>();

            foreach (IVersionSupport vr in list) {
                IVersionSupport vrClone = (IVersionSupport)CopyForVersion(vr as IXPSimpleObject);
                dict.Add(vr, vrClone);
            }

            return dict;
        }


        /// <summary>
        /// Копирование объекта. Результат - это копия объекта, в котором все обычные, но неверсионные, свойства копируются из прежнего объекта
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetSession"></param>
        /// <param name="synchronize"></param>
        /// <returns></returns>
        public object CopyForVersion(IXPSimpleObject source) {
            if (source == null) return null;

            XPClassInfo classInfo = sourceSession.GetClassInfo(source.GetType());

            // Копия объекта. Есть проблема. Если в AfterConstruction объекта создаётся некий версионный объект, то
            // этот версионный объект окажется незамещённым никакой копией из исходного объекта и тем самым "повиснет"
            IVersionSupport copy = (IVersionSupport)classInfo.CreateNewObject(sourceSession);

            // Паша
            copy.IsProcessCloning = true;
            foreach (XPMemberInfo m in classInfo.PersistentProperties) {
                if (m is DevExpress.Xpo.Metadata.Helpers.ServiceField || m.IsKey) continue;
                if (m is IVersionSupport) continue;
                m.SetValue(copy, m.GetValue(source));
            }

            foreach (XPMemberInfo m in classInfo.CollectionProperties) {
                if (m.HasAttribute(typeof(AggregatedAttribute))) {
                    XPBaseCollection colCopy = (XPBaseCollection)m.GetValue(copy);
                    XPBaseCollection colSource = (XPBaseCollection)m.GetValue(source);
                    foreach (IXPSimpleObject obj in new ArrayList(colSource)) {
                        if (obj is IVersionSupport) continue;
                        colCopy.BaseAdd(obj);
                    }
                }
            }

            return copy;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ivCopiedObj">основной объект для копированя</param>
        /// <param name="ivTargetObj">Новая версия объекта ivCopiedObj</param>
        /// <param name="ignoredProperties">Список игнорируемых свойств</param>
        /// <param name="dict">Словарь с копиями версионируемых объектов</param>
        public void ResetVersionProperty(Session ssn, string ignoredProperties, Dictionary<IVersionSupport, IVersionSupport> dict) {

            if (dict == null || dict.Count == 0) return;
            //if (ivCopiedObj == null) return;
            //Session ssn = ivCopiedObj.Session;
            ArrayList IgnoredPropertiesList = new ArrayList(ignoredProperties.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));


            // Прогон по словарю объектов
            foreach (IVersionSupport sourceObj in dict.Keys) {

                if (dict[sourceObj] == null) continue;

                IXPSimpleObject sourceObject = (IXPSimpleObject)sourceObj;
                IXPSimpleObject targetObject = (IXPSimpleObject)dict[sourceObj];
// Паша
                IVersionSupport to = (IVersionSupport)targetObject;

                XPClassInfo sourceClassInfo = sourceObject.ClassInfo;   // ssn.GetClassInfo(sourceObject.GetType());

                foreach (XPMemberInfo m in sourceClassInfo.PersistentProperties) {
                    if (m is DevExpress.Xpo.Metadata.Helpers.ServiceField || m.IsKey) continue;
                    if (IgnoredPropertiesList.Contains(m.Name)) continue;
                    //if (m.GetValue(sourceObject) is IVersionSupport == null) continue;

                    IVersionSupport ob = m.GetValue(sourceObject) as IVersionSupport;
                    if (ob != null && dict.ContainsKey(ob)) {
                        IXPSimpleObject newValue = dict[ob] as IXPSimpleObject;
                        if (newValue != null) {
                            m.SetValue(targetObject, newValue);
                        }
                    }
                }


                // Здесь надо иметь основной объект исходного слоя(source) и его версию (clone)
                foreach (XPMemberInfo m in sourceClassInfo.CollectionProperties) {
                    //if (m.HasAttribute(typeof(AggregatedAttribute))) {
                    XPBaseCollection colSource = (XPBaseCollection)m.GetValue(sourceObject);
                    XPBaseCollection colTarget = (XPBaseCollection)m.GetValue(targetObject);
                    foreach (IXPSimpleObject obj in new ArrayList(colSource)) {
                        if (obj == null) continue;
                        if (obj is IVersionSupport) {
                            if (dict.ContainsKey((IVersionSupport)obj)) {
                                IVersionSupport objClone = dict[(IVersionSupport)obj];
                                colTarget.BaseAdd(objClone);
                            }
                        } else {
                            colTarget.BaseAdd(obj);
                        }
                    }
                    //}
                }
                to.IsProcessCloning = false;

            }

        }


        /// <summary>
        /// Простановка у всех копий объектов из словаря dict указанного статуса versionState
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="versionState"></param>
        public void SetVersionState(Dictionary<IVersionSupport, IVersionSupport> dict, VersionStates versionState) {
            foreach (IVersionSupport obj in dict.Values) obj.VersionState = versionState;
        }


        public void SetVersionStateExt(IVersionSupport obj, VersionStates vs) {
            List<IVersionSupport> sourceVersionStrate = GetVersionedStrate(obj, this);
            foreach (IVersionSupport elem in sourceVersionStrate) elem.VersionState = vs;
        }


        /// <summary>
        /// Добавление объекта в список
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dependentObjectList"></param>
        public static void AddObjectToList(IVersionSupport obj, List<IVersionSupport> dependentObjectList) {
            if (dependentObjectList.Contains((IVersionSupport)obj)) return;
            dependentObjectList.Add((IVersionSupport)obj);
        }

    }
}