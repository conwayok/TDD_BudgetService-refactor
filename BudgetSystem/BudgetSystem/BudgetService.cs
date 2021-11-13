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
            var period = new Period(start, end);
            return _budgetRepo.GetAll().Sum(budget => budget.OverlappingAmount(period));
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}