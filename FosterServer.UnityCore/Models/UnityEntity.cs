using FosterServer.Core.Models;
using FosterServer.UnityCore.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Vector2Int m_Position;
        public Size m_Size;
        public bool m_canInteract;
        public bool m_isPassable;
        public float m_rotation;

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
            StartListener();
            EventsManager.Instance.TriggerEvent(Core.Enumerations.EventManagerEvent.Created, new EntityModel { Entity = Entity }, EntityId);
        }

        #endregion
    }
}
