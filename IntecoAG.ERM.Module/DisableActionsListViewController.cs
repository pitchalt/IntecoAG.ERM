using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace IntecoAG.ERM.Module {
    public partial class DisableActionsListViewController : ViewController {
        private const string DefaultReason = "Open Object is active";
        protected override void OnActivated() {
            base.OnActivated();


            /* ƒÀﬂ –¿¡Œ“€ –¿— ŒÃ≈Õ“¿–»“‹
            bool flag = true;
            if (View.Id == "SimpleContract_ListView" | View.Id == "ComplexContract_ListView" | View.Id == "WorkPlan_ListView") {
                flag = false;
            }

            Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.OpenObjectController>().Active[DefaultReason] = flag;

            //Frame.GetController<ListViewProcessCurrentObjectController>().Active[DefaultReason] = flag;
            //Frame.GetController<ListViewProcessCurrentObjectController>().Active[DefaultReason] = flag;
            //Frame.GetController<DeleteObjectsViewController>().Active[DefaultReason] = flag;
            //Frame.GetController<NewObjectViewController>().Active[DefaultReason] = flag;
            //Frame.GetController<FilterController>().Active[DefaultReason] = flag;
            */
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            /* ƒÀﬂ –¿¡Œ“€ –¿— ŒÃ≈Õ“¿–»“‹
            Frame.GetController<DevExpress.ExpressApp.Win.SystemModule.OpenObjectController>().Active.RemoveItem(DefaultReason);

            //Frame.GetController<ListViewProcessCurrentObjectController>().Active.RemoveItem(DefaultReason);
            //Frame.GetController<DeleteObjectsViewController>().Active.RemoveItem(DefaultReason);
            //Frame.GetController<NewObjectViewController>().Active.RemoveItem(DefaultReason);
            //Frame.GetController<FilterController>().Active.RemoveItem(DefaultReason);
            */
        }
    }
}