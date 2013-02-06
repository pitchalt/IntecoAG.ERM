using System;
using System.Collections.Generic;
using System.Text;

using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalEditorState.Win;

using DevExpress.XtraLayout;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Drawing;

using System.ComponentModel;
using System.Diagnostics;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.DC;

using DevExpress.Persistent.Validation;


namespace IntecoAG.ERM.Module
{
    // СУПЕР УНИВЕРСАЛЬНЫЙ КОНТРОЛЛЕР. ПОЗВОЛЯЕТ МЕНЯТЬ ЛЮБЫЕ ПАРАМЕТРЫ ЛЮБЫХ СВОЙСТВ В ЗАВИСИМОСТИ ОТ ОБСТОЯТЕЛЬСТВ
    // http://documentation.devexpress.com/#Xaf/CustomDocument3203

    public class EditorStateCustomizationController_OLD : ViewController<DetailView>
    {
        private EditorStateCustomizationDetailViewController detailViewCustomization;

        protected override void OnActivated() {
            base.OnActivated();

            //this.TargetViewType = ViewType.DetailView;

            detailViewCustomization = Frame.GetController<EditorStateCustomizationDetailViewController>();
            if (detailViewCustomization != null) {
                detailViewCustomization.CustomEditorStateCustomization += new EventHandler<CustomEditorStateCustomizationEventArgs>(detailViewCustomization_CustomEditorStateCustomization);
            }
        }


        private void detailViewCustomization_CustomEditorStateCustomization(object sender, CustomEditorStateCustomizationEventArgs e) {
            e.Handled = true;
            if (e.EditorState != EditorState.Disabled & e.EditorState != EditorState.Hidden) {

                DetailView detailView = (DetailView)View;
                foreach (PropertyEditor editor in detailView.GetItems<PropertyEditor>()) {
                    Attribute attr = editor.MemberInfo.FindAttribute<RuleRequiredFieldAttribute>();
                    if (attr != null) {

                        // Исследование структуры редакторов элементов
                        DXPropertyEditor dxEditor = editor as DXPropertyEditor;
                        //object dvi = editor.Control;
                        if (dxEditor != null && dxEditor.Control != null) {
                            //Control.ControlCollection contrcol = dxEditor.Control.Controls;
                            dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(215, 255, 255);
                        }

                        if (dxEditor != null) {
                            bool disabled = e.Active;
                            if (disabled) {
                                dxEditor.Control.Properties.BorderStyle = BorderStyles.Default;
                                dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(245, 255, 255);
                            }
                        }
                    }
                }

            }
        }

        private void detailViewCustomization_CustomEditorStateCustomization111(object sender, CustomEditorStateCustomizationEventArgs e) {
            if (e.EditorState == EditorState.Disabled) {
                DXPropertyEditor dxEditor = e.Item as DXPropertyEditor;
                if (dxEditor != null) {
                    bool disabled = e.Active;
                    if (disabled) {
                        dxEditor.Control.Properties.BorderStyle = BorderStyles.Default;
                        dxEditor.Control.Properties.Appearance.BackColor = Color.FromArgb(245, 245, 245);
                        // Очистка содержимого поля
                        if (dxEditor.AllowEdit) {
                            try {
                                dxEditor.PropertyValue = null;
                            }
                            catch (IntermediateMemberIsNullException) {
                                dxEditor.Refresh();
                            }
                        }
                    }
                    else {
                        dxEditor.Control.Properties.BorderStyle = BorderStyles.Default;
                        dxEditor.Control.Properties.Appearance.BackColor = Color.Empty;
                    }
                    TextEdit txtBox = dxEditor.Control as TextEdit;
                    if (txtBox != null)
                        txtBox.TabStop = !disabled;
                    else
                        dxEditor.Control.TabStop = !disabled;
                }
            }
        }

        protected override void OnDeactivated() {
            if (detailViewCustomization != null) {
                detailViewCustomization.CustomEditorStateCustomization -= new EventHandler<CustomEditorStateCustomizationEventArgs>(detailViewCustomization_CustomEditorStateCustomization);  //detailViewCustomization_CustomEditorStateCustomization;
            }
            base.OnDeactivated();
        }
    }

}
