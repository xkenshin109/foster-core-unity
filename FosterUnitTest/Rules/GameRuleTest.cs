using FosterServer.Core.DataModels;
using FosterServer.Core.Enumerations;
using FosterServer.Core.Manager;
using FosterServer.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterUnitTest.Rules
{
    [TestClass]
    public class GameRuleTest
    {
        public GameRule m_validGameRule;
        public GameRule m_invalidGameRule;

        [TestMethod]
        public void GameRule_initialized_with_no_rules()
        {
            //SETUP

            //ACT
            var totalrules = RuleManager.Instance.Rules.Count;

            //ASSERT
            Assert.AreEqual(0, totalrules);
        }
        
        [TestMethod]
        public void GameRule_initialized_with_two_rules()
        {
            //SETUP
            m_validGameRule = new TestRule_Valid();
            m_invalidGameRule = new TestRule_Invalid();

            //ACT
            var totalrules = RuleManager.Instance.Rules.Count;

            //ASSERT
            Assert.AreEqual(2, totalrules);

            //CLEANUP
            m_validGameRule.RemoveRule();
            m_invalidGameRule.RemoveRule();
        }

        [TestMethod]
        public void GameRule_executed_valid()
        {
            //SETUP
            m_validGameRule = new TestRule_Valid();

            //ACT
            var totalrules = RuleManager.Instance.Rules.Count;
            var validateResult = m_validGameRule.Validate(null);
            var executeResult = m_validGameRule.Execute(null);

            //ASSERT
            Assert.AreEqual(1, totalrules);
            Assert.AreEqual(true, validateResult.Value);
            Assert.AreEqual(true, executeResult.Value);

            //CLEANUP
            m_validGameRule.RemoveRule();
        }

        [TestMethod]
        public void GameRule_executed_invalid()
        {
            //SETUP
            m_invalidGameRule = new TestRule_Invalid();

            //ACT
            var totalrules = RuleManager.Instance.Rules.Count;
            var validateResult = m_invalidGameRule.Validate(null);
            var executeResult = m_invalidGameRule.Execute(null);

            //ASSERT
            Assert.AreEqual(1, totalrules);
            Assert.AreEqual(false, validateResult.Value);
            Assert.AreEqual(false, executeResult.Value);

            //CLEANUP
            m_invalidGameRule.RemoveRule();
        }
    }
    public class TestRule_Valid : GameRule
    {
        public TestRule_Valid(string a_ruleName = "TestRule_Valid", ExecutionType a_type = ExecutionType.OnCall, Priority a_priority = Priority.Low)
            :base(a_ruleName, a_type, a_priority)
        {

        }
        public override Result<bool> Execute(object data)
        {
            return Result<bool>.Valid(true);
        }

        public override Result<bool> Validate(object data)
        {
            return Result<bool>.Valid(true);
        }
    }

    public class TestRule_Invalid : GameRule
    {
        public TestRule_Invalid(string a_ruleName = "TestRule_Invalid", ExecutionType a_type = ExecutionType.OnCall, Priority a_priority = Priority.Low)
            : base(a_ruleName, a_type, a_priority)
        {

        }
        public override Result<bool> Execute(object data)
        {
            return Result<bool>.Valid(false);
        }

        public override Result<bool> Validate(object data)
        {
            return Result<bool>.Valid(false);
        }
    }
}