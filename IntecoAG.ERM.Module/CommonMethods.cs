using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;

using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.SystemModule;

namespace IntecoAG.ERM.Module {

    public static class CommonMethods {

        #region Методы загрузки View



        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        public static void ShowConcreteDetailViewInWindow(Frame frame, IObjectSpace objectSpace, string DetailViewID, object currentObject, TargetWindow tw) {
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, currentObject);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            svp.TargetWindow = tw;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }



/*
        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void SetConcreteListView<classType>(Frame frame) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //SimpleContract objCurrent1 = objectSpace1.GetObject<SimpleContract>((SimpleContract)currentObject);
            ListView lv = frame.Application.CreateListView(objectSpace, typeof(classType), true);
            frame.SetView(lv, frame);
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void SetConcreteDetailView<classType>(Frame frame, classType Obj, string DeatilViewId) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DeatilViewId, true, objCurrent);
            frame.SetView(dv, frame);
        }
*/

/*
        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        public static void ShowConcreteShortCutDetailView<classType>(Frame frame, View view, classType Obj, TargetWindow tw) {
            BaseObject obj = Obj as BaseObject;
            if (obj == null) return;

            ShowViewParameters svp = new ShowViewParameters();
            ViewShortcut shortcut = new ViewShortcut(frame.Application.FindDetailViewId(typeof(classType)), obj.Oid);
            svp.CreatedView = frame.Application.ProcessShortcut(shortcut);

            //Specify various display settings.
            svp.TargetWindow = tw;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));

            view.ObjectSpace.CommitChanges();
            view.Close();
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        public static void ShowConcreteDetailView<classType>(Frame frame, View view, string DetailViewID, classType Obj, TargetWindow tw) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = tw;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        public static void ShowConcreteDetailView<classType>(Frame frame, View view, string DetailViewID, classType Obj) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = TargetWindow.Current;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }

        /// <summary>
        /// Загрузка указанного параметром типа ListView
        /// </summary>
        /// <param name="DetailViewID"></param>
        public static void ShowConcreteListView<classType>(Frame frame, TargetWindow tw) {
            IObjectSpace objectSpaceList = frame.Application.CreateObjectSpace();
            ListView lv = frame.Application.CreateListView(objectSpaceList, typeof(classType), true);

            ShowViewParameters svpList = new ShowViewParameters();
            svpList.CreatedView = lv;

            //Specify various display settings.
            svpList.TargetWindow = tw;
            svpList.Context = TemplateContext.View;
            svpList.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svpList, new ShowViewSource(frame, null));
        }
*/
        #endregion

    }
}
