*Action Navigation(Partners.Физическое лицо)
*Action New(Физическое лицо)
*FillForm
 Last Name = Иванов
 First Name = Иван
 Middle Name = Иванович
*ExecuteEditorAction Address
*FillForm
 Country  = RUS
 Zip Postal = 100000
 Region = Московский
 State Province = Московская провинция
 City = Москва
 Street = Профсоюзная, 1
 Address String = стр. 1
*Action OK
*FillForm
 INN = 1234567890
*Action Save and New(Физическое лицо)
*FillForm
 Last Name = Петров
 First Name = Пётр
 Middle Name = Петрович
*ExecuteEditorAction Address
*FillForm
 Country  = RUS
 Zip Postal = Петербург
 Region = Северо-западный
 State Province = Московская провинция
 City = Санкт-Петербург
 Street = Невский, 5
; Address String = 
*Action OK
*Action Save and New(Физическое лицо)
*FillForm
 Last Name = Сидоров
 First Name = Сидор
 Middle Name = Сидорович
*ExecuteEditorAction Address
*FillForm
 Country  = RUS
 Zip Postal = 143900
 Region = Центральный
 State Province = Московская
 City = Балашиха
 Street = Флёрова, 1
; Address String = 
*Action OK
*Action Save
*Action Save and Close
