
#Application IntecoAG.ERM.Win
*Action OK

#IncludeFile 002 - Системные - Справочник Категория договора.inc
#IncludeFile 003 - Системные - Справочник Модель цены.inc
#IncludeFile 004 - Системные - Справочник Страна.inc
#IncludeFile 005 - Системные - Справочник Единица измерения.inc
#IncludeFile 006 - Системные - Справочник Тип Номенклатур.inc
#IncludeFile 007 - Системные - Справочник csMaterial и ДДС.inc
#IncludeFile 009 - Системные - Справочник csValuta.inc
#IncludeFile 010 - Партнёры - Справочник Физические лица.inc
#IncludeFile 011 - Партнёры - Справочник Предприниматели.inc
#IncludeFile 012 - Партнёры - Справочник Юридические лица.inc
#IncludeFile 013 - Финансы - Справочник Тематика.inc
#IncludeFile 014 - Кадры - Справочник Подразделение.inc
#IncludeFile 015 - Кадры - Справочник Сотрудник.inc
;#IncludeFile 016 - Default - Справочник Role.ets
#IncludeFile 017 - Системные - Справочник Организация пользователя.inc
#IncludeFile 018 - Договоры - Справочник Новый договор (без этапов).inc
#IncludeFile 019 - Договоры - Утверждение договора (попутно с вводом НДС в справочник).inc


*ReopenApplication IntecoAG.ERM.Win
*Action OK

#IncludeFile 019a - Договоры - Утверждение договора.inc
#IncludeFile 020 - Договоры - Создание новой версии простого договора (со сменой значений свойств).inc

*ReopenApplication IntecoAG.ERM.Win
*Action OK

#IncludeFile 021 - Договоры - Утверждение проектной версии простого договора (со сменой значений свойств).inc
