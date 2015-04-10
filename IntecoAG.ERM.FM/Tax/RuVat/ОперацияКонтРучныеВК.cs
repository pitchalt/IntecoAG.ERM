using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
//
using FileHelpers;
using FileHelpers.DataLink;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    public partial class ОперацияКонтРучныеВК : ObjectViewController {
        public ОперацияКонтРучныеВК() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ОперацияКонтРучные cont = View.CurrentObject as ОперацияКонтРучные;
            if (cont == null)
                return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                cont = os.GetObject<ОперацияКонтРучные>(cont);
                Import(os, cont, dialog.FileName);
                os.CommitChanges();
            }
        }
        [FixedLengthRecord]
        public class OperationImport {
            [FieldFixedLength(2)]
            public String BOOK;
            [FieldFixedLength(3)]
            public String OPERATION;
            [FieldFixedLength(3)]
            public String VAT_MODE;
            [FieldFixedLength(2)]
            public String VAT_RATE;
            [FieldFixedLength(6)]
            public String BUH_PROV;
            [FieldFixedLength(9)]
            public String BUH_DT;  
            [FieldFixedLength(9)]
            public String VAT_DT;  
            [FieldFixedLength(6)]
            public String VO_CODE; 
            [FieldFixedLength(20)]
            public String SF_REGNUM; 
            [FieldFixedLength(3)]
            public String SF_TYPE;   
            [FieldFixedLength(3)]
            public String SF_TYPE_ORIG;
            [FieldFixedLength(20)]
            public String SF_NUMBER;  
            [FieldFixedLength(8)]
            public String SF_DATE;    
            [FieldFixedLength(3)]
            public String PD_TYPE;    
            [FieldFixedLength(20)]
            public String PD_NUMBER;  
            [FieldFixedLength(8)]
            public String PD_DATE;    
            [FieldFixedLength(17)]
            public String SUMM_ALL;   
            [FieldFixedLength(17)]
            public String SUMM_COST;  
            [FieldFixedLength(17)]
            public String SUMM_VAT_PAY; 
            [FieldFixedLength(17)]
            public String SUMM_VAT_IN;
            [FieldFixedLength(17)]
            public String SUMM_VAT_COST;
            [FieldFixedLength(17)]
            public String SUMM_VAT_BAY;
        }

        private void Import(IObjectSpace os, ОперацияКонтРучные конт, String file_name) {
            FixedFileEngine engine = new FixedFileEngine(typeof(OperationImport));
            OperationImport[] imp_res = (OperationImport[])engine.ReadFile(file_name);
            os.Delete(конт.Операции);
            DateTime date = default(DateTime);
            foreach (OperationImport oper_imp in imp_res) {
                ОснованиеДокумент doc = null;
                if (oper_imp.SF_TYPE.Trim() != "СФЗ") {
                    doc = os.FindObject<ОснованиеДокумент>(new BinaryOperator("РегНомер", oper_imp.SF_REGNUM.Trim()), true);
                }
                else {
                    DateTime.TryParseExact(oper_imp.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date);
                    Основание осн = os.FindObject<Основание>(
                        new BinaryOperator("ИннПродавца", "5012039795") &
                        new BinaryOperator("Номер", oper_imp.SF_NUMBER.Trim()) &
                        new BinaryOperator("Дата", date, BinaryOperatorType.GreaterOrEqual) &
                        new BinaryOperator("Дата", date.AddDays(1), BinaryOperatorType.Less ), true);
                    doc = осн != null ? осн.ДействующийДокумент : null;
                }
                Операция oper = os.CreateObject<Операция>();
                конт.Операции.Add(oper);
//                Decimal summ_cost = Decimal.Parse(imp_rec.SUMM_COST.Trim().Replace('.', ','));
//                DateTime.TryParseExact(imp_rec.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out sfdoc_date);
                oper.ТипКниги = (Операция.ТипКнигиТип) Int32.Parse(oper_imp.BOOK.Trim());
                oper.ТипОперВнутр = (Операция.ТипОперВнутрТип) Int32.Parse(oper_imp.OPERATION.Trim());
                String stavka = oper_imp.VAT_RATE.Trim();
                if (String.IsNullOrEmpty(stavka) || stavka == "0")
                    stavka = "2";
                oper.Ставка = (СтавкаНДС) Int32.Parse(stavka);
                //                oper_imp.VAT_MODE;
                oper.Проводка = oper_imp.BUH_PROV.Trim();
                DateTime.TryParseExact(oper_imp.BUH_DT.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date);
                oper.ДатаБУ = date;
                DateTime.TryParseExact(oper_imp.VAT_DT.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date);
                oper.ДатаНДС = date;
                oper.ОснованиеДокумент = doc;
                oper.КодПартнера = oper_imp.VO_CODE.Trim();
                if (oper_imp.SF_TYPE.Trim() != "СФЗ")
                    oper.ОснованиеРегНомер = oper_imp.SF_REGNUM.Trim();
                oper.СФТип = oper_imp.SF_TYPE.Trim();
                oper.СФНаим = oper_imp.SF_TYPE_ORIG.Trim() + ' ' + oper_imp.SF_NUMBER.Trim() + ' ' + oper_imp.SF_DATE;
                oper.ПДНаим = oper_imp.PD_TYPE.Trim() + ' ' + oper_imp.PD_NUMBER.Trim() + ' ' + oper_imp.PD_DATE;
                oper.СуммаВсего = Decimal.Parse(oper_imp.SUMM_ALL.Trim().Replace('.', ','));
                oper.СуммаНДСБаза = Decimal.Parse(oper_imp.SUMM_VAT_PAY.Trim().Replace('.', ','));
                if (oper.СФТип != "СФВ") {
                    oper.СуммаСтоимость = oper.СуммаВсего;
                    oper.СуммаСтоимость += -oper.СуммаНДСБаза;
                }
                else {
                    oper.СуммаСтоимость = Decimal.Round(oper.СуммаНДСБаза * 100 / 18, 2);
                    oper.СуммаВсего = oper.СуммаСтоимость + oper.СуммаНДСБаза;
                }
                //oper_imp.SUMM_ALL;   
                //oper_imp.SUMM_COST;  
                //oper_imp.SUMM_VAT_PAY; 
                //oper_imp.SUMM_VAT_IN;
                //oper_imp.SUMM_VAT_COST;
                //oper_imp.SUMM_VAT_BAY;
                //System.Console.WriteLine(oper_import.SUMM_ALL + "_" + oper_import.SUMM_VAT_PAY + "_" + oper_import.SUMM_VAT_BAY);
            }
        }

    }
}
