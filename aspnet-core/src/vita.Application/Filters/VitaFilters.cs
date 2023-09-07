using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vita.Filters
{
    public class VitaFilter_Validation : Attribute
    {
        public VitaFilter_ValidationType filter { get; set; }

        public VitaFilter_Validation(VitaFilter_ValidationType f)
        {
            filter = f;
        }
        public enum VitaFilter_ValidationType { None, EinvoiceValidation, XmlValidation, UnicoreValidation, Sales,Credit,Debit }
    }

    public class VitaFilter_Authorization : Attribute
    {
        public VitaFilter_ModuleName module { get; set; }

        public VitaFilter_Authorization(VitaFilter_ModuleName m)
        {
            module = m;
        }
        public enum VitaFilter_ModuleName { None, Einvoice, Sales, Credit,Debit }
    }

}
