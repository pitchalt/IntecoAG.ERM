using System;
using System.IO;
using DevExpress.EasyTest.Framework;
using System.Reflection;

namespace IntecoAG.ERM.CRM.Contract.Utils {
    public abstract class WebEasyTestFixtureHelperBase : IEasyTestFixtureHelper {
        private const string testWebApplicationRootUrl = "http://localhost:3057";
        protected TestWebAdapter webAdapter;
        protected TestCommandAdapter commandAdapter;
        protected ICommandAdapter adapter;
        protected TestApplication application;
        public WebEasyTestFixtureHelperBase(string relativePathToWebApplication, string absolutePathToWebApplication) {
            string testApplicationDir = Path.Combine(Assembly.GetExecutingAssembly().Location, relativePathToWebApplication);
            if (!Directory.Exists(testApplicationDir))
                testApplicationDir = absolutePathToWebApplication;
            application = new TestApplication(string.Empty, testApplicationDir, testWebApplicationRootUrl + GetUrlOptions(), string.Empty);
            application.AddParam("SingleWebDev", true.ToString());
        }
        protected virtual string GetUrlOptions() {
            return "/default.aspx";
        }
        public virtual void SetupFixture() {
            webAdapter = new TestWebAdapter();
            webAdapter.RunApplication(application);
        }
        public virtual void SetUp() {
            adapter = webAdapter.CreateCommandAdapter();
            commandAdapter = new TestCommandAdapter(adapter);
        }
        public virtual void TearDown() {
            while(webAdapter.WebBrowsers.Count > 1) {
                webAdapter.WebBrowsers[1].Browser.Quit();
            }
            string urlParams = GetUrlOptions();
            webAdapter.WebBrowsers[0].Navigate(testWebApplicationRootUrl + urlParams + (urlParams.Contains("?") ? "&" : "?") + "Reset=true");
        }
        public virtual void TearDownFixture() {
            webAdapter.WebBrowsers[0].Browser.Quit();
            webAdapter.KillApplication(application, KillApplicationConext.TestAborted);
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