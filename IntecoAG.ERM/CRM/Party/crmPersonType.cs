using System;
using System.ComponentModel;

using DevExpress.ExpressApp;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace IntecoAG.ERM.CRM.Party {

    [FriendlyKeyProperty("Code")]
    [DefaultProperty("Name")]
    [Persistent("crmPartyPersonType")]
    public class crmPersonType : BaseObject {
        public crmPersonType(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        //
        private string _Code;
        private string _Name;
        /// <summary>
        /// Code
        /// </summary>
        [Size(6)]
        public string Code {
            get { return _Code; }
            set {
                SetPropertyValue("Code", ref _Code, value == null ? String.Empty : value.Trim()); 
            }
        }
        /// <summary>
        /// Name
        /// </summary>
        [Size(70)]
        public string Name {
            get { return _Name; }
            set { 
                SetPropertyValue("Name", ref _Name, value == null ? String.Empty : value.Trim()); 
            }
        }
    }

}