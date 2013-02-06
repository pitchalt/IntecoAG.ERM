
; Похоже, что если список образует дерево, то элементы от 2-го и глубже уровней скрипт не умеет выбирать
; Это относительно параметра Code для выбора подразделения: ProcessRecord hrm Department

*Action Navigation(Staffs.hrm Staff)
*Action New
*FillForm
 Level = 1
 Last Name = Путин
 First Name = Владимир
 Middle Name = Владимирович
*ExecuteEditorAction Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = АДМ 1
*Action Save and New

*FillForm
 Level = 2
 Last Name = Медведев
 First Name = Дмитрий
 Middle Name = Анатольевич
*ExecuteEditorAction Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = ЦПП
*Action Save and New

*FillForm
 Level = 3
 Last Name = Зюганов
 First Name = Геннадий
 Middle Name = Андреевич
*ExecuteEditorAction Department
*Action Filter by Text()
*ProcessRecord hrm Department
 Code = Отд 1
*Action Save and Close