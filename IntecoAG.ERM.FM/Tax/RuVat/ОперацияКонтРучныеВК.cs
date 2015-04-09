using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
//
using FileHelpers;
using FileHelpers.DataLink;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    public partial class ОперацияКонтРучныеВК : ViewController {
        public ОперацияКонтРучныеВК() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                Import(os, dialog.FileName);
                os.CommitChanges();
            }
        }
        [FixedLengthRecord]
        public class OperationImport {
            [FieldFixedLength(2)]
            public String BOOK;
            [FieldFixedLength(3)]
            public String OPERATION;
            [FieldFixedLength(3)]
            public String VAT_MODE;
            [FieldFixedLength(2)]
            public String VAT_RATE;
            [FieldFixedLength(6)]
            public String BUH_PROV;
            [FieldFixedLength(9)]
            public String BUH_DT;  
            [FieldFixedLength(9)]
            public String VAT_DT;  
            [FieldFixedLength(6)]
            public String VO_CODE; 
            [FieldFixedLength(20)]
            public String SF_REGNUM; 
            [FieldFixedLength(3)]
            public String SF_TYPE;   
            [FieldFixedLength(3)]
            public String SF_TYPE_ORIG;
            [FieldFixedLength(20)]
            public String SF_NUMBER;  
            [FieldFixedLength(8)]
            public String SF_DATE;    
            [FieldFixedLength(3)]
            public String PD_TYPE;    
            [FieldFixedLength(20)]
            public String PD_NUMBER;  
            [FieldFixedLength(8)]
            public String PD_DATE;    
            [FieldFixedLength(17)]
            public String SUMM_ALL;   
            [FieldFixedLength(17)]
            public String SUMM_COST;  
            [FieldFixedLength(17)]
            public String SUMM_VAT_PAY; 
            [FieldFixedLength(17)]
            public String SUMM_VAT_IN;
            [FieldFixedLength(17)]
            public String SUMM_VAT_COST;
            [FieldFixedLength(17)]
            public String SUMM_VAT_BAY;
        }

        private void Import(IObjectSpace os, String file_name) {
            FixedFileEngine engine = new FixedFileEngine(typeof(OperationImport));
            OperationImport[] imp_res = (OperationImport[])engine.ReadFile(file_name);
            foreach (OperationImport oper_import in imp_res) {
                System.Console.WriteLine(oper_import.SUMM_ALL + "_" + oper_import.SUMM_VAT_PAY + "_" + oper_import.SUMM_VAT_BAY);
            }
        }

    }
}
