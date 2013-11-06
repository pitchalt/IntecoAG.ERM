using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.CS.Settings {

    /// <summary>
    /// Справочник "Кодов основания платежа"
    /// http://www.econ-profi.ru/index.php?area=1&p=static&page=inf_o_platezhe
    /// </summary>
    [Persistent("fmPaymentBase")]
    [NavigationItem("Settings")]
    //[NavigationItem("Settings.SettingsCommon")]
    public class  fmCPaymentBase : csCCodedComponent {
        public fmCPaymentBase(Session ses)
            : base(ses) {
        }

        [RuleUniqueValue(" fmCPaymentBase_Code", DefaultContexts.Save)]
        public override string Code {
            get {
                return base.Code;
            }
            set {
                base.Code = value;
            }
        }

    }
}


/*
Код основания платежа позволяет определить, на каком основании налог перечисляется в бюджетную систему Российской Федерации. Он указывается в поле 106 платежного поручения, состоит из двух знаков и означает:

«ТП» - платежи текущего года;
«ЗД» - добровольное погашение задолженности по истекшим налоговым периодам при отсутствии требования об уплате налогов (сборов) от налогового органа;
«БФ» - текущие платежи физических лиц - клиентов банка (владельцев счета), уплачиваемые со своего банковского счета;
«ТР» - погашение задолженности по требованию налогового органа об уплате налогов (сборов);
«PC» - погашение рассроченной задолженности;
«ОТ» - погашение отсроченной задолженности;
«РТ» - погашение реструктурируемой задолженности;
«ВУ» - погашение отсроченной задолженности в связи с введением внешнего управления;
«ПР» - погашение задолженности, приостановленной к взысканию;
«АП» - погашение задолженности по акту проверки;
«АР» - погашение задолженности по исполнительному документу. 



Код основания платежа   Дата документа

ТП 	Дата подписания декларации
ЗД 	0
ТР 	Дата требования налогового органа об уплате налогов (сборов)
РС 	Дата решения о рассрочке
ОТ 	Дата решения об отсрочке
РТ 	Дата решения о реструктуризации
ВУ 	Дата принятия арбитражным судом решения о введении внешнего управления
ПР 	Дата решения о приостановлении взыскания
АП 	Дата акта проверки
АР 	Дата исполнительного документа и возбужденного на основании его исполнительного производства
*/