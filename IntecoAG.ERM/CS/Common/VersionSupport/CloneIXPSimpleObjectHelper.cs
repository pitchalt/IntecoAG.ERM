using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Xpo.Metadata;
using System.Collections;

namespace IntecoAG.ERM.CS {

    public class CloneIXPSimpleObjectHelper {


        // Что такое MemberwiseClone? Например, ComplexContractVersion copyThis = (ComplexContractVersion)this.MemberwiseClone();


        // В процессе копирования копии неверсионных объектов оставляем равными исходным объектам

        /// <summary>
        /// A dictionary containing objects from the source session as key and objects from the 
        /// target session as values
        /// </summary>
        /// <returns></returns>
        Dictionary<object, object> clonedObjects;
        Session sourceSession;
        Session targetSession;

        //public List<IVersionSupport> ignoredObjectList;

        /// <summary>
        /// Initializes a new instance of the CloneIXPSimpleObjectHelper class.
        /// </summary>
        public CloneIXPSimpleObjectHelper(Session source, Session target) {
            this.clonedObjects = new Dictionary<object, object>();
            this.sourceSession = source;
            this.targetSession = target;
            //ignoredObjectList = new List<IVersionSupport>();
        }

        public T Clone<T>(T source) where T : IXPSimpleObject {
            return Clone<T>(source, targetSession, false);
        }

        public T Clone<T>(T source, bool synchronize) where T : IXPSimpleObject {
            return (T)Clone(source as IXPSimpleObject, targetSession, synchronize);
        }

        public object Clone(IXPSimpleObject source) {
            return Clone(source, targetSession, false);
        }

        public object Clone(IXPSimpleObject source, bool synchronize) {
            return Clone(source, targetSession, synchronize);
        }

        public T Clone<T>(T source, Session targetSession, bool synchronize) where T : IXPSimpleObject {
            return (T)Clone(source as IXPSimpleObject, targetSession, synchronize);
        }

        /// <summary>
        /// Clones and / or synchronizes the given IXPSimpleObject.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetSession"></param>
        /// <param name="synchronize">If set to true, reference properties are only cloned in case
        /// the reference object does not exist in the targetsession. Otherwise the exising object will be
        /// reused and synchronized with the source. Set this property to false when knowing at forehand 
        /// that the targetSession will not contain any of the objects of the source.</param>
        /// <returns></returns>
        public object Clone(IXPSimpleObject source, Session targetSession, bool synchronize) {
            if(source == null) return null;

            // Копии неверсионируемых объектов оставляем равными первоначальным объектам
            //if (!(source is IVersionSupport)) return source;
            //if (ignoredObjectList.Contains(source as IVersionSupport)) return source;

            if(clonedObjects.ContainsKey(source)) return clonedObjects[source];

            XPClassInfo targetClassInfo = targetSession.GetClassInfo(source.GetType());

            object clone = null;
            if (!(source is IVersionSupport)) {
                if (synchronize) clone = targetSession.GetObjectByKey(targetClassInfo, source.Session.GetKeyValue(source));
            }
            //if (source is IVersionSupport && ((IVersionSupport)source).VersionState == VersionStates.CURRENT) {
            //    if (synchronize) clone = targetSession.GetObjectByKey(targetClassInfo, source.Session.GetKeyValue(source));
            //}
            if (clone == null) clone = targetClassInfo.CreateNewObject(targetSession);
            IVersionSupport cv = clone as IVersionSupport;
            if (cv != null) {
                cv.VersionState = VersionStates.VERSION_CONSTRACT;
            }

            clonedObjects.Add(source, clone);

            foreach(XPMemberInfo m in targetClassInfo.PersistentProperties) {
                if(m is DevExpress.Xpo.Metadata.Helpers.ServiceField || m.IsKey) continue;

                object val;
                if(m.ReferenceType != null) {
                    object createdByClone = m.GetValue(clone);
                    if ((createdByClone != null) && synchronize == false)
                        val = createdByClone;
                    else {
                        val = Clone((IXPSimpleObject)m.GetValue(source), targetSession, synchronize);
                    }

                }
                else {
                    val = m.GetValue(source);
                }
                m.SetValue(clone, val);
            }

            foreach(XPMemberInfo m in targetClassInfo.CollectionProperties) {
                if(m.HasAttribute(typeof(AggregatedAttribute))) {
                    XPBaseCollection col = (XPBaseCollection)m.GetValue(clone);
                    XPBaseCollection colSource = (XPBaseCollection)m.GetValue(source);
                    foreach(IXPSimpleObject obj in new ArrayList(colSource))
                        col.BaseAdd(Clone(obj, targetSession, synchronize));
                }
            }

            return clone;
        }



        /// <summary>
        /// Копирование объекта. Результат - это копия объекта, в котором все обычные свойства копируются из прежнего объекта
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetSession"></param>
        /// <param name="synchronize"></param>
        /// <returns></returns>
        public object CopyForVersion(IXPSimpleObject source) {
            if (source == null) return null;

            XPClassInfo classInfo = sourceSession.GetClassInfo(source.GetType());
            object copy = classInfo.CreateNewObject(sourceSession);

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


    }
}