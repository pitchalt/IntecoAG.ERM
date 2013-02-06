using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.FM.PaymentRequest;
//

namespace IntecoAG.ERM.FM.Controllers {

    /// <summary>
    /// Данный контроллер добавляет к стандартной кнопке NEW пункт для создания Служебной записки по образцу
    /// </summary>
    public partial class fmCPRPaymentRequestMemorandumViewController : ObjectViewController {

        NewObjectViewController novc = null;
        PrintSelectionBaseController psbc = null;

        const String captionStandart = "Создать пустую служебную записку";

        const String idCustom = "NewByTemplate";
        const String captionCustom = "Создать Служебную записку по образцу";

        const String idSaveAsTemplate = "SaveAsTemplate";
        const String captionCustomAsTemplate = "Создать как шаблон";

        const String DO_ENABLED = "DO_ENABLED";
        const String DO_ACTIVE = "DO_ACTIVE";

        public fmCPRPaymentRequestMemorandumViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            novc = Frame.GetController<NewObjectViewController>();
            if (novc != null) {
                bool exists = false;
                for (int i = 0; i < novc.NewObjectAction.Items.Count; i++) {
                    if (novc.NewObjectAction.Items[i].Id == idCustom) {
                        exists = true;
                        break;
                    }
                }
                if (!exists && novc.NewObjectAction.Items.Count > 0) {
                    // Смена заголовка на кнопке создания пустой служебной записки
                    novc.NewObjectAction.Items[0].Caption = captionStandart;

                    // Создать по образцу
                    ChoiceActionItem NewByTemplate = new ChoiceActionItem(idCustom, captionCustom, novc.NewObjectAction.Items[0].Data);
                    NewByTemplate.ImageName = novc.NewObjectAction.Items[0].ImageName;
                    novc.NewObjectAction.Items.Add(NewByTemplate);
                    /*
                    // Сохранить как шаблон
                    ChoiceActionItem SaveTemplate = new ChoiceActionItem(idSaveAsTemplate, captionCustomAsTemplate, novc.NewObjectAction.Items[0].Data);
                    SaveTemplate.ImageName = novc.NewObjectAction.Items[0].ImageName;
                    novc.NewObjectAction.Items.Add(SaveTemplate);
                    */
                }
                novc.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);

                // Настройка способа реакции на кнопке
                novc.NewObjectAction.ShowItemsOnClick = !(novc.NewObjectAction.Items.Count < 2);
            }

            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);

            View.ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            View.ObjectSpace.ObjectSaved += new EventHandler<ObjectManipulatingEventArgs>(ObjectSpace_ObjectSaved);

            EnableButton();

            psbc = Frame.GetController<PrintSelectionBaseController>();
            if (psbc != null) {
                psbc.ShowInReportAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            }

            // Настройка фильтров
            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            //csCSecurityUser user = ObjectSpace.FindObject<csCSecurityUser>(new BinaryOperator("UserName", "PERSONAL\\12222"));

            user = ObjectSpace.GetObjectByKey<csCSecurityUser>(user.Oid);
            if ((View is ListView) & (View.ObjectTypeInfo.Type == typeof(fmPaymentRequestMemorandum))) {
                csCSecurityRole administratorRole = ObjectSpace.FindObject<csCSecurityRole>(new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName), true);
                CriteriaOperator criteriaOr = null;
                if (user != null && administratorRole != null) {
                    bool isAdmin = false;
                    foreach (var role in user.Roles) {
                        if (role.Name == SecurityStrategy.AdministratorRoleName) {
                            isAdmin = true;
                            break;
                        }
                    }
                    if (!isAdmin) {
//                    if (true) {
/*
                        // Список пользователей, ассоциированных с группами текущего пользователя
                        List<csCSecurityUser> userList = new List<csCSecurityUser>();
                        foreach (csCSecurityRole role in GetMainBuhRole(ObjectSpace).ChildRoles) {
                            if (user.Roles.IndexOf(role) != -1) {
                                foreach (SecurityUserWithRolesBase userOfGroupBase in role.Users) {
                                    csCSecurityUser userOfGroup = userOfGroupBase as csCSecurityUser;
                                    if (userOfGroup != null && !userList.Contains(userOfGroup)) {
                                        userList.Add(userOfGroup);
                                    }
                                }
                            }
                        }

                        // Список всех доступных служебных записок reqMemoListTotal
                        List<fmPaymentRequestMemorandum> reqMemoListTotal = new List<fmPaymentRequestMemorandum>();
                        foreach (csCSecurityUser person in userList) {
                            XPQuery<fmPaymentRequestMemorandum> RMs = new XPQuery<fmPaymentRequestMemorandum>(((ObjectSpace)ObjectSpace).Session);
                            List<fmPaymentRequestMemorandum> queryRM = (from rm in RMs
                                                                        where rm.Creator == person
                                                                           //&& user.Roles.IndexOf(rm.OwnerRole) != 0
                                                                        select rm).ToList();
                            foreach (fmPaymentRequestMemorandum rm in queryRM) {
                                if (user.Roles.IndexOf(rm.OwnerRole) != -1) {
                                    reqMemoListTotal.Add(rm);
                                }
                            }
                            //reqMemoListTotal.AddRange(queryRM);
                        }

                        Guid[] reqMemoIdListTotal = (from r in reqMemoListTotal
                                                     select r.Oid).ToArray();
*/
                        //CriteriaOperator UserHasAdminRole = CriteriaOperator.Parse("Creator.Roles[Name = 'Administrator'].Count() > 0");
                        //CriteriaOperator isTemplate = CriteriaOperator.Parse("State == 'TEMPLATE'");
                        criteriaOr = CriteriaOperator.Or
                            (
                                new BinaryOperator(new OperandProperty("FBKReceiver"), new ConstantValue(user.Staff), BinaryOperatorType.Equal)
                                ,new BinaryOperator(new OperandProperty("Creator"), new ConstantValue(user), BinaryOperatorType.Equal)
                                ,new BinaryOperator(new OperandProperty("Requester"), new ConstantValue(user.Staff), BinaryOperatorType.Equal)
                                ,new BinaryOperator(new OperandProperty("FirstSignaturePerson"), new ConstantValue(user.Staff), BinaryOperatorType.Equal)
                                ,new BinaryOperator(new OperandProperty("SecondSignaturePerson"), new ConstantValue(user.Staff), BinaryOperatorType.Equal)
                                //,isTemplate
                                , new InOperator("OwnerRole", fmCPRPaymentRequestBusinesLogic.GetActualRoles(ObjectSpace, user).ToArray())
                            );
                    }
                }
                ((ListView)View).CollectionSource.Criteria["MemorandumFilter"] = criteriaOr;
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e) {
            EnableButton();
        }

        private void ObjectSpace_ObjectSaved(object sender, ObjectManipulatingEventArgs e) {
            EnableButton();
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e) {
            EnableButton();
        }

        private void EnableButton() {
            if (View == null || View.CurrentObject == null || View.CurrentObject as fmCPRPaymentRequest == null)
                return;
            fmCPRPaymentRequest current = View.CurrentObject as fmCPRPaymentRequest;
            if (current.State != PaymentRequestStates.TEMPLATE) {
                this.CreateTemplate.Enabled[DO_ENABLED] = true;
            } else if (current.State == PaymentRequestStates.TEMPLATE) {
                this.CreateTemplate.Enabled[DO_ENABLED] = false;
            }

            // Кнопка Утвердить служебны. записку
            if (current.State != PaymentRequestStates.OPEN || current.State != PaymentRequestStates.REGISTERED) {
                this.ApproveMemorandum.Enabled[DO_ENABLED] = true;
                if (this.ObjectSpace.GetObjectsToSave(true).Count > 0) {
                    this.ApproveMemorandum.Enabled[DO_ENABLED] = false;
                }
            } else if (current.State == PaymentRequestStates.TEMPLATE) {
                this.ApproveMemorandum.Enabled[DO_ENABLED] = false;
            }

            // В связи с настройка триады кнопок перехода по бизнесс-процессу, две кнопки пока прячем, 
            // затем их надо будет удалить навсегда. Аминь.
            this.ApproveMemorandum.Active[DO_ACTIVE] = false;
        }

        protected override void OnDeactivated() {
            if (novc != null) {
                novc.NewObjectAction.Execute -= new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);
                for (int i = 0; i < novc.NewObjectAction.Items.Count; i++) {
                    if (novc.NewObjectAction.Items[i].Id == idCustom) {
                        novc.NewObjectAction.Items.Remove(novc.NewObjectAction.Items[i]);
                    }
                }
            }
            if (this.CreateTemplate.Enabled.Contains(DO_ENABLED)) {
                this.CreateTemplate.Enabled.RemoveItem(DO_ENABLED);
            }
            if (this.ApproveMemorandum.Enabled.Contains(DO_ENABLED)) {
                this.ApproveMemorandum.Enabled.RemoveItem(DO_ENABLED);
            }
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            
            View.ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            View.ObjectSpace.ObjectSaved -= new EventHandler<ObjectManipulatingEventArgs>(ObjectSpace_ObjectSaved);

            base.OnDeactivated();
        }
        
        void CustomNewActionController_Execute(object sender, ActionBaseEventArgs e) {
            //if (((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Id == idSaveAsTemplate) {
            //    e.;
            //}

            //if (((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Id == idCustom) {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                fmCPRPaymentRequestMemorandumCreator memorandumCreator = objectSpace.CreateObject<fmCPRPaymentRequestMemorandumCreator>();

                if (View.CurrentObject != null && View.CurrentObject as fmPaymentRequestMemorandum != null) {
                    fmPaymentRequestMemorandum rm = View.CurrentObject as fmPaymentRequestMemorandum;
                    fmPaymentRequestMemorandum rm1 = objectSpace.GetObject<fmPaymentRequestMemorandum>(rm);
                    memorandumCreator.MemorandumKind = rm1.MemorandumKind;
                    if (((SingleChoiceActionExecuteEventArgs)(e)).SelectedChoiceActionItem.Id == idCustom) {
                        memorandumCreator.RequestMemorandum = rm1;
                    }
                }

                string DetailViewId = Frame.Application.FindDetailViewId(memorandumCreator.GetType());

                //object passedMemorandumCreator = objectSpace.GetObject(memorandumCreator);

                TargetWindow openMode = TargetWindow.NewWindow;
                DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, memorandumCreator);

                ShowViewParameters svp = new ShowViewParameters() {
                    CreatedView = dv,
                    TargetWindow = openMode,
                    Context = TemplateContext.View,
                    CreateAllControllers = true
                };

                e.ShowViewParameters.Assign(svp);
            //}
        }

        // Создание шаблона из текущего документа
        private void CreateTemplate_Execute(object sender, SimpleActionExecuteEventArgs e) {
            /*
            if (View != null && View.CurrentObject != null && View.CurrentObject as fmPaymentRequestMemorandum != null) {
                IObjectSpace objectSpace = ObjectSpace.CreateNestedObjectSpace();   // Application.CreateObjectSpace();

                fmPaymentRequestMemorandum rm = View.CurrentObject as fmPaymentRequestMemorandum;
                fmPaymentRequestMemorandum rm1 = objectSpace.GetObject<fmPaymentRequestMemorandum>(rm);
                fmPaymentRequestMemorandum newMemoReqTemplate = rm1.CreateTemplate() as fmPaymentRequestMemorandum;

                string DetailViewId = Frame.Application.FindDetailViewId(newMemoReqTemplate.GetType());

                //object passedMemorandumCreator = objectSpace.GetObject(memorandumCreator);

                TargetWindow openMode = TargetWindow.NewModalWindow;
                DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, newMemoReqTemplate);

                ShowViewParameters svp = new ShowViewParameters() {
                    CreatedView = dv,
                    TargetWindow = openMode,
                    Context = TemplateContext.View,
                    CreateAllControllers = true
                };

                e.ShowViewParameters.Assign(svp);
            }
            */
            if (View != null && View.CurrentObject != null && View.CurrentObject as fmPaymentRequestMemorandum != null) {
                IObjectSpace objectSpace = Application.CreateObjectSpace();

                fmPaymentRequestMemorandum rm = View.CurrentObject as fmPaymentRequestMemorandum;
                fmPaymentRequestMemorandum rm1 = objectSpace.GetObject<fmPaymentRequestMemorandum>(rm);
                fmPaymentRequestMemorandum newMemoReqTemplate = rm1.CreateTemplate() as fmPaymentRequestMemorandum;

                string DetailViewId = Frame.Application.FindDetailViewId(newMemoReqTemplate.GetType());

                //object passedMemorandumCreator = objectSpace.GetObject(memorandumCreator);

                TargetWindow openMode = TargetWindow.NewModalWindow;
                DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewId, true, newMemoReqTemplate);

                ShowViewParameters svp = new ShowViewParameters() {
                    CreatedView = dv,
                    TargetWindow = openMode,
                    Context = TemplateContext.View,
                    CreateAllControllers = true
                };

                e.ShowViewParameters.Assign(svp);
            }

        }

        private void ApproveMemorandum_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View != null && View.CurrentObject as fmPaymentRequestMemorandum != null) {
                fmPaymentRequestMemorandum rm = View.CurrentObject as fmPaymentRequestMemorandum;
                if (this.ObjectSpace.GetObjectsToSave(true).Count == 0) {
                    rm.State = PaymentRequestStates.IN_BUDGET;
                    //EnableButton();
                }
            }
        }

        /// <summary>
        /// Получение головной группы бухгалтерии, содержащей группы простые группы сотрудников бухгалтерии
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static csCSecurityRole GetMainBuhRole(IObjectSpace os) {
            csCSecurityRole mainBuhRole = fmCSettingsFinance.GetInstance(((ObjectSpace)os).Session).MainBuhRole;
            if (mainBuhRole == null) {
                throw new Exception("Main role for buh. groups is not defined. See Settings --> Finance setting.");
            }
            return mainBuhRole;
        }

    }
}
