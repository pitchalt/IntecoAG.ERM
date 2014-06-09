using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//using DevExpress.Persistent.Base.Security;
//
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.CRM;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.XAFExt;
using IntecoAG.ERM.Module.ReportHelper;
using IntecoAG.ERM.Trw;
//
namespace IntecoAG.ERM.Module {
    /// <summary>
    /// Паша!!! Очень большой вопрос с генерацией исключений при обновлении данных
    /// </summary>
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
                // Добавление отчётов
                CheckDirectoryOnReports();
                // Добавление аналитических данных
                CreateDataToBeAnalysed();
                //
                UpdateAdminRole();
                UpdateNullRightRole();
                UpdateReadAllRole();
                UpdateEditAllRole();
                //
                fmCSettingsFinance.GetInstance(((ObjectSpace)ObjectSpace).Session);
                crmCSettingsContract.GetInstance(((ObjectSpace)ObjectSpace).Session);
                TrwSettings.GetInstance(ObjectSpace);
                //
                //UpdateParty();
                //UpdateStaff();
        }
        private void UpdateStaff() {
            IList<hrmStaff> staffs = ObjectSpace.GetObjects<hrmStaff>(
                CriteriaOperator.Or(
                    new UnaryOperator(UnaryOperatorType.IsNull, "BuhCode"),
                    new BinaryOperator("BuhCode", "")
                ));
            foreach (hrmStaff staff in staffs) {
                staff.IsClosed = true;
            }
            IList<crmContract> contracts = ObjectSpace.GetObjects<crmContract>(new BinaryOperator("UserRegistrator.IsClosed",true),true);
            foreach (crmContract contract in contracts) {
                String last_name = contract.UserRegistrator.LastName.Split(' ')[0];
                IList<hrmStaff> staff = ObjectSpace.GetObjects<hrmStaff>(CriteriaOperator.And(
                    new BinaryOperator("LastName", last_name),
                    new BinaryOperator("FirstName", contract.UserRegistrator.FirstName),
                    new BinaryOperator("Department.BuhCode", "560"),
                    CriteriaOperator.Or(
                        new BinaryOperator("IsClosed", false),
                        new UnaryOperator(UnaryOperatorType.IsNull, "IsClosed")
                    )), true);
                if (staff.Count == 1) {
                    contract.UserRegistrator = staff[0];
                }
                else
                    continue;
            }
            IList<crmContractDeal> deals = ObjectSpace.GetObjects<crmContractDeal>(new BinaryOperator("UserRegistrator.IsClosed", true), true);
            foreach (crmContractDeal deal in deals) {
                String last_name = deal.UserRegistrator.LastName.Split(' ')[0];
                IList<hrmStaff> staff = ObjectSpace.GetObjects<hrmStaff>(CriteriaOperator.And(
                    new BinaryOperator("LastName", last_name),
                    new BinaryOperator("FirstName", deal.UserRegistrator.FirstName),
                    new BinaryOperator("Department.BuhCode", "560"),
                    CriteriaOperator.Or(
                        new BinaryOperator("IsClosed", false),
                        new UnaryOperator(UnaryOperatorType.IsNull, "IsClosed")
                    )), true);
                if (staff.Count == 1) {
                    deal.UserRegistrator = staff[0];
                }
                else
                    continue;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateParty() {
            IList<crmCParty> partys = ObjectSpace.GetObjects<crmCParty>(CriteriaOperator.Parse("Person is Null"));
            foreach (crmCParty party in partys) {
                if (party.ComponentType == typeof(crmCLegalPerson)) {
                    crmCLegalPerson lp = (crmCLegalPerson) party.ComponentObject;
                    party.Person = lp.Person;
                }
                if (party.ComponentType == typeof(crmCLegalPersonUnit)) {
                    crmCLegalPersonUnit lpu = (crmCLegalPersonUnit)party.ComponentObject;
                    if (lpu.LegalPerson != null)
                        party.Person = lpu.LegalPerson.Person;
                }
                if (party.ComponentType == typeof(crmCPhysicalParty)) {
                    crmCPhysicalParty php = (crmCPhysicalParty)party.ComponentObject;
                    if (php.Person != null)
                        party.Person = php.Person.Person;
                }
                if (party.ComponentType == typeof(crmCBusinessman)) {
                    crmCBusinessman bm = (crmCBusinessman)party.ComponentObject;
                    if (bm.Person != null)
                        party.Person = bm.Person.Person;
                }
            }
        }

        /// <summary>
        /// Проверяется директория запуска и её поддиректория Reports, если таковая существует
        /// </summary>
        private void CheckDirectoryOnReports() {
            // Директория запуска
            string fname = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(fname);

            string checkDir1 = fi.Directory.FullName;
            string checkDir2 = checkDir1 + "\\ReportLayouts";

            ReportHelper.ReportHelper.GetAllReportsFromDirectory(checkDir1, ObjectSpace);
            ReportHelper.ReportHelper.GetAllReportsFromDirectory(checkDir2, ObjectSpace);
        }

        // ANALYSIS & PIVOT
        // http://documentation.devexpress.com/#Xaf/CustomDocument3050   "Distribute the Created Analysis with the Application"
        // По указанному адресу находится описание приёмов как сохранить Layout (располжение полей и т.п.) и восстанавливать его при последующих запусках
        private void CreateDataToBeAnalysed() {
            Analysis taskAnalysis1 = ObjectSpace.FindObject<Analysis>(CriteriaOperator.Parse("Name='Анализ движения денег'"));
            if (taskAnalysis1 == null) {
                taskAnalysis1 = ObjectSpace.CreateObject<Analysis>();
                taskAnalysis1.Name = "Анализ движения денег";
                taskAnalysis1.ObjectTypeName = typeof(crmPaymentPlan).FullName;

                // DataType не будет отображаться в выпадающем списке в дизайнере, возможная причина - этот тип исключён из пунктов навигатора.
                taskAnalysis1.DataType = typeof(crmPaymentPlan);

                taskAnalysis1.Criteria = null;   // "[DueDate] < '@CurrentDate' and [Status] = 'Completed'";
                taskAnalysis1.Save();
            }
        }
        /// <summary>
        /// Обновим права администратора, для политики Windows Autentication пользователь с административными 
        /// правами создается автоматически, а вот список прав не обновляется
        /// Паша!!! Реализация не учитывает вариантов в системе безопасности и использует стандартный класс роли
        /// или его производные
        /// </summary>
        private void UpdateAdminRole() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                csCSecurityRole administratorRole = os.FindObject<csCSecurityRole>(
                    new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName), true);
                if (administratorRole == null) {
                    administratorRole = os.CreateObject<csCSecurityRole>();
                    administratorRole.Name = SecurityStrategy.AdministratorRoleName;
                    ModelOperationPermissionData modelPermission =
                        os.CreateObject<ModelOperationPermissionData>();
                    administratorRole.PersistentPermissions.Add(modelPermission);
                }
                administratorRole.BeginUpdate();
                administratorRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Read);
                administratorRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Write);
                administratorRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Create);
                administratorRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Delete);
                administratorRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Navigate);
                administratorRole.EndUpdate();
                if (administratorRole.Users.Count == 0) {
                    // Паша !!! Неустойчивый вариант, нужен код определяющий тип User по конфигу Application
                    csCSecurityUser user = os.FindObject<csCSecurityUser>(
                            new BinaryOperator("UserName", ConfigurationManager.AppSettings["DefaultAdminName"]));
                    if (user != null) {
                        user.Roles.Add(administratorRole);
                    }
                }
                os.CommitChanges();
            }
        }
        private void UpdateNullRightRole() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                csCSecurityRole NullRightRole = os.FindObject<csCSecurityRole>(
                    new BinaryOperator("Name", ConfigurationManager.AppSettings["SecurityGroups.NullRightRole"]), true);
                if (NullRightRole == null) {
                    NullRightRole = os.CreateObject<csCSecurityRole>();
                    NullRightRole.Name = ConfigurationManager.AppSettings["SecurityGroups.NullRightRole"];
                }
                NullRightRole.BeginUpdate();
                //
                NullRightRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Read);
                NullRightRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Navigate);
                NullRightRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Write);
                NullRightRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Create);
                NullRightRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Delete);
                //
                NullRightRole.EndUpdate();
                os.CommitChanges();
            }
        }
        private void UpdateReadAllRole() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                csCSecurityRole ReadAllRole = os.FindObject<csCSecurityRole>(
                    new BinaryOperator("Name", ConfigurationManager.AppSettings["SecurityGroups.ReadAllRole"]), true);
                if (ReadAllRole == null) {
                    ReadAllRole = os.CreateObject<csCSecurityRole>();
                    ReadAllRole.Name = ConfigurationManager.AppSettings["SecurityGroups.ReadAllRole"];
                }
                ReadAllRole.BeginUpdate();
                //
                ReadAllRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Read);
                ReadAllRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Navigate);
                ReadAllRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Write);
                ReadAllRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Create);
                ReadAllRole.Permissions.DenyRecursive(typeof(object), SecurityOperations.Delete);
                //
                ReadAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityRole), SecurityOperations.Read);
                ReadAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityRole), SecurityOperations.Navigate);
                //
                ReadAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityUser), SecurityOperations.Read);
                ReadAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityUser), SecurityOperations.Navigate);
                //
                ReadAllRole.EndUpdate();
                os.CommitChanges();
            }
        }
        private void UpdateEditAllRole() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                csCSecurityRole EditAllRole = os.FindObject<csCSecurityRole>(
                    new BinaryOperator("Name", ConfigurationManager.AppSettings["SecurityGroups.EditAllRole"]), true);
                if (EditAllRole == null) {
                    EditAllRole = os.CreateObject<csCSecurityRole>();
                    EditAllRole.Name = ConfigurationManager.AppSettings["SecurityGroups.EditAllRole"];
                }
                EditAllRole.BeginUpdate();
                //
                EditAllRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Read);
                EditAllRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Write);
                EditAllRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Create);
                EditAllRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Delete);
                EditAllRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.Navigate);
                //
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityRole), SecurityOperations.Read);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityRole), SecurityOperations.Write);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityRole), SecurityOperations.Create);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityRole), SecurityOperations.Delete);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityRole), SecurityOperations.Navigate);
                //
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityUser), SecurityOperations.Read);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityUser), SecurityOperations.Write);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityUser), SecurityOperations.Create);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityUser), SecurityOperations.Delete);
                EditAllRole.Permissions.DenyRecursive(typeof(IntecoAG.ERM.CS.Security.csCSecurityUser), SecurityOperations.Navigate);
                //
                EditAllRole.EndUpdate();
                os.CommitChanges();
            }
        }
    }
}
