using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Module
{
    public sealed partial class ERMModule : ModuleBase
    {
        public ERMModule() {
            InitializeComponent();
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
    }
}
