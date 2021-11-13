#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace BudgetSystem
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            var budgets = _budgetRepo.GetAll();
            if (start == end)
            {
                return GetAmountForOneDay(start, budgets);
            }

            if (start < end)
            {
                var amount = 0;
                var startYearMonth = new DateTime(start.Year, start.Month, 1);
                var lendYearMonth = new DateTime(end.Year, end.Month, 1);

                if (startYearMonth != lendYearMonth)
                {
                    var lastDayOfStartMonth =
                        new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                    var days = (lastDayOfStartMonth - start).Days + 1;
                    amount += days * GetAmountForOneDay(start, budgets);

                    var firstDayOfEndMonth = new DateTime(end.Year, end.Month, 1);
                    days = (end - firstDayOfEndMonth).Days + 1;
                    amount += days * GetAmountForOneDay(end, budgets);

                    var secondYearMonth = new DateTime(start.Year, start.Month + 1, 1);
                    var lastSecondEndYearMonth = new DateTime(end.Year, end.Month - 1, 1);

                    while (secondYearMonth <= lastSecondEndYearMonth)
                    {
                        var yearMonth = secondYearMonth.ToString("yyyyMM");
                        amount += GetAmountForAllMonth(budgets, yearMonth);

                        secondYearMonth = secondYearMonth.AddMonths(1);
                    }
                }
                else
                {
                    return ((end - start).Days + 1) * GetAmountForOneDay(start, budgets);
                }

                return amount;
            }

            return 0;
        }

        private static int GetAmountForAllMonth(List<Budget> allAmount, string yearMonth)
        {
            var budget = allAmount.FirstOrDefault(x => x.YearMonth.Equals(yearMonth));
            return budget?.Amount ?? 0;
        }

        private int GetAmountForOneDay(DateTime start, List<Budget> allAmount)
        {
            var budget = allAmount.FirstOrDefault(x => x.YearMonth.Equals(start.ToString("yyyyMM")));
            return budget == null
                ? 0
                : budget.Amount /
                DateTime.DaysInMonth(start.Year, start.Month);
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }
    }
}