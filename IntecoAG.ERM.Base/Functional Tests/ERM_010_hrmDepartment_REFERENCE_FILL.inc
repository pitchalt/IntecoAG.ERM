*Action Navigation(Staffs.hrm Department)
*Action New
*FillForm
 Code = Dep1
 Buh Code = BuhDep1
 Post Code = 111222
 Name = Отдел разработок
*Action Save and New
*FillForm
 Code = ADM1
 Buh Code = BuhADM1
 Post Code = 101000
 Name = Администрация
*Action Save and New
*FillForm
 Code = 1отд
 Buh Code = BuhOtd1
 Post Code = 000000
 Name = Первый отдел
*Action Save and New
*FillForm
 Code = ВОХР
 Buh Code = BuhOHR
 Post Code = 000000
 Name = Отдел охраны
*ExecuteEditorAction Up Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = ADM1
*Action Save and Close