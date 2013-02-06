using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.CS {

    public interface csIComponent {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        Guid   CID { get; }
        /// <summary>
        /// 
        /// </summary>
        Type   ComponentType { get;  }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        Object ComponentObject { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inter"></param>
        /// <returns></returns>
        Object GetInterfaceImplementation(Type inter);
    }
}
