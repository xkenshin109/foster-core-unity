using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Manager
{
    public class GameEngine
    {
        private static List<Tuple<string, Type, object>> m_gameProperties = new List<Tuple<string, Type, object>>();

        /// <summary>
        /// Add Game Property for Game Entities
        /// </summary>
        /// <param name="a_propertyName"></param>
        /// <param name="a_type"></param>
        /// <param name="a_defaultValue"></param>
        public static void AddGameProperty(string a_propertyName, Type a_type, object a_defaultValue)
        {
            m_gameProperties.Add(new Tuple<string, Type, object>(a_propertyName, a_type, a_defaultValue));
        }

        /// <summary>
        /// Get Properties and Default Values for Game Entities
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> GetProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            m_gameProperties.ForEach(x => dictionary.Add(x.Item1, x.Item3));
            return dictionary;
        }
        
        /// <summary>
        /// Get Property Types for the Game Entities
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Type> GetPropertyTypes()
        {
            Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
            m_gameProperties.ForEach(x => dictionary.Add(x.Item1, x.Item2));
            return dictionary;
        }

        /// <summary>
        /// Remove and Clear all Game Properties. Used for resetting game
        /// </summary>
        public static void ClearGameProperties()
        {
            m_gameProperties.Clear();
        }
    }
}
