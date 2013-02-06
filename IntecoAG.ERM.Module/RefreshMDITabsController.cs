using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

namespace IntecoAG.ERM.Module {
    public partial class RefreshMDITabsController : WindowController {
        public RefreshMDITabsController() {
            InitializeComponent();
            RegisterActions(components);

            TargetWindowType = WindowType.Main;
        }


        protected override void OnActivated() {
            base.OnActivated();
            Frame.TemplateChanged += new EventHandler(Frame_TemplateChanged);
        }

        void Frame_TemplateChanged(object sender, EventArgs e) {
            System.Windows.Forms.Form form = Frame.Template as System.Windows.Forms.Form;
            if (form == null) return;
            form.MdiChildActivate += new EventHandler(form_MdiChildActivate);
        }

        void form_MdiChildActivate(object sender, EventArgs e) {
            System.Windows.Forms.Form currentMdiForm = ((System.Windows.Forms.Form)sender).ActiveMdiChild;
            WinShowViewStrategyBase strategy = Application.ShowViewStrategy as WinShowViewStrategyBase;
            foreach (WinWindow frame in strategy.Windows) {
                if (frame.View == null) continue;
                ListView listView = frame.View as ListView;
                if (listView == null) continue;

                listView.CollectionSource.Reload();

/*
                return;

                if (frame.Form != currentMdiForm) continue;

                //listView.ObjectSpace.ReloadCollection(listView);
                //listView.Refresh();

                // Всякая информация
                string viewId = listView.Id;
                Type objType = listView.ObjectTypeInfo.Type;
                bool isroot = listView.IsRoot;
                object currentObject = listView.CurrentObject;

                string selectedOid = "";
                if (listView.SelectedObjects.Count > 0) {
                    BaseObject selectedObj = (BaseObject)listView.SelectedObjects[0];
                    if (selectedObj != null) selectedOid = selectedObj.Oid.ToString();
                    //IList<> selectedObjects = listView.SelectedObjects;
                }

                IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
                ListView lv = frame.Application.CreateListView(objectSpace, objType, isroot);

                if (!string.IsNullOrEmpty(selectedOid)) {
                    object selObj = objectSpace.FindObject(objType, CriteriaOperator.Parse("Oid = '" + selectedOid + "'"));
                    //if (selObj != null) lv.SelectedObjects.Add(selObj);
                }

                frame.SetView(lv, frame);
                listView.Close();

                

                //Type objType = typeof(listView.ObjectTypeInfo);
                //SetConcreteListView<classType>(frame);

                //frame.SetView(listView);
                //if (frame.View.CurrentObject != null) frame.View.ObjectSpace.ReloadObject(frame.View.CurrentObject);
                //listView.ObjectSpace.Refresh();

                //frame.View.ObjectSpace.Refresh();
                //frame.View.Refresh();
                //if (frame.View.CurrentObject != null) frame.View.ObjectSpace.ReloadObject(frame.View.CurrentObject);
*/
            }
        }

    }
}
