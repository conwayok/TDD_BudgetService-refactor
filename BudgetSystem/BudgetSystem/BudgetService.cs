using System;
using System.Collections.Generic;
using System.Linq;

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
            if (start <= end)
            {
                var allAmount = _budgetRepo.GetAll();
                var yearMonth = start.ToString("yyyyMM");
                return allAmount.FirstOrDefault(x => x.YearMonth.Equals(yearMonth)).Amount;
            }
            return 0;
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }
}