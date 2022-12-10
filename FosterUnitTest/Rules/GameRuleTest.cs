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
            var executeResult = m_validGameRule.ExecuteRule(null);

            //ASSERT
            Assert.AreEqual(1, totalrules);
            Assert.AreEqual(true, executeResult.IsSuccess);

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
            var executeResult = m_invalidGameRule.ExecuteRule(null);

            //ASSERT
            Assert.AreEqual(1, totalrules);
            Assert.AreEqual(false, executeResult.IsSuccess);

            //CLEANUP
            m_invalidGameRule.RemoveRule();
        }

        [TestMethod]
        public void GameRule_execute_rule_valid()
        {
            //SETUP
            var executeRule = new TestRule_ValidRuleExecute();

            //ACT
            var totalrules = RuleManager.Instance.Rules.Count;
            var executeResult = executeRule.Execute(new ExecuteParams { Param1 = 1, Param2 = 2 });

            //ASSERT
            Assert.AreEqual(1, totalrules);
            Assert.AreEqual(true, executeResult.IsSuccess);

            //CLEANUP
            executeRule.RemoveRule();
        }

        [TestMethod]
        public void GameRule_execute_rule_invalid()
        {
            //SETUP
            var executeRule = new TestRule_ValidRuleExecute();

            //ACT
            var totalrules = RuleManager.Instance.Rules.Count;
            var param = new ExecuteParams { Param1 = 3, Param2 = 2 };
            var executeResult = executeRule.ExecuteRule(param);

            //ASSERT
            Assert.AreEqual(1, totalrules);
            Assert.AreEqual(false, executeResult.IsSuccess);

            //CLEANUP
            executeRule.RemoveRule();
        }
    }
    public class TestRule_Valid : GameRule
    {
        public TestRule_Valid(ExecutionType a_type = ExecutionType.OnCall, Priority a_priority = Priority.Low)
            :base(a_type, a_priority)
        {

        }
        public override Result<bool> Execute(GameParameters data)
        {
            var result = Validate(data);
            if (!result.IsSuccess)
            {
                return result;
            }
            return Result<bool>.Valid(true);
        }

        public override Result<bool> Validate(GameParameters data)
        {
            return Result<bool>.Valid(true);
        }
    }

    public class TestRule_Invalid : GameRule
    {
        public TestRule_Invalid(ExecutionType a_type = ExecutionType.OnCall, Priority a_priority = Priority.Low)
            : base(a_type, a_priority)
        {

        }
        public override Result<bool> Execute(GameParameters data)
        {
            var result = Validate(data);
            if (!result.IsSuccess)
            {
                return result;
            }
            return Result<bool>.Valid(true);
        }

        public override Result<bool> Validate(GameParameters data)
        {
            return Result<bool>.Error("Error");
        }
    }

    public class TestRule_ValidRuleExecute : GameRule
    {

        public TestRule_ValidRuleExecute(ExecutionType a_type = ExecutionType.OnCall, Priority a_priority = Priority.Low)
            : base(a_type, a_priority)
        {

        }
        public override Result<bool> Execute(GameParameters data)
        {
            var result = Validate(data);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result<bool>.Valid(true);
        }
        public override Result<bool> Validate(GameParameters data)
        {
            var a_params = data as ExecuteParams;
            if(a_params.Param1 > a_params.Param2)
            {
                return Result<bool>.Error("Validation Error: Param1 is greated than Param2");
            }
            return Result<bool>.Valid(true);
        }

    }
    public class ExecuteParams : GameParameters
    {
        public int Param1 { get; set; }
        public int Param2 { get; set; }

    }
}