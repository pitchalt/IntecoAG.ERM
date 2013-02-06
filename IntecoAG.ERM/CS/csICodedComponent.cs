using System;
//
using DevExpress.Persistent.Base;
using DC = DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.CS {

    [DC.XafDefaultProperty("Name")]
    public interface csICodedComponent : csIComponent {
        /// <summary>
        /// Object code
        /// </summary>
        [DC.FieldSize(7)]
        [VisibleInLookupListView(true)]
        String Code { get; set; }
        /// <summary>
        /// Object Name
        /// </summary>
        [DC.FieldSize(80)]
        String Name { get; set; }
        /// <summary>
        /// Description of object
        /// </summary>
        [DC.FieldSize(-1)]
        [VisibleInListView(false)]
        String Description { get; set; }
    }
}
