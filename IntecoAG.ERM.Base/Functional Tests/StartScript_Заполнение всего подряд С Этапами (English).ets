#Application IntecoAG.ERM.Win
*Action OK

*Action Choose Language(English (en-us))

#IncludeFile e002 - Системные - Справочник Категория договора.inc
#IncludeFile e003 - Системные - Справочник Модель цены.inc
#IncludeFile e004 - Системные - Справочник Страна.inc
#IncludeFile e005 - Системные - Справочник Единица измерения.inc
#IncludeFile e006 - Системные - Справочник Тип Номенклатур.inc
#IncludeFile e007 - Системные - Справочник csMaterial и ДДС.inc
#IncludeFile e008 - Системные - Справочник csService.inc
#IncludeFile e009 - Системные - Справочник csValuta.inc
#IncludeFile e010 - Партнёры - Справочник Физические лица.inc
#IncludeFile e011 - Партнёры - Справочник Предприниматели.inc
#IncludeFile e012 - Партнёры - Справочник Юридические лица.inc

*ReopenApplication IntecoAG.ERM.Win
*Action OK

#IncludeFile e013 - Финансы - Справочник Тематика.inc
#IncludeFile e014 - Кадры - Справочник Подразделение.inc
#IncludeFile e015 - Кадры - Справочник Сотрудник.inc
;#IncludeFile e016 - Default - Справочник Role.ets
#IncludeFile e017 - Системные - Справочник Организация пользователя.inc

*ReopenApplication IntecoAG.ERM.Win
*Action OK

#IncludeFile e018 - Договоры - Справочник Новый договор (С ЭТАПАМИ).inc

;*ReopenApplication IntecoAG.ERM.Win
;*Action OK

;#IncludeFile e020 - Договоры - Создание новой версии простого договора С ЭТАПАМИ (со сменой значений свойств) и утверждение.inc