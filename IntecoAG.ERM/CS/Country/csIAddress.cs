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
        [DC.FieldSize(40)]
        String Street { get; set; } 
        [DC.FieldSize(80)]
        String AddressHandmake { get; set; } 
        String AddressComponent { get; }
        String AddressString { get; }

        csIAddress Copy();

        String ToString();
    }
}
