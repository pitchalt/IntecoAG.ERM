using System;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw.References {

    [Persistent("TrwReferences")]
    [DefaultProperty("Name")]
    public abstract class TrwRefBase : XPLiteObject {
        public const String RefCNDTCode = "CNDT";
        public const String RefSOSCode = "SOS";
        public const String RefMUCode = "MU";
        public const String RefNSTCode = "NST";
        public const String RefNTTCode = "NTT";
        public const String RefNITCode = "NIT";
        public const String RefFATCode = "FAT";
        public const String RefEXPTCode = "EXPT";
        public const String RefSCTCode = "SCT";
        public const String RefFINSCode = "FINS";
        public const String RefCNTCode = "CNT";
        public const String RefCNMCode = "CNM";
        public const String RefBGLCode = "BGL";
        public const String RefWTCode = "WT";
        public const String RefTAXTCode = "TAXT";
        public const String RefTAXCode = "TAX";
        public const String RefPELTCode = "PELT";
        public const String RefPEMCode = "PEM";
        public const String RefPETCode = "PET";
        public const String RefTaxVatRateCode = "TaxVATRate";
        public const String RefCUCode = "CU";
        public const String RefSOSOCode = "SOSO";
        public const String RefIVCCode = "IVC";

        [Persistent("Oid"), Key(AutoGenerate = true), Browsable(false), MemberDesignTimeVisibility(false)]
        private Int32 _Oid = 0;
        [PersistentAlias("_Oid"), Browsable(false)]
        public int Oid {
            get { return _Oid; }
        }
        //
        [Persistent("RefCode")]
        [Size(10)]
        protected String _RefCode;
        [PersistentAlias("_RefCode")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public String RefCode {
            get { return _RefCode; }
        }
        //
        private String _Code;
        [Size(10)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }
        //
        private String _Name;
        [Size(80)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Удалять нельзя");
        }

        public TrwRefBase(Session session): base(session) { }

        public override string ToString() {
            return Name;
        }

        public static TrwRefBase Create(IObjectSpace os, String ref_code) {
            switch (ref_code) {
                case RefCNDTCode:
                    return os.CreateObject<TrwRefCNDT>();
                case RefSOSCode:
                    return os.CreateObject<TrwRefSOS>();
                case RefMUCode:
                    return os.CreateObject<TrwRefMU>();
                case RefNSTCode:
                    return os.CreateObject<TrwRefNST>();
                case RefNTTCode:
                    return os.CreateObject<TrwRefNTT>();
                case RefNITCode:
                    return os.CreateObject<TrwRefNIT>();
                case RefFATCode:
                    return os.CreateObject<TrwRefFAT>();
                case RefEXPTCode:
                    return os.CreateObject<TrwRefEXPT>();
                case RefSCTCode:
                    return os.CreateObject<TrwRefSCT>();
                case RefFINSCode:
                    return os.CreateObject<TrwRefFINS>();
                case RefCNTCode:
                    return os.CreateObject<TrwRefCNT>();
                case RefCNMCode:
                    return os.CreateObject<TrwRefCNM>();
                case RefBGLCode:
                    return os.CreateObject<TrwRefBGL>();
                case RefWTCode:
                    return os.CreateObject<TrwRefWT>();
                case RefTAXTCode:
                    return os.CreateObject<TrwRefTAXT>();
                case RefTAXCode:
                    return os.CreateObject<TrwRefTAX>();
                case RefPELTCode:
                    return os.CreateObject<TrwRefPELT>();
                case RefPEMCode:
                    return os.CreateObject<TrwRefPEM>();
                case RefPETCode:
                    return os.CreateObject<TrwRefPET>();
                case RefTaxVatRateCode:
                    return os.CreateObject<TrwRefTaxVatRate>();
                case RefCUCode:
                    return os.CreateObject<TrwRefCU>();
                case RefSOSOCode:
                    return os.CreateObject<TrwRefSOSO>();
                case RefIVCCode:
                    return os.CreateObject<TrwRefIVC>();
                default:
                    throw new ArgumentException(ref_code, "ref_code");
            }
        }
    }
    /// <summary>
    /// Contract document type
    /// Тип приложения
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefCNDT : TrwRefBase {
        public TrwRefCNDT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefCNDTCode;
        }
    }
    /// <summary>
    /// Sale Order Subject
    /// ФКД Функциональный классификатор дейтельности
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefSOS : TrwRefBase {
        public TrwRefSOS(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefSOSCode;
        }
    }
    /// <summary>
    /// Measure unit 
    /// Единица измерения
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefMU : TrwRefBase {
        public TrwRefMU(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefMUCode;
        }
    }
    /// <summary>
    /// Sale Nomenclature Type
    /// Группа продукции
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefNST : TrwRefBase {
        public TrwRefNST(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefNSTCode;
        }
    }
    /// <summary>
    /// Sale Nomenclature Target Type
    /// Назначение продукции
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefNTT : TrwRefBase {
        public TrwRefNTT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefNTTCode;
        }
    }
    /// <summary>
    /// Inventory Nomenclature Type
    /// Виды ТМЦ
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefNIT : TrwRefBase {
        public TrwRefNIT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefNITCode;
        }
    }
    /// <summary>
    /// Fixed Asset Type
    /// Виды ОС и НМА
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefFAT : TrwRefBase {
        public TrwRefFAT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefFATCode;
        }
    }
    /// <summary>
    /// Expendition Type
    /// Категории затрат
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefEXPT : TrwRefBase {
        public TrwRefEXPT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefEXPTCode;
        }
    }
    /// <summary>
    /// Security Type
    /// Виды ЦБ
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefSCT : TrwRefBase {
        public TrwRefSCT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefSCTCode;
        }
    }
    /// <summary>
    /// Finance Source
    /// Источники финансирования
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefFINS : TrwRefBase {
        public TrwRefFINS(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefFINSCode;
        }
    }
    /// <summary>
    /// Contract type
    /// Категории договоров
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefCNT : TrwRefBase {
        public TrwRefCNT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefCNTCode;
        }
    }
    /// <summary>
    /// Contract market
    /// Генеральные заказчики
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefCNM : TrwRefBase {
        public TrwRefCNM(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefCNMCode;
        }
    }
    /// <summary>
    /// Budget Level
    /// Уровень бюджетной системы
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefBGL : TrwRefBase {
        public TrwRefBGL(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefBGLCode;
        }
    }
    /// <summary>
    /// Worker type
    /// Вид персонала
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefWT : TrwRefBase {
        public TrwRefWT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefWTCode;
        }
    }
    /// <summary>
    /// TAX Type
    /// Вид налогов
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefTAXT : TrwRefBase {
        public TrwRefTAXT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefTAXTCode;
        }
    }

    /// <summary>
    /// TAX 
    /// Наименование налогов
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefTAX : TrwRefBase {
        public TrwRefTAX(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefTAXCode;
        }
    }

    /// <summary>
    /// Person Legal Type
    /// Вид контрагента
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefPELT : TrwRefBase {
        public TrwRefPELT(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefPELTCode;
        }
    }
    /// <summary>
    /// Person Market
    /// Тип рынка контрагента
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefPEM : TrwRefBase {
        public TrwRefPEM(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefPEMCode;
        }
    }
    /// <summary>
    /// Person Type
    /// Тип контрагента
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefPET : TrwRefBase {
        public TrwRefPET(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefPETCode;
        }
    }
    /// <summary>
    /// TAX VAT Rate
    /// Ставки НДС
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefTaxVatRate : TrwRefBase {
        public TrwRefTaxVatRate(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefTaxVatRateCode;
        }
    }
    /// <summary>
    /// Currency
    /// Валюта
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefCU : TrwRefBase {
        public TrwRefCU(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefCUCode;
        }
    }
    /// <summary>
    /// Sale Order Subject Old 
    /// Код вида деятельности (Возможно не используется)
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefSOSO : TrwRefBase {
        public TrwRefSOSO(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefSOSOCode;
        }
    }
    /// <summary>
    /// Investment Category
    /// Разделы мероприятий
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Trw.Codif.References")]
    public class TrwRefIVC : TrwRefBase {
        public TrwRefIVC(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _RefCode = RefIVCCode;
        }
    }

}
