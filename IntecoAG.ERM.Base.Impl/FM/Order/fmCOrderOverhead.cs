using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.Order {
    
    [Persistent("fmOrderOverhead")]
    public abstract class fmCOrderOverhead: csCComponent, fmIOrderOverhead {
        public fmCOrderOverhead(Session session): base(session) { 

        }

        private fmIOrderOverheadValueType _PlanOverheadType;
        private fmIOrderOverheadValueType _BuhOverheadType;
        private Decimal _KoeffKB;
        private Decimal _KoeffOZM;

        public fmIOrderOverheadValueType PlanOverheadType {
            get { return _PlanOverheadType; }
            set { SetPropertyValue<fmIOrderOverheadValueType>("PlanOverheadType", ref _PlanOverheadType, value); }
        }
        public fmIOrderOverheadValueType BuhOverheadType {
            get { return _BuhOverheadType; }
            set { SetPropertyValue<fmIOrderOverheadValueType>("BuhOverheadType", ref _BuhOverheadType, value); }
        }
        public Decimal FixKoeff {
            get { return _KoeffKB; }
            set { SetPropertyValue("FixKoeff", ref _KoeffKB, value); }
        }
        public Decimal FixKoeffOZM {
            get { return _KoeffOZM; }
            set { SetPropertyValue("FixKoeff", ref _KoeffOZM, value); }
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    [FriendlyKeyProperty("Code")]
    public class fmCOrderOverheadStandart : fmCOrderOverhead, fmIOrderOverheadStandart { 
        public fmCOrderOverheadStandart(Session session): base(session) { 

        }
        private String _Code;
        private String _Name;
        private String _Description;

        [Size(14)]
        [RuleRequiredField]
        public virtual String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        [Size(60)]
        [RuleRequiredField]
        public virtual String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public virtual String Description {
            get { return _Description; }
            set { SetPropertyValue<String>("Description", ref _Description, value); }
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCOrderOverheadIndividual : fmCOrderOverhead, fmIOrderOverheadIndividual {
        public fmCOrderOverheadIndividual(Session session): base(session) { 

        }
    }

}
