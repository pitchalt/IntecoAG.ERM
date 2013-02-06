*Action Navigation(Договоры.Простые Договора)

*ProcessRecord 
 Name = Тип дог 1 № 1/******-2011 от 14.10.2011

;*Sleep 10000

*Action Mini Navigation Action(Действующая редакция)
;*Action Общие сведения
*Action Common
*FillForm
; Deal Code = Сделка 2
 Код Ведомости = Сделка 2
*Action Validate
*Action Close
;*Action Обязательства
*Action Obligations
*FillForm
 Модель цены = Мод цены 2

;*Sleep 10000

;*ExecuteEditorAction NDSRate
*ExecuteEditorAction Ставка НДС
*Action New
*FillForm
 Numerator = 18
 Denominator = 118
 Code = 20%
 Name = 20 процентов
 Code = 18/118
*Action Save and Close
*Action Save and Close
