// Developer Express Code Central Example:
// How to Customize the New Action's Items List
// 
// When an application contains numerous business classes, the New Action can
// contain most of them in its Items list. In this instance, the use of this Action
// can become cumbersome. To make manipulations with this Action easier, you can
// add to the Action's Items list only those types that are contained in the
// currently selected navigation control group. To group navigation control items,
// use the Application Model's NavigationItems node. To customize the New Action's
// Items list, handle the NewObjectViewController.CollectDescendants and
// NewObjectViewController.CollectRoot events of the NewObjectViewController, which
// contains the New Action. The former event is raised when the current object type
// and its descendants are added to the Action's Items list, and the latter is
// raised when all the remaining types whose CreatableItem attribute is set to true
// in the Application Model are added. This example demonstrates how to do
// this.
// 
// See code in the BusinessClasses.cs and MyController.cs
// (BusinessClasses.vb and MyController.vb) files. For details, refer to the How
// to: Customize the New Action's Items List
// (ms-help://DevExpress.Xaf/CustomDocument2915.htm) topic in XAF documentation.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E238

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.SystemModule;
using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {
public partial class MyWinNewObjectViewController : WinNewObjectViewController {
	//Subscribe the required events
	protected override void OnActivated() {
		//Get the ShowNavigationItemController,
		//then get its ShowNavigationItemAction and subscribe the SelectedItemChanged event
		Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.SelectedItemChanged +=
			new EventHandler(ShowNavigationItemAction_SelectedItemChanged);
		CollectCreatableItemTypes += new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(MyController_CollectCreatableItemTypes);
		CollectDescendantTypes += new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(MyController_CollectDescendantTypes);
		base.OnActivated();
	}
	void MyController_CollectDescendantTypes(object sender, DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs e) {
		//CustomizeList(e.Types);
	}
	void MyController_CollectCreatableItemTypes(object sender, DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs e) {
		CustomizeList(e.Types);
	}
	void ShowNavigationItemAction_SelectedItemChanged(object sender, EventArgs e) {
		this.UpdateActionState();
	}
	public void CustomizeList(ICollection<Type> types) {
        if (!types.Contains(typeof(crmContractNewForm)))
            types.Add(typeof(crmContractNewForm));
        ////Get the ShowNavigationItemController, then get its ShowNavigationItemAction
        //SingleChoiceAction showNavigationItemAction =
        //    Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction;
        ////Get the item selected in the navigation control
        //ChoiceActionItem selectedItem = showNavigationItemAction.SelectedItem;
        //ChoiceActionItem currentGroup = null;
        //if(selectedItem != null) {
        //    //Get the selected item's parent group
        //    currentGroup = selectedItem.ParentItem;
        //    List<Type> unusableTypes = new List<Type>();
        //    //Collect the types that must be deleted
        //    foreach(Type type in types) {
        //        bool deletionRequired = true;
        //        foreach(ChoiceActionItem item in currentGroup.Items) {
        //            ViewShortcut shortcut = item.Data as ViewShortcut;
        //            if(shortcut.ViewId == Application.FindListViewId(type)) {
        //                deletionRequired = false;
        //            }
        //        }
        //        if(deletionRequired == true)
        //            unusableTypes.Add(type);
        //    }
        //    //Remove the collected types
        //    foreach(Type type in unusableTypes)
        //        types.Remove(type);
        //}
	}
	//Unsubscribe from the events
	protected override void OnDeactivated() {
		Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.SelectedItemChanged -=
			new EventHandler(ShowNavigationItemAction_SelectedItemChanged);
		CollectCreatableItemTypes -= new EventHandler<CollectTypesEventArgs>(MyController_CollectCreatableItemTypes);
		CollectDescendantTypes -= new EventHandler<CollectTypesEventArgs>(MyController_CollectDescendantTypes);
        base.OnDeactivated();
	}
}
}
