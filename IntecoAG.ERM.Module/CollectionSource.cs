using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
///
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.Module {
    //
    public class InterfaceCollectionSource<CT, PT> : CollectionSource
                            where PT : CT {

        public const string DefaultSuffix = "_Linq";

        //        public IList ConvertQueryToCollection(IQueryable sourceQuery) {
        //            List<object> list = new List<object>();
        //            foreach (var item in sourceQuery) { list.Add(item); }
        //            return list;
        //        }

        //        private IQueryable queryCore = null;
        //        public IQueryable Query {
        //            get { return queryCore; }
        //            set { 
        //                if (value == null) {
        //                    throw new ArgumentNullException("Query");
        //                }
        //                queryCore = value;
        //            }
        //        }
        //        override Rec
        protected override object CreateCollection() {
            XPCollection<PT> col = new XPCollection<PT>(((ObjectSpace)this.ObjectSpace).Session);

            return new ListConverter<CT, PT>(col);
            //base.RecreateCollection(CriteriaOperator criteria, SortingCollection sortings);
        }

        public InterfaceCollectionSource(IObjectSpace objectSpace)
            : base(objectSpace, typeof(CT)) { }

    }
}
