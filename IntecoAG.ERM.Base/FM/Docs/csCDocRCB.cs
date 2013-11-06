using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.FM.Docs {

    // Так как платёжки все почти одинаковы, то этот класс содержит общую их часть

    // Описание смысла полей см. http://sprbuh.systecs.ru/uchet/bank/platezhnoe_poruchenie.html

    // Правила редактирования платёжки.
    // 0. Редактирование платёжки предполагает редактирование любого её поля. Таким образом, достаточно одной кнопки на форме, 
    //    позволяющей начать редактирование.
    // 1. Если платёжка только что создана, то платёжка доступна для редактирования.
    // 2. Если платёжка загружена в систему извне, то при её открытии в системе, она недоступна для редактирования.
    // 3. Если платёжка образована путём копирования другой платёжки, то при первом открытии она доступна для редактирования.
    // 4. Если платёжка создана в системе и не утверждена, то при каждом её открытии, она недоступна для редактирования.

    // 5. Утоверждённая платёжка никаким образом не может быть отредактирована (недоступны поля и недоступна кнопка редактирования). Утверждённая платёжка имеет значением
    //    ReadOnly = true
    // 6. Во всех прочих случаях платёжка может быть отредактирована после нажития кнопки редактирования на форме.
    // 7. Если реквизиты плательщика/получателя уже введены, а пользователь меняет выбор партнёра или его расчётный счёт, то реквизиты обновляются.
    // 8. Подтверждением изменений является сохранение формы.

    //[Appearance("fmCDocRCBPaymentOrder.PaymentPayer.NameParty.Enabled", AppearanceItemType = "LayoutItem", TargetItems = "PaymentPayer.NameParty", Criteria = "0=1", Visibility = ViewItemVisibility.Show, Enabled = true)]
    //[Appearance("fmCDocRCBPaymentOrder.PaymentPayer.NameParty.Enabled", AppearanceItemType = "LayoutItem", TargetItems = "DocNumber", Method = "AllowEditPayer", Enabled = false)]
    //[Appearance("fmCDocRCBPaymentOrder.PaymentPayer.NameParty.Enabled", AppearanceItemType = "LayoutItem", TargetItems = "DocNumber", Criteria = "False", Enabled = false)]

    /// <summary>
    /// Статусы заявки
    /// </summary>
    public enum PaymentDocProcessingStates {
        /// <summary>
        /// Загружен в систему
        /// </summary>
        LOAD = 1,
        /// <summary>
        /// Импортирован из выписки
        /// </summary>
        IMPORTED = 2,
        /// <summary>
        /// Неопознанный платёж
        /// </summary>
        UNKNOWN = 3,
        /// <summary>
        /// Проработан
        /// </summary>
        PROCESSED = 4
    }

    [NavigationItem("Money")]
    [Persistent("fmDocRCB")]
    [DefaultProperty("DocRCBName")]
    public class csCDocRCB : csCComponent
    {
        public csCDocRCB(Session session)
            : base(session) {
        }

        #region ПОЛЯ КЛАССА

        private String _DocNumber; // номер документа
        private DateTime _DocDate; // дата документа

        #endregion

        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// Имя для отображения в списке
        /// </summary>
        public String DocRCBName {
            get {
                return DocNumber + " от " + DocDate.ToString("dd.MM.yyyy");
            }
        }
        /// <summary>
        /// Номер платёжного документа
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.DocNumber.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[RuleRequiredField]
        [Size(300)]
        public String DocNumber {
            get { return _DocNumber; }
            set {
                SetPropertyValue<String>("NameParty", ref _DocNumber, value == null ? String.Empty : value.Trim()); }
        }

        /// <summary>
        /// Дата платежа
        /// </summary>
        //[RuleRequiredField]
        public DateTime DocDate {
            get { return _DocDate; }
            set {
                SetPropertyValue<DateTime>("DocDate", ref _DocDate, value);
            }
        }

        #endregion

        #region МЕТОДЫ
        #endregion

    }

}
