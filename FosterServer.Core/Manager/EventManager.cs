using FosterServer.Core.Enumerations;
using FosterServer.Core.Logging;
using FosterServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace FosterServer.Core.Manager
{
    public class EventManager
    {
        #region Private Members

        private Dictionary<string, EventManagerParameter> m_Events;
        private static EventManager m_EventsManager;

        #endregion

        #region Properties
        public static EventManager Instance 
        { 
            get 
            { 
                if(m_EventsManager == null)
                {
                    m_EventsManager = new EventManager();
                    m_EventsManager.Init();
                }
                return m_EventsManager;
            } 
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize Event Manager for Game
        /// </summary>
        public void Init()
        {
            m_Events = new Dictionary<string, EventManagerParameter>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start Listening For An Event
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        public void StartListening(string a_EventName, UnityAction<GameParameters> listener)
        {
            EventManagerParameter uEvent = null;
            if (m_Events.TryGetValue(a_EventName, out uEvent))
            {
                uEvent.AddListener(listener);
            }
            else
            {
                uEvent = new EventManagerParameter();
                uEvent.AddListener(listener);
                m_Events.Add(a_EventName, uEvent);
            }
        }

        /// <summary>
        /// Start Listening For An Event
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        public void StartListening(EventManagerEvent a_EventName, UnityAction<GameParameters> listener)
        {
            StartListening(a_EventName.Name(), listener);
        }

        /// <summary>
        /// Start Listening For An Event.
        /// Logs Id for Listener
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        /// <param name="listenerId"></param>
        public void StartListening(EventManagerEvent a_EventName, UnityAction<GameParameters> listener, Guid listenerId)
        {
            StartListening(a_EventName.Name(), listener, listenerId);
        }

        /// <summary>
        /// Start Listening For An Event.
        /// Logs Id for Listener
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        /// <param name="listenerId"></param>
        public void StartListening(string a_EventName, UnityAction<GameParameters> listener, Guid listenerId)
        {
            FosterLog.Log($"EntityId - {listenerId} STARTED listening on {a_EventName}");
            StartListening(a_EventName, listener);
        }

        /// <summary>
        /// Stop Listening For An Event
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        public void StopListening(string a_EventName, UnityAction<GameParameters> listener)
        {
            EventManagerParameter uEvent = null;

            if (m_Events.TryGetValue(a_EventName, out uEvent))
            {
                uEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Stop Listening For An Event
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        public void StopListening(EventManagerEvent a_EventName, UnityAction<GameParameters> listener)
        {
            StopListening(a_EventName.Name(), listener);
        }

        /// <summary>
        /// Stop Listening For An Event and Logs
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        /// <param name="listenerId"></param>
        public void StopListening(EventManagerEvent a_EventName, UnityAction<GameParameters> listener, Guid listenerId)
        {
            StopListening(a_EventName.Name(), listener, listenerId);
        }

        /// <summary>
        /// Stop Listening For An Event and Logs
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="listener"></param>
        /// <param name="listenerId"></param>
        public void StopListening(string a_EventName, UnityAction<GameParameters> listener, Guid listenerId)
        {
            FosterLog.Log($"EntityId - {listenerId} STOPPED listening on {a_EventName}");
            StopListening(a_EventName, listener);
        }

        /// <summary>
        /// Trigger An Event
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="data"></param>
        public void TriggerEvent(string a_EventName, GameParameters data)
        {
            EventManagerParameter uEvent = null;

            if (m_Events.TryGetValue(a_EventName, out uEvent))
            {
                uEvent.Invoke(data);
            }
        }

        /// <summary>
        /// Trigger An Event
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="data"></param>
        public void TriggerEvent(EventManagerEvent a_EventName, GameParameters a_Data)
        {
            TriggerEvent(a_EventName.Name(), a_Data);
        }

        /// <summary>
        /// Trigger An Event and Log
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="a_Data"></param>
        /// <param name="a_ListenerId"></param>
        public void TriggerEvent(EventManagerEvent a_EventName, GameParameters a_Data, Guid a_ListenerId)
        {
            TriggerEvent(a_EventName.Name(), a_Data, a_ListenerId);
        }

        /// <summary>
        /// Trigger An Event and Log
        /// </summary>
        /// <param name="a_EventName"></param>
        /// <param name="a_Data"></param>
        /// <param name="a_ListenerId"></param>
        public void TriggerEvent(string a_EventName, GameParameters a_Data, Guid a_ListenerId)
        {
            FosterLog.Log($"EntityId - {a_ListenerId} TRIGGERED listening on {a_EventName}");
            TriggerEvent(a_EventName, a_Data);
        }
        #endregion
    }
}
