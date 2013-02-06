*Action Navigation(System.crm User Party)
*Action New
*ExecuteEditorAction Party
*Action New
*FillForm
 Name = ИнтекоАГ
 Name Full = ИнтекоАГруп
 INN = 0000000
 KPP = 8888888
 Description = Наше родное предприятие
*ExecuteEditorAction Address Fact
*ExecuteEditorAction Country
*Action New
*FillForm
 Name En Short Low = Ru
 Name En Full = Russia
 Name Ru Short Low = Рус
 Name Ru Full = РОССИЯ
 Code Alfa 2 = RU
 Code Alfa 3 = Rus
 Code Ru Alfa 3 = Рос
 Code Numeric = 001
 Comment = Россия
*Action Save and Close
*FillForm
 Region = Центральный
 City = Москва
 Zip Postal = 100001
 State Province = ЦАО
 Street = Правды, 29
*Action OK
*ExecuteEditorAction Address Post
*FillForm
 Country = Рус
 Region = Центральный
 City = Москва
 Zip Postal = 100001
 State Province = ЦАО
 Street = Правды, 29
*Action OK
*Action Save and Close
*Action Save and Close

;*Sleep 10000
