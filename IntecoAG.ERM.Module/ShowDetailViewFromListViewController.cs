using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model;

using DevExpress.ExpressApp.Editors;

using System.Reflection;
using DevExpress.XtraGrid;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// http://www.devexpress.com/Support/Center/p/Q106485.aspx
    /// http://documentation.devexpress.com/#Xaf/CustomDocument2820
    /// http://www.devexpress.com/Support/Center/p/Q97601.aspx
    /// http://www.devexpress.com/Support/Center/p/K18038.aspx
    /// http://www.devexpress.com/Support/Center/p/S34026.aspx
    /// 
    /// � ������ ������:
    /// ����� ��������� DetailView � ���������� �� �������� � ListView 
    /// 
    /// </summary>
    public partial class ShowDetailViewFromListViewController : ViewController<ListView> {

        private SimpleAction sa = new SimpleAction();
        private SimpleAction defaultListViewAction;
        private SimpleAction LoadVAppropriateAction;

        private object currentObject = null;
        private IObjectSpace objSpace = null;

        Type objType = typeof(object);

        //private CustomProcessListViewSelectedItemEventArgs param = null;

        public ShowDetailViewFromListViewController() {
            InitializeComponent();

            TargetViewType = ViewType.ListView;
            TargetObjectType = typeof(SimpleContract);

            this.LoadVAppropriateAction = new SimpleAction(this.components);
            sa.Execute += new SimpleActionExecuteEventHandler(LoadAppropriateDetailView_Execute);
        }


        protected override void OnActivated() {
            base.OnActivated();

            if (View.GetType() == typeof(DashboardView)) {
                return;
            }

            if (View.GetType() == typeof(DetailView)) {
                return;
            }

            if ((((ListView)View).ObjectTypeInfo.Type != typeof(SimpleContract)) & (((ListView)View).ObjectTypeInfo.Type != typeof(ComplexContract))) { return; }

            ListViewProcessCurrentObjectController controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem += new EventHandler<CustomProcessListViewSelectedItemEventArgs>(controller_CustomProcessSelectedItem);
            }

            objSpace = View.ObjectSpace;
        }

        private void controller_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {
            ShowAppropriateView(e);
        }

        private void ShowAppropriateView(CustomProcessListViewSelectedItemEventArgs e) {

            currentObject = e.InnerArgs.CurrentObject;

            if (currentObject == null) return;

            // ������ �� �����
            ContractVersion rgn = null;

            if (currentObject.GetType() == typeof(SimpleContract)) {
                rgn = (SimpleContractVersion)((SimpleContract)currentObject).Current;   // !!! �������� ��  VERSION_CURRENT
            }

            if (currentObject.GetType() == typeof(ComplexContract)) {
                rgn = (ComplexContractVersion)((ComplexContract)currentObject).Current;   // !!! �������� ��  VERSION_CURRENT
            }

            if (rgn == null) return;

            objType = ((System.Reflection.MemberInfo)(rgn.GetType())).DeclaringType;

            // ����� ��������� ����� ��������� DetailView �� ������������� � ������

            TargetObjectType = objType;
            RegisterActions(components);
            defaultListViewAction = LoadVAppropriateAction;

            sa.Application = Application;
            sa.Id = "@" + Guid.NewGuid().ToString();
            sa.TargetObjectType = objType;
            sa.TargetViewType = ViewType.DetailView;


            if (!e.Handled) {
                if ((Frame.Context != TemplateContext.LookupControl) &&
                    (Frame.Context != TemplateContext.LookupWindow)
                    && (DefaultListViewAction != null)
                    && DefaultListViewAction.Active && DefaultListViewAction.Enabled) {

                    e.Handled = true;
                    sa.DoExecute();
/*
                    // ������� ������ DetailView � ������
                    //IModelDetailView modelShowDetailView = View.Model as IModelDetailView;
                    IModelViewHiddenActions actions = View.Model as IModelViewHiddenActions;
                    if (actions != null) {
                        IModelActionLink action = actions.HiddenActions["LoadAppropriateContractDetailView"];

                        //defaultListViewAction = (SimpleAction)action.Action;
                        //defaultListViewAction.Execute += new SimpleActionExecuteEventHandler<SimpleActionExecuteEventArgs>(LoadAppropriateDetailView_Execute);

                        e.Handled = true;
                        sa.DoExecute();

                        //View.ObjectSpace.CommitChanges();
                        //View.ObjectSpace.Refresh();
                    }
 */
                }
            }

        }

        protected override void OnDeactivated() {
            ListViewProcessCurrentObjectController controller = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (controller != null) {
                controller.CustomProcessSelectedItem -= new EventHandler<CustomProcessListViewSelectedItemEventArgs>(controller_CustomProcessSelectedItem);
            }
            base.OnDeactivated();
        }


        //Provide access to the default List View Action so that you can set another Action 
        public SimpleAction DefaultListViewAction {
            get { return defaultListViewAction; }
            set { defaultListViewAction = value; }
        }

        private void LoadAppropriateDetailView_Execute(object sender, SimpleActionExecuteEventArgs e) {

            object currentObject = ((DevExpress.ExpressApp.ListView)(View)).CurrentObject;

            if (currentObject == null) return;

            IObjectSpace objSpace = Frame.Application.CreateObjectSpace();

            //XPCollection<SimpleContractVersion> currentVers = new XPCollection<SimpleContractVersion>(((ObjectSpace)objSpace).Session); //new Session());  //, criteria, sortProps);
            //if (!currentVers.IsLoaded) currentVers.Load();

            //// �������� ��� ������ SimpleContractVersion, � �������
            //// SimpleContract == this
            //OperandProperty prop = new OperandProperty("SimpleContract");
            //CriteriaOperator op = prop == this;
            //currentVers.Filter = op;





            // ������ �� �����
            ContractVersion rgn = null;
            string DetailViewID = ((System.Reflection.MemberInfo)(rgn.GetType())).Name + "_DetailView";  // ((System.RuntimeType)(rgn.GetType())).Name + "_DetailView";

            if (currentObject.GetType() == typeof(SimpleContract)) {
                rgn = (SimpleContractVersion)((SimpleContract)currentObject).Current;   // !!! �������� ��  VERSION_CURRENT
                if (rgn == null) return;

                // ����� DetailView ������������ ������������, �.�. ��� ���� + ������� 
                ShowConcreteDetailView<SimpleContractVersion>(DetailViewID, (SimpleContractVersion)rgn);
            }

            if (currentObject.GetType() == typeof(ComplexContract)) {
                rgn = (ComplexContractVersion)((ComplexContract)currentObject).Current;   // !!! �������� ��  VERSION_CURRENT
                if (rgn == null) return;

                // ����� DetailView ������������ ������������, �.�. ��� ���� + ������� 
                ShowConcreteDetailView<ComplexContractVersion>(DetailViewID, (ComplexContractVersion)rgn);
            }




            //SimpleContractVersion rgn = (SimpleContractVersion)((SimpleContract)currentObject).Current;
            //if (rgn == null) return;

            //// ����� DetailView ������������ ������������, �.�. ��� ���� + ������� 
            //string DetailViewID = ((System.Reflection.MemberInfo)(rgn.GetType())).Name + "_DetailView";  // ((System.RuntimeType)(rgn.GetType())).Name + "_DetailView";
            //ShowConcreteDetailView<SimpleContractVersion>(DetailViewID, rgn);

            // � ����� � ��������� �� �����, �������� �����-���� ������� �������������� �������������� DetailView
            //RegionAmerica rgnAmerica = rgn as RegionAmerica;
            //RegionEuropa rgnEuropa = rgn as RegionEuropa;
            //if (rgnAmerica == null & rgnEuropa == null) return;

            //if (rgnAmerica != null) {
            //    ShowConcreteDetailView<RegionAmerica>("RegionAmerica_DetailView", rgnAmerica);
            //}

            //if (rgnEuropa != null) {
            //    ShowConcreteDetailView<RegionEuropa>("RegionEuropa_DetailView", rgnEuropa);
            //}
        }

        /// <summary>
        /// �������� ���������� ���������� DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(string DetailViewID, classType but) {
            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(but);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            //TaskInvoiceInstanceDefinition objTaskInvoiceInstanceDefinition = objectSpace.CreateObject<TaskInvoiceInstanceDefinition>();
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);
            //ListView dv = Frame.Application.CreateListView(objectSpace, typeof(TaskInvoiceInstanceDefinition), true);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            //Specify various display settings.
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            // Here we show our detail view.
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));

        }

    }

}
