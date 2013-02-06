*ProcessRecord 
 Категория = ConCat 1
*FillForm
 Advance Percentage = 1110
 Advance Summ = 2220

*Action Delivery Items
*Action Delivery Items.New(crm Delivery Material)
*FillForm
 Nomenclature Name = Железо
 Summ Full = 3330
 Price = 10
 Summ Cost = 3300
 Cost Item = Cost It 1
 Count Unit = Money

*ExecuteEditorAction Cost Model
*Action New
*FillForm
 Code = CM1
 Name = CM1
*Action Save and Close

*Action Save and Close

*Action Approve
*HandleDialog
 Respond = Yes

*Action Close
