using System;
using DevExpress.ExpressApp.EasyTest.WebAdapter;

namespace IntecoAG.ERM.CRM.Contract.Utils {
    public class TestWebAdapter : WebAdapter {
        protected override void RestartIIS() { }
        public void TestRestartIIS() {
            base.RestartIIS();
        }
    }
}
