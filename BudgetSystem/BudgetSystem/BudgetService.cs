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

                    DateTime overlappingStart;
                    DateTime overlappingEnd;
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingStart = start;

                        var daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                        overlappingEnd = new DateTime(currentMonth.Year, currentMonth.Month, daysInMonth);
                    }
                    else if (currentMonth.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        overlappingStart = new DateTime(end.Year, end.Month, 1);
                        overlappingEnd = end;
                    }
                    else
                    {
                        overlappingStart = new DateTime(currentMonth.Year, currentMonth.Month, 1);

                        var daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                        overlappingEnd = new DateTime(currentMonth.Year, currentMonth.Month, daysInMonth);
                    }

                    var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
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
    }
}