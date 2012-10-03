using System.Linq.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace System.Linq.Dynamic.Tests
{
    class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Boss> Bosses { get; set; }
    }

    class Boss
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    
    /// <summary>
    ///This is a test class for DynamicQueryableTest and is intended
    ///to contain all DynamicQueryableTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DynamicQueryableTest
    {


        private TestContext testContextInstance;
        private IQueryable _source;
        private IQueryable<Employee> _sourceT;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _sourceT = (new List<Employee>() { 
                new Employee(){ID=1, Name="A", Bosses = new List<Boss>(){new Boss(){ID=7, Name="A Boss"}}},
                new Employee(){ID=2, Name="B"},
                new Employee(){ID=3, Name="C"},
                new Employee(){ID=4, Name="D"},
                new Employee(){ID=5, Name="E"},
                new Employee(){ID=6, Name="F"}
            }).AsQueryable();
            _source = _sourceT;
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }
        
        #endregion


        /// <summary>
        ///A test for Select
        ///</summary>
        [TestMethod()]
        public void SelectTest()
        {
            IQueryable actual = _source.Select("new (id)", "where(p=>true)");
            Assert.AreEqual(actual.Count(), _source.Count());
        }

        /// <summary>
        ///A test for Select<T>
        ///</summary>
        [TestMethod()]
        public void SelectTestT()
        {
            SelectTestTHelper<Employee>();
        }

        public void SelectTestTHelper<TResult>()
        {
            IQueryable actual = _source.Select("new (id, bosses.select(new (id, name)) as bosses)", "where(p=>true)");
            Assert.AreEqual(actual.Count(), _source.Count());
        }

        ///// <summary>
        /////A test for Any
        /////</summary>
        //[TestMethod()]
        //public void AnyTest()
        //{
        //    IQueryable source = null; // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = DynamicQueryable.Any(source);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Count
        /////</summary>
        //[TestMethod()]
        //public void CountTest()
        //{
        //    IQueryable source = null; // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = DynamicQueryable.Count(source);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GroupBy
        /////</summary>
        //[TestMethod()]
        //public void GroupByTest()
        //{
        //    IQueryable source = null; // TODO: Initialize to an appropriate value
        //    string keySelector = string.Empty; // TODO: Initialize to an appropriate value
        //    string elementSelector = string.Empty; // TODO: Initialize to an appropriate value
        //    object[] values = null; // TODO: Initialize to an appropriate value
        //    IQueryable expected = null; // TODO: Initialize to an appropriate value
        //    IQueryable actual;
        //    actual = DynamicQueryable.GroupBy(source, keySelector, elementSelector, values);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for OrderBy
        /////</summary>
        //[TestMethod()]
        //public void OrderByTest()
        //{
        //    IQueryable source = null; // TODO: Initialize to an appropriate value
        //    string ordering = string.Empty; // TODO: Initialize to an appropriate value
        //    object[] values = null; // TODO: Initialize to an appropriate value
        //    IQueryable expected = null; // TODO: Initialize to an appropriate value
        //    IQueryable actual;
        //    actual = DynamicQueryable.OrderBy(source, ordering, values);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for OrderBy
        /////</summary>
        //public void OrderByTest1Helper<T>()
        //{
        //    IQueryable<T> source = null; // TODO: Initialize to an appropriate value
        //    string ordering = string.Empty; // TODO: Initialize to an appropriate value
        //    object[] values = null; // TODO: Initialize to an appropriate value
        //    IQueryable<T> expected = null; // TODO: Initialize to an appropriate value
        //    IQueryable<T> actual;
        //    actual = DynamicQueryable.OrderBy<T>(source, ordering, values);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //[TestMethod()]
        //public void OrderByTest1()
        //{
        //    OrderByTest1Helper<GenericParameterHelper>();
        //}

        ///// <summary>
        /////A test for Take
        /////</summary>
        //[TestMethod()]
        //public void TakeTest()
        //{
        //    IQueryable source = null; // TODO: Initialize to an appropriate value
        //    int count = 0; // TODO: Initialize to an appropriate value
        //    IQueryable expected = null; // TODO: Initialize to an appropriate value
        //    IQueryable actual;
        //    actual = DynamicQueryable.Take(source, count);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Skip
        /////</summary>
        //[TestMethod()]
        //public void SkipTest()
        //{
        //    IQueryable source = null; // TODO: Initialize to an appropriate value
        //    int count = 0; // TODO: Initialize to an appropriate value
        //    IQueryable expected = null; // TODO: Initialize to an appropriate value
        //    IQueryable actual;
        //    actual = DynamicQueryable.Skip(source, count);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Where
        /////</summary>
        //public void WhereTestHelper<T>()
        //{
        //    IQueryable<T> source = null; // TODO: Initialize to an appropriate value
        //    string predicate = string.Empty; // TODO: Initialize to an appropriate value
        //    object[] values = null; // TODO: Initialize to an appropriate value
        //    IQueryable<T> expected = null; // TODO: Initialize to an appropriate value
        //    IQueryable<T> actual;
        //    actual = DynamicQueryable.Where<T>(source, predicate, values);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //[TestMethod()]
        //public void WhereTest()
        //{
        //    WhereTestHelper<GenericParameterHelper>();
        //}

        ///// <summary>
        /////A test for Where
        /////</summary>
        //[TestMethod()]
        //public void WhereTest1()
        //{
        //    IQueryable source = null; // TODO: Initialize to an appropriate value
        //    string predicate = string.Empty; // TODO: Initialize to an appropriate value
        //    object[] values = null; // TODO: Initialize to an appropriate value
        //    IQueryable expected = null; // TODO: Initialize to an appropriate value
        //    IQueryable actual;
        //    actual = DynamicQueryable.Where(source, predicate, values);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
