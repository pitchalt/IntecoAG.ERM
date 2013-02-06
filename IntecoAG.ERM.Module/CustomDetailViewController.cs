using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Module {

    // "Шаблон" для перекрытия действий SaveAndNew, SaveAndClose и т.д.
    
    public partial class CustomDetailViewController : DetailViewController {

        //DetailViewController detailViewController;

        public CustomDetailViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            /*
            detailViewController = Frame.GetController<DetailViewController>();
            if (detailViewController != null) {
                detailViewController.SaveAction += new SimpleActionExecuteEventHandler(DetailViewController_Save);
                detailViewController.SaveAndNewAction += new EventHandler<SingleChoiceActionExecuteEventArgs>(DetailViewController_SaveAndNew);
                detailViewController.SaveAndCloseAction += new EventHandler<SimpleActionExecuteEventArgs>(DetailViewController_SaveAndClose);
            }
            */
        }

        /*
        protected override void OnDeactivated() {
            if (detailViewController != null) {
                detailViewController.SaveAction -= new SimpleActionExecuteEventHandler(DetailViewController_Save);
                detailViewController.SaveAndNewAction -= new EventHandler<SingleChoiceActionExecuteEventArgs>(DetailViewController_SaveAndNew);
                detailViewController.SaveAndCloseAction -= new EventHandler<SimpleActionExecuteEventArgs>(DetailViewController_SaveAndClose);
            }
            base.OnDeactivated();
        }

        public void DetailViewController_Save(object sender, SimpleActionExecuteEventArgs e) {

        }
        */

    
        protected override void SaveAndNew(SingleChoiceActionExecuteEventArgs args) {
            base.SaveAndNew(args);
        }

        protected override void SaveAndClose(SimpleActionExecuteEventArgs args) {
            base.SaveAndClose(args);
        }

        protected override void Save(SimpleActionExecuteEventArgs args) {
            base.Save(args);
        }

    }
}
