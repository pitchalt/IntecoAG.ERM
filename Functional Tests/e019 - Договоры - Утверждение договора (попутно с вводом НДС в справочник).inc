*Action Navigation(Contract.crm Contract Deal)

*ProcessRecord 
 Док.Наим. = Тип дог 1 № 1/******-2011 от 19.10.2011

;*Sleep 10000

;*Action Mini Navigation Action(Действующая редакция)
*Action Common
*FillForm
 Deal Code = Сделка 1
 Cost Item = Ст ДДС 1
 Description Long = Опытный договор без этапов
*Action Validate
*Action Close
*Action Obligations
*ExecuteEditorAction NDSRate
*Action New
*FillForm
 Numerator = 18
 Denominator = 118
 Code = 18/118
 Name = 18/118
*Action Save and Close
*FillForm
 Cost Model = Мод цены 1

;*Sleep 10000


*Action Approve
*HandleDialog
 Respond = Yes

*Action Close
