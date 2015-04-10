using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.Tax.RuVat {

    public partial class ДокИмпортОснованийВК : ViewController {
        public ДокИмпортОснованийВК() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ДокИмпортОснований док = View.CurrentObject as ДокИмпортОснований;
            if (док == null)
                return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                using (StreamReader reader = new StreamReader( dialog.FileName, Encoding.GetEncoding(1251))) {
                    док = os.GetObject<ДокИмпортОснований>(док);
                    ДокИмпортОснований.ImportInvoices(os,док,reader);
                    reader.Close();
                }
                os.CommitChanges();
            }

        }
    }
}
