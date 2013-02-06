using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NUnit.Framework;

using DevExpress.ExpressApp;

namespace IntecoAG.ERM.FM.Tests {

    [TestFixture]
    public class BaseTest {
//        protected IObjectSpace objectSpace;
//        PostponeController controller;
        protected const String DirTestName = "IntecoAG.ERM.FM.Tests.TestData.";
        protected TestApplication Application;
        protected ModuleBase Module;

        protected Stream GetTestDataStream(String name) {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //                String [] names = .GetManifestResourceNames();
            //                System.Reflection.Module [] modules = assembly.GetModules();
            //                String[] names = assembly.GetManifestResourceNames();
            //                FileStream[] files = assembly.GetFiles(true);
            //                String name = assembly.GetName() + "." + "TestData.fmSAImportTest01.txt";
            return assembly.GetManifestResourceStream(DirTestName + name);
        }

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp() {
            ObjectSpaceProvider objectSpaceProvider =
                new ObjectSpaceProvider(new MemoryDataStoreProvider());
            Application = new TestApplication();
            Module = new ModuleBase();
            Application.Modules.Add(Module);
            SetupModuleTypes();
            Application.Setup("TestApplication", objectSpaceProvider);
        }
        
        protected virtual void SetupModuleTypes() { 
        }


        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown() {
        }

        [SetUp]
        public virtual void SetUp() {
//            testModule.AdditionalExportedTypes.Add(typeof(Task));
//            application.Modules[0].AdditionalExportedTypes.Add(typeof(Task));
            //objectSpace = objectSpaceProvider.CreateObjectSpace();
//            controller = new PostponeController();
        }

        [TearDown]
        public virtual void TearDown() { 
        }

    }
}
