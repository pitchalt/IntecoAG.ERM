using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;


using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.StatementAccount;
using DevExpress.ExpressApp.SystemModule;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.ReportHelper;

namespace IntecoAG.ERM.FM {
    public sealed partial class ERMFinancialModule : ModuleBase {
        public ERMFinancialModule() {
            InitializeComponent();
        }

        public override void Setup(XafApplication application) {
            if (!XafTypesInfo.IsInitialized) {
                XafTypesInfo.Instance.RegisterSharedPart(typeof(Docs.Cache.fmIDocCache));
                XafTypesInfo.Instance.RegisterSharedPart(typeof(Docs.Cache.fmIDocCacheLine));
                XafTypesInfo.Instance.RegisterSharedPart(typeof(Docs.Cache.fmIDocCacheIn));
                XafTypesInfo.Instance.RegisterEntity("fmIDocCacheInRealPrepare", typeof(Docs.Cache.fmIDocCacheInRealPrepare));
                XafTypesInfo.Instance.RegisterEntity("fmIDocCacheInRealPrepareLine", typeof(Docs.Cache.fmIDocCacheInRealPrepareLine));
                XafTypesInfo.Instance.RegisterEntity("fmIDocCacheInRealFinal", typeof(Docs.Cache.fmIDocCacheInRealFinal));
                XafTypesInfo.Instance.RegisterEntity("fmIDocCacheJournal", typeof(Docs.Cache.fmIDocCacheJournal));
                XafTypesInfo.Instance.RegisterEntity("fmIDocCacheJournalLine", typeof(Docs.Cache.fmIDocCacheJournalLine));
                //                XafTypesInfo.Instance.RegisterEntity("fmIDocCacheIn", typeof(Docs.Cache.fmIDocCacheIn));
                //                XafTypesInfo.Instance.RegisterEntity("fmIDocCacheOut", typeof(Docs.Cache.fmIDocCacheOut));
            }
            base.Setup(application);
            application.DetailViewCreating += new EventHandler<DetailViewCreatingEventArgs>(application_DetailViewCreating);
            application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(application_CreateCustomCollectionSource);

            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(fmCRHPayedRequestReportParameters));
            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(fmCRHUnpayedRequestReportParameters));
            //SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(fmMapDocsManual));
            //SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(fmCPRCourseEditor));
        }

        void application_DetailViewCreating(object sender, DetailViewCreatingEventArgs e) {
            //throw new NotImplementedException();
//            if (e.Obj is fmCDirection) {
//                e.ViewID = "fmIDirection_DetailView";
//            }
            //if (e.Obj is fmCSubjectExt) {
            //    e.ViewID = "fmISubjectExt_DetailView";
            //}
            //if (e.Obj is fmCOrderExt) {
            //    e.ViewID = "fmIOrderExt_DetailView";
            //}
        }

        void application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
//            if (e.ObjectType == typeof(fmIDirection)) {
//                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(fmCDirection));
//            }
            if (e.ObjectType == typeof(fmISubject)) {
                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(fmCSubject));
            }
            if (e.ObjectType == typeof(fmIOrderExt)) {
                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(fmCOrderExt));
            }
        }
    }
}
