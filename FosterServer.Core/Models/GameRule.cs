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
            else
            {
                StartRule();
            }
        }
        public abstract Result<bool> Execute(GameParameters data);

        public abstract Result<bool> Validate(GameParameters data);

        public void ExecuteRule(GameParameters data)
        {
            Result result = Validate(data);
            if (!result.IsSuccess)
            {
                FosterLog.Error(result.Message);
                return;
            }

            result = Execute(data);

            if (!result.IsSuccess)
            {
                FosterLog.Error(result.Message);
            }
        }

        public void StartRule()
        {
            EventManager.Instance.StartListening("GameEngineRule_"+RuleName, ExecuteRule);
        }

        public void StopRule()
        {
            EventManager.Instance.StopListening("GameEngineRule_" + RuleName, ExecuteRule);
        }

        public Result RemoveRule()
        {
            var result = RuleManager.Instance.RemoveRule(this);
            if (result.IsSuccess)
            {
                StopRule();
            }
            return result;
        }
    }
}
