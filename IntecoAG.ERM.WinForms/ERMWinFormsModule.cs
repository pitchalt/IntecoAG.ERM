using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Templates;

namespace IntecoAG.ERM.WinForms
{
    public sealed partial class ERMWinFormsModule : ModuleBase
    {
        public ERMWinFormsModule()
        {
            InitializeComponent();
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            application.CreateCustomTemplate += winApplication_CreateCustomTemplate;
        }

        static void winApplication_CreateCustomTemplate(object sender, CreateCustomTemplateEventArgs e)
        {
            if (e.Context == TemplateContext.ApplicationWindow)
            {
                e.Template = new IagErmMainForm();
            }
            if (e.Context == TemplateContext.View)
            {
                e.Template = new IagErmDetailForm();
            }
        }
    }
}
