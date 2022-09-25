using FosterServer.Core.Logging;
using FosterServer.Core.Models;
using FosterServer.Core.Utilities;
using FosterServer.UnityCore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FosterServer.UnityCore.Models
{
    public class UnityEntity : MonoBehaviour
    {
        #region Private Member

        #endregion

        #region Public Member
        private Vector2Int m_PreviousPosition;
        public Vector2Int m_Position;
        public Size m_Size;
        public bool m_canInteract;
        public bool m_isPassable;
        public float m_rotation;
        public GameEntity m_test;
        #endregion

        #region Properties

        public GameEntity Entity = new GameEntity();

        public Guid EntityId { get { return (Entity.EntityId); } }

        #endregion

        #region Constructors

        #endregion

        #region Public Methods
        public void StartListener()
        {

        }

        public void StopListener()
        {

        }

        #endregion

        #region Private Methods
        private void OnDestroy()
        {
            EventsManager.Instance.TriggerEvent(Core.Enumerations.EventManagerEvent.Removed, new EntityModel { Entity = Entity }, EntityId);
            StopListener();
        }

        private void Awake()
        {
            Entity.Initialize(m_Position.x, m_Position.y, m_Size, m_rotation, m_canInteract, m_isPassable);
            m_PreviousPosition = m_Position;
            StartListener();
            EventsManager.Instance.TriggerEvent(Core.Enumerations.EventManagerEvent.Created, new EntityModel { Entity = Entity }, EntityId);
        }

        private void LateUpdate()
        {
            if (!m_PreviousPosition.EqualsTo(m_Position))
            {
                Entity.SetPosition(m_Position.x, m_Position.y);
                m_PreviousPosition = m_Position;
            }
            if (!gameObject.transform.position.EqualsTo(Entity.Vector3Position))
            {
                Vector3 distance = gameObject.transform.position.CalculateMovement(Entity.Vector3Position, Time.deltaTime, EventsManager.m_speed);
                FosterLog.Log($"{EntityId} - Moving position");
                gameObject.transform.Translate(distance);
            }
            
        }

        #endregion
    }
}
