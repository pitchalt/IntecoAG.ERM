using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;


namespace IntecoAG.ERM.FM {

    public static class SessionHelper {
        public static object GetObjectInSession(IXPObject obj, Session session) {
            if (obj == null)
                return obj;
            return session.GetObjectByKey(obj.GetType(), obj.ClassInfo.GetId(obj));
        }

        // generic version
        public static T GetObjectInSession<T>(T obj, Session session) where T : IXPObject {
            if (obj == null)
                return obj;
            return session.GetObjectByKey<T>(session.GetKeyValue(obj));
        }
    }

}
