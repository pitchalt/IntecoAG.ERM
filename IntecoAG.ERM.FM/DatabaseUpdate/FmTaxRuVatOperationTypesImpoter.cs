using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS.Import;
using IntecoAG.ERM.FM.Tax.RuVat;
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.DatabaseUpdate {

    public class FmTaxRuVatOperationTypesImpoter: Excel2003XmlReader {
        private IObjectSpace _ObjectSpace;

        public FmTaxRuVatOperationTypesImpoter(IObjectSpace os, Stream stream) : base(stream) {
            _ObjectSpace = os;
        }

        public override void Load() {
            base.Load();
        }

        private String _Name;
        private String _Code;

        protected override void ProcessCell(string sheet, int row, int column, string type, string value) {
            if (row < 2)
                return;
            if (column == 1)
                return;
            if (column == 2) {
                _Name = value;
                return;
            }
            if (column == 3) {
                _Code = value;
                UInt16 code = UInt16.Parse(_Code);
                String scode = code.ToString("00");
                ВидОперации ot = _ObjectSpace.FindObject<ВидОперации>(new BinaryOperator("Код", scode), true);
                if (ot == null) {
                    ot = _ObjectSpace.CreateObject<ВидОперации>();
                }
                ot.Код = scode;
                ot.Наименование = _Name;
                return;
            }
        }
                    
    }
}
