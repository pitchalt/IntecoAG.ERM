using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Globalization;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;


// http://documentation.devexpress.com/#Xaf/DevExpressExpressAppXafApplication_SetLanguagetopic

namespace IntecoAG.ERM.Module {
    public partial class ChangeLanguageController : WindowController {

        //private string defaultCulture;
        //private string defaultFormattingCulture;

        public ChangeLanguageController() {
            InitializeComponent();
            RegisterActions(components);
        }

//        private void ChangeLanguageController_Activated(object sender, EventArgs e) {
        protected override void OnActivated() {
            base.OnActivated();
            //GetDefaultCulture();      
            
            //ChooseLanguage.Items.Add(new ChoiceActionItem(string.Format("Default ({0})", defaultCulture), defaultCulture));
            //ChooseLanguage.Items.Add(new ChoiceActionItem("German (de)", "de"));
            ChooseLanguage.Items.Add(new ChoiceActionItem("Russian (ru-RU)", "ru-RU"));
            ChooseLanguage.Items.Add(new ChoiceActionItem("English (en-us)", "en-us"));

            //ChooseFormattingCulture.Items.Add(new ChoiceActionItem(string.Format("Default ({0})", defaultFormattingCulture), defaultFormattingCulture));
            //ChooseFormattingCulture.Items.Add(new ChoiceActionItem("German (de)", "de"));
            ChooseFormattingCulture.Items.Add(new ChoiceActionItem("Russian (ru-RU)", "ru-RU"));
            ChooseFormattingCulture.Items.Add(new ChoiceActionItem("English (en-us)", "en-us"));

            ChooseFormattingCulture.Active["ChooseFormattingCulture_Enabled"] = false;
        }
        
        //private void GetDefaultCulture() {
        //    defaultCulture = CultureInfo.InvariantCulture.TwoLetterISOLanguageName;
        //    defaultFormattingCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        //}
        
        private void ChooseLanguage_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            Application.SetLanguage(e.SelectedChoiceActionItem.Data as string);

            // Считаем, что форматирование не меняется (чтобы тесты работали в формате выбранной локализации)
            //Application.SetFormattingCulture(e.SelectedChoiceActionItem.Data as string);

            DevExpress.ExpressApp.Win.WinShowViewStrategyBase showViewStrategy = ((DevExpress.ExpressApp.Win.WinApplication)Application).ShowViewStrategy;
            if (showViewStrategy.CloseAllWindows()) {
                showViewStrategy.ShowStartupWindow();
            }
        }
        
        private void ChooseFormattingCulture_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            Application.SetFormattingCulture(e.SelectedChoiceActionItem.Data as string);

            //DevExpress.ExpressApp.Win.WinShowViewStrategyBase showViewStrategy = ((DevExpress.ExpressApp.Win.WinApplication)Application).ShowViewStrategy;
            //if (showViewStrategy.CloseAllWindows()) {
            //    showViewStrategy.ShowStartupWindow();
            //}
        }

    
    }
}
