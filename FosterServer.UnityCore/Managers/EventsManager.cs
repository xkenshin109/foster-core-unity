using FosterServer.Core.Enumerations;
using FosterServer.Core.Logging;
using FosterServer.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace FosterServer.UnityCore.Managers
{
    [RequireComponent(typeof(MouseManager))]
    [RequireComponent(typeof(KeyboardManager))]
    public class EventsManager : MonoBehaviour
    {
        #region Events

        #endregion

        #region Private Members

        private static EventManager m_EventsManager;

        #endregion

        #region Public Members

        public static float m_speed = 2;

        #endregion

        #region Properties

        public static EventManager Instance
        {
            get
            {
                if (m_EventsManager == null)
                {
                    var eventManager = FindObjectsOfType<EventsManager>();
                    if (eventManager == null)
                    {
                        Debug.LogError($"There needs to be on active '{nameof(EventsManager)}' script in a GameObject");
                        return null;
                    }
                    else if(eventManager.Length > 1)
                    {
                        Debug.LogError($"There needs to be only ONE active '{nameof(EventsManager)}' script in a GameObject");
                        return null;
                    }
                    else
                    {
                        m_EventsManager = new EventManager();
                        m_EventsManager.Init();
                    }
                }
                return m_EventsManager;

            }
        }

        #endregion

        #region Constructors

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

        #region Unity Methods

        private void OnDestroy()
        {
            //foreach (var key in Instance.m_Events.Keys)
            //{
            //    EventManagerParameter uEvent = Instance.m_Events[key];
            //    uEvent.RemoveAllListeners();
            //}
        }

        #endregion
    }

}
