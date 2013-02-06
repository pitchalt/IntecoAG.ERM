using System;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo.DB;

namespace IntecoAG.ERM.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
#if EASYTEST
			DevExpress.ExpressApp.EasyTest.WinAdapter.RemotingRegistration.Register(4100);
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            ERMWindowsFormsApplication winApplication = new ERMWindowsFormsApplication();
            //ObjectAccessComparer.SetCurrentComparer(new IntecoAG.ERM.Module.ConditionalObjectAccessComparer());
            ObjectAccessComparer.SetCurrentComparer(new IntecoAG.ERM.Security.ConditionalObjectAccessComparer());
            ((SecurityComplex)winApplication.Security).IsGrantedForNonExistentPermission = true;
#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
//            System.Console.WriteLine(DevExpress.Xpo.DB.PostgreSqlConnectionProvider.GetConnectionString("localhost", "iag_usr", "qwerty", "intecoag_erm"));
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            try {
                //winApplication.ShowViewStrategy = new MdiShowViewStrategy(winApplication); // ƒл€ MDI
                // Ёто дл€ более быстрого открыти€ DetailView 
                winApplication.DelayedViewItemsInitialization = true;

                // язык
                //string defaultCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName; //CultureInfo.InvariantCulture.TwoLetterISOLanguageName;
                //string defaultFormattingCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                //winApplication.SetLanguage(defaultCulture);
                //winApplication.SetFormattingCulture(defaultFormattingCulture);


                //winApplication.CreateCustomTemplate += winApplication_CreateCustomTemplate;

                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e) {
                winApplication.HandleException(e);
            }

        }

        static void winApplication_CreateCustomTemplate(object sender, CreateCustomTemplateEventArgs e) {
            if (e.Context == TemplateContext.ApplicationWindow)
                e.Template = new DevExpress.ExpressApp.Win.CustomTemplates.MainForm();
        }

    }
}
