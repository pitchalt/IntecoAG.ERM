using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using DevExpress.ExpressApp;

namespace IntecoAG.ERM.CS {
    public interface csIImportSupport {
        void Import(IObjectSpace os, String file_name);
    }
}
