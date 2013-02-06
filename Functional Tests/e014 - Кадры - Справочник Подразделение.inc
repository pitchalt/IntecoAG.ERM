*Action Navigation(Staffs.hrm Department)
*Action New
*FillForm
 Code = АДМ 1
 Buh Code = БухАДМ
 Post Code = 000000
 Name = Администрация
*Action Save and New

*FillForm
 Code = БУХ
 Buh Code = БухБУХ
 Post Code = 100000
 Name = Центральная бухгалтерия
*ExecuteEditorAction Up Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = АДМ 1
*Action Save and New

*FillForm
 Code = ПО
 Buh Code = БухПЛН
 Post Code = 110000
 Name = Плановый отдел
*Action Save and New

*FillForm
 Code = ЦПП
 Buh Code = БухЦПП
 Post Code = 111000
 Name = Цех предпусковой подготовки
*ExecuteEditorAction Up Department
*Action Clear
*Action Save and New

*FillForm
 Code = Отд 1
 Buh Code = БухСЕК
 Post Code = ******
 Name = ********** (Название Первого отдела особо секретно)
*Action Save and New

*Action Save and Close
