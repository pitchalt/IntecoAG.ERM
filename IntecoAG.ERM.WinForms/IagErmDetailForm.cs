using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Templates;
using DevExpress.ExpressApp.Win.Templates.ActionContainers;
using DevExpress.XtraBars;
using DevExpress.ExpressApp;

namespace DevExpress.ExpressApp.Win.Templates {
    public partial class IagErmDetailForm : DevExpress.ExpressApp.Win.Templates.XtraFormTemplateBase, ISupportClassicToRibbonTransform {
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            CheckTransformToRibbon();
        }
        protected override IModelFormState GetFormStateNode() {
            if (View != null) {
                return TemplatesHelper.GetFormStateNode(View.Id);
            } else {
                return base.GetFormStateNode();
            }
        }
        public override void SetSettings(IModelTemplate modelTemplate) {
            base.SetSettings(modelTemplate);
            formStateModelSynchronizerComponent.Model = GetFormStateNode();
        }
        [Obsolete("Use the DetailViewForm() constructor"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public IagErmDetailForm(XafApplication application) : this() {
        }
        public IagErmDetailForm() {
            InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new XafComponentResourceManager(typeof(IagErmDetailForm));
        }
        public Bar MainMenuBar {
            get {
                return _mainMenuBar;
            }
        }
        public Bar ToolBar {
            get {
                return standardToolBar;
            }
        }
        public Bar ClassicStatusBar {
            get {
                return _statusBar;
            }
        }
    }
}
