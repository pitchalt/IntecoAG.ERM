using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;

namespace IntecoAG.ERM.CS {
    public class csLinqCollectionSource : CollectionSourceBase {

        private IBindingList collectionCore;
        private ITypeInfo objectTypeInfoCore;
        //public IList ConvertQueryToCollection(IQueryable sourceQuery) {
        //    collectionCore = new BindingList<object>();
        //    foreach (var item in sourceQuery) { collectionCore.Add(item); }
        //    return collectionCore;
        //}
        private IQueryable queryCore = null;
        protected csLinqCollectionSource(IObjectSpace objectSpace, CollectionSourceMode mode)
            : base(objectSpace, mode) {
        }
        protected csLinqCollectionSource(IObjectSpace objectSpace)
            : base(objectSpace) {
        }
        public csLinqCollectionSource(IObjectSpace objectSpace, Type objectType, IQueryable query)
            : base(objectSpace) {
            objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(objectType);
            queryCore = query;
        }
        public IQueryable Query {
            get { return queryCore; }
            set { queryCore = value; }
        }
        protected override object CreateCollection() {
            ((XPQueryBase)Query).Session = ((ObjectSpace)ObjectSpace).Session;
            //return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Query.ElementType), Query);
            //return Activator.CreateInstance(typeof(List<>).MakeGenericType(Query.ElementType), Query);
            //return Activator.CreateInstance(typeof(List<>).MakeGenericType(Query.ElementType), Query);

            var queryList = Activator.CreateInstance(typeof(List<>).MakeGenericType(Query.ElementType), Query);
            return Activator.CreateInstance(typeof(BindingList<>).MakeGenericType(Query.ElementType), queryList);
        }
        public override bool? IsObjectFitForCollection(object obj) {
            return collectionCore.Contains(obj);
        }
        protected override void ApplyCriteriaCore(CriteriaOperator criteria) { }
        public override ITypeInfo ObjectTypeInfo {
            get { return objectTypeInfoCore; }
        }
    }
}