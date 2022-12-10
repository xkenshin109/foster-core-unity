using FosterServer.Core.DataModels;
using FosterServer.Core.Enumerations;
using FosterServer.Core.Logging;
using FosterServer.Core.Manager;
using FosterServer.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Models
{
    public abstract class GameRule : IRules
    {
        public static int TOTAL_RULES = 0;
        
        public int Id { get; set; }
        public string RuleName { get; set; }
        public ExecutionType ExecutionType { get; set; }
        public Priority Priority { get; set; }
        public bool RuleStarted
        { get { if (string.IsNullOrEmpty(RuleName)){ return false; } return RuleManager.Instance.RuleExists(this).IsSuccess; } }
        public GameRule(
            ExecutionType a_executionType = ExecutionType.OnCall, 
            Priority a_priority = Priority.Low)
        {
            Id = GameRule.TOTAL_RULES++;
            RuleName = this.GetType().Name;
            ExecutionType = a_executionType;
            Priority = a_priority;
            Result result = RuleManager.Instance.AddRule(this);
            if (!result.IsSuccess)
            {
                GameRule.TOTAL_RULES--;
                FosterLog.Error(result.Message);
            }
        }
        
        /// <summary>
        /// Execute Logic for Game Rule
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract Result<bool> Execute(GameParameters data);

        /// <summary>
        /// Validation Method for Game Rule
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract Result<bool> Validate(GameParameters data);

        /// <summary>
        /// Validate and Execute Rule: Returns Result on success/failure
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<bool> ExecuteRule(GameParameters data)
        {
            Result<bool> result = Validate(data);
            if (!result.IsSuccess)
            {
                FosterLog.Error(result.Message);
                return result;
            }

            result = Execute(data);

            if (!result.IsSuccess)
            {
                FosterLog.Error(result.Message);
            }

            return result;
        }

        public Result RemoveRule()
        {
            var result = RuleManager.Instance.RemoveRule(this);
            return result;
        }
    }
}
