using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Xpo.Metadata;
using System.Collections;

namespace IntecoAG.ERM.CS {

    public class CopyIXPSimpleObjectHelper {


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
        public CopyIXPSimpleObjectHelper(Session source, Session target) {
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
        /// Копирование объекта. Результат - это копия объекта, в котором все обычные свойства копируются из прежжнего объекта
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetSession"></param>
        /// <param name="synchronize"></param>
        /// <returns></returns>
        public object Copy(IXPSimpleObject source, Session targetSession, bool synchronize) {
            if(source == null) return null;

            XPClassInfo targetClassInfo = targetSession.GetClassInfo(source.GetType());
            object copy = targetClassInfo.CreateNewObject(targetSession);

            foreach (XPMemberInfo m in targetClassInfo.PersistentProperties) {
                if (m is DevExpress.Xpo.Metadata.Helpers.ServiceField || m.IsKey) continue;
                m.SetValue(copy, m.GetValue(source));
            }

            foreach (XPMemberInfo m in targetClassInfo.CollectionProperties) {
                if (m.HasAttribute(typeof(AggregatedAttribute))) {
                    XPBaseCollection colCopy = (XPBaseCollection)m.GetValue(copy);
                    XPBaseCollection colSource = (XPBaseCollection)m.GetValue(source);
                    foreach (IXPSimpleObject obj in new ArrayList(colSource))
                        colCopy.BaseAdd(obj);
                }
            }

            return copy;
        }
    }
}