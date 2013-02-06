using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.CS.Common {

    /// <summary>
    /// Указывает свойство, которое будет представлять объект при его открытии из списка ListView
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RepresentativePropertyAttribute : Attribute {

        //public LikeSearchPathListAttribute(params string[] prms) { }
        // Тогда применение: [LikeSearchPathList("string1", "string2", "string3", "string4")]

        //...
        // Можно и так:
        public string RepresentativeProperty { get; set; }
        public bool Enable { get; set; }

        public RepresentativePropertyAttribute(string representativeProperty) {
            this.RepresentativeProperty = representativeProperty;
            this.Enable = true;
        }

        public RepresentativePropertyAttribute(string representativeProperty, bool enable) {
            this.RepresentativeProperty = representativeProperty;
            this.Enable = enable;
        }

        public RepresentativePropertyAttribute(bool enable) {
            this.Enable = enable;
        }

        public RepresentativePropertyAttribute() {
            this.Enable = false;
        }

        
        // Тогда применение: [LikeSearchPathList(new string[] { "string1", "string2", "string3", "string4" })]


    }
}
