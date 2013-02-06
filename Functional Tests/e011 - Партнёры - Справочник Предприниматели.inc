*Action Navigation(Partners.Предприниматель)
*Action New(Предприниматель)
*FillForm
 Physical Person = Иванов Иван Иванович
*ExecuteEditorAction Address
*FillForm
 Country  = RUS
 Zip Postal = 111000
 Region = Центральный
 State Province = Московская провинция
 City = Москва
 Street = Ленинский проспект, 4
; Address String = 
*Action OK
*FillForm
 INN = 1234567
 Name = Рыночник Иванов
 Reg Number = Рег Ном 1
 Name Full = Иванов & Ко
*Action Save and New(Предприниматель)
*FillForm
 Physical Person = Петров Пётр Петрович
*ExecuteEditorAction Address
*FillForm
 Country  = RUS
 Zip Postal = 122000
 Region = Центральный
 State Province = Московская провинция
 City = Москва
 Street = Зелёный проспю, 2
; Address String = 
*Action OK
*FillForm
 INN = 33333333333333
 Name = Бизнесмен Петров
 Name Full = Петров & Ко
 Reg Number = ааа111
*Action Save
*Action Save and Close
