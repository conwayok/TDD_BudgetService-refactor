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
            if (start > end)
            {
                return 0;
            }

            var budgets = _budgetRepo.GetAll();

            var amount = 0;
            if (start.ToString("yyyyMM") != end.ToString("yyyyMM"))
            {
                var currentMonth = new DateTime(start.Year, start.Month, 1);

                while (currentMonth <= end)
                {
                    int overlappingDays;
                    if (currentMonth.ToString("yyyyMM") == start.ToString("yyyyMM"))
                    {
                        var overlappingEnd =
                            new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                        var overlappingStart = start;
                        overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
                    }
                    else if (currentMonth.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        var overlappingStart = new DateTime(end.Year, end.Month, 1);
                        var overlappingEnd = end;
                        overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
                    }
                    else
                    {
                        overlappingDays = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                    }

                    amount += overlappingDays * GetAmountForOneDay(currentMonth, budgets);

                    currentMonth = currentMonth.AddMonths(1);
                }
            }
            else
            {
                return ((end - start).Days + 1) * GetAmountForOneDay(start, budgets);
            }

            return amount;
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