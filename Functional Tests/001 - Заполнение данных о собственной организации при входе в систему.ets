
#Application IntecoAG.ERM.Win

*Action New(Организация пользователя)
*ExecuteEditorAction Сторона
*Action New
*ExecuteEditorAction Контрагент
*Action New(Предприниматель)
*ExecuteEditorAction Физическое лицо
*Action New
*FillForm
 Фамилия = Иванов
 Имя = Иван
 Отчество = Иванович
*ExecuteEditorAction Адрес
*ExecuteEditorAction Страна
*Action New
*FillForm
 En Короткое имя = RUS
 En Полное имя = Russia
 Ru Короткое имя = Рус
 Ru Полное имя = РОССИЯ
 Букв2С = RU
 Букв3С = Rus
 Рус3C = Рос
 Цифровой = 001
 Комментарий = Россия
*Action Save and Close
*FillForm
 Почтовый индекс = 100000
 Регион = Москва
 Регион = Московский
 Город = Москва
 Улица, дом = Правды, 1
 Адрес не компонентный = корп. 2, стр. 3
*Action OK
*FillForm
 ИНН = 1111111111
*Action Банковские счета
*Action Банковские счета.New
*ExecuteEditorAction Банк
*Action New
*Action Стороны
*Action Стороны.Link
*Action Cancel
*Action Стороны.New
*FillForm
 Наименование стороны = Предприниматель Иванов
 КПП = 123
 Код Регистрации Филиала = 456
*Action Банковские счета.New
*Action Save and Close
*Action Банковские счета
*Action Банковские счета.New
*ExecuteEditorAction Банк
*Action New
*FillForm
 Сторона = Предприниматель Иванов
 Страна = RU - Рус
 Наименование = РастратБанк
 Swift BIC = 111
 БИК = 222
 Кор.Счет = 333333333333333
*Action Save and Close
*FillForm
 Номер счета = 666
*Action Save and Close
*Action Save and Close
*ExecuteEditorAction Адрес
*FillForm
 Почтовый индекс = 101000
 Регион = Московский
 Город = Москва
 Улица, дом = ш. Энтузиастов, 1
*Action OK
*FillForm
 ИНН = 1111111111
 Наименование = ПБОЮЛ Иванов & Ko
 Рег. Номер (ОГРЮЛ) = 5555555555555
 Name Full = Компания Иванова и его друзей
*Action Save and Close
*FillForm
 Наименование стороны = Бизнесмен Иванов
*Action Save and Close
*Action Save and Close
*Action OK
