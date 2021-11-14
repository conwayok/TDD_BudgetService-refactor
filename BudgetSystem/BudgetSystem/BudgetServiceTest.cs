#region

using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace BudgetSystem
{
    [TestFixture]
    public class BudgetServiceTest
    {
        [SetUp]
        public void Setup()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_budgetRepo);
        }

        private IBudgetRepo _budgetRepo;
        private BudgetService _budgetService;

        [Test]
        public void cross_3_months()
        {
            GenerateFakeData(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget
                {
                    YearMonth = "202110",
                    Amount = 31
                },
                new Budget
                {
                    YearMonth = "202112",
                    Amount = 31000
                }
            });
            var query = _budgetService.Query(new DateTime(2021, 10, 31), new DateTime(2021, 12, 02));
            Assert.AreEqual(1 + 3000 + 2000, query);
        }

        [Test]
        public void EmptyData()
        {
            GenerateFakeData(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget
                {
                    YearMonth = "202112",
                    Amount = 31000
                }
            });
            var query = _budgetService.Query(new DateTime(2021, 10, 01), new DateTime(2021, 10, 31));
            Assert.AreEqual(0, query);
        }

        [Test]
        public void PartialMonth()
        {
            GenerateFakeData(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget
                {
                    YearMonth = "202112",
                    Amount = 31000
                }
            });
            var query = _budgetService.Query(new DateTime(2021, 11, 01), new DateTime(2021, 11, 10));
            Assert.AreEqual(1000, query);
        }

        [Test]
        public void SameDate()
        {
            GenerateFakeData(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget
                {
                    YearMonth = "202112",
                    Amount = 31000
                }
            });
            var query = _budgetService.Query(new DateTime(2021, 11, 01), new DateTime(2021, 11, 01));
            Assert.AreEqual(100, query);
        }

        [Test]
        public void StartDateBiggerThanEndDate()
        {
            var query = _budgetService.Query(new DateTime(2021, 12, 01), new DateTime(2021, 11, 01));
            Assert.AreEqual(0, query);
        }

        [Test]
        public void TwoWholeMonth()
        {
            GenerateFakeData(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget
                {
                    YearMonth = "202112",
                    Amount = 31000
                }
            });
            var query = _budgetService.Query(new DateTime(2021, 11, 01), new DateTime(2021, 12, 31));
            Assert.AreEqual(34000, query);
        }

        [Test]
        public void WholeMonth()
        {
            GenerateFakeData(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget
                {
                    YearMonth = "202112",
                    Amount = 31000
                }
            });
            var query = _budgetService.Query(new DateTime(2021, 11, 01), new DateTime(2021, 11, 30));
            Assert.AreEqual(3000, query);
        }

        private void GenerateFakeData(List<Budget> budgets)
        {
            _budgetRepo.GetAll()
                .Returns(budgets);
        }
    }
}