using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

namespace IntecoAG.ERM.Trw.Contract {

    [Persistent("TrwContractExchangeDoc")]
    public class TrwContractExchangeDoc : csCComponent {

        [Persistent("TrwContractExchangeDocDealLink")]
        public class TrwContractExchangeDocDealLink : csCComponent {
            public TrwContractExchangeDocDealLink(Session session) : base(session) { }
            public override void AfterConstruction() {            
                base.AfterConstruction();
            }
            private TrwContractExchangeDoc _ExchangeDoc;
            private crmContractDeal _Deal;

            [Association("TrwContractExchangeDoc-TrwContractExchangeDocDealLink")]
            public TrwContractExchangeDoc ExchangeDoc {
                get { return _ExchangeDoc; }
                set { SetPropertyValue<TrwContractExchangeDoc>("ExchangeDoc", ref _ExchangeDoc, value); }
            }

            public crmContractDeal Deal {
                get { return _Deal; }
                set { SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value); }
            }
        
        }

        public class DealList : IList<crmContractDeal>, ICollection<crmContractDeal>, IEnumerable<crmContractDeal> {
            private TrwContractExchangeDoc _ExchangeDoc;
            public DealList(TrwContractExchangeDoc doc) {
                _ExchangeDoc = doc;
            }

            public int IndexOf(crmContractDeal item) {
                XPCollection<TrwContractExchangeDocDealLink> deal_links = _ExchangeDoc.DealLinks;
                for (Int32 index = 0; index < deal_links.Count; index++) {
                    if (deal_links[index].Deal == item)
                        return index;
                }
                return -1;
            }

            public void Insert(int index, crmContractDeal item) {
                ((IList<TrwContractExchangeDocDealLink>)_ExchangeDoc.DealLinks).Insert(index, DealLinkNew(item));
            }

            private TrwContractExchangeDocDealLink DealLinkNew(crmContractDeal deal) {
                return
                    new TrwContractExchangeDocDealLink(_ExchangeDoc.Session) {
                        ExchangeDoc = _ExchangeDoc,
                        Deal = deal
                    };
            }

            public void RemoveAt(int index) {
                ((IList<TrwContractExchangeDocDealLink>)_ExchangeDoc.DealLinks).RemoveAt(index);
            }

            public crmContractDeal this[int index] {
                get {
                    return _ExchangeDoc.DealLinks[index].Deal;
                }
                set {
                    _ExchangeDoc.DealLinks[index].Deal = value;
                }
            }

            public void Add(crmContractDeal item) {
                _ExchangeDoc.DealLinks.Add(DealLinkNew(item));
            }

            public void Clear() {
                ((IList<TrwContractExchangeDocDealLink>)_ExchangeDoc.DealLinks).Clear();
            }

            public bool Contains(crmContractDeal item) {
                if (IndexOf(item) >= 0)
                    return true;
                else
                    return false;
            }

            public void CopyTo(crmContractDeal[] array, int arrayIndex) {
                TrwContractExchangeDocDealLink[] link_array = new TrwContractExchangeDocDealLink[array.Length];
                ((IList<TrwContractExchangeDocDealLink>)_ExchangeDoc.DealLinks).CopyTo(link_array, arrayIndex);
                for (Int32 i = arrayIndex; i < link_array.Length; i++) {
                    array[i] = link_array[i].Deal;
                }
            }

            public int Count {
                get { return _ExchangeDoc.DealLinks.Count; }
            }

            public bool IsReadOnly {
                get { return ((IList<TrwContractExchangeDocDealLink>)_ExchangeDoc.DealLinks).IsReadOnly; }
            }

            public bool Remove(crmContractDeal item) {
                Int32 index = IndexOf(item);
                if (index >= 0) {
                    RemoveAt(index);
                    return true;
                }
                return false;
            }
            public class DealEnumerator: IEnumerator<crmContractDeal> {
                IEnumerator<TrwContractExchangeDocDealLink> enumerator;
                public DealEnumerator(IEnumerator<TrwContractExchangeDocDealLink> enumerator) {
                    this.enumerator = enumerator;
                }
                #region IEnumerator<OfType> Members
                public crmContractDeal Current {
                    get { return enumerator.Current.Deal; }
                }
                #endregion
                #region IDisposable Members
                public void Dispose() {
                    enumerator.Dispose();
                }
                #endregion
                #region IEnumerator Members
                object System.Collections.IEnumerator.Current {
                    get { return enumerator.Current.Deal; }
                }
                public bool MoveNext() {
                    return enumerator.MoveNext();
                }
                public void Reset() {
                    enumerator.Reset();
                }
                #endregion
            }
            public IEnumerator<crmContractDeal> GetEnumerator() {
                return new DealEnumerator(((IList<TrwContractExchangeDocDealLink>)_ExchangeDoc.DealLinks).GetEnumerator());
            }
            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        public TrwContractExchangeDoc(Session session) : base(session) { }
        public override void AfterConstruction() {            
            base.AfterConstruction();
            _DateCreate = DateTime.Now;
        }

        [Persistent("DateCreate")]
        private DateTime _DateCreate;

        [PersistentAlias("_DateCreate")]
        public DateTime DateCreate {
            get { return _DateCreate; }
        }

//        [Browsable(false)]
        [Aggregated]
        [Association("TrwContractExchangeDoc-TrwContractExchangeDocDealLink")]
        public XPCollection<TrwContractExchangeDocDealLink> DealLinks {
            get { return GetCollection<TrwContractExchangeDocDealLink>("DealLinks"); }
        }

        public IList<crmContractDeal> Deals {
            get {
                return new DealList(this);
            }
        }
    }
}
