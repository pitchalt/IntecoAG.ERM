
#Application IntecoAG.ERM.Win


*Action Navigation(Default.Role)
*Action New(Role)
*FillForm
 Name = Администратор
*Action Permissions
*Action Permissions.New(Object Access Permission)
*FillForm
 Permission.Object Type = System.Object
 Permission.Create Access = Allow
 Permission.Read Access = Allow
 Permission.Write Access = Allow
 Permission.Delete Access = Allow
 Permission.Navigate Access = Allow
*Action Save and Close
*Action Save and New(Role)
*FillForm
 Name = Пользователь
*Action Permissions
*Action Permissions.New(Object Access Permission)
*FillForm
 Permission.Object Type = System.Object
 Permission.Create Access = Allow
 Permission.Read Access = Allow
 Permission.Navigate Access = Allow
 Permission.Write Access = Allow
 Permission.Delete Access = Allow
 Permission.Object Type = DevExpress.Xpo.IXPSimpleObject
*Action Save and Close
*Action Save and New(Role)
*FillForm
 Name = Гость
*Action Permissions
*Action Permissions.New(Object Access Permission)
*FillForm
 Permission.Object Type = DevExpress.Xpo.IXPSimpleObject
 Permission.Create Access = Deny
 Permission.Read Access = Allow
 Permission.Write Access = Deny
 Permission.Delete Access = Deny
 Permission.Navigate Access = Allow
*Action Save and Close
*Action Save and Close
