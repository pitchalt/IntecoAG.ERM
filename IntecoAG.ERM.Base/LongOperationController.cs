using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Demos;

namespace IntecoAG.ERM {
    public class LongOperationTerminateException : Exception { }

    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    //public class BatchCreationOptionsAttribute : Attribute {
    //    private int? objectsCount;
    //    private int? commitInterval;
    //    public BatchCreationOptionsAttribute(int objectsCount) {
    //        this.objectsCount = objectsCount;
    //    }
    //    public BatchCreationOptionsAttribute(int objectsCount, int commitInterval) : this(objectsCount) {
    //        this.commitInterval = commitInterval;
    //    }
    //    public int? ObjectsCount {
    //        get { return objectsCount; }
    //    }
    //    public int? CommitInterval {
    //        get { return commitInterval; }
    //    }
    //}

    //public interface IObjectPropertiesInitializer {
    //    void InitializeObject(int index);
    //}

    public interface IProgressControl : IDisposable {
        void ShowProgress(LongOperation longOperation);
    }

    public class LongOperationController : ViewController {
        private IProgressControl progressControl;
        //private AsyncOperation waitLongOperationCompleted;


        private void WorkCompleted(object state) {
            OnOperationCompleted();
        }
        private void LongOperation_CancellingTimeoutExpired(object sender, EventArgs e) {
            ((LongOperation)sender).TerminateAsync();
        }
        private void LongOperation_Completed(object sender, LongOperationCompletedEventArgs e) {
            progressControl.Dispose();
            progressControl = null;
            ((LongOperation)sender).CancellingTimeoutExpired -= new EventHandler(LongOperation_CancellingTimeoutExpired);
            ((LongOperation)sender).Completed -= new EventHandler<LongOperationCompletedEventArgs>(LongOperation_Completed);
            ((LongOperation)sender).Dispose();

            //waitLongOperationCompleted.PostOperationCompleted(WorkCompleted, null);
            //waitLongOperationCompleted = null;
        }

        //protected virtual void DoWorkCore(LongOperation longOperation) {}
        protected virtual IProgressControl CreateProgressControl() {return null; }
        protected virtual void OnOperationStarted() {
            if(OperationStarted != null) {
                OperationStarted(this, EventArgs.Empty);
            }
        }
        protected virtual void OnOperationCompleted() {
            View.ObjectSpace.Refresh();
            if(OperationCompleted != null) {
                OperationCompleted(this, EventArgs.Empty);
            }
        }
        protected virtual LongOperation CreateLongOperation() {
            return null;
        }
        protected void StartLongOperation(LongOperation longOperation) {
            //waitLongOperationCompleted = AsyncOperationManager.CreateOperation(null);
            longOperation.CancellingTimeoutMilliSeconds = 10000;
            longOperation.CancellingTimeoutExpired += new EventHandler(LongOperation_CancellingTimeoutExpired);
            longOperation.Completed += new EventHandler<LongOperationCompletedEventArgs>(LongOperation_Completed);

            progressControl = CreateProgressControl();
            progressControl.ShowProgress(longOperation);
            longOperation.StartAsync();
            OnOperationStarted();
        }
        public event EventHandler OperationStarted;
        public event EventHandler OperationCompleted;
    }
}
