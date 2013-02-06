using System;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

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
#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            try {
                //winApplication.ShowViewStrategy = new MdiShowViewStrategy(winApplication); // ��� MDI
                // ��� ��� ����� �������� �������� DetailView 
                winApplication.DelayedViewItemsInitialization = false;

                // ����
                //string defaultCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName; //CultureInfo.InvariantCulture.TwoLetterISOLanguageName;
                //string defaultFormattingCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                //winApplication.SetLanguage(defaultCulture);
                //winApplication.SetFormattingCulture(defaultFormattingCulture);

                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e) {
                winApplication.HandleException(e);
            }
        }
    }
}
