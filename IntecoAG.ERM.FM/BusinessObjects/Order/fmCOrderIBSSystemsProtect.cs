using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.Order {
    [Persistent("fmOrderIBSSystemsProtect")]
    public class fmCOrderIBSSystemsProtect : BaseObject {
        public fmCOrderIBSSystemsProtect(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _SysASRZ = true;
            _SysASUM = true;
            _SysUOC = true;
            _SysKOM = true;
            ProtectString = "++++ ";
        }

        private fmCOrderExt _Order;
        [Browsable(false)]
        public fmCOrderExt Order {
            get { return _Order; }
            set { 
                SetPropertyValue<fmCOrderExt>("Order", ref _Order, value);
                if (!IsLoading && value != null) {
                    value.IBSSystemsProtectString = ProtectString;
                }
            }
        }

        private String _ProtectString;
        public String ProtectString {
            get { return _ProtectString; }
            set { 
                SetPropertyValue<String>("ProtectString", ref _ProtectString, value);
                if (!IsLoading) {
                    if (Order != null)
                        Order.IBSSystemsProtectString = ProtectString;
                }
            }
        }

        private Boolean _SysASRZ;
        public Boolean SysASRZ {
            get { return _SysASRZ; }
            set { 
                SetPropertyValue<Boolean>("SysASRZ", ref _SysASRZ, value);
                if (!IsLoading)
                    UpdateProtectString();
            }
        }
        private Boolean _SysASUM;
        public Boolean SysASUM {
            get { return _SysASUM; }
            set { 
                SetPropertyValue<Boolean>("SysASUM", ref _SysASUM, value);
                if (!IsLoading)
                    UpdateProtectString();
            }
        }
        private Boolean _SysKOM;
        public Boolean SysKOM {
            get { return _SysKOM; }
            set { 
                SetPropertyValue<Boolean>("SysKOM", ref _SysKOM, value);
                if (!IsLoading)
                    UpdateProtectString();
            }
        }
        private Boolean _SysUOC;
        public Boolean SysUOC {
            get { return _SysUOC; }
            set { 
                SetPropertyValue<Boolean>("SysUOC", ref _SysUOC, value);
                if (!IsLoading)
                    UpdateProtectString();
            }
        }

        void UpdateProtectString() {
            String result = "";
            if (SysASRZ)
                result = result + "+";
            else
                result = result + " ";
            if (SysASUM)
                result = result + "+";
            else
                result = result + " ";
            if (SysKOM)
                result = result + "+";
            else
                result = result + " ";
            if (SysUOC)
                result = result + "+";
            else
                result = result + " ";
            result += " ";
            this.ProtectString = result;
        }

        public override string ToString() {
            return ProtectString;
        }
    }

}
