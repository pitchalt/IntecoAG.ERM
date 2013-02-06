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
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {
    //public partial class ReplaceWinNewObjectViewController : WindowController {
    //    public ReplaceWinNewObjectViewController() {
    //        InitializeComponent();
    //        RegisterActions(components);
    //    }
    //}

    /// <summary>
    /// http://www.devexpress.com/Support/Center/p/K18080.aspx
    /// http://www.devexpress.com/Support/Center/e/E229.aspx
    /// http://documentation.devexpress.com/#Xaf/CustomDocument2915
    /// http://documentation.devexpress.com/#Xaf/CustomDocument2920
    /// </summary>
    public partial class ReplaceWinNewObjectViewController : WinNewObjectViewController {

        public ReplaceWinNewObjectViewController() {
            //InitializeComponent();
            //RegisterActions(components);

            //TargetViewType = ViewType.ListView;
            //TargetObjectType = typeof(SimpleContract);
        }

        //public ReplaceWinNewObjectViewController() {
        //    //TargetViewType = ViewType.ListView;
        //    //TargetObjectType = typeof(SimpleContract);
        //}

        //Subscribe the required events 
        protected override void OnActivated() {
            base.OnActivated();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
        }

        protected override void New(SingleChoiceActionExecuteEventArgs args) {
            View view = View;

            if (view == null) { base.New(args); return; }
            if (view.GetType() != typeof(ListView)) { base.New(args); return; }
            if ((((ListView)view).ObjectTypeInfo.Type != typeof(SimpleContract))) { base.New(args); return; }


            if ((view != null) && (view.GetType() != typeof(ListView))) { base.New(args); return; }

            if ((view != null) && (((ListView)view).ObjectTypeInfo.Type == typeof(SimpleContract))) {

                IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
                //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();

                SimpleContract objSimpleContract = objectSpace.CreateObject<SimpleContract>();

                /*
                SimpleContractVersion scv = objectSpace.CreateObject<SimpleContractVersion>();  //new SimpleContractVersion(this.Session);
                scv.SimpleContract = objSimpleContract;
                objSimpleContract.Current = scv;

                ContractDocument cd = objectSpace.CreateObject<ContractDocument>(); 
                //ContractDocument cd = new ContractDocument(this.Session);
                */

                SimpleContractVersion objSimpleContractVersion = (SimpleContractVersion)objSimpleContract.Current;

                // Пояснение. objSimpleContract при своём создании образовал два объекта SimpleContractVersion со статусом
                // VERSION_NEW, доступный по ссылке objSimpleContract.Current, и ConractDocument

                string detailViewId = "SimpleContractVersion_DetailView";

                DetailView dv = Frame.Application.CreateDetailView(objectSpace, detailViewId, true, objSimpleContractVersion);
                //dv.IsRoot = true;

                ShowViewParameters svp = new ShowViewParameters();
                svp.CreatedView = dv;
                //Specify various display settings.
                svp.TargetWindow = TargetWindow.NewModalWindow;
                svp.Context = TemplateContext.View;
                svp.CreateAllControllers = true;
                // Here we show our detail view.
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));

                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            } else {
                base.New(args);
            }

        }
    }

}
