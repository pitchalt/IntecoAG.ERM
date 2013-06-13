using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
//
using IntecoAG.XAFExt.CDS;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Forms;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CS.Nomenclature;
//
namespace IntecoAG.ERM.Module
{

    public sealed partial class ERMBaseModule : ModuleBase
    {
        public ERMBaseModule() {
            InitializeComponent();

            csCSecurityUserId.Register();
            csCSecurityUserStaff.Register();
            csCSecurityUserDepartment.Register();

            //ParametersFactory.RegisterParameter(new csCSecurityUserIdParameter());

            crmCPartyUserPartyCurrent.Register();
            // SHU 2011-12-08 Регистрация самодельного оператора критерия ILike
            // Работает этот ILike с ошибкой, устранить которую пока не удалось,
            // поэтому регистрацию комментарим
            //if (CriteriaOperator.GetCustomFunction("ILike") == null)
            //    CriteriaOperator.RegisterCustomFunction(new PostgessILikeOperator());
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            Application.CreateCustomCollectionSource += Application_CreateCustomCollectionSource;
            Application.DetailViewCreating += Application_DetailViewCreating;
            Application.DetailViewCreated += Application_DetailViewCreated;
            //
            application.SetupComplete += application_SetupComplete;
            //
            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(crmCommonRegistrationForm));
            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(crmDealRegistrationForm));
            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(crmContractRegistrationForm));
            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(crmDealRegistrationStatistics));
            SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(csCNMCourseEditor));
            //
            //SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(ActionExecutePermissionDescriptor));
            //SecurityStrategy.SecuredNonPersistentTypes.Add(typeof(ActionExecutePermissionDescriptorsList));
            //
            CustomCollectionSourceManager.Register(typeof(crmDealRegistrationStatistics));
        }

        void Application_DetailViewCreated(object sender, DetailViewCreatedEventArgs e) {
            //throw new NotImplementedException();
        }

        void Application_DetailViewCreating(object sender, DetailViewCreatingEventArgs e) {
            
        }

        void Application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            if (e.ObjectType == typeof(crmIParty)) {
//                e.CollectionSource = new  InterfaceCollectionSource<crmIParty, crmPartyRu>(e.ObjectSpace);
                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(crmCParty));
            }
            if (e.ObjectType == typeof(crmIPerson)) {
                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(crmCPerson));
            }
            if (e.ObjectType == typeof(hrmIStaff)) {
                e.CollectionSource = new CollectionSource(e.ObjectSpace, typeof(hrmStaff));
            }
            //if (e.ObjectType == typeof(crmDealRegistrationStatistics)) { 
            //    e.CollectionSource = 
            //        new csLinqCollectionSource(e.ObjectSpace, 
            //            typeof(crmDealRegistrationStatistics), 
            //            crmDealRegistrationStatistics.Query(((ObjectSpace)e.ObjectSpace).Session));
            //}
        }

        public override IList<PopupWindowShowAction> GetStartupActions() {
            IList<PopupWindowShowAction> startupActions = base.GetStartupActions();
            PopupWindowShowAction selectUserPartyAction = new PopupWindowShowAction();
            selectUserPartyAction.CustomizePopupWindowParams += selectUserPartyAction_CustomizePopupWindowParams;
            selectUserPartyAction.Execute += selectUserPartyAction_Execute;
            startupActions.Add(selectUserPartyAction);
            return startupActions;
        }

        void selectUserPartyAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            crmUserParty.CurrentUserParty = ValueManager.GetValueManager<crmUserParty>("UserParty");
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

        void application_SetupComplete(object sender, EventArgs e) {
            //if (SecuritySystem.Instance is SecurityStrategy) {
            //    ((SecurityStrategy)SecuritySystem.Instance).RequestProcessors.Register(
            //        new ActionPermissionRequestProcessor());
            //}

            //if (SecuritySystem.Instance is SecurityStrategyComplex) {
            //    ((SecurityStrategyComplex)SecuritySystem.Instance).RequestProcessors.Register(
            //        new ActionPermissionRequestProcessor());
            //}
        }
    }
}
