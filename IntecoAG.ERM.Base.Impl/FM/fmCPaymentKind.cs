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
    /// Справочник "Типы платежей" (применяются при оформлении платёжек)
    /// http://www.econ-profi.ru/index.php?area=1&p=static&page=inf_o_platezhe
    /// </summary>
    [Persistent("fmPaymentKind")]
    [NavigationItem("Settings")]
    //[NavigationItem("Settings.SettingsCommon")]
    public class fmCPaymentKind : csCCodedComponent {
        public fmCPaymentKind(Session ses)
            : base(ses) {
        }

        [RuleUniqueValue("fmCPaymentKind_Code", DefaultContexts.Save)]
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
В поле 110 указывается типа платежа, который имеет два знака и может принимать следующие значения:

Код

Тип платежа
НС 	Налог или сбор
АВ 	Аванс или предоплата
ПЛ 	Платеж
ГП 	Пошлина
ВЗ 	Взнос
ПЕ 	Пеня
ПЦ 	Проценты
СА 	Налоговые санкции, установленные НК РФ
АШ 	Административные штрафы
ИШ 	Иные штрафы, установленные соответствующими законодательными или иными нормативными актами


Пример. Организация, применяющая упрощенную систему налогообложения, заполняет платежное поручение на перечисление авансового платежа.
Порядок заполнения поля 110:
в поле 110 указывается код авансового платежа – «АВ» (по окончании года при уплате единого налога следует внести код – «НС»).

Налогоплательщикам следует иметь в виду, что для каждого типа платежа нужно оформлять отдельное платежное поручение.
Стоит также отметить, что цифра в 14-м знаке кода КБК связана с типом платежа:

Коды типа платежа

                        14 знак КБК
НС - налог или сбор 	1
АВ - аванс или предоплата 	1
ПЛ - Платеж 	1
ГП - пошлина 	1
ВЗ - взнос 	1
ПЕ - пеня 	2
ПЦ - проценты 	2
СА - налоговые санкции, установленные НК РФ 	3
АШ - административные штрафы 	3
ИШ - иные штрафы, установленные соответствующими законодательными или иными нормативными актами 	3
*/