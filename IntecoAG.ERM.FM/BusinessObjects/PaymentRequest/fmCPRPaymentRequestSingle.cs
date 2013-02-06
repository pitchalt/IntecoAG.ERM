using System;
using System.ComponentModel;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.PaymentRequest {

    /// <summary>
    /// Заявка на оплату по разовому счету
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCPRPaymentRequestSingle : fmCPRPaymentRequest
    {
        public fmCPRPaymentRequestSingle(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRPaymentRequestSingle);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Ответственный планового
        /// </summary>
        [PersistentAlias("FBKManager")]
        [RuleRequiredField]
        [DataSourceProperty("ManagerPlanDepartmentSource")]
        public hrmStaff PlanManager {
            get { return FBKManager; }
            set {
                hrmStaff old = FBKManager;
                if (old != value) {
                    FBKManager = value;
                    if (!IsLoading)
                        OnChanged("PlanManager", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerPlanDepartmentSource {
            get { return fmCSettingsFinance.GetInstance(this.Session).ManagerGroupOfPlanDepartmentStaffs; }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
