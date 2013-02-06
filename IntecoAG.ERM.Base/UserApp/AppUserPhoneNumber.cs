using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.Module {

	[DefaultProperty("Number")]
    [Persistent("AppPhoneNumber")]
	public class AppPhoneNumber : BaseObject, IPhoneNumber {

		private PhoneNumberImpl phone = new PhoneNumberImpl();

		public AppPhoneNumber(Session session) : base(session) { }
		public override string ToString() {
			return Number;
		}

		[Persistent]
		public string Number {
			get { return phone.Number; }
			set {
				phone.Number = value;
				OnChanged("Number");
			}
		}

        private AppUser _AppUser = null;
        [Association("AppUser-AppPhoneNumbers")]
        public AppUser AppUser {
            get { return _AppUser; }
			set {
                _AppUser = value;
                OnChanged("AppUser");
			}
		}

		public string PhoneType {
			get { return phone.PhoneType; }
			set {
				phone.PhoneType = value;
				OnChanged("PhoneType");
			}
		}
	}

	public class PhoneType : BaseObject {
		public PhoneType(Session session) : base(session) { }
		private string typeName;
		public string TypeName {
			get { return typeName; }
			set {
				typeName = value;
				OnChanged("TypeName");
			}
		}
	}

}
