using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace IntecoAG.ERM.CS {
    public interface csIImportSupport {
        void Import(TextReader reader);
    }
}
