*Action Navigation(Contract.crm Contract Registration Form)
*FillForm
 New Number Required = True
 Price = 1000
 Category = КатДог 1
 Date Begin = 24.12.2011
 Date End = 28.12.2011
 Description Short = Опытный договор с этапами
 Kind Of Deal = DEAL_WITH_STAGE
 User Registrator = Медведев Дмитрий Анатольевич
 Our Role = CUSTOMER

; Valuta = EUR

*ExecuteEditorAction Valuta
*Action New
*FillForm
 Code = RUR
 Name Short = RUR
 Name Full = Рубль
*ExecuteEditorAction Base Unit
*Action New
*FillForm
 Code = Money
 Name = Money
 Description = Денежки
*Action Save and Close
*ExecuteEditorAction Cost Item
*Action New
*FillForm
 Code = Cost It 1
 Name = Cost Item 1
 Description = Cost Item 1
*Action Save and Close
*ExecuteEditorAction Nomenclature Type
*Action New
*FillForm
 Code = NomType1
 Name = Nomenclature type 1
*Action Save and Close
*Action Save and Close

*ExecuteEditorAction Curator Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = ЦПП

*ExecuteEditorAction Document Category
*Action New
*FillForm
 Code = CDT
 Name = Contract Doc Type 1
*Action Save and Close

*ExecuteEditorAction Partner Party
*Action New(crm CPhysical Party)
*Action CommonGroup
*FillForm
 First Name = Сурен
 Last Name = Гаспарян
 Middle Name = Автандилович
 Name Handmake = Гаспарян
 INN = 444444444

*ExecuteEditorAction Address Legal
*FillForm
 Region = Московский
 City = Москва
 Zip Postal = 111000
 State Province = ВАО
 Street = Купавинский проезд, 1
*Action OK
*ExecuteEditorAction Address Fact
*FillForm
 Region = Московский
 City = Москва
 Zip Postal = 111222
 State Province = СВАО
 Street = 1500-й проектируемый пр., 15
*Action OK
*Action Save and Close
*Action Save and Close
*Action Close

*Action Wizard Support Complete Action

*ReopenApplication IntecoAG.ERM.Win
*Action OK

*ProcessRecord 
 Категория = КатДог 1
*Action Stages
*Action Stages.New
*Action Common
*FillForm
 Description Short = First Stage
 Description Long = First Stage
 Code = FS1
 Cost Item = Ст ДДС 1
*Action Save and Close

*Action Approve
*HandleDialog
 Respond = Yes

*Action Create Version
*Action Common
*FillForm
 Date Finish = 31.12.2011

*Action Approve
*HandleDialog
 Respond = Yes