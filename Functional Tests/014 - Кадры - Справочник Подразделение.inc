*Action Navigation(Кадры.Подразделение)

*Action New(Подразделение)
*FillForm
 Код = АДМ 1
 Почтовый номер = 000000
 Наименование = Администрация
*Action Save and New(Подразделение)

*FillForm
 Код = БУХ
 Почтовый номер = 100000
 Наименование = Центральная бухгалтерия
 Топ.Подр. = АДМ 1
*Action Save and New(Подразделение)

*FillForm
 Код = ПО
 Почтовый номер = 110000
 Наименование = Плановый отдел
*Action Save and New(Подразделение)

*FillForm
 Код = ЦПП
 Почтовый номер = 111000
 Наименование = Цех предпусковой подготовки
*ExecuteEditorAction Топ.Подр.
*Action Clear
*Action Save and New(Подразделение)

*FillForm
 Код = Отд 1
 Почтовый номер = ******
 Наименование = ********** (Название Первого отдела особо секретно)
*ExecuteEditorAction Топ.Подр.
*Action Clear
*Action Save
*Action Save and Close
