*Action Navigation(Staffs.hrm Staff)
*Action New
*FillForm
 Level = 0
 Last Name = Скоробей
 First Name = Сколопендр
 Middle Name = Червович
*ExecuteEditorAction Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = 1отд
*Action Save and New
*FillForm
 Last Name = Муравей
 First Name = Ломехуза
 Middle Name = Тараканович
 Level = 1
*ExecuteEditorAction Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = ADM1
*Action Save and New
*FillForm
 Last Name = Древоточец
 First Name = Паразит
 Middle Name = Гельминтович
 Level = 3
*ExecuteEditorAction Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = Dep1
*Action Save and Close
