using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DC=DevExpress.ExpressApp.DC;

namespace IntecoAG.ERM.CS.Country {
    [DC.DomainComponent]
    [DC.XafDefaultProperty("AddressString")]
    public interface csIAddress {
        csCountry Country { get; set; }
        [DC.FieldSize(10)]
        String ZipPostal { get; set; }
        [DC.FieldSize(30)]
        String Region  { get; set; }
        [DC.FieldSize(30)]
        String StateProvince { get; set; } 
        [DC.FieldSize(30)]
        String City { get; set; } 
        [DC.FieldSize(60)]
        String Street { get; set; } 
        [DC.FieldSize(120)]
        String AddressHandmake { get; set; }
        [DC.FieldSize(120)]
        String AddressComponent { get; }
        [DC.FieldSize(120)]
        String AddressString { get; }

        csIAddress Copy();

        String ToString();
    }
}
