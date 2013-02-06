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

using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Contract.Analitic;

using NUnit.Framework;

namespace IntecoAG.ERM.CRM.Contract
{

    [RequiresMTA]
    [TestFixture, Description("Заполнение БД данными для проверки работы формы анализа")]
    public class CreateDBForAnalysisTests : DealBaseTest
    {

        public override void Init() {
            base.Init();

            //UnitOfWork uow = new UnitOfWork(Common.dataLayer);
            //FillDatabase(uow);
            //uow.CommitChanges();

            //ruleSet = new RuleSet();

            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }


        protected override void FillDatabase(Session ssn) {
            base.FillDatabase(ssn);
        }

        protected void FillDatabaseForAnalysis(Session ssn) {

            IObjectSpace objectSpace = new ObjectSpace((UnitOfWork)ssn);

            int RecordCountGenerated = 10000;  // Максимальное количество генерируемых записей в журнале регистрации crmCashFlowRegister
            Guid token = Guid.NewGuid();

            // Делаем три валюты
            IList<csValuta> valutaList = new List<csValuta>();

            csValuta valutaRUR = objectSpace.CreateObject<csValuta>();
            valutaRUR.Code = "RUR";
            valutaRUR.NameShort = "Руб.";
            valutaRUR.NameFull = "Рубль";
            valutaList.Add(valutaRUR);

            csValuta valutaUSD = objectSpace.CreateObject<csValuta>();
            valutaUSD.Code = "USD";
            valutaUSD.NameShort = "bak";
            valutaUSD.NameFull = "Dollar";
            valutaList.Add(valutaUSD);

            csValuta valutaEUR = objectSpace.CreateObject<csValuta>();
            valutaEUR.Code = "EUR";
            valutaEUR.NameShort = "euro";
            valutaEUR.NameFull = "Euro";
            valutaList.Add(valutaEUR);

            objectSpace.CommitChanges();


            //// Создаём maxContractDocumentCount документов
            //int maxContractDocumentCount = 300;
            //IList<fmSubject> ContractDocumentList = new List<fmSubject>();

            //for (int i = 0; i < maxContractDocumentCount; i++) {
            //    fmSubject сontractDocument = objectSpace.CreateObject<fmSubject>();
            //    сontractDocument.Code = "S" + i.ToString();
            //    сontractDocument.Name = "Subj" + i.ToString();
            //    сontractDocument.DateBegin = DateTime.Now;
            //    сontractDocument.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 30));
            //    ContractDocumentList.Add(subject);
            //}

            //objectSpace.CommitChanges();


            // Создаём maxSubjectCount тем
            int maxSubjectCount = 120;
            IList<fmCSubjectExt> subjectList = new List<fmCSubjectExt>();

            for (int i = 0; i < maxSubjectCount; i++) {
                fmCSubjectExt subject = objectSpace.CreateObject<fmCSubjectExt>();
                subject.Code = "S" + i.ToString();
                subject.Name = "Subj" + i.ToString();
                subject.DateBegin = DateTime.Now;
                subject.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 30));
                subjectList.Add(subject);
            }

            objectSpace.CommitChanges();

            // Создаём maxCostItemCount 
            int maxCostItemCount = 10;
            IList<fmCostItem> costItemList = new List<fmCostItem>();

            for (int i = 0; i < maxCostItemCount; i++) {
                fmCostItem costItem = objectSpace.CreateObject<fmCostItem>();
                costItem.Code = "CI" + i.ToString();
                costItem.Name = "CostItem" + i.ToString();
                costItemList.Add(costItem);
            }

            objectSpace.CommitChanges();


            // Заготавливаем maxCostModelCount
            int maxCostModelCount = 5;
            IList<crmCostModel> CostModelList = new List<crmCostModel>();

            for (int i = 0; i < maxCostModelCount; i++) {
                crmCostModel costModel = objectSpace.CreateObject<crmCostModel>();

                costModel.Code = "CM: " + i.ToString();
                costModel.Description = "CostModel: " + i.ToString();
                costModel.Name = "CostModel " + i.ToString();

                CostModelList.Add(costModel);

                //objectSpace.CommitChanges();
            }

            objectSpace.CommitChanges();

            /*
            // Заготавливаем maxContragentPartyRuCount штук контрагентов
            int maxContragentPartyRuCount = 120;
            IList<crmPartyRu> ContragentPartyRuList = new List<crmPartyRu>();

            for (int i = 0; i < maxContragentPartyRuCount; i++) {
                crmPartyRu partyRu = objectSpace.CreateObject<crmPartyRu>();

                partyRu.Code = "cF";
                partyRu.Description = "ContragentParty " + i.ToString();

                //crmPhysicalPerson person = objectSpace.CreateObject<crmPhysicalPerson>();
                //person.INN = "CP INN" + i;
                //person.FirstName = "Гадя " + i;
                //person.MiddleName = "Петрович " + i;
                //person.MiddleName = "Хренова " + i;

                crmCPerson person = objectSpace.CreateObject<crmCPerson>();
                person.INN = "CP INN" + i;
                //person.FirstName = "Гадя " + i;
                //person.MiddleName = "Петрович " + i;
                //person.MiddleName = "Хренова " + i;

                partyRu.Person = person;
                person.Partys.Add(partyRu);

                ContragentPartyRuList.Add(partyRu);

                //objectSpace.CommitChanges();
            }

            objectSpace.CommitChanges();
            */

           
            // Заготавливаем maxContragentcLegalPersonCount штук контрагентов
            int maxCLegalPersonCount = 120;
            IList<crmCLegalPerson> CLegalPersonList = new List<crmCLegalPerson>();

            for (int i = 0; i < maxCLegalPersonCount; i++) {
                crmCLegalPerson cLegalPerson = objectSpace.CreateObject<crmCLegalPerson>();
                cLegalPerson.Name = "ИнтекоАГ" + i;
                cLegalPerson.NameFull = "ИнтекоАГ" + i;
                cLegalPerson.INN = "ИНН 1111111111";
                cLegalPerson.Code = "LP" + i;
                cLegalPerson.Description = "LegalPerson Description" + i;
                cLegalPerson.KPP = "КПП 222222222";
                cLegalPerson.RegCode = "RC";
                //cLegalPerson.Person.Address = address;
                //cLegalPerson.Party.AddressFact = address;
                //cLegalPerson.Party.AddressPost = address;
                cLegalPerson.Party.Person = cLegalPerson.Person;

                CLegalPersonList.Add(cLegalPerson);
            }

            objectSpace.CommitChanges();

            // Заготавливаем maxContractPartyCount штук участников договоров
            int maxContractPartyCount = 50;
            IList<crmContractParty> ContractPartyList = new List<crmContractParty>();

            for (int i = 0; i < maxContractPartyCount; i++) {
                crmContractParty contractParty = objectSpace.CreateObject<crmContractParty>();

                contractParty.INN = "INN: " + i.ToString();
                contractParty.KPP = "KPP: " + i.ToString();
                contractParty.Name = "ContractParty " + i.ToString();
                contractParty.Party = CLegalPersonList[GetRandomIntegerFromInterval(1, CLegalPersonList.Count) - 1].Party;
                contractParty.RegNumber = "RegNumber " + i.ToString();

                ContractPartyList.Add(contractParty);

                //objectSpace.CommitChanges();
            }

            objectSpace.CommitChanges();

            // SHU 2011-12-26 Order не вводится, т.к. имеет срабатывает правило Validation Required для какого-то поля
            // Заготавливаем maxOrderCount штук заказов
            int maxOrderCount = 30;
            IList<fmCOrderExt> OrderList = new List<fmCOrderExt>();

            for (int i = 0; i < maxOrderCount; i++) {
                fmCOrderExt order = objectSpace.CreateObject<fmCOrderExt>();

                //order.Code = "Ord: " + i.ToString();
                order.Description = "Order: " + i.ToString();
                //order.Name = "Order " + i.ToString();
                //order.DateBegin = DateTime.Now;
                //order.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 30));
                order.Subject = subjectList[GetRandomIntegerFromInterval(1, subjectList.Count) - 1];

                OrderList.Add(order);

                //objectSpace.CommitChanges();
            }

            objectSpace.CommitChanges();

            // Заготавливаем maxNDSRateCount штук формул расчёта НДС
            int maxNDSRateCount = 1;
            IList<csNDSRate> NDSRateList = new List<csNDSRate>();

            for (int i = 0; i < maxNDSRateCount; i++) {
                csNDSRate ndsRate = objectSpace.CreateObject<csNDSRate>();

                ndsRate.Code = "20/120";  // + i.ToString();
                ndsRate.Numerator = 20;  // + i.ToString();
                ndsRate.Denominator = 120;  // + i.ToString();
                ndsRate.Name = "20/120";
                //ndsRate.RateOfNDS = 120/100;

                NDSRateList.Add(ndsRate);

                //objectSpace.CommitChanges();
            }

            objectSpace.CommitChanges();


            // Создаём maxStageCount этапов
            int maxStageCount = 20;
            IList<crmStage> stageList = new List<crmStage>();

            for (int i = 0; i < maxStageCount; i++) {
                crmDeliveryPlan deliveryPlan = objectSpace.CreateObject<crmDeliveryPlan>();
                deliveryPlan.DateBegin = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 5));
                deliveryPlan.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 15));
                deliveryPlan.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                deliveryPlan.CostModel = CostModelList[GetRandomIntegerFromInterval(1, CostModelList.Count) - 1];
                deliveryPlan.DeliveryMethod = DeliveryMethod.UNIT_AT_THE_END;
                deliveryPlan.NDSRate = NDSRateList[GetRandomIntegerFromInterval(1, NDSRateList.Count) - 1];
                deliveryPlan.Valuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];

                crmPaymentPlan paymentPlan = objectSpace.CreateObject<crmPaymentPlan>();
                paymentPlan.DateBegin = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 5));
                paymentPlan.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 15));
                paymentPlan.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                paymentPlan.CostModel = CostModelList[GetRandomIntegerFromInterval(1, CostModelList.Count) - 1];
                paymentPlan.NDSRate = NDSRateList[GetRandomIntegerFromInterval(1, NDSRateList.Count) - 1];
                paymentPlan.Valuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];

                crmStage stage = objectSpace.CreateObject<crmStage>();
                deliveryPlan.Stage = stage;
                stage.Code = "S" + i.ToString();
                //stage.DeliveryDate = null; // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 20));
                ////stage.CurrentCost = "Stg" + i.ToString();
                ////stage.CurrentPayment = "Stg" + i.ToString();
                //stage.Customer = null; // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                ////stage.DeliveryItem = "Stg" + i.ToString();
                ////stage.DeliveryItems = "Stg" + i.ToString();
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.DeliveryMethod = DeliveryMethod.UNIT_AT_THE_END;
                ////stage.DeliveryUnits = "Stg" + i.ToString();
                stage.DescriptionLong = "Stage" + i.ToString();
                stage.DescriptionShort = "Stg" + i.ToString();
                //stage.FullCode = "Stg" + i.ToString();
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.Order = OrderList[GetRandomIntegerFromInterval(1, OrderList.Count) - 1];
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.PaymentMethod = PaymentMethod.ADVANCE;
                stage.DeliveryPlan = deliveryPlan;  // ДОРАБОТАТЬ!!! "Stg" + i.ToString();
                stage.PaymentPlan = paymentPlan;  // ДОРАБОТАТЬ!!! "Stg" + i.ToString();
                stage.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                stage.CostModel = CostModelList[GetRandomIntegerFromInterval(1, CostModelList.Count) - 1];

                //stage.PaymentUnits = "Stg" + i.ToString();
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.PaymentValuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                //stage.Settlement = "Stg" + i.ToString();
                stage.SettlementDate = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 50));
                stage.StageStructure = null;  // ДОРАБОТАТЬ!!! "Stg" + i.ToString();
                //stage.StageMain = "Stg" + i.ToString();
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.StageType = StageType.FINANCE;
                //stage.Supplier = null; // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.Valuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];

                stage.DateFinish = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 40));
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.DateBegin = DateTime.Now;
                // ОТВАЛИВАЕТСЯ, ПОКА НЕТ ВРЕМЕНИ РАЗБИРАТЬСЯ !!! stage.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 30));

                stageList.Add(stage);
            }

            objectSpace.CommitChanges();

            // Создаём maxContractCategoryCount Категорий контрактов
            int maxContractCategoryCount = 10;
            IList<crmContractCategory> сontractCategoryList = new List<crmContractCategory>();

            for (int i = 0; i < maxContractCategoryCount; i++) {
                crmContractCategory contractCategory = objectSpace.CreateObject<crmContractCategory>();
                contractCategory.Code = "CC" + i.ToString();
                contractCategory.Name = "ContractCategory " + i.ToString();
                сontractCategoryList.Add(contractCategory);
            }

            objectSpace.CommitChanges();

            // Создаём maxDepartmentCount подразделений
            int maxDepartmentCount = 20;
            IList<hrmDepartment> departmentList = new List<hrmDepartment>();

            for (int i = 0; i < maxDepartmentCount; i++) {
                hrmDepartment department = objectSpace.CreateObject<hrmDepartment>();
                department.Code = "Dep" + i.ToString();
                department.Name = "Department " + i.ToString();
                departmentList.Add(department);
            }

            objectSpace.CommitChanges();

            // Создаём maxStaffCount сотрудников
            int maxStaffCount = 30;
            IList<hrmStaff> staffList = new List<hrmStaff>();
            IList<crmPhysicalPerson> physicalPersonList = new List<crmPhysicalPerson>();

            for (int i = 0; i < maxStaffCount; i++) {
                crmPhysicalPerson physicalPerson = objectSpace.CreateObject<crmPhysicalPerson>();
                physicalPerson.FirstName = "Иван " + i.ToString();
                physicalPerson.MiddleName = "Иванович " + i.ToString();
                physicalPerson.LastName = "Иванов " + i.ToString();
                physicalPerson.INN = "ИНН " + i.ToString();

                hrmStaff staff = objectSpace.CreateObject<hrmStaff>();
                staff.PhysicalPerson = physicalPerson;
                staff.Department = departmentList[GetRandomIntegerFromInterval(1, departmentList.Count) - 1];
                staffList.Add(staff);
            }

            // Создаём maxDocumentCategoryCount видов категорий документов
            int maxDocumentCategoryCount = 10;
            IList<crmContractDocumentType> contractDocumentTypeList = new List<crmContractDocumentType>();

            for (int i = 0; i < maxDocumentCategoryCount; i++) {
                crmContractDocumentType contractDocumentType = objectSpace.CreateObject<crmContractDocumentType>();
                contractDocumentType.Code = "CD " + i.ToString();
                contractDocumentType.Name = "Contract Document Type " + i.ToString();
                contractDocumentTypeList.Add(contractDocumentType);
            }

            objectSpace.CommitChanges();

            // Создаём maxPaymentItemCount
            int maxPaymentItemCount = 10;
            IList<crmPaymentMoney> paymentItemList = new List<crmPaymentMoney>();

            for (int i = 0; i < maxPaymentItemCount; i++) {
                crmPaymentMoney paymentItem = objectSpace.CreateObject<crmPaymentMoney>();
                paymentItem.AccountSumma = GetRandomIntegerFromInterval(1, 100);
                paymentItem.AccountValuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                //paymentItem.CostItem;
                //paymentItem.CostModel;
                //paymentItem.CurrentCost;
                paymentItem.Date = DateTime.Now;
                paymentItem.Description = "PaymentItem " + i.ToString();
                paymentItemList.Add(paymentItem);
            }

            objectSpace.CommitChanges();


            // Создаём maxObligationUnitCount обязательств
            int maxObligationUnitCount = 20;
            IList<crmObligationUnit> obligationUnitList = new List<crmObligationUnit>();

            for (int i = 0; i < maxObligationUnitCount; i++) {
                crmPaymentUnit obligationUnit = objectSpace.CreateObject<crmPaymentUnit>();
                obligationUnit.Code = "PU" + i.ToString();
                obligationUnit.Name = "PaymentUnit" + i.ToString();
                obligationUnit.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                obligationUnit.CostModel = CostModelList[GetRandomIntegerFromInterval(1, CostModelList.Count) - 1];
                ////obligationUnit.Creditor = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                //obligationUnit.CurrentCost = "Subj" + i.ToString();
                ////obligationUnit.Debitor = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                obligationUnit.NDSRate = NDSRateList[GetRandomIntegerFromInterval(1, NDSRateList.Count) - 1];
                obligationUnit.Order = OrderList[GetRandomIntegerFromInterval(1, OrderList.Count) - 1];
                obligationUnit.Receiver = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                obligationUnit.Sender = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                ////obligationUnit.Stage = stageList[GetRandomIntegerFromInterval(1, stageList.Count) - 1];
                ////obligationUnit.SummCost = GetRandomIntegerFromInterval(1, 100);
                ////obligationUnit.SummNDS = GetRandomIntegerFromInterval(1, 100);
                obligationUnit.SummFull = GetRandomIntegerFromInterval(1, 100);
                obligationUnit.Valuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                obligationUnit.DatePlane = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 40));
                obligationUnit.DateBegin = DateTime.Now;
                obligationUnit.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 30));

                obligationUnitList.Add(obligationUnit);
            }

            objectSpace.CommitChanges();

            //const int maxObligationUnitCount = 30;
            //IList<crmObligationUnit> obligationUnitList = new List<crmObligationUnit>();

            for (int i = 0; i < maxObligationUnitCount; i++) {
                crmDeliveryUnit obligationUnit = objectSpace.CreateObject<crmDeliveryUnit>();
                obligationUnit.Code = "DU" + i.ToString();
                obligationUnit.Name = "DeliveryUnit" + i.ToString();
                obligationUnit.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                obligationUnit.CostModel = CostModelList[GetRandomIntegerFromInterval(1, CostModelList.Count) - 1];
                ////obligationUnit.Creditor = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                //obligationUnit.CurrentCost = "Subj" + i.ToString();
                ////obligationUnit.Debitor = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                obligationUnit.NDSRate = NDSRateList[GetRandomIntegerFromInterval(1, NDSRateList.Count) - 1];
                if (OrderList.Count > 0) {
                    obligationUnit.Order = OrderList[GetRandomIntegerFromInterval(1, OrderList.Count) - 1];
                }
                obligationUnit.Receiver = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                obligationUnit.Sender = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1];
                ////obligationUnit.Stage = stageList[GetRandomIntegerFromInterval(1, stageList.Count) - 1];
                ////obligationUnit.SummCost = GetRandomIntegerFromInterval(1, 100);
                ////obligationUnit.SummNDS = GetRandomIntegerFromInterval(1, 100);
                obligationUnit.SummFull = GetRandomIntegerFromInterval(1, 100);
                obligationUnit.Valuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                obligationUnit.DatePlane = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 40));
                obligationUnit.DateBegin = DateTime.Now;
                obligationUnit.DateEnd = DateTime.Now.AddDays(GetRandomIntegerFromInterval(1, 30));

                obligationUnitList.Add(obligationUnit);
            }

            objectSpace.CommitChanges();

            /*
            // Заготавливаем maxPrimaryPartyRuCount штук первичных участников договоров
            int maxPrimaryPartyRuCount = 7;
            IList<crmPartyRu> primaryPartyRuList = new List<crmPartyRu>();

            for (int i = 0; i < maxPrimaryPartyRuCount; i++) {
                crmPartyRu partyRu = objectSpace.CreateObject<crmPartyRu>();

                partyRu.Code = "pF";
                partyRu.Description = "PrimaryParty " + i.ToString();

                //crmPhysicalPerson person = objectSpace.CreateObject<crmPhysicalPerson>();
                //person.INN = "PP INN" + i;
                //person.FirstName = "Ганс " + i;
                //person.MiddleName = "Христиан " + i;
                //person.MiddleName = "Андерсен " + i;

                crmCPerson person = objectSpace.CreateObject<crmCPerson>();
                person.INN = "PP INN" + i;
                //person.FirstName = "Ганс " + i;
                //person.MiddleName = "Христиан " + i;
                //person.MiddleName = "Андерсен " + i;

                partyRu.Person = person;
                person.Partys.Add(partyRu);

                primaryPartyRuList.Add(partyRu);
            }

            objectSpace.CommitChanges();
            */


            // Заготавливаем maxContragentcLegalPersonCount штук контрагентов
            int maxPrimaryCLegalPersonCount = 120;
            IList<crmCLegalPerson> PrimaryCLegalPersonList = new List<crmCLegalPerson>();

            for (int i = 0; i < maxPrimaryCLegalPersonCount; i++) {
                crmCLegalPerson cLegalPerson = objectSpace.CreateObject<crmCLegalPerson>();
                cLegalPerson.Name = "Кремль" + i;
                cLegalPerson.NameFull = "Кремль" + i;
                cLegalPerson.INN = "ИНН ААААААААА";
                cLegalPerson.Code = "LPK" + i;
                cLegalPerson.Description = "LegalPerson Кремль Description" + i;
                cLegalPerson.KPP = "КПП ББББББББ";
                cLegalPerson.RegCode = "RCK";
                //cLegalPerson.Person.Address = address;
                //cLegalPerson.Party.AddressFact = address;
                //cLegalPerson.Party.AddressPost = address;
                cLegalPerson.Party.Person = cLegalPerson.Person;

                PrimaryCLegalPersonList.Add(cLegalPerson);
            }

            objectSpace.CommitChanges();


            // Создаём maxContractCount контрактов
            int maxContractCount = 300;
            IList<crmContract> contractList = new List<crmContract>();

            for (int i = 0; i < maxContractCount; i++) {

                crmContractDocument contractDocument = objectSpace.CreateObject<crmContractDocument>();
                contractDocument.Date = DateTime.Now;
                contractDocument.DocumentCategory = contractDocumentTypeList[GetRandomIntegerFromInterval(1, contractDocumentTypeList.Count) - 1];
                contractDocument.Number = "Doc № " + i.ToString();


                crmContractDeal contractDeal = objectSpace.CreateObject<crmContractDeal>();
                contractDeal.Category = сontractCategoryList[GetRandomIntegerFromInterval(1, сontractCategoryList.Count) - 1];
                contractDeal.ContractKind = ContractKind.CONTRACT;
                contractDeal.CuratorDepartment = departmentList[GetRandomIntegerFromInterval(1, departmentList.Count) - 1];
                //contractDeal.Customer = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1].Party;
                contractDeal.DateRegistration = DateTime.Now;
                //contractDeal.Supplier = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1].Party;
                contractDeal.UserRegistrator = staffList[GetRandomIntegerFromInterval(1, staffList.Count) - 1];
                contractDeal.DepartmentRegistrator = contractDeal.UserRegistrator.Department;

                //contractDeal.Customer = ContragentPartyRuList[GetRandomIntegerFromInterval(1, ContragentPartyRuList.Count) - 1];
                //contractDeal.DealVersions
                //contractDeal.Project = ;
                //contractDeal.State
                //contractDeal.Supplier = ContragentPartyRuList[GetRandomIntegerFromInterval(1, ContragentPartyRuList.Count) - 1];

                
                //---
                crmDealVersion dealVersion = objectSpace.CreateObject<crmDealVersion>();
                //dealVersion.Category = сontractCategoryList[GetRandomIntegerFromInterval(1, сontractCategoryList.Count) - 1];
                //dealVersion.ContractDeal = contractDeal;
                //dealVersion.ContractDocument = contractDocument;
                //dealVersion.ContractDocuments.Add(contractDocument);
                //dealVersion.Customer = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1].Party;
                dealVersion.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                //dealVersion.Supplier = ContractPartyList[GetRandomIntegerFromInterval(1, ContractPartyList.Count) - 1].Party;
                dealVersion.CostModel = CostModelList[GetRandomIntegerFromInterval(1, CostModelList.Count) - 1];
                //dealVersion.Curator = contractDeal.CuratorDepartment;
                dealVersion.DateBegin = DateTime.Now;
                dealVersion.DateEnd = DateTime.Now.AddDays(10);
                dealVersion.DateFinish = DateTime.Now.AddDays(20);
                dealVersion.DealCode = "DC " + i.ToString();
                //dealVersion.DealDocument = contractDocument;
                //dealVersion.DealNomenclatures.Add(
                if (dealVersion != null) {
                    dealVersion.DescriptionLong = "Полное описание для " + dealVersion.DealCode;
                    dealVersion.DescriptionShort = "Краткое описание для " + dealVersion.DealCode;
                }
                dealVersion.NDSRate = NDSRateList[GetRandomIntegerFromInterval(1, NDSRateList.Count) - 1];
                if (OrderList.Count > 0) {
                    dealVersion.Order = OrderList[GetRandomIntegerFromInterval(1, OrderList.Count) - 1];
                }
                dealVersion.PaymentValuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                //dealVersion.Registrator = contractDeal.DepartmentRegistrator;
                dealVersion.Valuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                //dealVersion.StageStructure
                //---


                contractDeal.Current = dealVersion;
                contractDeal.DealVersions.Add(dealVersion);

                crmContract contract = objectSpace.CreateObject<crmContract>();
                contract.ContractCategory = сontractCategoryList[GetRandomIntegerFromInterval(1, сontractCategoryList.Count) - 1];

                contractDeal.Contract = contract;
                contractDeal.ContractDocument = contractDocument;
                contractDeal.ContractDocuments.Add(contractDocument);
                contractDocument.Contract = contract;
                contract.ContractDeals.Add(contractDeal);
                contract.ContractDocuments.Add(contractDocument);

                //contract.ContractDocument = DateTime.Now;
                //contract.Delo = "Delo " + i.ToString();
                contract.Description = "Описание документа " + i.ToString();
                contract.UserRegistrator = contractDeal.UserRegistrator;
                contract.DepartmentRegistrator = contract.UserRegistrator.Department;
                contractList.Add(contract);

                //this.ObjectSpace.CommitChanges();
                if ((i % 100) == 0) objectSpace.CommitChanges();
            }
            
            objectSpace.CommitChanges();




            // Создаём записи регистра crmDebtorCreditorDebtRegister
            for (int i = 0; i < RecordCountGenerated; i++) {

                //try {
                // crmDebtorCreditorDebtRegister
                crmDebtorCreditorDebtRegister debtorCreditorDebtRegister = objectSpace.CreateObject<crmDebtorCreditorDebtRegister>();

                debtorCreditorDebtRegister.Token = token;

                debtorCreditorDebtRegister.ContragentParty = CLegalPersonList[GetRandomIntegerFromInterval(1, CLegalPersonList.Count) - 1].Party;
                debtorCreditorDebtRegister.PrimaryParty = PrimaryCLegalPersonList[GetRandomIntegerFromInterval(1, PrimaryCLegalPersonList.Count) - 1].Party;

                debtorCreditorDebtRegister.Contract = contractList[GetRandomIntegerFromInterval(1, contractList.Count) - 1];
                debtorCreditorDebtRegister.ContractDeal = debtorCreditorDebtRegister.Contract.ContractDeals[0];

                debtorCreditorDebtRegister.CreditValuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                debtorCreditorDebtRegister.CreditCost = GetRandomIntegerFromInterval(1, 100);
                debtorCreditorDebtRegister.CreditCostInRUR = GetRandomIntegerFromInterval(1, 100);

                debtorCreditorDebtRegister.DebitValuta = debtorCreditorDebtRegister.CreditValuta;
                debtorCreditorDebtRegister.DebitCost = GetRandomIntegerFromInterval(1, 100);
                debtorCreditorDebtRegister.DebitCostInRUR = GetRandomIntegerFromInterval(1, 100);

                debtorCreditorDebtRegister.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                if (OrderList.Count > 0) {
                    debtorCreditorDebtRegister.fmOrder = OrderList[GetRandomIntegerFromInterval(1, OrderList.Count) - 1];
                }
                debtorCreditorDebtRegister.ObligationUnit = obligationUnitList[GetRandomIntegerFromInterval(1, obligationUnitList.Count) - 1];
                debtorCreditorDebtRegister.ObligationUnitDateTime = DateTime.Now;
                debtorCreditorDebtRegister.PlaneFact = ((i % 2) == 0) ? PlaneFact.PLAN : PlaneFact.FACT;
                debtorCreditorDebtRegister.Stage = stageList[GetRandomIntegerFromInterval(1, stageList.Count) - 1];
                //debtorCreditorDebtRegister.StageTech = ;
                if (subjectList.Count > 0) {
                    debtorCreditorDebtRegister.Subject = subjectList[GetRandomIntegerFromInterval(1, subjectList.Count) - 1];
                }

                //cashFlowRegister.Save();
                //objectSpace.CommitChanges();
                //} catch {
                //}
                if ((i % 1000) == 0) objectSpace.CommitChanges();
            }

            objectSpace.CommitChanges();


            // Создаём записи регистра crmCashFlowRegister
            for (int i = 0; i < RecordCountGenerated; i++) {

                //try {
                    // crmCashFlowRegister
                    crmCashFlowRegister cashFlowRegister = objectSpace.CreateObject<crmCashFlowRegister>();

                    cashFlowRegister.Token = token;

                    cashFlowRegister.ContragentParty = CLegalPersonList[GetRandomIntegerFromInterval(1, CLegalPersonList.Count) - 1].Party;
                    cashFlowRegister.PrimaryParty = PrimaryCLegalPersonList[GetRandomIntegerFromInterval(1, PrimaryCLegalPersonList.Count) - 1].Party;

                    cashFlowRegister.Contract = contractList[GetRandomIntegerFromInterval(1, contractList.Count) - 1];
                    cashFlowRegister.ContractDeal = cashFlowRegister.Contract.ContractDeals[0];   //cashFlowRegister.Contract.ContractDeals[0];
                    cashFlowRegister.Cost = GetRandomIntegerFromInterval(1, 100);
                    //cashFlowRegister.CostInRUR = GetRandomIntegerFromInterval(1, 100);
                    cashFlowRegister.SumIn = GetRandomIntegerFromInterval(1, 100);
                    cashFlowRegister.CostItem = costItemList[GetRandomIntegerFromInterval(1, costItemList.Count) - 1];
                    if (OrderList.Count > 0) {
                        cashFlowRegister.fmOrder = OrderList[GetRandomIntegerFromInterval(1, OrderList.Count) - 1];
                    }
                    if (obligationUnitList.Count > 0) {
                        cashFlowRegister.ObligationUnit = obligationUnitList[GetRandomIntegerFromInterval(1, obligationUnitList.Count) - 1];
                    }
                    cashFlowRegister.ObligationUnitDateTime = DateTime.Now;
                    cashFlowRegister.PaymentCost = GetRandomIntegerFromInterval(1, 100);
                    cashFlowRegister.PaymentItem = paymentItemList[GetRandomIntegerFromInterval(1, paymentItemList.Count) - 1];
                    cashFlowRegister.PaymentValuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];
                    //cashFlowRegister.PlaneFact = ((i % 2) == 0) ? PlaneFact.PLAN : PlaneFact.FACT;
                    cashFlowRegister.Section = CashFlowRegisterSection.CONTRACT_PLAN;   // Исправить на нормальное назначение
                    //cashFlowRegister.Stage = stageList[GetRandomIntegerFromInterval(1, stageList.Count) - 1];
                    //cashFlowRegister.StageTech = ;
                    if (subjectList.Count > 0) {
                        cashFlowRegister.Subject = subjectList[GetRandomIntegerFromInterval(1, subjectList.Count) - 1];
                    }
                    cashFlowRegister.Valuta = valutaList[GetRandomIntegerFromInterval(1, valutaList.Count) - 1];

                    //cashFlowRegister.FinancialDeal


                    //cashFlowRegister.Save();
                    //objectSpace.CommitChanges();
                //} catch {
                //}
                if ((i % 1000) == 0) objectSpace.CommitChanges();
            }

            objectSpace.CommitChanges();

        }



        private static Int32 GetRandomIntegerFromInterval(int minVal, int maxVal) {
            System.Random rand = new Random(DateTime.Now.Millisecond);
            return rand.Next(minVal, maxVal);
        }


#region Тестирование создания договороа без этапов
        [RequiresMTA]
        [Test, Description("Тестирование создания двух договоров без этапов")]
        //[Category("Производительность создания договоров")]
        [Category("Debug")]
        public void StartFillDBForAnalysisTest() {
            UnitOfWork uow1 = new UnitOfWork(Common.dataLayer);
            FillDatabase(uow1);
            uow1.CommitChanges();

            UnitOfWork uow = new UnitOfWork(Common.dataLayer);
            FillDatabaseForAnalysis(uow);
            uow.CommitChanges();
        }

        #endregion

    }
}
