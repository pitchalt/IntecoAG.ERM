using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.Imports {
    class Program {
        static void Main(string[] args) {
            ImportParty import = new ImportParty();
            import.import(args[0]);
        }
    }
}
