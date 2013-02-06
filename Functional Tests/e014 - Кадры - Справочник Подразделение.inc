*Action Navigation(Staffs.hrm Department)
*Action New(hrm Department)
*FillForm
 Code = АДМ 1
 Post Code = 000000
 Name = Администрация
*Action Save and New(hrm Department)

*FillForm
 Code = БУХ
 Post Code = 100000
 Name = Центральная бухгалтерия
 Up Department = АДМ 1
*Action Save and New(hrm Department)

*FillForm
 Code = ПО
 Post Code = 110000
 Name = Плановый отдел
*Action Save and New(hrm Department)

*FillForm
 Code = ЦПП
 Post Code = 111000
 Name = Цех предпусковой подготовки
*ExecuteEditorAction Up Department
*Action Clear
*Action Save and New(hrm Department)

*FillForm
 Code = Отд 1
 Post Code = ******
 Name = ********** (Название Первого отдела особо секретно)
*ExecuteEditorAction Up Department
*Action Clear
*Action Save
*Action Save and Close
