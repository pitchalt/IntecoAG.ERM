*Action Navigation(Partners.crm CBusinessman)
*Action New
*Action CommonGroup
*FillForm
 First Name = Первопечатник
 Last Name = Бумагоделов
 Middle Name = Книгович
 INN = 1234567
 Reg Code = RC1
*Action Validate
*Action Close
*ExecuteEditorAction Address Legal
*FillForm
 Region = Западный
 City = Смоленск
 Zip Postal = 555555
 Street = Ленина, 1
*Action OK
*ExecuteEditorAction Address Fact
*FillForm
 Region = Центральный
 City = Москва
 Zip Postal = 100001
 State Province = ВАО
 Street = Купавинский проезд, 20
*Action OK
*Action Save and New
*Action CommonGroup
*FillForm
 First Name = Врун
 Last Name = Лжецов
 Middle Name = Обманович
 INN = 121212
 Reg Code = Л1
*ExecuteEditorAction Address Legal
*FillForm
 Region = Центральный
 City = Балашиха
 Zip Postal = 143900
 State Province = Московская обл.,
 Street = Маркса, 10
*Action OK
*ExecuteEditorAction Address Fact
*FillForm
 Region = Сибирь
 City = Новосибирск
 Zip Postal = 444444
 State Province = Новосибирск 1
*Action OK
*ExecuteEditorAction Person Type
*Action New
*FillForm
 Code = Раб
 Name = Работник
*Action Save and Close
*Action Save and New
*Action CommonGroup
*FillForm
 First Name = Арамис
 Last Name = Д'Артаньян
 Middle Name = Портосович
 INN = 001001001
 Reg Code = А3
*ExecuteEditorAction Address Legal
*FillForm
 Region = Французский
 City = Париж
 Zip Postal = 12345678
 Street = проспект Ширака, 3
*Action OK
*ExecuteEditorAction Person Type
*Action New
*FillForm
 Code = Сот
 Name = Рабочий скот
*Action Save and Close
*Action Save and Close
