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

                var startYearMonth = new DateTime(start.Year,start.Month,1);
                var endYearMonth = new DateTime(end.Year,end.Month,1);
                var amount=0;
                while (startYearMonth<=endYearMonth)
                {
                    var yearMonth = startYearMonth.ToString("yyyyMM");
                    amount+=allAmount.FirstOrDefault(x => x.YearMonth.Equals(yearMonth)).Amount;

                    startYearMonth=startYearMonth.AddMonths(1);
                }
                return amount;
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