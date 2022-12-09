using FosterServer.Core.DataModels;
using FosterServer.Core.Interface;
using FosterServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Manager
{
    public class RuleManager
    {
        private List<IRules> m_rules = new List<IRules>();
        private static RuleManager m_ruleManager;

        public List<IRules> Rules
        {
            get
            {
                if(m_rules == null)
                {
                    m_rules = new List<IRules>();
                }
                return m_rules;
            }
        }


        public static RuleManager Instance
        {
            get 
            { 
                if (m_ruleManager == null)
                {
                    m_ruleManager = new RuleManager();
                }
                return m_ruleManager;
            }
        }

        public RuleManager()
        {
            m_rules = new List<IRules>();
        }

        public Result AddRule(GameRule a_rule)
        {
            var result = RuleExists(a_rule);
            if(result.IsSuccess)
            {
                return Result.Error($"Game Rule: {a_rule.RuleName} already exists in RuleManager");
            }
            m_rules.Add(a_rule);
            return Result.Valid();
        }

        public Result RemoveRule(GameRule a_rule)
        {
            var result = RuleExists(a_rule);
            if (result.IsSuccess)
            {
                m_rules.Remove(a_rule);
            }
            return result;
        }

        public Result RuleExists(GameRule a_rule)
        {
            if (m_rules.Find(x => x.RuleName == a_rule.RuleName) == null)
            {
                return Result.Error("No Game Rule Found");
            }
            return Result<bool>.Valid(true);
        }
    }
}
