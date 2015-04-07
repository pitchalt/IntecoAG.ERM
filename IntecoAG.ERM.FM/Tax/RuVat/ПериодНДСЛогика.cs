using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace IntecoAG.ERM.FM.Tax.RuVat {

    public class ПериодНДСЛогика {
        public static ПериодНДС ПолучитьПериод(IObjectSpace os, DateTime date, ref IList<ПериодНДС> periods ) {
            ПериодНДС result = null;
            if (periods == null) {
                periods = os.GetObjects<ПериодНДС>();
            }
            foreach(var period in periods) {
                if (period.ДатаС <= date && date < period.ДатаПо.AddDays(1)) { 
                    result = period;
                    break;
                }
            }
            if (result == null) {
                result = os.CreateObject<ПериодНДС>();
                result.TimeBorderSet(date);
            }
            return result;
        }
    }
}
