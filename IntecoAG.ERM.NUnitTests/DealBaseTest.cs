using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CRM;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Forms;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;

using NUnit.Framework;

namespace IntecoAG.ERM.CRM.Contract
{

    //[TestFixture, Description("Проверка создания простого договора без этапов")]
    public class DealBaseTest
    {
        protected hrmDepartment department1;
        protected hrmDepartment department2;
        protected csCountry country1;
        protected csAddress address1;
        protected csAddress address2;
        protected crmContractCategory contractCategory1;
        protected crmLegalPersonRussianRu legalPersonRussianRu1;
        protected crmLegalPersonRussianRu legalPersonRussianRu2;
        protected crmPartyRu partyRu1;
        protected crmPartyRu partyRu2;

        protected crmContractDocumentType documentCategory1;
        protected csUnit unit1;
        protected csNomenclatureType nomenclatureType1;

        protected fmCostItem costItem1;

        protected csValuta valuta1;

        protected crmPhysicalPersonRu physicalPersonRu1;
        protected crmPhysicalPersonRu physicalPersonRu2;
        protected hrmStaff staff1;
        protected hrmStaff staff2;

        protected RuleSet ruleSet;
        protected RuleSetValidationResult ruleResult;

        [TestFixtureSetUp]
        public virtual void Init() {
            // Метод выполняется один раз до любых тестов
            Common.PrepareDB();

            // Прочистка БД
            if (Common.dataStore != null) ((IDataStoreForTests)Common.dataStore).ClearDatabase();
        }

        [SetUp]
        public virtual void TestSetup() {
            // Метод выполняется перед каждым ("элементарным") тестом.
            Trace.WriteLine("Test started at " + DateTime.Now);
        }

        [TearDown]
        public virtual void TestDispose() {
            // Метод выполняется после каждого ("элементарного") теста.
            Trace.WriteLine("Test finished at " + DateTime.Now);
        }

        [TestFixtureTearDown]
        public virtual void Cleanup() {
            // Метод выполняется после завершения всех тестов.
            Trace.WriteLine("Completed at " + DateTime.Now);
        }


        protected virtual void FillDatabase(Session ssn) {
            department1 = Prepare_hrmDepartment(ssn, "1");
            department2 = Prepare_hrmDepartment(ssn, "2");
            country1 = Prepare_csCountry(ssn, "1");
            address1 = Prepare_csAddress(ssn, "1", country1);
            address2 = Prepare_csAddress(ssn, "1", country1);
            contractCategory1 = Prepare_crmContractCategory(ssn, "1");
            documentCategory1 = Prepare_crmDocumentCategory(ssn, "1");
            legalPersonRussianRu1 = Prepare_crmLegalPersonRussianRu(ssn, "1", address1);
            legalPersonRussianRu2 = Prepare_crmLegalPersonRussianRu(ssn, "2", address2);
            partyRu1 = Prepare_crmPartyRu(ssn, "1", address1, legalPersonRussianRu1);
            partyRu2 = Prepare_crmPartyRu(ssn, "2", address2, legalPersonRussianRu2);

            unit1 = Prepare_csUnit(ssn, "1");
            nomenclatureType1 = Prepare_csNomenclatureType(ssn, "1");
            costItem1 = Prepare_fmCostItem(ssn, "1");

            valuta1 = Prepare_csValuta(ssn, "1", nomenclatureType1, unit1, costItem1);

            physicalPersonRu1 = Prepare_crmPhysicalPersonRu(ssn, "1", address1);
            physicalPersonRu2 = Prepare_crmPhysicalPersonRu(ssn, "1", address2);
            staff1 = Prepare_hrmStaff(ssn, "1", physicalPersonRu1, department1);
            staff2 = Prepare_hrmStaff(ssn, "2", physicalPersonRu2, department2);
        }


        #region Занесение информации в справочники по отдельности

        protected virtual crmContractDocumentType Prepare_crmDocumentCategory(Session ssn, string modificator) {
            crmContractDocumentType documentCategory = new crmContractDocumentType(ssn);
            documentCategory.Code = "Тип:" + modificator;
            documentCategory.Name = "Тип договорного документа " + modificator;
            return documentCategory;
        }

        protected virtual crmContractCategory Prepare_crmContractCategory(Session ssn, string modificator) {
            crmContractCategory contractCategory1 = new crmContractCategory(ssn);
            contractCategory1.Code = "КатДог" + modificator;
            contractCategory1.Name = "Категория договора " + modificator;
            return contractCategory1;
        }

        protected virtual crmCostModel Prepare_crmCostModel(Session ssn, string modificator) {
            crmCostModel costModel1 = new crmCostModel(ssn);
            costModel1.Code = "Мод цены " + modificator;
            costModel1.Name = "Модель цены " + modificator;
            costModel1.Description = "Мод цены " + modificator + " Описание " + modificator;
            return costModel1;
        }

        protected virtual csCountry Prepare_csCountry(Session ssn, string modificator) {
            csCountry country1 = new csCountry(ssn);
            country1.NameEnShortLow = "R" + modificator;
            country1.NameEnFull = "Country" + modificator;
            country1.NameRuShortLow = "С" + modificator;
            country1.NameRuFull = "Страна" + modificator;
            country1.CodeAlfa2 = "C" + modificator;
            country1.CodeAlfa3 = "R" + modificator;
            country1.CodeRuAlfa3 = "Р" + modificator;
            country1.CodeNumeric = "001"; //+modificator;
            country1.Comment = "Страна как она называется" + modificator;
            return country1;
        }

        protected virtual csUnit Prepare_csUnit(Session ssn, string modificator) {
            csUnit unit1 = new csUnit(ssn);
            unit1.Code = "Ед" + modificator;
            unit1.Name = "Единица" + modificator;
            unit1.Description = "Описание" + modificator;
            return unit1;
        }

        protected virtual csNomenclatureType Prepare_csNomenclatureType(Session ssn, string modificator) {
            csNomenclatureType nomenclatureType1 = new csNomenclatureType(ssn);
            nomenclatureType1.Code = "Ном Тип " + modificator;
            nomenclatureType1.Name = "Номенклатурный тип " + modificator;
            return nomenclatureType1;
        }

        protected virtual fmCostItem Prepare_fmCostItem(Session ssn, string modificator) {
            fmCostItem costItem1 = new fmCostItem(ssn);
            costItem1.Code = "СтДДС" + modificator;
            costItem1.Name = "Статья ДДС " + modificator;
            costItem1.Description = "Статья ДДС " + modificator;
            return costItem1;
        }

        protected virtual csMaterial Prepare_csMaterial(Session ssn, string modificator, csNomenclatureType nomenclatureType, csUnit unit, fmCostItem costItem) {
            csMaterial material1 = new csMaterial(ssn);
            material1.NomenclatureType = nomenclatureType;
            material1.Code = "Мат" + modificator;
            material1.NameShort = "Мт" + modificator;
            material1.NameFull = "Материал" + modificator;
            material1.BaseUnit = unit;
            material1.CostItem = costItem;
            return material1;
        }

        protected virtual csService Prepare_csService(Session ssn, string modificator, csNomenclatureType nomenclatureType, csUnit unit, fmCostItem costItem) {
            csService service1 = new csService(ssn);
            service1.NomenclatureType = nomenclatureType;
            service1.Code = "Услуга" + modificator;
            service1.NameShort = "Усл" + modificator;
            service1.NameFull = "Услуга медвежья " + modificator;
            service1.BaseUnit = unit;
            service1.CostItem = costItem;
            return service1;
        }

        protected virtual csValuta Prepare_csValuta(Session ssn, string modificator, csNomenclatureType nomenclatureType, csUnit unit, fmCostItem costItem) {
            csValuta valuta1 = new csValuta(ssn);
            valuta1.NomenclatureType = nomenclatureType;
            valuta1.Code = "Р" + modificator;
            valuta1.NameShort = "Р" + modificator;
            valuta1.NameFull = "Руб" + modificator;
            valuta1.BaseUnit = unit;
            valuta1.CostItem = costItem;
            return valuta1;
        }

        protected virtual csAddress Prepare_csAddress(Session ssn, string modificator, csCountry country) {
            csAddress address1 = new csAddress(ssn);
            address1.Country = country;
            address1.ZipPostal = "100000";
            address1.Region = "Московский";
            address1.StateProvince = "Московская провинция";
            address1.City = "Москва";
            address1.Street = "Профсоюзная, " + modificator;
            address1.AddressString = "стр. " + modificator;
            return address1;
        }

        protected virtual crmPhysicalPersonRu Prepare_crmPhysicalPersonRu(Session ssn, string modificator, csAddress address) {
            crmPhysicalPersonRu physicalPerson1 = new crmPhysicalPersonRu(ssn);
            physicalPerson1.LastName = "Иванов" + modificator;
            physicalPerson1.FirstName = "Иван" + modificator;
            physicalPerson1.MiddleName = "Иванович" + modificator;
            physicalPerson1.INN = "123456789";
            physicalPerson1.Address = address;
            return physicalPerson1;
        }

        protected virtual crmBusinessmanRu Prepare_crmBusinessmanRu(Session ssn, string modificator, crmPhysicalPersonRu physicalPersonRu, csAddress address) {
            crmBusinessmanRu businessman1 = new crmBusinessmanRu(ssn);
            businessman1.PhysicalPerson = physicalPersonRu;
            businessman1.Name = "Иван" + modificator;
//            businessman1.NameFull = "Иванов & Ко" + modificator;
            businessman1.RegNumber = "Рег Ном " + modificator;
            businessman1.INN = "123456789";
            businessman1.Address = address;
            return businessman1;
        }

        protected virtual crmLegalPersonRussianRu Prepare_crmLegalPersonRussianRu(Session ssn, string modificator, csAddress address) {
            crmLegalPersonRussianRu legalPersonRussianRu1 = new crmLegalPersonRussianRu(ssn);
            legalPersonRussianRu1.Name = "ИнтекоАГ" + modificator;
            legalPersonRussianRu1.NameFull = "ИнтекоАГ" + modificator;
//            legalPersonRussianRu1.RegNumber = "Рег Ном " + modificator;
            legalPersonRussianRu1.INN = "1111111111";
//            legalPersonRussianRu1.Address = address;
            return legalPersonRussianRu1;
        }

        protected virtual crmPartyRu Prepare_crmPartyRu(Session ssn, string modificator, csAddress address, crmLegalPersonRussianRu legalPersonRussianRu) {
            crmPartyRu partyRu1 = new crmPartyRu(ssn);
            partyRu1.AddressFact = address;
            partyRu1.Code = "Код ФА " + modificator;
//            partyRu1.Description = "Фирма по производству программного обеспечени" + modificator;
            partyRu1.KPP = "ewwewe33434";
//            partyRu1.PartyName = "PartyName " + modificator;
            partyRu1.Person = legalPersonRussianRu.Person;
            return partyRu1;
        }

        protected virtual SubjectClass Prepare_fmSubject(Session ssn, string modificator) {
            SubjectClass subject1 = new SubjectClass(ssn);
            subject1.Code = "Тема " + modificator;
            subject1.Name = "Название темы" + modificator;
            subject1.DateBegin = System.DateTime.Now;
            subject1.DateEnd = System.DateTime.Now.AddDays(30.0);
            return subject1;
        }

        protected virtual hrmDepartment Prepare_hrmDepartment(Session ssn, string modificator) {
            hrmDepartment department1 = new hrmDepartment(ssn);
            department1.Code = "АДМ " + modificator;
            department1.Name = "Администрация" + modificator;
            department1.PostCode = "000000";
            return department1;
        }

        protected virtual hrmStaff Prepare_hrmStaff(Session ssn, string modificator, crmPhysicalPersonRu physicalPersonRu, hrmDepartment department) {
            hrmStaff staff1 = new hrmStaff(ssn);
            staff1.PhysicalPerson = physicalPersonRu;
            staff1.Department = department;
            staff1.Level = "Должность" + modificator;
            return staff1;
        }

        protected virtual crmUserParty Prepare_crmUserParty(Session ssn, string modificator, crmPartyRu partyRu) {
            crmUserParty userParty1 = new crmUserParty(ssn);
            userParty1.Party = partyRu;
            return userParty1;
        }

        #endregion

    }
}
