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
        public List<Employees_Bosses> Employees_Bosses { get; set; }
    }

    class Boss
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Note> Notes { get; set; }
    }

    class Employees_Bosses
    {
        public int ID { get; set; }
        public int Boss_ID { get; set; }
        public int Employee_ID { get; set; }
        public Boss Boss { get; set; }
        public Employee Employee { get; set; }

    }

    class Note
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
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
        private List<Note> _notes;

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
            _notes = new List<Note>() { new Note() { ID = 13, Body = "Some note body", Title = "Some note title" } };

            var bossA = new Boss(){ID= 7, Name="A Boss", Notes = _notes };
            var bossB = new Boss(){ID= 8, Name="B Boss", Notes = _notes };
            var bossC = new Boss(){ID= 9, Name="C Boss", Notes = _notes };
            var bossD = new Boss(){ID=10, Name="D Boss", Notes = _notes };
            var bossE = new Boss(){ID=11, Name="E Boss", Notes = _notes };
            var bossF = new Boss(){ID=12, Name="F Boss", Notes = _notes };
            var empA = new Employee(){ID=1, Name="A", Bosses = new List<Boss>(){bossA} };
            var empB = new Employee(){ID=2, Name="B", Bosses = new List<Boss>(){bossB} };
            var empC = new Employee(){ID=3, Name="C", Bosses = new List<Boss>(){bossC} };
            var empD = new Employee(){ID=4, Name="D", Bosses = new List<Boss>(){bossD} };
            var empE = new Employee(){ID=5, Name="E", Bosses = new List<Boss>(){bossE} };
            var empF = new Employee(){ID=6, Name="F", Bosses = new List<Boss>(){bossF} };

            var empl_boss = new Employees_Bosses() { ID = 1, Boss = bossA, Boss_ID = bossA.ID, Employee = empA, Employee_ID = empA.ID };
            empA.Employees_Bosses = new List<Employees_Bosses>() { empl_boss };

            _sourceT = (new List<Employee>() { empA, empB, empC, empD, empE, empF }).AsQueryable();
            _source = _sourceT;
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }
        
        #endregion

        /// <summary>
        ///A test for selecting properties from a singular object
        ///</summary>
        [TestMethod()]
        public void SelectFromObjectTests()
        {
            var actual = _sourceT.First().Select("new (Name)");
            Assert.AreEqual("A", actual.Name);
            try
            {
                var id = actual.ID;
                Assert.Fail("Property ID should not be on dynamic object");
            }catch(Exception){}
        }

        /// <summary>
        ///A test for Select
        ///</summary>
        [TestMethod()]
        public void SelectTest()
        {
            var actual = _source.Select("new (ID)");
            var emp = actual.First(e => e.ID == 1);
            Assert.AreEqual(1, emp.ID);
        }

        /// <summary>
        ///A test for Select<T>
        ///</summary>
        [TestMethod()]
        public void SelectTestT()
        {
            var actual = _sourceT.Select("new (ID)", ""); // TODO - try to remove the need to pass another param to differentiate
            var emp = actual.First(e => e.ID == 1);
            Assert.AreEqual(1, emp.ID);
        }

        /// <summary>
        ///A test for string select aggregate
        ///</summary>
        [TestMethod()]
        public void SelectStringTest()
        {
            var actual = _source.Select("new (ID, Bosses.select(new (ID, Name)) as Bosses)");
            var emp = actual.First(e => e.ID == 1);
            IEnumerable<dynamic> bosses = emp.Bosses;
            Assert.AreEqual(bosses.First().Name, "A Boss");
        }

        /// <summary>
        ///A test for string select aggregate
        ///</summary>
        [TestMethod()]
        public void SubSelectStringTest()
        {
            var actual = _source.Select("new (ID, Bosses.select(new (ID, Name, Notes.select(new (ID, Title)) as Notes)) as Bosses)");
            var emp = actual.First(e => e.ID == 1);
            IEnumerable<dynamic> bosses = emp.Bosses;
            IEnumerable<dynamic> notes = bosses.First().Notes;
            Assert.AreEqual(_notes.First().Title, notes.First().Title);
        }

        /// <summary>
        ///A test for string select aggregate
        ///</summary>
        [TestMethod()]
        public void SubSelectStringSameNameTest()
        {
            var actual = _source.Select("new (ID, Employees_Bosses.select(new (ID, Boss.select(new (ID, Name)) as Boss)) as Employees_Bosses)");
            var emp = actual.First(e => e.ID == 1);
            IEnumerable<dynamic> employeesBosses = emp.Employees_Bosses;  
            var boss = employeesBosses.First().Boss;
            Assert.AreEqual("A Boss", boss.Name);
        }

        /// <summary>
        ///A test for Any
        ///</summary>
        [TestMethod()]
        public void AnyTest()
        {
            IQueryable emptySource = (new List<Employee>()).AsQueryable();
            bool actual = _source.Any();
            bool actualEmpty = emptySource.Any();
            Assert.IsTrue(actual);
            Assert.IsFalse(actualEmpty);
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        public void CountTest()
        {
            int count = _source.Count();
            Assert.AreEqual(count, 6);
        }

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

        /// <summary>
        ///A test for Take
        ///</summary>
        [TestMethod()]
        public void TakeTest()
        {
            int count = 2;
            var actual = _sourceT.Take(count);
            var first = actual.First();
            var second = actual.Skip(1).First();
            Assert.AreEqual("A", first.Name);
            Assert.AreEqual("B", second.Name);
        }

        /// <summary>
        ///A test for Skip
        ///</summary>
        [TestMethod()]
        public void SkipTest()
        {
            int count = 2;
            var actual = _sourceT.Skip(count);
            var first = actual.First();
            var second = actual.Skip(1).First();
            Assert.AreEqual("C", first.Name);
            Assert.AreEqual("D", second.Name);
        }

        /// <summary>
        ///A test for Where
        ///</summary>
        [TestMethod()]
        public void WhereTestT()
        {
            var actual = _sourceT.Where("ID == 1");
            Assert.AreEqual("A", actual.First().Name);
        }

        /// <summary>
        ///A test for Where
        ///</summary>
        [TestMethod()]
        public void WhereTest()
        {
            var actual = _source.Where("ID == 1");
            Assert.AreEqual("A", actual.First().Name);
        }
    }
}
