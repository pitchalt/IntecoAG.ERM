*Action Navigation(Договоры.Новый договор...)

*FillForm
; Тип Рег. = По проекту
 Тип Рег. = Новый
 Рег.Польз. = Путин Владимир Владимирович

;*ExecuteEditorAction Док. Тип
;*ExecuteEditorAction Рег.Подр.

*ExecuteEditorAction Док. Тип
*Action New
*FillForm
 Код = Тип дог 1
 Наименование = Договор подряда
*Action Save and Close

*FillForm
 Новый Ном. = True
 Тип Формы = Простой договор
 Категория = Кат. дог. 2
 Кур.Подр. = ЦПП
 Стоимость = 1000000
 Валюта = Руб
 Дата с = 03.10.2011
 Дата по = 18.10.2011
 Дата Завер. = 28.10.2011
 Крат.Опис. = Договор о воздержании от договоров
 Орг.Роль = Заказчик
 Организация = ИнтекоАГ

; Временно убрано создание контрагента на ходу
;*ExecuteEditorAction Контрагент
;*Action New
;*FillForm
; Контрагент = Асфальто-бетонный завод
; Наименование стороны = Асфальто-бетонный завод
; Описание = Поставка бетона
;*Action Save and Close

 Контрагент = ИнтекоАГ

*Action Готово
