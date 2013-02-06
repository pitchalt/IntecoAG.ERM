using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.GFM;

namespace IntecoAG.ERM.FM.Subject {
    public interface IDirection: IAbstractAnalitic {
        IList<ISubject> Subjects { get; }
    }
}
