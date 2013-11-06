using System;
using System.Collections.Generic;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

using DevExpress.ExpressApp;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.CS.Common {

    public enum LongOperationStatus {
        Creating = 0,
        Created = 1,
        Processing = 2,
        Cancelling = 4,
        Terminating = 8,
        Error = 16,
        Completed = 32,
        ProcessingError = Processing | Error,
        CancellingError = Cancelling | Error,
        TerminatingError = Terminating | Error,
        Processed = Processing | Completed,
        Cancelled = Cancelling | Completed,
        Terminated = Terminating | Completed
    }

    public interface ILongOperation: IDisposable {
        
        LongOperationStatus Status { get; }

        Int32 StageCurrent  { get; }
        Int32 StageMax      { get; }
        Int32 StepCurrent   { get; }
        Int32 StepMax       { get; }

        void Process();
        void Cancell();
        void Terminate();

    }

//    public interface ILongOperationFactory<T> where T: ILongOperation {
//    }

    public interface ILongOperationManager {
        T Create<T>() where T : ILongOperation;
    }

}
