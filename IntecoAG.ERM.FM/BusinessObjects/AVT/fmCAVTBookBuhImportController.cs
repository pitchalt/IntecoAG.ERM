using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using FileHelpers;
using FileHelpers.DataLink;

namespace IntecoAG.ERM.FM.AVT {
    public partial class fmCAVTBookBuhImportController : ObjectViewController {
        [FixedLengthRecord]  
        public class fmCAVTBookBuhImportImport {
            [FieldFixedLength(1)]
            public String BOOK_TYPE;
            [FieldFixedLength(3)]
            public String REC_TYPE;
            [FieldFixedLength(3)]
            public String SUMM_TYPE;
            [FieldFixedLength(8)]
            public String FISCAL_LINE;
            [FieldFixedLength(2)]
            public String NDS_RATE;
            [FieldFixedLength(5)]
            public String BUH_PROV;
            [FieldFixedLength(5)]
            public String BUH_PROV_ORIG;
            [FieldFixedLength(5)]
            public String BUH_PCK;
            [FieldFixedLength(6)]
            public String BUH_DOC_NUM;
            [FieldFixedLength(8)]
            //[FieldConverter(ConverterKind.Date, "ddMMyyyy")]   
            public String BUH_DOC_DATE;
            [FieldFixedLength(5)]
            public String DEBET;
            [FieldFixedLength(5)]
            public String CREDIT;
            [FieldFixedLength(17)]
            [FieldConverter(ConverterKind.Decimal, ".")]
            public Decimal SUMM;
            [FieldFixedLength(3)]
            public String SF_TYPE;
            [FieldFixedLength(20)]
            public String SF_REGNUM;
            [FieldFixedLength(20)]
            public String SF_NUMBER;
            [FieldFixedLength(8)]
            //[FieldConverter(ConverterKind.Date, "yyyyMMdd")]
            public String SF_DATE;
            [FieldFixedLength(5)]
            public String COD_VO;
            [FieldFixedLength(3)]
            public String PP_TYPE;
            [FieldFixedLength(20)]
            public String PP_NUMBER;
            [FieldFixedLength(8)]
            public String PP_DATE;
            [FieldFixedLength(7)]
            public String PERIOD_OTCH;
            [FieldFixedLength(7)]
            public String PERIOD_BUH;
       
//            [FieldFixedLength(20)]   
//            public string Name;   
   
//            [FieldFixedLength(8)]   
//            [FieldConverter(ConverterKind.Date, "yyyyMMdd")]   
//            public DateTime AddedDate;   
 
        }

        public fmCAVTBookBuhImportController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCAVTBookBuhImport book_buh = e.CurrentObject as fmCAVTBookBuhImport;
            if (book_buh == null) return;
            ObjectSpace.CommitChanges();
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                FixedFileEngine engine = new FixedFileEngine(typeof(fmCAVTBookBuhImportImport));
                //         engine.Options.IgnoreFirstLines = 10;   
                fmCAVTBookBuhImportImport[] imp_res = (fmCAVTBookBuhImportImport[])engine.ReadFile(dialog.FileName);
//                try {
                  using (IObjectSpace os = this.ObjectSpace.CreateNestedObjectSpace()) {
                    book_buh = os.GetObject<fmCAVTBookBuhImport>(book_buh);
                    os.Delete(new List<fmCAVTBookBuhRecord>(book_buh.BookBuhRecords));
//                    book_buh = ObjectSpace.GetObject<fmCAVTBookBuhImport>(book_buh);
//                    ObjectSpace.Delete(new List<fmCAVTBookBuhRecord>(book_buh.BookBuhRecords));
                    foreach (fmCAVTBookBuhImportImport imp_rec in imp_res) {
                        fmCAVTBookBuhRecord rec = os.CreateObject<fmCAVTBookBuhRecord>();
                        //fmCAVTBookBuhRecord rec = ObjectSpace.CreateObject<fmCAVTBookBuhRecord>();
                        book_buh.BookBuhRecords.Add(rec);

                        rec.BookType = imp_rec.BOOK_TYPE.Trim();

                        rec.RecordType = imp_rec.REC_TYPE.Trim();
                        rec.RecordSummType = imp_rec.SUMM_TYPE.Trim();
                        rec.FiscalLetLine = imp_rec.FISCAL_LINE.Trim();

                        rec.NDSRate = imp_rec.NDS_RATE.Trim();
                        rec.BuhProvCode = imp_rec.BUH_PROV.Trim();
                        rec.BuhProvOrigCode = imp_rec.BUH_PROV_ORIG.Trim();
                        rec.BuhPckCode = imp_rec.BUH_PCK.Trim();
                        rec.BuhDocNumber = imp_rec.BUH_DOC_NUM.Trim();
                        if (!String.IsNullOrEmpty(imp_rec.BUH_DOC_DATE.Trim()))
                            //rec.BuhDocDate = DateTime.TryParseExact((imp_rec.BUH_DOC_DATE);
                            DateTime.TryParseExact(imp_rec.BUH_DOC_DATE, "yyyyMMdd", null, DateTimeStyles.None, out rec.BuhDocDate);
                        //rec.BuhDocDate = imp_rec.BUH_DOC_DATE;
                        rec.AccSubDebetCode = imp_rec.DEBET.Trim();
                        rec.AccSubCreditCode = imp_rec.CREDIT.Trim();
                        //                        if (!String.IsNullOrEmpty(imp_rec.SUMM.Trim())) {
                        //                            Decimal summ = 0;
                        //                            Decimal.TryParse(imp_rec.SUMM.Trim(), out summ );
                        //                            rec.RecordSumm = summ;
                        //                        }
                        rec.RecordSumm = imp_rec.SUMM;
                        if (rec.RecordSummType == "COS") {
                            rec.SummCost = rec.RecordSumm;
                            if (rec.NDSRate == "2") {
                                rec.SummVATControl = Decimal.Round(rec.RecordSumm * 18 / 100, 2);
                            }
                            else if (rec.NDSRate == "3") {
                                rec.SummVATControl = Decimal.Round(rec.RecordSumm * 10 / 100, 2);
                            }
                        }
                        else if (rec.RecordSummType == "NDS") {
                            rec.SummVAT = rec.RecordSumm;
                            rec.SummVATControl = -rec.RecordSumm;
                        }
                        else if (rec.RecordSummType == "IN") {
                            rec.SummVATIn = rec.RecordSumm;
                            rec.SummVATControl = rec.RecordSumm;
                        }
                        else if (rec.RecordSummType == "SEB") {
                            rec.SummVATCost = rec.RecordSumm;
                            rec.SummVATControl = -rec.RecordSumm;
                        }
                        else if (rec.RecordSummType == "EXP") {
                            rec.SummVATExp = rec.RecordSumm;
                            rec.SummVATControl = -rec.RecordSumm;
                        }
                        else if (rec.RecordSummType == "NSF") {
                            rec.SummVATNoInvoice = rec.RecordSumm;
                            rec.SummVATControl = -rec.RecordSumm;
                        }
                        else if (rec.RecordSummType == "CRD") {
                            rec.SummVATCrdOther = rec.RecordSumm;
                            rec.SummVATControl = -rec.RecordSumm;
                        }
                        else if (rec.RecordSummType == "ALL") {
                            rec.SummAll = rec.RecordSumm;
                            if (rec.NDSRate == "2") {
                                rec.SummVATControl = Decimal.Round(rec.RecordSumm * 18 / 118, 2);
                            }
                            else if (rec.NDSRate == "3") {
                                rec.SummVATControl = Decimal.Round(rec.RecordSumm * 10 / 110, 2);
                            }
                        }
                        rec.AVTInvoiceType = imp_rec.SF_TYPE.Trim();
                        rec.AVTInvoiceRegNumber = imp_rec.SF_REGNUM.Trim();
                        rec.AVTInvoiceNumber = imp_rec.SF_NUMBER.Trim();
                        if (!String.IsNullOrEmpty(imp_rec.SF_DATE.Trim()))
                            //    rec.AVTInvoiceDate = DateTime.Parse(imp_rec.BUH_DOC_DATE);
                            DateTime.TryParseExact(imp_rec.SF_DATE, "yyyyMMdd", null, DateTimeStyles.None, out rec.AVTInvoiceDate);
                        //rec.AVTInvoiceDate = (DateTime) imp_rec.SF_DATE;
                        rec.AVTInvoicePartyCode = imp_rec.COD_VO.Trim();
                        rec.PeriodOtchet = imp_rec.PERIOD_OTCH.Trim();
                        rec.PeriodBuhgal = imp_rec.PERIOD_BUH.Trim();
                        //
                        rec.PayDocType = imp_rec.PP_TYPE.Trim();
                        rec.PayDocNumber = imp_rec.PP_NUMBER.Trim();
                        if (!String.IsNullOrEmpty(imp_rec.PP_DATE.Trim()))
                            //    rec.AVTInvoiceDate = DateTime.Parse(imp_rec.BUH_DOC_DATE);
                            DateTime.TryParseExact(imp_rec.PP_DATE, "yyyyMMdd", null, DateTimeStyles.None, out rec.PayDocDate);
                    }
                    os.CommitChanges();
                    //ObjectSpace.CommitChanges();
                }
                //catch {
                //    ObjectSpace.Rollback();
                //}
            }
            ObjectSpace.CommitChanges();
        }
    }
}
