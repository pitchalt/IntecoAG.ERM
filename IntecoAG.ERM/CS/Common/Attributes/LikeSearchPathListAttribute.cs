using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.CS.Common {

    /// <summary>
    /// Содержит список путей к свойствам класса и входящих в него по ссылкам классов для построения критерия Like
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
//    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class LikeSearchPathListAttribute : Attribute {

        //public LikeSearchPathListAttribute(params string[] prms) { }
        // Тогда применение: [LikeSearchPathList("string1", "string2", "string3", "string4")]

        //...
        // Можно и так:
        public string[] Values { get; set; }

        public LikeSearchPathListAttribute(string[] values) {
            this.Values = values;
        }

        // Тогда применение: [LikeSearchPathList(new string[] { "string1", "string2", "string3", "string4" })]


    }
}
