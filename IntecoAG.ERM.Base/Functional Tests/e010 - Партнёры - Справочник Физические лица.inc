*Action Navigation(Partners.crm CPhysical Party)
*Action New
*Action CommonGroup
*FillForm
 Last Name = Иванов
 First Name = Иван
 Middle Name = Иванович
 Name Handmake = Иван Иваныч Иваноff
 INN = 121212
 Description = Иванов & сыновья

*ExecuteEditorAction Address Legal
*FillForm
 Region = Московский
 City = Москва
 Zip Postal = 111222
 State Province = Иваново
 Street = Крокодилов, 1
*Action OK

*Action Save and New
*FillForm
 Last Name = Петров
 First Name = Пётр
 Middle Name = Петрович
 Name Handmake = Петросян 
 INN = 232323
 Description = Предалагает разные товары, по хозяйству ненужные.
*ExecuteEditorAction Address Fact
*FillForm
 Country = УКР
 Region = Киiв
 City = Киiв
 Zip Postal = 878787
 State Province = Киiв регион
 Street = Подол, 111
*Action OK
*Action Save and New

*FillForm
 Last Name = Сидоров
 First Name = Сидор
 Middle Name = Сидорович
 Name Handmake = Сидорыч 
 INN = 343434
 Description = Строгий и честный товаропроизводитель
*ExecuteEditorAction Address Fact
*FillForm
 Country = РБ
 Region = Минский
 City = Минск
 Zip Postal = 191919
 State Province = Минск
 Street = Лукашенко, 1
*Action OK

*Action Save and Close