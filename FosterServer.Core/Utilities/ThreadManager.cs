using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Utilities
{
    public class ThreadManager
    {
        private static readonly List<Action> m_executeOnMainThread = new List<Action>();
        private static readonly List<Action> m_executeCopiedOnMainThread = new List<Action>();
        private static bool m_actionToExecuteOnMainThread = false;

        public static void ExecuteOnMainThread(Action a_action)
        {
            if(a_action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
                return;
            }

            lock (m_executeOnMainThread)
            {
                m_executeOnMainThread.Add(a_action);
                m_actionToExecuteOnMainThread = true;
            }
        }

        public static void UpdateMain()
        {
            if (m_actionToExecuteOnMainThread)
            {
                m_executeCopiedOnMainThread.Clear();
                lock (m_executeOnMainThread)
                {
                    m_executeCopiedOnMainThread.AddRange(m_executeOnMainThread);
                    m_executeOnMainThread.Clear();
                    m_actionToExecuteOnMainThread = false;
                }
                for(int i = 0; i< m_executeCopiedOnMainThread.Count; i++)
                {
                    m_executeCopiedOnMainThread[i]();
                }
            }
        }
    }
}
