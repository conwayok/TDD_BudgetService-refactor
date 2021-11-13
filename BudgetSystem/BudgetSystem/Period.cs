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
            var firstDay = budget.FirstDay();
            var lastDay = budget.LastDay();
            var overlappingStart = Start > firstDay
                ? Start
                : firstDay;

            var overlappingEnd = End < lastDay
                ? End
                : lastDay;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}