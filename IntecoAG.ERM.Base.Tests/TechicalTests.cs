using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Xpo.Metadata;
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

    // Технические тесты для исследования различных аспектов работы XPO и XAF
    // Для тестов используются элементы IntecoAG.ERM

    [TestFixture, Description("Проверка создания простого договора без этапов")]
    public class TecnicalTests : DealBaseTest
    {
        public override void Init() {
            base.Init();

            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }

        #region Тестирование создания договороа без этапов

        [Test, Description("Тестирование передачи объектов между UnitOfWork")]
        [Category("Debug")]
        public void CreateAllReferencesOnlyTest() {
            UnitOfWork uow = new UnitOfWork(Common.dataLayer);

            csCountry country = Prepare_csCountry(uow, "");
            csAddress address = Prepare_csAddress(uow, "", country);

            csCountry country1 = Prepare_csCountry(uow, "1");
            csAddress address1 = Prepare_csAddress(uow, "1", country1);

            csCountry country2 = Prepare_csCountry(uow, "2");
            csAddress address2 = Prepare_csAddress(uow, "2", country2);

            csCountry country3 = Prepare_csCountry(uow, "3");
            csAddress address3 = Prepare_csAddress(uow, "3", country3);

            csCountry country4 = Prepare_csCountry(uow, "4");
            csAddress address4 = Prepare_csAddress(uow, "4", country4);

            uow.CommitChanges();

            // Изменение какого-либо поля. Если вложенный uow читал бы снова из БД, то это изменение было бы проигнорировано
            address.Region = "ХРЕНОВЫЙ РЕГИОН";

            // Организуем коллецию
            XPCollection<csAddress> addressCoollection = new XPCollection<csAddress>(uow);
           

            NestedUnitOfWork nuow = uow.BeginNestedUnitOfWork();

            XPCollection<csAddress> nestedAddressCoollectionBEFORECHANGE = new XPCollection<csAddress>(nuow);

            csAddress nestedAddress = nuow.GetObjectByKey<csAddress>(address.Oid);
            nestedAddress.City = "Абракадабра";

            XPCollection<csAddress> nestedAddressCoollectionAFTERCHANGE = new XPCollection<csAddress>(nuow);

            
            XPClassInfo addressClassInfo = uow.GetClassInfo(typeof(csAddress));
            //var AddrKeyCollection = from addr in addressCoollection select addr.Oid;
            List<System.Guid> AddrKeyCollection = new List<System.Guid>();
            foreach (csAddress addr in addressCoollection) {
                ((IList)AddrKeyCollection).Add(addr.Oid);
            }

            //ICollection nestedAddressCoollectionFromDB = nuow.GetObjectsByKey(addressClassInfo, AddrKeyCollection as IList, true);
            // И/ИЛИ :
            ICollection nestedAddressCoollectionFromDS = nuow.GetObjectsByKey(addressClassInfo, AddrKeyCollection as IList, false);
            

            string xxx = nuow.ToString();

            nuow.CommitChanges();

            uow.CommitChanges();



            // **********************************************************************************************************************
            /*

            1. После создания вложенного uow в нём ничего нет. Если же csAddress nestedAddress = nuow.GetObjectByKey<csAddress>(address.Oid);
            то во вложенном uow окажется не только объект адрес, но и объект страна, которая является свойством объекта адреса

            2. При измеениее объекта во вложенном uow он в родительском не меняется, пока изменения не утверждены.

            3. При получении объекта, находящегося в корневм uow, во вложенном, объект оказывается во вложенном uow со всеми своими изменениями,
            сделанными в корневом uow, даже если они не утверждены.

            Чтение и передача коллекций.
             
            4. Коллекция во вложенном uow может быть прочитана в двух режимах: из БД и из datastore. В любом случае изменение сделанное
            с объектом в корневом uow будет присутствовать в объекте из коллекции во вложенном uow (из-за кэширования, т.е. реальное чтение 
            БД не происходит в данном контрольном примере). Изменениеже сделанное в объекте в рамках вложенного uow будет учтено только в 
            режиме чтения datastore (т.е. опять же чтение будет прроисходить из кэша, где уже произведено изменение данных).
            
            5. Вложенный uow после создания не содержит объектов. Объекты могут попасть во вложенный uow путём передачи из корневого со всеми
            изменениями, сделанными в корневом uow.
            */
            // **********************************************************************************************************************


        }

        #endregion

    }
}
