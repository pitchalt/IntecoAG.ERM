using System;
using DevExpress.ExpressApp.EasyTest.WinAdapter;
using System.IO;
using DevExpress.EasyTest.Framework;

namespace IntecoAG.ERM.CRM.Contract.Utils {
    public abstract class WinEasyTestFixtureHelperBase : IEasyTestFixtureHelper {
        private string applicationDirectoryName;
        private string applicationName;
        private TestApplication application;
        private WinAdapter applicationAdapter;
        protected TestCommandAdapter commandAdapter;
        protected ICommandAdapter adapter;
        public WinEasyTestFixtureHelperBase(string applicationDirectoryName, string applicationName) {
            this.applicationDirectoryName = applicationDirectoryName;
            this.applicationName = applicationName;
        }
        public void SetupFixture() {
            application = new TestApplication("", Path.GetFullPath(Path.Combine(@"..\..\..\" + applicationDirectoryName, @"Bin\EasyTest\" + applicationName)), "", null);
            application.AddParam("CommunicationPort", "4100");
        }
        public void SetUp() {
            applicationAdapter = new WinAdapter();
            applicationAdapter.RunApplication(application);
            adapter = ((IApplicationAdapter)applicationAdapter).CreateCommandAdapter();
            commandAdapter = new TestCommandAdapter(adapter);
        }
        public void TearDown() {
            applicationAdapter.KillApplication(application, KillApplicationConext.TestAborted);
        }
        public void TearDownFixture() {
        }
        public TestCommandAdapter CommandAdapter {
            get {
                return commandAdapter;
            }
        }
        public ICommandAdapter Adapter {
            get {
                return adapter;
            }
        }
    }
}
