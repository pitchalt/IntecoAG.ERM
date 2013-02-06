using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {


    /// <summary>
    /// Кнопка. По которой показывается список (некое ListView) свободных WorkPlan'ов с возможностью выбора какого-либо из них
    /// и подключения к ComplexContractVersion
    /// </summary>
    public partial class LinkWorkPlanToComplexContractController : ViewController {
        public LinkWorkPlanToComplexContractController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ListWorkPlanPopupWindowShowAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            //e.View = Application.CreateListView(Application.FindListViewId(typeof(WorkPlan)), new CollectionSource(objectSpace, typeof(WorkPlan)), true);
            e.View = Application.CreateListView("WorkPlan_ListView_Free", new CollectionSource(objectSpace, typeof(WorkPlan)), true);
        }

        private void ListWorkPlanPopupWindowShowAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            // Находим родительский DetailView
            CompositeView parentView = ((NestedFrame)this.Frame).DetailViewItem.View;

            try {
                ComplexContractVersion ccv = (ComplexContractVersion)parentView.CurrentObject;
                ObjectSpace.SetModified(ccv);

                // Присоединяем WorkPlan к коллекции
                foreach (WorkPlan workPlan in e.PopupWindow.View.SelectedObjects) {
                    WorkPlanVersion wpv = ObjectSpace.GetObject<WorkPlanVersion>(workPlan.Current);
                    if (ccv.WorkPlanVersions.IndexOf(wpv) == -1) {
                        ccv.WorkPlanVersions.Add(wpv);
                    }
                }

                View.ObjectSpace.CommitChanges();
            }
            catch (Exception ex) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Привязывание версии рабочего плана доступно только в версиях контракта");
            }
        }
    }
}
