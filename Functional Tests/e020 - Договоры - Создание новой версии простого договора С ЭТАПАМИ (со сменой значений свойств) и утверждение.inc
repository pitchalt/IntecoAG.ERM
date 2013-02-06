*ProcessRecord 
 Состояние = DEAL_PROJECT
*Action Create Version
*Action Common
*FillForm
 Description Short = Договор с этапами 1 версия 2
 Description Long = Догоовр с этапами - вторая версия
*Action Stages
*Action Stages.New
*FillForm
 Stage Type = AGREGATE
 Code = TECH 3
*Action Common
*FillForm
 Description Short = Новый технический этап
 Description Long = Новый технический этап, образованный по наитию
*Action Save and Close
*Action Common
*FillForm
 Price = 2000
*Action Approve
*HandleDialog
 Respond = Yes
*Action Save and Close