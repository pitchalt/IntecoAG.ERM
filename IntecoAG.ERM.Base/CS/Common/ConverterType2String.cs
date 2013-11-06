using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.CS.Common {
    
    public class ConverterType2String: ValueConverter<Type, String> {

        public override String ConvertTo(Type type) {
            if (type == null) return null;  // SHU 2011-12-26 при прогоне Unit тестов при заполнении Order value == null
            return type.FullName;
        }

        public override Type ConvertFrom(String str) {
            if (str == null) return null; // SHU 2011-12-26 При попытке выдачи отчётов str может окзаться == 
            return Type.GetType(str);
        }
    }
}
