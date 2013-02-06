*Action Navigation(System.crm User Party)
*Action New
*ExecuteEditorAction Party
*Action New
*FillForm
 Name = ИнтекоаАГ
 Name Full = ИнтекоАГ
 INN = 00000000000
 KPP = 1234567
 Description = Наша фирма
*ExecuteEditorAction Address Fact
*FillForm
 Region = Центральный
 City = Москва
 Zip Postal = 100000
 State Province = ЦАО
 Street = Правды, 29а
*Action OK
*ExecuteEditorAction Address Post
*FillForm
 Region = Центральный
 City = Москва
 Zip Postal = 100000
 State Province = ЦАО
 Street = Бумажный проезд, 4
*Action OK
*Action Save and Close
*Action Save and Close