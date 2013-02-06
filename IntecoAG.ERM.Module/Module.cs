using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Module
{

    public sealed partial class ERMBaseModule : ModuleBase
    {
        public ERMBaseModule() {
            InitializeComponent();
//            if (CriteriaOperator.GetCustomFunction("ILike") == null)
//                CriteriaOperator.RegisterCustomFunction(new PostgessILikeOperator());
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            Application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(Application_CreateCustomCollectionSource);
            Application.CreateCustomObjectSpaceProvider += new EventHandler<CreateCustomObjectSpaceProviderEventArgs>(Application_CreateCustomObjectSpaceProvider);
            //
            //ConnectionProviderSql provider = (ConnectionProviderSql) 
            //        XpoDefault.GetConnectionProvider(
            //            AccessConnectionProvider.GetConnectionString(@"..\..\CustomFunction.mdb"),
            //            AutoCreateOption.DatabaseAndSchema);
            //XPDictionary dict = new ReflectionDictionary();

            // New registration technique. 
            //CriteriaOperator.RegisterCustomFunction(new MyGetMonthFunction());

            // Outdated registration technique. 
            //provider.RegisterCustomFunctionOperator(new PostgessILikeOperator()); 
            //dict.CustomFunctionOperators.Add(new PostgessILikeOperator()); 

        }

        void Application_CreateCustomObjectSpaceProvider(object sender, CreateCustomObjectSpaceProviderEventArgs e) {
//            e.ObjectSpaceProvider = new ObjectSpaceProvider();
        }

        void Application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            if (e.ObjectType == typeof(crmIParty)) {
//                e.CollectionSource = new  InterfaceCollectionSource<crmIParty, crmPartyRu>(e.ObjectSpace);
                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(crmPartyRu));
            }
            if (e.ObjectType == typeof(crmIPerson)) {
                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(crmCPerson));
            }
        }

        public override IList<PopupWindowShowAction> GetStartupActions() {
            IList<PopupWindowShowAction> startupActions = base.GetStartupActions();
            PopupWindowShowAction selectUserPartyAction = new PopupWindowShowAction();
            selectUserPartyAction.CustomizePopupWindowParams +=
              new CustomizePopupWindowParamsEventHandler(selectUserPartyAction_CustomizePopupWindowParams);
            selectUserPartyAction.Execute +=
              new PopupWindowShowActionExecuteEventHandler(selectUserPartyAction_Execute);
            startupActions.Add(selectUserPartyAction);
            return startupActions;
        }

        void selectUserPartyAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            crmUserParty.CurrentUserParty = ValueManager.CreateValueManager<crmUserParty>();
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            String listViewId = Application.FindListViewId(typeof(crmUserParty));
            e.View = Application.CreateListView(
               listViewId,
               Application.CreateCollectionSource(objectSpace, typeof(crmUserParty), listViewId),
               true);
            e.DialogController = new DialogController();
        }
        void selectUserPartyAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            if (e.PopupWindow.View.SelectedObjects.Count > 0) {
                crmUserParty.CurrentUserParty.Value = (crmUserParty)e.PopupWindow.View.SelectedObjects[0];
            }
        }

        // Вставка в модель узла для мининавигатора

        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelView, IModelMiniNavigationExtension>();
        }



    }
}
