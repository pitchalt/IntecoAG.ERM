using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    public partial class ОперацияКонтРучныеВК : ViewController {
        public ОперацияКонтРучныеВК() {
            InitializeComponent();
            RegisterActions(components);
        }
    }
}
