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

        public int OverlappingDays(Period another)
        {
            if (IsInvalid() || HasNoOverlapping(another))
            {
                return 0;
            }

            var overlappingStart = Start > another.Start
                ? Start
                : another.Start;

            var overlappingEnd = End < another.End
                ? End
                : another.End;

            return (overlappingEnd - overlappingStart).Days + 1;
        }

        private bool HasNoOverlapping(Period another)
        {
            return End < another.Start || Start > another.End;
        }

        private bool IsInvalid()
        {
            return Start > End;
        }
    }
}