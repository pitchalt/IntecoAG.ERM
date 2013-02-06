using System;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
//

namespace IntecoAG.ERM.CRM.Party {

    /// <summary>
    /// Данный контроллер устанавливает кнопку "Проверено"
    /// </summary>
    public partial class crmCLegalPersonUnitViewController : ObjectViewController {

        String DO_ENABLED = "DO_ENABLED";

        public crmCLegalPersonUnitViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
            EnableButton();
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e) {
            EnableButton();
        }

        private void EnableButton() {
            if (View == null || View.CurrentObject == null || View.CurrentObject as crmCLegalPersonUnit == null)
                return;
            crmCLegalPersonUnit current = View.CurrentObject as crmCLegalPersonUnit;
            if (current.ManualCheckStatus == ManualCheckStateEnum.IS_CHECKED) {
                this.SetChecked.Enabled[DO_ENABLED] = false;
            } else {   //if (current.ManualCheckStatus == ManualCheckStateEnum.IS_CHECKED) {
                this.SetChecked.Enabled[DO_ENABLED] = true;
            }
        }

        protected override void OnDeactivated() {
            if (this.SetChecked.Enabled.Contains(DO_ENABLED)) {
                this.SetChecked.Enabled.RemoveItem(DO_ENABLED);
            }
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            base.OnDeactivated();
        }

        // Простановка штампа о ручной проверке
        private void SetChecked_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View != null && View.CurrentObject != null && View.CurrentObject as crmCLegalPersonUnit != null) {
                crmCLegalPersonUnit current = View.CurrentObject as crmCLegalPersonUnit;
                current.ManualCheckStatus = ManualCheckStateEnum.IS_CHECKED;
                EnableButton();
            }
        }
    }
}
