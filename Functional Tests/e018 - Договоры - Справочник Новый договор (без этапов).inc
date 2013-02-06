*Action Navigation(Contract.crm Contract Registration Form)
*FillForm
 User Registrator = Путин Владимир Владимирович
 New Number Required = True
 Kind Of Deal = DEAL_WITHOUT_STAGE
 Price = 1110
 Date Begin = 22.12.2011
 Date End = 31.12.2011
 Description Short = Тестовый контракт
 Our Role = CUSTOMER

*ExecuteEditorAction Document Category
*Action New
*FillForm
 Code = DocCat 1
 Name = Document Category 1
*Action Save and Close

*ExecuteEditorAction Curator Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = ЦПП

*ExecuteEditorAction Category
*Action New
*FillForm
 Code = ConCat 1
 Name = Contract Category 1
*Action Save and Close

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

*ExecuteEditorAction Partner Party
*Action New(crm CPhysical Party)
*Action CommonGroup
*FillForm
 First Name = Немцов
 Last Name = Джон
 Middle Name = Джеймсович
 Name Handmake = Немцов
 INN = 454567
 Description = Поставщик новаций
*ExecuteEditorAction Address Legal
*FillForm
 Region = Центральный
 City = Москва
 Zip Postal = 102100
 State Province = ЮАО
 Street = Никольская, 1
*Action OK
*ExecuteEditorAction Address Fact
*FillForm
 Region = Центральный
 City = Химки
 Zip Postal = 123456
 Street = Ленина, 5
*Action OK
*Action Save and Close

;*ExecuteEditorAction Our Party
;*Action New(crm Party Ru)
;*FillForm
; Name = Наше предприятие
; Name Full = Наше предприятие
; INN = 1111111
; KPP = 2222222
;*Action Save and Close

*Action Wizard Support Complete Action
*Action Save and Close

;*ReopenApplication IntecoAG.ERM.Win
