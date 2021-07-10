using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromotionEngine.Models;

namespace PromotionEngine.Models
{
    static class MasterData
    {
        public static Dictionary<string, int> GetActualPriceperUnit()
        {
            Dictionary<string, int> PricePerUnit = new Dictionary<string, int>();
            PricePerUnit.Add("A", 50);
            PricePerUnit.Add("B", 30);
            PricePerUnit.Add("C", 20);
            PricePerUnit.Add("D", 15);
            PricePerUnit.Add("E", 60);
            PricePerUnit.Add("F", 40);
            return PricePerUnit;
        }

        public static List<PromoMaster> GetPromoRates()
        {
            List<PromoMaster> PromoRateMaster = new List<PromoMaster>() {
                new PromoMaster () { PromoID=1, Quantity=3,Unit="A",Price=130 ,IsMixPromo=false},
                new PromoMaster () { PromoID=2, Quantity=2,Unit="B",Price=45 ,IsMixPromo=false},
                new PromoMaster () { PromoID=3, Quantity=2,Unit="C&D",Price=30,IsMixPromo=true },
                new PromoMaster () { PromoID=4, Quantity=2,Unit="E&F",Price=30,IsMixPromo=true }
            };
            return PromoRateMaster;
        }
    }
}
