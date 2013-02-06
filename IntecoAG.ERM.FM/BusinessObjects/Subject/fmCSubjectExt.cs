using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM.Subject {
    [NavigationItem("Finance")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [VisibleInReports]
    public class fmCSubjectExt : fmCSubject, fmISubjectExt {
        public fmCSubjectExt(Session ses) : base(ses) { }

        public override void  AfterConstruction() {
            this.ComponentType = typeof(fmCSubjectExt);
            this.CID = Guid.NewGuid();
            base.AfterConstruction();
        }

        IList<fmIOrderExt> fmISubjectExt.Orders {
            get { throw new NotImplementedException(); }
        }

        [Browsable(false)]
        public XPCollection<fmCOrderExt> OrderExts {
            get {
                return new XPCollection<fmCOrderExt>(this.Session, this.Orders);
                //XPCollection<fmCOrderExt> res = new XPCollection<fmCOrderExt>(this.Session, this.Orders);
                //foreach (fmCOrder ord in this.Orders) {
                //    fmCOrderExt ext = ord as fmCOrderExt;
                //    if (ext != null)
                //        res.Add(ext);
                //}
                //return res;
            }
        }
    }
}
