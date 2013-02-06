using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace IntecoAG.ERM.CS.Common {
    public abstract class ValueConverter<F, T> : ValueConverter  {
        public override object ConvertToStorageType(object value) {
            return ConvertTo((F) value);
        }
        public override object ConvertFromStorageType(object value) {
            return ConvertFrom((T)value);
        }

        
        public abstract T ConvertTo(F value);
        public abstract F ConvertFrom(T value);

        public override Type StorageType {
            get { return typeof(T); }
        }
           
    }
}
