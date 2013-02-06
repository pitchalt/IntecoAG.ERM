*Action Navigation(Contract.crm Contract Registration Form)
*FillForm
 User Registrator = Медведев Дмитрий Анатольевич
*ExecuteEditorAction Document Category
*Action New
*FillForm
 Code = Doc Type 1
 Name = Document Type 1
*Action Save and Close
*FillForm
 New Number Required = True
 Kind Of Deal = DEAL_WITH_STAGE
 Category = Кат. дог. 1
 Curator Department = ЦПП
 Price = 1110
 Valuta = Руб
 Date Begin = 19.10.2011
 Description Short = Договор с этапами 1
 Our Role = CUSTOMER
 Our Party = ИнтекоАГ
 Partner Party = ИнтекоАГ
*Action Wizard Support Complete Action

*Action Common
*FillForm
 Cost Model = Мод цены 1
*ExecuteEditorAction NDSRate
*Action New
*FillForm
 Numerator = 20
 Denominator = 100
 Code = 20/100
 Name = 20/100
*Action Save and Close
*FillForm
 Cost Item = Ст ДДС 1
 Description Long = Догоовр с этапами
*Action Stages
*Action Stages.New
*FillForm
 Stage Type = AGREGATE
 Code = AGR 1
*Action Common
*FillForm
 Description Short = ОБщий этап 1
 Description Long = Общие данные
;*Action Validate
*Action Save and Close
*Action Stages.New
*FillForm
 Stage Type = FINANCE
 Code = FIN 1
*Action Common
*FillForm
 Description Short = Этап для учёта карбованцев
 Description Long = Этап для активизации армии финансистов и бухгалтеров
*Action Save and Close
*Action Stages.New
*FillForm
 Stage Type = AGREGATE
 Code = TECH 1
*Action Common
*FillForm
 Description Short = Мелкие технические манипуляции 1
 Description Long = Этап для полёта фантазии инженеров 1
*Action Save and Close
*Action Stages.New
*FillForm
 Stage Type = AGREGATE
 Code = TECH 2
*Action Common
*FillForm
 Description Short = Технические специалисты не нарадуются
 Description Long = Задействование технических гениев.
*Action Save and Close
*Action Save
*Action Save and Close
*ProcessRecord 
 Состояние = DEAL_PROJECT
*Action Approve
*HandleDialog
 Respond = Yes