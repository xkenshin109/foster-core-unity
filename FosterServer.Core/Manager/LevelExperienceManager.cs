using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Manager
{
    public class LevelExperienceManager
    {
        private int m_totalLevels = 10;
        private static LevelExperienceManager m_manager;
        public static LevelExperienceManager Manager 
        { 
            get 
            { 
                if(m_manager == null)
                {
                    m_manager = new LevelExperienceManager();
                }
                return m_manager;
            } 
        }

        public LevelExperienceManager()
        {
            InitializeExperienceGrid();
        }

        public void InitializeExperienceGrid()
        {
            float m_baseExp = 1000;
            for (int x = 1; x <= m_totalLevels; x++) 
            {
                ExperienceToLevel.Add(x, m_baseExp);
                m_baseExp = m_baseExp * 3 * m_totalLevels / 500;
            }
        }
        private Dictionary<int, float> ExperienceToLevel = new Dictionary<int, float>();

        public int GetLevel(float a_experience)
        {
            foreach (int key in ExperienceToLevel.Keys)
            {
                if (a_experience <= ExperienceToLevel[key])
                {
                    return key;
                }
            }
            return 1;
        }
    }
}
