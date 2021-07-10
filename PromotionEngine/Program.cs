using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PromotionEngine.Models;

namespace PromotionEngine
{
    class Program
    {
        Decimal TotalAmount = 0;
        Decimal actualPrice = 0;
        Decimal amount = 0;
        MixPromo MixPromo = new MixPromo();
        static void Main(string[] args)
        {
            Dictionary<string, int> PricePerUnit = MasterData.GetActualPriceperUnit();
            Dictionary<string, int> userSelectionList = new Dictionary<string, int>();
            Console.Write("Please Select Unit or cal to calculate output");
            string Unit = Console.ReadLine();
            Console.Write("Please Enter Quantity for " + Unit + "");
            int Quantity = Convert.ToInt32(Console.ReadLine());
            while (Unit != "")
            {
                if (Unit != "cal")
                {
                    if (userSelectionList.Count == 0 && Unit != "")
                    {
                        userSelectionList.Add(Unit, Quantity);
                    }
                    Console.Write("Please Select Unit ");
                    Unit = Console.ReadLine();
                    if (Unit.ToLower() == "cal")
                    {
                        break;
                    }
                    Console.Write("Please Enter Quantity for " + Unit);
                    Quantity = Convert.ToInt32(Console.ReadLine());
                    userSelectionList.Add(Unit, Quantity);
                }
            }
            if (Unit.ToLower() == "cal")
            {
                Program objProg = new Program(); 
                Console.WriteLine();
                Console.WriteLine("---------Output-------");
                objProg.CalculateTotal(userSelectionList);
            }

        }
        public void CalculateTotal(Dictionary<string, int> userSelectionList)
        {
            Dictionary<string, int> PricePerUnit = MasterData.GetActualPriceperUnit();
            List<PromoMaster> PromoRateMaster = MasterData.GetPromoRates();
            DataTable tblMixShipment = new DataTable();
            tblMixShipment.Columns.Add("PromoID", typeof(int));
            tblMixShipment.Columns.Add("Unit", typeof(string));
            tblMixShipment.Columns.Add("Quantity", typeof(int));
            foreach (KeyValuePair<string, int> userList in userSelectionList)
            {
                actualPrice = PricePerUnit[userList.Key];
                var UnitKey = PromoRateMaster.Find(x => x.Unit.Contains(userList.Key)).Unit;
                if (UnitKey != "")
                {
                    var PerUnitDetails = PromoRateMaster.Find(x => x.Unit == UnitKey);
                    var PramotionQuantity = PerUnitDetails.Quantity;
                    var PromoRatePrice = PerUnitDetails.Price;
                    var PromoID = PerUnitDetails.PromoID;
                    var IsMixPromo = PerUnitDetails.IsMixPromo;
                    if (IsMixPromo) 
                    {
                        DataRow dtRow = tblMixShipment.NewRow();
                        dtRow["PromoID"] = PromoID;
                        dtRow["Unit"] = UnitKey;
                        dtRow["Quantity"] = userList.Value;
                        tblMixShipment.Rows.Add(dtRow);
                    }
                    else
                    {
                        amount = (userList.Value / PramotionQuantity) * PromoRatePrice + (userList.Value % PramotionQuantity) * Convert.ToInt32(actualPrice);
                        Console.WriteLine("Unit: {0}, Quantity: {1}, Amount: {2}", userList.Key, userList.Value, amount);
                        TotalAmount = TotalAmount + amount;
                    }
                }
                else
                {
                    amount = userList.Value * Convert.ToInt32(actualPrice);
                    Console.WriteLine("Unit: {0}, Quantity: {1}, Amount: {2}", userList.Key, userList.Value, amount);
                    TotalAmount = TotalAmount + amount;
                }
            }
            DataTable FilterMixData = tblMixShipment.AsEnumerable()
              .GroupBy(r => r.Field<int>("PromoID"))
              .Select(g =>
              {
                  var row = tblMixShipment.NewRow();
                  row["PromoID"] = g.Key;
                  row["Quantity"] = g.Sum(r => r.Field<int>("Quantity"));
                  return row;
              }).CopyToDataTable();
            foreach (DataRow td in FilterMixData.Rows)
            {
                var MasterData = PromoRateMaster.Find(x => x.PromoID == Convert.ToInt32(td[0]));
                string[] MixPromo = MasterData.Unit.Split('&');
                var PramotionQuantity = MasterData.Quantity;
                var PromoRatePrice = MasterData.Price;
                int Count = MixPromo.Count();
                for (int i = 0; i < Count; i++)
                {
                    if (i != Count - 1)
                    {   amount = (Convert.ToInt32(td[2])) * Convert.ToInt32(actualPrice);
                        Console.WriteLine("Unit: {0}, Quantity: {1}, Amount: {2}", MixPromo[i], 5, amount);
                        TotalAmount = TotalAmount + amount;
                    }
                    else
                    {
                        amount = ((Convert.ToInt32(td[2])) / PramotionQuantity) * PromoRatePrice + ((Convert.ToInt32(td[2])) % PramotionQuantity) * Convert.ToInt32(actualPrice);
                        Console.WriteLine("Unit: {0}, Quantity: {1}, Amount: {2}", MixPromo[i], 5, 0);
                        //TotalAmount = TotalAmount + amount;
                    }
                }
            }
            Console.WriteLine("Total: {0}", TotalAmount);
            Console.ReadLine();
        }
    }
}
