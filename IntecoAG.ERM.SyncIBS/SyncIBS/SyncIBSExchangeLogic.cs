using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
//
using IntecoAG.ERM.FM.FinJurnal;
//
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.FSJ;
//
namespace IntecoAG.ERM.SyncIBS {
    /// <summary>
    /// 
    /// </summary>
    static public class SyncIBSExchangeLogic {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="doc"></param>
        static public void ExportTo(IObjectSpace os, fmCFJSaleDoc doc) {
            foreach (fmCFJSaleDocLine line in doc.DocLines) {
                using (IObjectSpace nos = os.CreateNestedObjectSpace()) {
                    fmCFJSaleDocLine curline = os.GetObject<fmCFJSaleDocLine>(line);
                    if (curline.IsSyncIBS) continue;
                    if (!curline.IsApproved) continue;
                    if (curline.SaleJurnalLine == null) continue;
                    curline.SaleJurnalLine.SyncIBS();
                    //            User currentUser = null;
                    IIBSSyncService syncservice = new HTTPSyncService(ConfigurationManager.AppSettings["IBS.SyncService"]);

                    FWSJXMIA msg_in = new FWSJXMIA();
                    msg_in.CMD = "UPDATE";
                    msg_in.OPERATION = curline.SaleOperation.Code;
                    msg_in.SJOID = curline.SaleJurnalLine.Oid.ToString();
                    ////
                    Decimal code;
                    if (!Decimal.TryParse(curline.PartyCode, out code)) return;
                    msg_in.VOCODE = code;
                    msg_in.ZKCODE = curline.OrderNumber;
                    msg_in.SFNUMBER = curline.AVTInvoiceNumber;
                    msg_in.SFDATE = curline.AVTInvoiceDate;
                    msg_in.DGNUMBER = curline.DealNumber;
                    msg_in.DGDATE = curline.DealDate;
                    msg_in.FINUMBER = curline.DocBaseNumber;
                    msg_in.FIDATE = curline.DocBaseDate;
                    //// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    if (curline.SaleOperation.IsNotAVTInvoice)
                        msg_in.PLNUMBER = "СФЗ" + curline.PayNumber;
                    else
                        msg_in.PLNUMBER = "СЧФ";

                    ////
                    msg_in.SUMMCOST = curline.SummCost;
                    msg_in.AVTRATE = curline.AVTRate.Code;
                    msg_in.SUMMAVT = curline.SummAVT;
                    msg_in.SUMMALL = curline.SummAll;
                    if (curline.Valuta == null) {
                        msg_in.VACODE = "";
                    }
                    else
                        msg_in.VACODE = curline.Valuta.Code;

                    msg_in.SUMMVALALL = curline.SummValuta;
                    ////
                    msg_in.UOGCODE = 1000;
                    msg_in.PERIOD = curline.SaleDoc.Period;
                    msg_in.DOCPROV = curline.DocBuhProv;
                    msg_in.DOCPCK = curline.DocBuhPck;
                    msg_in.DOCNUMBER = curline.DocBuhNumber;
                    msg_in.DOCDATE = curline.DocBuhDate;
                    msg_in.ACCSALEDEBET = curline.AccRealDebet.ToString();
                    msg_in.ACCSALECREDIT = curline.AccRealCredit.ToString();
                    msg_in.ACCAVTDEBET = curline.AccAVTDebet.ToString();
                    msg_in.ACCAVTCREDIT = curline.AccAVTCredit.ToString();
                    //// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    msg_in.CNTUSER = SecuritySystem.CurrentUserName;
                    ////
                    FWSJXMOA msg_out = syncservice.FWSJXM0N(msg_in);
                    curline.IsSyncIBS = true;
                    //
                    nos.CommitChanges();
                }
                os.CommitChanges();
            }
        }
    }
}
