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

            var period = new Period(start, end);
            return budgets.Sum(budget => budget.OverlappingAmount(period));
            // if (start.ToString("yyyyMM") != end.ToString("yyyyMM"))
            // {
            //     var period = new Period(start, end);
            //     return budgets.Sum(budget => budget.OverlappingAmount(period));
            // }
            //
            // return ((end - start).Days + 1) *
            //     GetAmountForOneDay(
            //         start, budgets.FirstOrDefault(x => x.YearMonth.Equals(start.ToString("yyyyMM"))));
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
}