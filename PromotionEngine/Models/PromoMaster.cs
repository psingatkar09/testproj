using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine.Models
{
    public class PromoMaster
    {
        public int PromoID { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public Decimal Price { get; set; }
        public bool IsMixPromo { get; set; }
        public List<PromoMaster> PromotionList { get { return PromotionList; } }
    }

    public class MixPromo
    {
        public int PromoID { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
