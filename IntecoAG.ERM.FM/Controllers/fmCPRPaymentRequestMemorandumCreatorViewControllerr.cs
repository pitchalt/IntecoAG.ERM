//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
//
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM.Controllers {

    /// <summary>
    /// Данный контроллер имеет кнопку для создания Служебной записки по параметрам на форме
    /// </summary>
    public partial class fmCPRPaymentRequestMemorandumCreatorViewController : ObjectViewController {

        public fmCPRPaymentRequestMemorandumCreatorViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
        }

        private void NextRequest_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View.CurrentObject != null && View.CurrentObject as fmCPRPaymentRequestMemorandumCreator != null) {
                fmCPRPaymentRequestMemorandumCreator mrc = View.CurrentObject as fmCPRPaymentRequestMemorandumCreator;

                IObjectSpace objectSpace = Application.CreateObjectSpace();   // ObjectSpace.CreateNestedObjectSpace();   // Application.CreateObjectSpace();
                fmCPRPaymentRequestMemorandumCreator mrc1 = objectSpace.GetObject<fmCPRPaymentRequestMemorandumCreator>(mrc);
                fmPaymentRequestMemorandum newRequestMemorandum = mrc1.CreateRequestMemorandum(objectSpace);
                //fmPaymentRequestMemorandum newRequestMemorandum1 = objectSpace.GetObject<fmPaymentRequestMemorandum>(newRequestMemorandum);
                //if (mrc.CreatingTemplate && newRequestMemorandum != null) {
                //    newRequestMemorandum.State = PaymentRequestStates.TEMPLATE;
                //}

                string DetailViewId = "fmPaymentRequestMemorandum_DetailView_With_PersonData";    // Frame.Application.FindDetailViewId(newRequestMemorandum.GetType());

                TargetWindow openMode = TargetWindow.Current;   // TargetWindow.NewModalWindow;
                DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, newRequestMemorandum);

                ShowViewParameters svp = new ShowViewParameters() {
                    CreatedView = dv,
                    TargetWindow = openMode,
                    Context = TemplateContext.View,
                    CreateAllControllers = true
                };

                e.ShowViewParameters.Assign(svp);
            }
        }


/*
        private void NextRequest_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View.CurrentObject != null && View.CurrentObject as fmCPRPaymentRequestMemorandumCreator != null) {
                fmCPRPaymentRequestMemorandumCreator mrc = View.CurrentObject as fmCPRPaymentRequestMemorandumCreator;

                IObjectSpace objectSpace = ObjectSpace.CreateNestedObjectSpace();   // Application.CreateObjectSpace();
                fmCPRPaymentRequestMemorandumCreator mrc1 = objectSpace.GetObject<fmCPRPaymentRequestMemorandumCreator>(mrc);
                fmPaymentRequestMemorandum newRequestMemorandum = mrc1.CreateRequestMemorandum();
                fmPaymentRequestMemorandum newRequestMemorandum1 = objectSpace.GetObject<fmPaymentRequestMemorandum>(newRequestMemorandum);
                //if (mrc.CreatingTemplate && newRequestMemorandum != null) {
                //    newRequestMemorandum.State = PaymentRequestStates.TEMPLATE;
                //}

                string DetailViewId = Frame.Application.FindDetailViewId(newRequestMemorandum1.GetType());

                TargetWindow openMode = TargetWindow.NewModalWindow;
                DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, newRequestMemorandum1);

                ShowViewParameters svp = new ShowViewParameters() {
                    CreatedView = dv,
                    TargetWindow = openMode,
                    Context = TemplateContext.View,
                    CreateAllControllers = true
                };

                e.ShowViewParameters.Assign(svp);
            }
        }
*/
    }
}
