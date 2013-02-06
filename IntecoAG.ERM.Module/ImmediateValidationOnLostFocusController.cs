using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Validation;

using DevExpress.Xpo;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

using System.Windows.Forms;

using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalEditorState.Win;
using DevExpress.XtraLayout;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Drawing;

using DevExpress.ExpressApp.DC;


namespace IntecoAG.ERM.Module {


    public class ImmediateValidationTargetObjectsSelector : ValidationTargetObjectSelector {
        protected override bool NeedToValidateObject(Session session, object targetObject) {
            return true;
        }
    }

    
    public partial class ImmediateValidationOnLostFocusController : ViewController {

        public ImmediateValidationOnLostFocusController() {
            InitializeComponent();
            RegisterActions(components);

            this.TargetViewType = ViewType.DetailView;
        }

        protected override void OnActivated() {
            base.OnActivated();

            /*
            // Проставновка звёздочек у надписей полей на форме. К сожалению, в пользовательскую модель записываются эти новые заголовки со звёздочками
            View.ControlsCreating += new EventHandler(View_ControlsCreating);
            */
            View.ControlsCreated += new EventHandler(View_ControlsCreated);
            //ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);

            // Простановка иконок около полей в первоначальный момент загрузки
            // ValidateViewObjects();
        }


        protected override void OnDeactivated() {
            //View.ControlsCreated -= new EventHandler(View_ControlsCreated);
            DetailView detailView = (DetailView)View;
            foreach (PropertyEditor editor in detailView.GetItems<PropertyEditor>()) {
                Attribute attr = editor.MemberInfo.FindAttribute<RuleRequiredFieldAttribute>();
                if (attr != null) {
                    // Перехват LostFocus (отписка от)
                    //((System.Windows.Forms.Control)editor.Control).LostFocus -= new EventHandler(FormField_LostFocus);
                    //((System.Windows.Forms.Control)editor.Control).TextChanged -= new EventHandler(FormField_TextChanged);

                }
            }

            base.OnDeactivated();
        }

        void View_ControlsCreating(object sender, EventArgs e) {
            DetailView detailView = (DetailView)View;
            foreach (PropertyEditor editor in detailView.GetItems<PropertyEditor>()) {
                Attribute attr = editor.MemberInfo.FindAttribute<RuleRequiredFieldAttribute>();
                if (attr != null) {
                    HighLightProperty(editor as DXPropertyEditor, true);
                } else {
                    HighLightProperty(editor as DXPropertyEditor, false);
                }
            }
        }

        void View_ControlsCreated(object sender, EventArgs e) {
            DetailView detailView = (DetailView)View;
            foreach (PropertyEditor editor in detailView.GetItems<PropertyEditor>()) {

                if (!editor.AllowEdit) continue;

                // Исследование структуры редакторов элементов
                /*
                DXPropertyEditor dxEditor = editor as DXPropertyEditor;
                //object dvi = editor.Control;
                if (dxEditor != null && dxEditor.Control != null) {
                    //Control.ControlCollection contrcol = dxEditor.Control.Controls;
                    dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(215, 255, 255);
                }
                */


                Attribute attr = editor.MemberInfo.FindAttribute<RuleRequiredFieldAttribute>();
                if (attr != null) {
                    // Перехват LostFocus - отменён 2011-08-01
                    //((System.Windows.Forms.Control)editor.Control).LostFocus += new EventHandler(FormField_LostFocus);
                    //((System.Windows.Forms.Control)editor.Control).TextChanged += new EventHandler(FormField_TextChanged);

                    // Раскраска обязательных полей на форме (в пользовательской модели не запоминается)
                    DXPropertyEditor dxEditor = editor as DXPropertyEditor;
                    if (dxEditor != null && dxEditor.Control != null) {

                        dxEditor.Control.Properties.BorderStyle = BorderStyles.Office2003;

                        // SHU!!! 2011-09-07 Три строки, которые управляют украшениями 
                        /*
                        dxEditor.Control.ForeColor = Color.FromArgb(0, 100, 0);
                        dxEditor.Control.Properties.BorderStyle = BorderStyles.Office2003;
                        dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(255, 235, 245);
                        */

                        /*
                        // Добавляем атрибут для выделения заголовка поля - НЕ РАБОТАЕТ ПО НЕИЗВЕСТНЫМ ПРИЧИНАМ
                        // http://documentation.devexpress.com/#Xaf/CustomDocument3286
                        // http://www.devexpress.com/Support/Center/p/Q298805.aspx
                        //XafTypesInfo.Instance.FindTypeInfo(typeof(PersistentObject1)).AddAttribute(new DevExpress.Persistent.Base.DefaultClassOptionsAttribute());
                        //[Appearance("DisplaySummMode.Caption.Bold", AppearanceItemType.LayoutItem, "true", FontStyle = FontStyle.Bold)]
                        string AttrId = dxEditor.Id + ".Caption.Bold";
                        AppearanceAttribute AppearAttr = new AppearanceAttribute(AttrId);
                        AppearAttr.AppearanceItemType = AppearanceItemType.LayoutItem.ToString();
                        AppearAttr.BackColor = "Red";
                        AppearAttr.FontColor = "Blue";
                        AppearAttr.FontStyle = FontStyle.Bold;
                        AppearAttr.Criteria = "isnull(" + dxEditor.Id + ") OR not isnull(" + dxEditor.Id + ")";
                        AppearAttr.TargetItems = dxEditor.Id;
                        dxEditor.MemberInfo.AddAttribute(AppearAttr);
                        dxEditor.Refresh();
                        //editor.MemberInfo.AddAttribute(AppearAttr);
                        //editor.Refresh();
                        */
                    }

                }
            }
        }

        void FormField_LostFocus(object sender, EventArgs e) {
            ValidateViewObjects();
            //RecolorFields();
        }

        void FormField_TextChanged(object sender, EventArgs e) {
            RecolorFields();
        }

        void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e) {
            ValidateViewObjects();
        }


        private void RecolorFields() {
            DetailView detailView = (DetailView)View;
            foreach (PropertyEditor editor in detailView.GetItems<PropertyEditor>()) {

                //if (!editor.AllowEdit) continue;

                // Исследование структуры редакторов элементов
                /*
                DXPropertyEditor dxEditor = editor as DXPropertyEditor;
                //object dvi = editor.Control;
                if (dxEditor != null && dxEditor.Control != null) {
                    //Control.ControlCollection contrcol = dxEditor.Control.Controls;
                    dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(215, 255, 255);
                }
                */


                Attribute attr = editor.MemberInfo.FindAttribute<RuleRequiredFieldAttribute>();
                if (attr != null) {
                    // Перехват LostFocus - отменён 2011-08-01
                    //((System.Windows.Forms.Control)editor.Control).LostFocus += new EventHandler(FormField_LostFocus);

                    // Раскраска обязательных полей на форме (в пользовательской модели не запоминается)
                    DXPropertyEditor dxEditor = editor as DXPropertyEditor;
                    if (dxEditor.Control != null) {

                        if (!editor.AllowEdit) {
                            // SHU!!! 2011-09-07 Три строки, которые управляют украшениями 
                            dxEditor.Control.ForeColor = Color.FromArgb(0, 100, 0);
                            dxEditor.Control.Properties.BorderStyle = BorderStyles.Default;
                            dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(245, 245, 245);
                        } else {
                            // SHU!!! 2011-09-07 Три строки, которые управляют украшениями 
                            dxEditor.Control.ForeColor = Color.FromArgb(0, 0, 0);
                            dxEditor.Control.Properties.BorderStyle = BorderStyles.Office2003;
                            dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(255, 235, 245);
                        }

                        /*
                        // Добавляем атрибут для выделения заголовка поля - НЕ РАБОТАЕТ ПО НЕИЗВЕСТНЫМ ПРИЧИНАМ
                        // http://documentation.devexpress.com/#Xaf/CustomDocument3286
                        // http://www.devexpress.com/Support/Center/p/Q298805.aspx
                        //XafTypesInfo.Instance.FindTypeInfo(typeof(PersistentObject1)).AddAttribute(new DevExpress.Persistent.Base.DefaultClassOptionsAttribute());
                        //[Appearance("DisplaySummMode.Caption.Bold", AppearanceItemType.LayoutItem, "true", FontStyle = FontStyle.Bold)]
                        string AttrId = dxEditor.Id + ".Caption.Bold";
                        AppearanceAttribute AppearAttr = new AppearanceAttribute(AttrId);
                        AppearAttr.AppearanceItemType = AppearanceItemType.LayoutItem.ToString();
                        AppearAttr.BackColor = "Red";
                        AppearAttr.FontColor = "Blue";
                        AppearAttr.FontStyle = FontStyle.Bold;
                        AppearAttr.Criteria = "isnull(" + dxEditor.Id + ") OR not isnull(" + dxEditor.Id + ")";
                        AppearAttr.TargetItems = dxEditor.Id;
                        dxEditor.MemberInfo.AddAttribute(AppearAttr);
                        dxEditor.Refresh();
                        //editor.MemberInfo.AddAttribute(AppearAttr);
                        //editor.Refresh();
                        */
                    }

                }
            }
        }


        private void ValidateViewObjects() {
            if (View != null) {
                if (View is DevExpress.ExpressApp.ListView) {
                    if (Frame != null && Frame.IsViewControllersActivation) {
                        ValidateObjects(((DevExpress.ExpressApp.ListView)View).CollectionSource.List);
                    }
                } else {
                    ImmediateValidationTargetObjectsSelector objectsSelector = new ImmediateValidationTargetObjectsSelector();
                    ValidateObjects(objectsSelector.GetObjectsToValidate(((ObjectSpace)View.ObjectSpace).Session, View.CurrentObject));
                }
            }
        }

        private void ValidateObjects(IEnumerable targets) {

            //string validationContext = "Immediate";
            RuleSetValidationResult result = Validator.RuleSet.ValidateAllTargets(targets);   //, validationContext);

            List<ResultsHighlightController> resultsHighlightControllers = new List<ResultsHighlightController>();
            resultsHighlightControllers.Add(Frame.GetController<ResultsHighlightController>());
            if (View is DetailView) {
                foreach (ListPropertyEditor listPropertyEditor in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                    //if (listPropertyEditor.Frame.GetController<ResultsHighlightController>() == null) continue;
                    if (listPropertyEditor.Frame == null) continue;
                    ResultsHighlightController nestedController = listPropertyEditor.Frame.GetController<ResultsHighlightController>();
                    if (nestedController != null) {
                        resultsHighlightControllers.Add(nestedController);
                    }
                }
            }

            foreach (ResultsHighlightController resultsHighlightController in resultsHighlightControllers) {
                //resultsHighlightController.View.ObjectSpace.ModifiedObjects[0]
                resultsHighlightController.ClearHighlighting();
                if (result.State == ValidationState.Invalid) {
                    resultsHighlightController.HighlightResults(result);
                }
            }
        }

        private void HighLightProperty(DXPropertyEditor dxEditor, bool Astra) {
            //DXPropertyEditor dxEditor = e.Item as DXPropertyEditor;
            if (dxEditor != null) {

                //dxEditor.Control..Properties.BorderStyle = BorderStyles.Default;
                //dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(245, 245, 245);

                if (Astra) {
                    string caption = dxEditor.Caption;
                    while (caption.IndexOf(" * *") > -1) caption = caption.Replace(" * *", " *");
                    if (caption.IndexOf(" *") == -1) dxEditor.Caption = caption + " *";
                    //while (dxEditor.Caption.IndexOf(" * *") > -1) dxEditor.Caption.Replace(" * *", " *");
                } else {
                    string caption = dxEditor.Caption;
                    caption = caption.Replace(" *", "");
                    dxEditor.Caption = caption;
                }

                //dxEditor.Control.Properties.BorderStyle = BorderStyles.Default;
                //dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(245, 245, 245);


                // Очистка содержимого поля
                //if (dxEditor.AllowEdit) {
                //    try {
                //        dxEditor.PropertyValue = null;
                //    } catch (IntermediateMemberIsNullException) {
                //        dxEditor.Refresh();
                //    }
                //}
                //} else {
                //    dxEditor.Control.Properties.BorderStyle = BorderStyles.Default;
                //    dxEditor.Control.Properties.Appearance.BackColor = Color.Empty;
                //}
                //TextEdit txtBox = dxEditor.Control as TextEdit;
                //if (txtBox != null)
                //    txtBox.TabStop = !disabled;
                //else
                //    dxEditor.Control.TabStop = !disabled;
            }
        }



    }
}
