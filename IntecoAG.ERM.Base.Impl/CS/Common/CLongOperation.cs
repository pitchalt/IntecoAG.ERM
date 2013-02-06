using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.CS.Common {
    public abstract class CLongOperation : ILongOperation {

        public abstract void Init();

        public abstract void DoWork();

        public virtual void Dispose() { }



        public LongOperationStatus Status {
            get { throw new NotImplementedException(); }
        }

        public int StageCurrent {
            get { throw new NotImplementedException(); }
        }

        public int StageMax {
            get { throw new NotImplementedException(); }
        }

        public int StepCurrent {
            get { throw new NotImplementedException(); }
        }

        public int StepMax {
            get { throw new NotImplementedException(); }
        }

        public void Process() {
            throw new NotImplementedException();
        }

        public void Cancell() {
            throw new NotImplementedException();
        }

        public void Terminate() {
            throw new NotImplementedException();
        }
    }
}
