Project Description

The IntecoAG.ERM.FM project implements a Module. The root project folder contains 
the Module.cs(vb) file with the class that inherits ModuleBase. This class can be 
designed with the Module Designer that allows you to view and customize Module 
components: referenced modules, Controllers and business classes. Additionally, 
the root folder contains Application Model difference files (XAFML files) that 
keep application settings specific for the current Module. Differences files 
can be designed with the Model Editor.


Relevant Documentation

Application Solution Components
http://help.devexpress.com/#Xaf/CustomDocument2569

ModuleBase Class
http://help.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic

Module Designer
http://help.devexpress.com/#Xaf/CustomDocument2828

Application Model
http://help.devexpress.com/#Xaf/CustomDocument2579

Model Editor
http://help.devexpress.com/#Xaf/CustomDocument2582


Правила работы контроллеров автоматической привязки заявок с платёжными документами.

1. Контроллер fmCDocRCBImportResultViewController работает только на Root DetailView и обрабатывает все выписки, входящие в импорт.

2. Контроллер fmCSAStatementAccountViewController работает в корневых списках, обрабатывая все подчинённые документы всех выбранных выписок
   и в корневом DetailView конкретной выписки, обрабатывая все её документы.

2. Контроллер fmCSAStatementAccountDocViewController работает только на корневом списке документов выписки и на корневом DetailView конкретнного
   документа выписки. 



