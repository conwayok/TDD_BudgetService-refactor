#region

using System;

#endregion

namespace BudgetSystem
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        public decimal OverlappingAmount(Period period)
        {
            return period.OverlappingDays(CreatePeriod()) * DailyAmount();
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private decimal DailyAmount()
        {
            return Amount / (decimal)Days();
        }

        private int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        private DateTime LastDay()
        {
            return DateTime.ParseExact(YearMonth + Days(), "yyyyMMdd", null);
        }
    }
}