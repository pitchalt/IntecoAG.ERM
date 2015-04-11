using System;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Principal;
//
using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.AuditTrail;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
//
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.AuditTrail;
//using DevExpress.Xpo.DB;
//using DevExpress.Data.Filtering;

namespace IntecoAG.ERM.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if EASYTEST
			DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            using (ERMWinFormsApplication winApplication = new ERMWinFormsApplication()
            {
                /* Это для более быстрого открытия DetailView */
                DelayedViewItemsInitialization = true
            })
            {
                AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.Lightweight;
                AuditTrailService.Instance.QueryCurrentUserName += AuditTrailService_QueryCurrentUserName;
                AuditTrailService.Instance.CustomizeAuditTrailSettings += AuditTrailService_CustomizeAuditTrailSettings;
                SecuritySystem.AllowReloadPermissions = true;
                
                //ObjectAccessComparer.SetCurrentComparer(new IntecoAG.ERM.Module.ConditionalObjectAccessComparer());
                //ObjectAccessComparer.SetCurrentComparer(new IntecoAG.ERM.Security.ConditionalObjectAccessComparer());
                //((SecurityComplex)winApplication.Security).IsGrantedForNonExistentPermission = true;
                //winApplication.ShowViewStrategy = new MdiShowViewStrategy(winApplication); // Для MDI
                // Язык
                //string defaultCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName; //CultureInfo.InvariantCulture.TwoLetterISOLanguageName;
                //string defaultFormattingCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                //winApplication.SetLanguage(defaultCulture);
                //winApplication.SetFormattingCulture(defaultFormattingCulture);
                //winApplication.CreateCustomTemplate += winApplication_CreateCustomTemplate;

#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
                //            System.Console.WriteLine(DevExpress.Xpo.DB.PostgreSqlConnectionProvider.GetConnectionString("localhost", "iag_usr", "qwerty", "intecoag_erm"));
                if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                {
                    winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }
                try
                {
                    winApplication.Setup();
                    winApplication.Start();
                }
                catch (Exception e)
                {
                    winApplication.HandleException(e);
                }
            }

        }

        //static void winApplication_CreateCustomTemplate(object sender, CreateCustomTemplateEventArgs e)
        //{
        //    if (e.Context == TemplateContext.ApplicationWindow)
        //    {
        //        e.Template = new IntecoAG.ERM.Win.Templates.MainForm();
        //    }
        //    if (e.Context == TemplateContext.View)
        //    {
        //        e.Template = new IntecoAG.ERM.Module.DetailViewForm1();
        //    }
        //}

        static void AuditTrailService_CustomizeAuditTrailSettings(object sender, CustomizeAuditTrailSettingsEventArgs e)
        {
            e.AuditTrailSettings.RemoveType(typeof(FM.AVT.fmCAVTBookBuhRecord));
            e.AuditTrailSettings.RemoveType(typeof(FM.FinPlan.FmFinPlanDocLine));
            e.AuditTrailSettings.RemoveType(typeof(FM.FinPlan.FmFinPlanDocTime));
            e.AuditTrailSettings.RemoveType(typeof(FM.Tax.RuVat.Основание));
            e.AuditTrailSettings.RemoveType(typeof(FM.Tax.RuVat.ОснованиеДокумент));
            e.AuditTrailSettings.RemoveType(typeof(FM.Tax.RuVat.ДокИмпортОснований.СтрокаОснов));
            e.AuditTrailSettings.RemoveType(typeof(FM.Tax.RuVat.КнигаСтрока));
            e.AuditTrailSettings.RemoveType(typeof(FM.Tax.RuVat.КнигаДокументСтрока));
            e.AuditTrailSettings.RemoveType(typeof(FM.Tax.RuVat.Операция));
            //e.AuditTrailSettings.AddType(typeof(FM.Order.fmCOrderExt), "Status");
            //e.AuditTrailSettings.AddType(typeof(FM.Order.fmCOrderManageDoc), "Status");
        }

        static void AuditTrailService_QueryCurrentUserName(object sender, QueryCurrentUserNameEventArgs e)
        {
            e.CurrentUserName = WindowsIdentity.GetCurrent().Name;
        }
    }
}
