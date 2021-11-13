#region

using System;

#endregion

namespace BudgetSystem
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        private DateTime End { get; }
        private DateTime Start { get; }

        public int OverlappingDays(Budget budget)
        {
            DateTime overlappingStart;
            DateTime overlappingEnd;
            if (budget.YearMonth == Start.ToString("yyyyMM"))
            {
                overlappingStart = Start;
                overlappingEnd = budget.LastDay();
            }
            else if (budget.YearMonth == End.ToString("yyyyMM"))
            {
                overlappingStart = budget.FirstDay();
                overlappingEnd = End;
            }
            else
            {
                overlappingStart = budget.FirstDay();
                overlappingEnd = budget.LastDay();
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            return overlappingDays;
        }
    }
}