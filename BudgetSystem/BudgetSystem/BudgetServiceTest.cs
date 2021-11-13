using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace BudgetSystem
{
    public class BudgetServiceTest
    {
        private IBudgetRepo _budgetRepo;
        private BudgetService _budgetService;

        [SetUp]
        public void Setup()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_budgetRepo);
        }

        [Test]
        public void StartDateBiggerThanEndDate()
        {
            var query = _budgetService.Query(new DateTime(2021, 12, 01), new DateTime(2021, 11, 01));
            Assert.AreEqual(0, query);
        }
        
        [Test]
        public void WholeMonth()
        {
            GenerateFakeData();
            var query = _budgetService.Query(new DateTime(2021, 11, 01), new DateTime(2021, 11, 30));
            Assert.AreEqual(3000, query);
        }

        private void GenerateFakeData()
        {
            _budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget()
                {
                    YearMonth = "202111",
                    Amount = 3000
                }
            });
        }
    }
}