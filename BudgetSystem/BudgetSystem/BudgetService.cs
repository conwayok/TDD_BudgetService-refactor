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
                    var budget = budgets.FirstOrDefault(x => x.YearMonth.Equals(currentMonth.ToString("yyyyMM")));
                    if (budget == null)
                    {
                        continue;
                    }

                    var overlappingDays = OverlappingDays(start, end, budget);
                    amount += overlappingDays * GetAmountForOneDay(currentMonth, budget);

                    currentMonth = currentMonth.AddMonths(1);
                }
            }
            else
            {
                return ((end - start).Days + 1) *
                    GetAmountForOneDay(
                        start, budgets.FirstOrDefault(x => x.YearMonth.Equals(start.ToString("yyyyMM"))));
            }

            return amount;
        }

        private static int OverlappingDays(DateTime start, DateTime end, Budget budget)
        {
            DateTime overlappingStart;
            DateTime overlappingEnd;
            if (budget.YearMonth == start.ToString("yyyyMM"))
            {
                overlappingStart = start;
                overlappingEnd = budget.LastDay();
            }
            else if (budget.YearMonth == end.ToString("yyyyMM"))
            {
                overlappingStart = budget.FirstDay();
                overlappingEnd = end;
            }
            else
            {
                overlappingStart = budget.FirstDay();
                overlappingEnd = budget.LastDay();
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            return overlappingDays;
        }

        private int GetAmountForOneDay(DateTime start, Budget budget)
        {
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

        public DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        public DateTime LastDay()
        {
            var daysInMonth = DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
            return DateTime.ParseExact(YearMonth + daysInMonth, "yyyyMMdd", null);
        }
    }
}