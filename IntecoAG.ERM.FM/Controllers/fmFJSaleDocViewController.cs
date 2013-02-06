using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numeric;
using System.Text;
using System.Windows.Forms;

using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using FileHelpers;
using FileHelpers.DataLink;

using IntecoAG.ERM.FM.FinJurnal;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.FM.Controllers {
    
    public partial class fmFJSaleDocViewController : ViewController {
        public fmFJSaleDocViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        [DelimitedRecord("|")]
        public class fmSaleRecord {
            //const DateTime DefaultDateValue = new DateTime();

            public String OperCode;
            public String AVTInvoiceNumber;
            public DateTime AVTInvoiceDate;
            public String DocBaseNumber;
            //[FieldNullValue(0)]
            public DateTime DocBaseDate;
            public String PartyCode;
            public String PartyName;
            public String ContractNumber;
            //[FieldNullValue(0)]
            public DateTime ContractDate;
            public String OrderCode;
            public Decimal SummCost;
            public Decimal SummAVT;
            public Decimal SummAll;
            public String PayNumber;
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCFJSaleDoc obj = e.CurrentObject as fmCFJSaleDoc;
            if (obj == null) return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
//                obj.Name = dialog.FileName;
                ExcelStorage provider = new ExcelStorage(typeof(fmSaleRecord));
                provider.ErrorManager.ErrorMode = ErrorMode.ThrowException;
                provider.StartRow = 3;
                provider.StartColumn = 1;
                provider.FileName = dialog.FileName;
                fmSaleRecord[] res = (fmSaleRecord[])provider.ExtractRecords();
                using (IObjectSpace os = this.ObjectSpace.CreateNestedObjectSpace()) {
                    foreach (fmSaleRecord sr in res) {
                        fmCFJSaleDocLine oper = os.CreateObject<fmCFJSaleDocLine>();
                        oper.SaleDoc = os.GetObject<fmCFJSaleDoc>(obj);
                        oper.SaleOperation = os.FindObject<fmCFJSaleOperation>(new BinaryOperator("Code", sr.OperCode));
                        oper.AVTInvoiceNumber = sr.AVTInvoiceNumber;
                        oper.AVTInvoiceDate = sr.AVTInvoiceDate;
                        oper.DocBaseNumber = sr.DocBaseNumber;
                        //if (sr.DocBaseDate != null)
                        oper.DocBaseDate = sr.DocBaseDate;
                        oper.PartyCode = sr.PartyCode;
                        if (sr.ContractNumber == null) sr.ContractNumber = "";
                        if (sr.ContractNumber.Trim().ToUpper() == "Ã/Ï")
                            oper.DealNumber = "Ã/Ï";
                        else
                            oper.DealNumber = sr.ContractNumber;

                        //if (sr.ContractDate != null)
                        oper.DealDate = sr.ContractDate;
                        oper.OrderNumber = sr.OrderCode;
                        oper.SummAVT = Decimal.Round(sr.SummAVT, 2);
                        oper.SummCost = Decimal.Round(sr.SummAll, 2) - oper.SummAVT;
//                        oper.Valuta = os.FindObject<csValuta>(new BinaryOperator("Code", sr.Valuta));
//                        oper.SummValuta = sr.SummValuta;
                        oper.PayNumber = sr.PayNumber;
                    }
                    os.CommitChanges();
                }

            }
        }

        private void ApproveAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCFJSaleDoc doc = e.CurrentObject as fmCFJSaleDoc;
            if (doc == null) return;
            ObjectSpace.CommitChanges();
            foreach (fmCFJSaleDocLine line in doc.DocLines) {
                using (IObjectSpace os = this.ObjectSpace.CreateNestedObjectSpace()) {
                    fmCFJSaleDocLine curline = os.GetObject<fmCFJSaleDocLine>(line);
                    curline.Approve();
                    os.CommitChanges();
                }
                ObjectSpace.CommitChanges();
            }

        }
    }
}
