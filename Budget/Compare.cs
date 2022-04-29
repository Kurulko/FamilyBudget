using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budget
{
    public static class Compare
    {
        static List<MoneyAndMonth> GetMoneyAndMonth(IEnumerable<OperationsWithMoney> operations)
        {
            var operationsList = operations.ToList();
            List<MoneyAndMonth> forOperations = new List<MoneyAndMonth>();
            if (operations.Any())
            {
                string month = operationsList[0].Time.ToString("Y");
                decimal sumCartForThisMonth = 0, sumCashForThisMonth = 0;
                int count = operationsList.Count;
                for (int i = 0; i < count; i++)
                {
                    OperationsWithMoney operation = operationsList[i];
                    string thisMonth = operation.Time.ToString("Y");

                    if (i != 0 && month != thisMonth)
                    {
                        forOperations.Add(new MoneyAndMonth
                        {
                            MoneyCart = sumCartForThisMonth,
                            MoneyCash = sumCashForThisMonth,
                            Month = month
                        });
                        sumCartForThisMonth = 0;
                        sumCashForThisMonth = 0;
                        month = thisMonth;
                    }

                    if (operation.IsCash)
                        sumCashForThisMonth += operation.Money;
                    else
                        sumCartForThisMonth += operation.Money;

                    if (month != thisMonth)
                    {
                        forOperations.Add(new MoneyAndMonth
                        {
                            MoneyCart = sumCartForThisMonth,
                            MoneyCash = sumCashForThisMonth,
                            Month = month
                        });
                        if (i == count - 1)
                            forOperations.Add(new MoneyAndMonth
                            {
                                MoneyCart = sumCartForThisMonth,
                                MoneyCash = sumCashForThisMonth,
                                Month = thisMonth
                            });

                        sumCartForThisMonth = 0;
                        sumCashForThisMonth = 0;
                        month = thisMonth;
                    }
                    else if (i == count - 1)
                        forOperations.Add(new MoneyAndMonth
                        {
                            MoneyCart = sumCartForThisMonth,
                            MoneyCash = sumCashForThisMonth,
                            Month = month
                        });
                }
            }
            return forOperations;
        }
        
        public static List<GetAndSpendForMonth> CompareGetAndSpend(
            List<SpendMoney> oldPurchases, List<GetMoney> oldPays)
        {
            List<GetAndSpendForMonth> getAndSpend = new List<GetAndSpendForMonth>();
            var purchases = oldPurchases.OrderBy(p => p.Time).ToList();
            var pays = oldPays.OrderBy(g => g.Time).ToList();

            List<MoneyAndMonth> forPurchases = GetMoneyAndMonth(purchases);
            List<MoneyAndMonth> forPays = GetMoneyAndMonth(pays);

            foreach (var item in forPays)
            {
                if (getAndSpend.Where(gs => gs.Month == item.Month).Count() == 0)
                {
                    GetAndSpendForMonth gsForMonth = new GetAndSpendForMonth();
                    var purchase = forPurchases.FirstOrDefault(p => p.Month == item.Month);
                    if (purchase == null)
                    {
                        gsForMonth.SpendCart = 0;
                        gsForMonth.SpendCash = 0;
                    }
                    else
                    {
                        gsForMonth.SpendCart = purchase.MoneyCart;
                        gsForMonth.SpendCash = purchase.MoneyCash;
                    }

                    gsForMonth.GetCart = item.MoneyCart;
                    gsForMonth.GetCash = item.MoneyCash;
                    gsForMonth.Month = item.Month;
                    getAndSpend.Add(gsForMonth);
                }
            }

            foreach (var item in forPurchases)
            {
                if (getAndSpend.Where(gs => gs.Month == item.Month).Count() == 0)
                {
                    GetAndSpendForMonth gsForMonth = new GetAndSpendForMonth();
                    var pay = forPays.FirstOrDefault(p => p.Month == item.Month);
                    if (pay == null)
                    {
                        gsForMonth.GetCart = 0;
                        gsForMonth.GetCash = 0;
                    }
                    else
                    {
                        gsForMonth.GetCart = pay.MoneyCart;
                        gsForMonth.GetCash = pay.MoneyCash;
                    }

                    gsForMonth.SpendCart = item.MoneyCart;
                    gsForMonth.SpendCash = item.MoneyCash;

                    gsForMonth.Month = item.Month;
                    getAndSpend.Add(gsForMonth);
                }
            }
            return getAndSpend;
        }
    }
    public class MoneyAndMonth
    {
        public string Month { get; set; }
        public decimal MoneyCash { get; set; }
        public decimal MoneyCart { get; set; }
    }
    public class GetAndSpendForMonth
    {
        public string Month { get; set; }
        public decimal GetCash { get; set; }
        public decimal GetCart { get; set; }
        public decimal SpendCash { get; set; }
        public decimal SpendCart { get; set; }
    }
}
