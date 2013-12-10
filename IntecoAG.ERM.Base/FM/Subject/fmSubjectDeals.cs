using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.XAFExt.CDS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Subject;
//
namespace IntecoAG.ERM.FM.Subject {

    [NonPersistent]
    public class fmSubjectDeals: XPCustomObject { 
//        public crmDealRegistrationStatistics() {}
        public fmSubjectDeals(Session ses): base(ses) { }

        protected IList<fmCSubject.DealInfo> _DealInfos;

        public IList<fmCSubject.DealInfo> GetInfos() {
            ReloadDealInfos();
            return _DealInfos;
        }

        public void ReloadDealInfos() {
            if (_DealInfos == null) {
                _DealInfos = new List<fmCSubject.DealInfo>();
            }
            _DealInfos.Clear();
            IList<crmContractDeal> Deals = new XPCollection<crmContractDeal>(this.Session);
            foreach (crmContractDeal deal in Deals) {
                DealInfoDealType deal_type;
                if (deal.TRVType == null || deal.TRVType.TrwContractSuperType != TrwContractSuperType.DEAL_SALE)
                    deal_type = DealInfoDealType.DEAL_INFO_EXPENDITURE;
                else
                    deal_type = DealInfoDealType.DEAL_INFO_PROCEEDS;
                ReloadDealInfos(deal_type, deal);
            }
        }

        //        protected void ReloadDealInfos(DealInfoDealType deal_type, crmDealWithStage deal) {
        protected void ReloadDealInfos(DealInfoDealType deal_type, crmContractDeal deal) {
            foreach (crmStage stage in deal.Current.StageStructure.Stages) {
                ReloadDealInfos(deal_type, deal, stage.DeliveryPlan);
                ReloadDealInfos(deal_type, deal, stage.PaymentPlan);
            }
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmDealWithoutStage deal) {
            ReloadDealInfos(deal_type, deal, ((crmDealWithoutStageVersion)deal.Current).DeliveryPlan);
            ReloadDealInfos(deal_type, deal, ((crmDealWithoutStageVersion)deal.Current).PaymentPlan);
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmContractDeal deal, crmDeliveryPlan plan) {
            foreach (crmDeliveryUnit unit in plan.DeliveryUnits) {
                ReloadDealInfos(deal_type, deal, DealInfoNomType.DEAL_INFO_DELIVERY, unit, new ListConverter<crmObligation, crmDeliveryItem>(unit.DeliveryItems));
            }
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmContractDeal deal, crmPaymentPlan plan) {
            foreach (crmPaymentUnit unit in plan.PaymentUnits) {
                ReloadDealInfos(deal_type, deal, DealInfoNomType.DEAL_INFO_PAYMENT, unit, new ListConverter<crmObligation, crmPaymentItem>(unit.PaymentItems));
            }
        }

        protected void ReloadDealInfos(DealInfoDealType deal_type, crmContractDeal deal, DealInfoNomType nom_type, crmObligationUnit unit, IList<crmObligation> items) {
            foreach (crmObligation obl in items) {
                var deal_info = _DealInfos.FirstOrDefault(
                    x => x.Deal == deal &&
                        x.DealType == deal_type &&
                        x.NomType == nom_type &&
                        x.Order == obl.Order &&
                        x.Valuta == obl.Valuta &&
                        x.Date == unit.DatePlane
                    );
                if (deal_info == null) {
                    deal_info = new fmCSubject.DealInfo(this.Session) {
                        DealType = deal_type,
                        Deal = deal,
                        NomType = nom_type,
                        Subject = obl.Order != null ? obl.Order.Subject : null,
                        Order = obl.Order,
                        Valuta = obl.Valuta,
                        Date = unit.DatePlane
                    };
                    _DealInfos.Add(deal_info);
                }
                deal_info.SummCost += obl.SummCost;
                deal_info.SummVat += obl.SummNDS;
                deal_info.SummFull += obl.SummFull;
            }
        }
    }
}
