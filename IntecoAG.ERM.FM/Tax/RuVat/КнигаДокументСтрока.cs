using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax.RuVat {

    [Persistent("FmTaxRuVatКнигаДокСтрока")]
    [Appearance(null, AppearanceItemType.ViewItem,  "Удалена", TargetItems="*", FontStyle=System.Drawing.FontStyle.Strikeout)]
    public abstract class КнигаДокументСтрока : BaseEntity {

        private КнигаСтрока _КнигаСтрока;
        [Association("КнигаСтрока-КнигаДокументСтрока")]
        [VisibleInDetailView(true)]
//        [VisibleInListView(true)]
        [DataSourceProperty("Книга.СтрокиКниги")]
        public КнигаСтрока КнигаСтрока {
            get { return _КнигаСтрока; }
            set {
                if (!IsLoading)
                    OnChanging("КнигаСтрока", value);
                SetPropertyValue<КнигаСтрока>("КнигаСтрока", ref _КнигаСтрока, value);
            }
        }

        [PersistentAlias("КнигаСтрока.Основание")]
        public Основание Основание {
            get { return КнигаСтрока != null ? КнигаСтрока.Основание : null; }
        }
        [PersistentAlias("КнигаСтрока.СчетФактура")]
        public Основание СчетФактура {
            get { return КнигаСтрока != null ? КнигаСтрока.СчетФактура : null; }
        }
        [PersistentAlias("КнигаСтрока.КорректировочныйСчетФактура")]
        public Основание КорректировочныйСчетФактура {
            get { return КнигаСтрока != null ? КнигаСтрока.КорректировочныйСчетФактура : null; }
        }

        private Boolean _ИзменитьВручную;
        public Boolean ИзменитьВручную {
            get { return _ИзменитьВручную; }
            set { SetPropertyValue<Boolean>("ИзменитьВручную", ref _ИзменитьВручную, value); }
        }

        private ОснованиеДокумент _ОснованиеДокумент;
        [RuleRequiredField]
        [Appearance(null, AppearanceItemType.ViewItem, "КнигаСтрока == Null", Enabled = false)]
        public ОснованиеДокумент ОснованиеДокумент {
            get { return _ОснованиеДокумент; }
            set { SetPropertyValue<ОснованиеДокумент>("ОснованиеДокумент", ref _ОснованиеДокумент, value); }
        }
        [Persistent]
        public ОснованиеДокумент СчетФактураДокумент {
            get {
                if (ОснованиеДокумент != null) {
                    if (ОснованиеДокумент.Корректировка == RuVat.Основание.ТипПодчиненности.ОСНОВНОЙ)
                        return ОснованиеДокумент;
                    return ОснованиеДокумент.БазовыйДокумент;
                }
                return null; 
            }
        }
        [Persistent]
        public ОснованиеДокумент КоррекСчетФактураДок {
            get {
                if (ОснованиеДокумент != null) {
                    if (ОснованиеДокумент.Корректировка == RuVat.Основание.ТипПодчиненности.ОСНОВНОЙ)
                        return null;
                    return ОснованиеДокумент;
                }
                return null;
            }
        }

        private КнигаДокумент _КнигаДокумент;
        [Association("КнигаДокумент-КнигаДокументСтрока")]
        [RuleRequiredField]
        [Appearance(null, AppearanceItemType.ViewItem, "КнигаДокумент != Null", Enabled = false)]
        public КнигаДокумент КнигаДокумент {
            get { return _КнигаДокумент; }
            set { SetPropertyValue<КнигаДокумент>("КнигаДокумент", ref _КнигаДокумент, value); }
        }

        private Книга _Книга;
        [Association("Книга-КнигаДокументСтрока")]
        public Книга Книга {
            get { return _Книга; }
            set { SetPropertyValue<Книга>("Книга", ref _Книга, value); }
        }

        private КнигаДокументСтрока _СторнированоСтрокой;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public КнигаДокументСтрока СторнированоСтрокой {
            get { return _СторнированоСтрокой; }
            set { SetPropertyValue<КнигаДокументСтрока>("СторнированоСтрокой", ref _СторнированоСтрокой, value); }
        }

        public Boolean Удалена {
            get { return СторнированоСтрокой != null; }
        }

        private КнигаДокументСтрока _СторноСтроки;

        public КнигаДокументСтрока СторноСтроки {
            get { return _СторноСтроки; }
            set { SetPropertyValue<КнигаДокументСтрока>("СторноСтроки", ref _СторноСтроки, value); }
        }

        public КнигаДокументСтрока(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();

        }

        public override void OnChanging(string propertyName, object newValue) {
            base.OnChanging(propertyName, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "КнигаДокумент":
                    if (КнигаДокумент != null)
                        throw new InvalidOperationException("Строка уже связана с книгой");
                    break;
                case "ОснованиеДокумент":
                    if (КнигаСтрока == null)
                        throw new InvalidOperationException("Сначала свяжите строку документа со строкой книги");
                    break;
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "КнигаДокумент":
                    if (КнигаДокумент != null)
                        Книга = КнигаДокумент.Книга;
                    else
                        Книга = null;
                    break;
                case "КнигаСтрока":
                    if (КнигаСтрока != null)
                        ОснованиеДокумент = Основание != null ? Основание.ДействующийДокумент : null;
                    break;
                case "СторноСтроки":
                    КнигаДокументСтрока old_str = (КнигаДокументСтрока)oldValue;
                    if (old_str != null)
                        old_str.СторнированоСтрокой = null;
                    if (СторноСтроки != null)
                        СторноСтроки.СторнированоСтрокой = this;
                    break;
                case "ОснованиеДокумент":
                    UpdateDocumentInfo();
                    break;
            }
        }

        protected override void OnDeleting() {
            base.OnDeleting();
        }

        protected virtual void UpdateDocumentInfo() {
        }
    }
}
