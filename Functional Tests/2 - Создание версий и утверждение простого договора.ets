
#Application IntecoAG.ERM.Win

*Action OK
*Action Mini Navigation Action(Редакция текущая)
*Action Общие сведения
*FillForm
 Deal Code = Сделка-1
 Описание = 1-я версия тестовой сделки.
*Action Утвердить редакцию
*HandleDialog
 Respond = Yes
*Action Новая редакция
*FillForm
 Deal Code = Сделка-2
 Описание = 2-я версия тестовой сделки.
 Дата по = 07.10.2011
*Action Обязательства
*FillForm
 Стоимость = 20
*Action Save
*Action Новая редакция
*Action Общие сведения
*FillForm
 Deal Code = Сделка-3
 Описание = 3-я версия тестовой сделки.
*Action Обязательства
*FillForm
 Стоимость = 30
*Action Утвердить редакцию
*HandleDialog
 Respond = Yes
