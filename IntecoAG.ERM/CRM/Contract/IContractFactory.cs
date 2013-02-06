using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CRM.Contract {
    public interface IContractFactory {
        BaseObject Create(crmContractNewForm frm);
    }
}
