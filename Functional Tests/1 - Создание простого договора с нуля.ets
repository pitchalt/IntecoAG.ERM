
#Application IntecoAG.ERM.Win

*Action OK
*Action Navigation(Договоры.Новый договор...)
*ExecuteEditorAction Рег.Польз.
*Action New
*FillForm
 Фамилия = Иванов
 Имя = Иван
 Отчество = Иванович
*ExecuteEditorAction Подразделение
*Action New
*FillForm
 Код = Отд 1
 Наименование = Отдел 1
*Action Сотрудники
*Action Сотрудники.Link
*Action Cancel
*Action Save and New(Подразделение)
*FillForm
 Код = Отд 2
 Наименование = Отдел 2
*Action Save and New(Подразделение)
*FillForm
 Код = Отд 3
 Наименование = Отдел 3
*Action Save and New(Подразделение)
*FillForm
 Код = Отд 4
 Наименование = Отдел 4
 Топ.Подр. = Отд 1
*Action Save and Close
*FillForm
 Должность = Начальник отдела
*Action Save and New(Сотрудник)
*FillForm
 Фамилия = Петров
 Имя = Пётр
 Отчество = Петрович
 Подразделение = Отд 2
 Должность = Зам. начальника отдела
*Action Save and New(Сотрудник)
*FillForm
 Фамилия = Сидоров
 Имя = Сидор
 Отчество = Сидорович
 Подразделение = Отд 3
 Должность = Отвественный за договоры
*Action Save and Close
*FillForm
 Новый Ном. = True
*ExecuteEditorAction Док. Тип
*Action New
*FillForm
 Код = Док. тип 1
 Наименование = Тип документа 1
*Action Save and New(Тип документа)
*FillForm
 Код = Док. тип 2
 Наименование = Тип документа 2
*Action Save and Close
*FillForm
 Тип Формы = Простой договор
*ExecuteEditorAction Категория
*Action New
*FillForm
 Код = Кат. дог. 1
 Наименование = Категория договора 1
*Action Save and New(Категория договора)
*FillForm
 Код = Кат. дог. 2
 Наименование = Категория договора 2
*Action Save and New(Категория договора)
*FillForm
 Код = Кат. дог. 3
 Наименование = Категория договора 3
*Action Save and Close
*FillForm
 Кур.Подр. = Отд 2
 Стоимость = 10
*ExecuteEditorAction Валюта
*Action New
*FillForm
 Код = RUR
 Сокр.Наимен. = Руб.
 Наименование = Рубль
*Action Save and Close
*FillForm
 Дата с = 06.10.2011
 Дата по = 06.10.2011
 Дата Завер. = 06.10.2011
 Крат.Опис. = Тестовый простой договор 1
 Орг.Роль = Заказчик
*ExecuteEditorAction Организация
*Action New
*FillForm
 Контрагент = Иванов Иван Иванович
 Наименование стороны = Тов. Иванов и Ко
*Action Save and Close
*ExecuteEditorAction Контрагент
*Action New
*FillForm
 Контрагент = Петров Пётр Петрович
 Наименование стороны = Хозяйственник Петров
*Action Save and Close
*Action Готово
