using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.CS.Common {
    
    class ConverterType2String: ValueConverter<Type, String> {

        public override String ConvertTo(Type type) {
            return type.FullName;
        }

        public override Type ConvertFrom(String str) {
            return Type.GetType(str);
        }
    }
}
