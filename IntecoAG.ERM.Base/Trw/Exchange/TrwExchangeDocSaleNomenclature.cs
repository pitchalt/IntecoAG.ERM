using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Xml.Serialization;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Nomenclature;

namespace IntecoAG.ERM.Trw.Exchange {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwExchangeDocSaleNomenclature : TrwExchangeDoc, TrwExchangeIDoc<TrwSaleNomenclature>
    {

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class SaleNomenclatureLink : ObjectLink, TrwExchangeIDocObjectLink<TrwSaleNomenclature> {
            public SaleNomenclatureLink(Session session) : base(session) { }
            public override void AfterConstruction() {            
                base.AfterConstruction();
            }

            private TrwSaleNomenclature _SaleNomenclature;
            public TrwSaleNomenclature SaleNomenclature {
                get { return _SaleNomenclature; }
                set { SetPropertyValue<TrwSaleNomenclature>("SaleNomenclature", ref _SaleNomenclature, value); }
            }

            [Browsable(false)]
            public override TrwExchangeIExportableObject ExchangeObject {
                get { return SaleNomenclature; }
            }

            TrwSaleNomenclature TrwExchangeIDocObjectLink<TrwSaleNomenclature>.ExchangeObject {
                get { return SaleNomenclature; }
            }
        }

        public TrwExchangeDocSaleNomenclature() { }
        public TrwExchangeDocSaleNomenclature(Session session) : base(session) { }
        public override void AfterConstruction() {            
            base.AfterConstruction();
        }

        [Aggregated]
        public XPCollection<SaleNomenclatureLink> SaleNomenclatureLinks {
            get { 
                return new ObjectLinkCollection<SaleNomenclatureLink>(this.Session, this); 
            }
        }
        IList<TrwExchangeIDocObjectLink<TrwSaleNomenclature>> TrwExchangeIDoc<TrwSaleNomenclature>.ObjectLinks {
            get { return new ListConverter<TrwExchangeIDocObjectLink<TrwSaleNomenclature>,SaleNomenclatureLink>(SaleNomenclatureLinks); }
        }

        public TrwExchangeIDocObjectLink<TrwSaleNomenclature> ObjectLinksCreate(IObjectSpace os, TrwSaleNomenclature obj) {
            SaleNomenclatureLink link = os.CreateObject<SaleNomenclatureLink>();
            link.SaleNomenclature = obj;
            ObjectLinks.Add(link);
            return link;
        }

        public override void Accept(IObjectSpace os) {
            foreach(SaleNomenclatureLink link in SaleNomenclatureLinks) {
                link.SaleNomenclature.TrwExportStateSet(TrwExchangeExportStates.EXPORTED);
            }
            base.Accept(os);
        }

        public override void Serialize(XmlDictionaryWriter writer) {
            return ;
        }
    }
}
