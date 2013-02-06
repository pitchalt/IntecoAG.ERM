using System;
using DevExpress.EasyTest.Framework;

namespace IntecoAG.ERM.CRM.Contract.Utils {
    public interface IEasyTestFixtureHelper {
        void SetupFixture();
        void SetUp();
        void TearDown();
        void TearDownFixture();
        TestCommandAdapter CommandAdapter { get; }
        ICommandAdapter Adapter { get; }
    }
}
