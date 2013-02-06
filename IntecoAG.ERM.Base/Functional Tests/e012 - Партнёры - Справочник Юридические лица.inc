*Action Navigation(Partners.crm CLegal Person)
*Action New
*FillForm
 Name = Асфальто-бетонный завод
*Action CommonGroup
*FillForm
 INN = 1111111111
 KPP = аааааааааа
 Reg Code = РК 1а
 Description = Асфальто-бетонный завод по производству асфальта и бетона, отгрохотки и прочего*ExecuteEditorAction Person Type
*ExecuteEditorAction Person Type
*Action New
*FillForm
 Code = ООО
 Name = Открытое Общество
*Action Save and Close
*ExecuteEditorAction Address Legal
*FillForm
 Region = Московский
 City = Москва
 Zip Postal = 101000
 State Province = ЮВАО
 Street = Мусорная, 100
*Action OK
;*Action Close
*ExecuteEditorAction Address Fact
*FillForm
 Region = Московский
 City = Москва
 Zip Postal = 101111
 State Province = ЗАО
 Street = Грязевая, 100
*Action OK
*Action Save and New


*FillForm
 Name = Завод по выделке кирпичей
*Action CommonGroup
*FillForm
 INN = 2222222222
 KPP = бббббббббб
 Reg Code = 2б
 Description = ИнтекоАГ
*ExecuteEditorAction Person Type
*Action New
*FillForm
 Code = ЗАО
 Name = ИнтекоАГ
*Action Save and Close
*ExecuteEditorAction Address Legal
*FillForm
 Region = Москва
 City = Москва
 Zip Postal = 333000
 State Province = Москва
 Street = Портовая, 1
*Action OK
*ExecuteEditorAction Address Fact
*FillForm
 Region = Дальний восток
 City = Владивосток
 Zip Postal = 676767
 State Province = Савёла
 Street = Бумажный проезд, 4
*Action OK
*Action Save and Close