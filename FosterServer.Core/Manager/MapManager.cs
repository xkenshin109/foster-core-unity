using FosterServer.Core.Logging;
using FosterServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Manager
{
    [DebuggerDisplay("MapManager: MapId({MapId}) GameEntities({TotalEntities})")]
    public class MapManager : IDisposable
    {
        #region Private Members

        private Guid m_mapId;
        private List<GameEntity> m_gameEntities = new List<GameEntity>();

        #endregion

        #region Public Members

        #endregion

        #region Properties

        /// <summary>
        /// Total Entities on Map
        /// </summary>
        public int TotalEntities => GameEntities.Count();

        /// <summary>
        /// Game Entities associated to the Map
        /// </summary>
        public List<GameEntity> GameEntities
        {
            get
            {
                return m_gameEntities;
            }
        }

        /// <summary>
        /// Map Id to attach Event Listeners for Game Engine
        /// </summary>
        public Guid MapId
        {
            get
            {
                if (m_mapId == Guid.Empty)
                {
                    m_mapId = Guid.NewGuid();
                }

                return m_mapId;
            }
        }

        #endregion

        #region Constructors

        public MapManager()
        {

        }

        #endregion

        #region Private Methods

        #endregion

        #region Unity Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Add Game Entities for the Current Map/Level
        /// </summary>
        /// <param name="a_entity"></param>
        public void AddGameEntity(object a_entity)
        {
            EntityModel addEntity = (EntityModel)a_entity;
            if (!m_gameEntities.Any(x => x.EntityId == addEntity.Entity.EntityId))
            {
                m_gameEntities.Add(addEntity.Entity);
            }
        }

        /// <summary>
        /// Game Entity Action 
        /// </summary>
        /// <param name="a_entity"></param>
        public void GameEntityAction(object a_entity)
        {

        }

        /// <summary>
        /// Game Entity Position Changed
        /// </summary>
        /// <param name="a_entity"></param>
        public void GameEntityPosition(object a_entity)
        {
            EntityModel entity = (EntityModel)a_entity;
            GameEntity gameEntity = m_gameEntities.FirstOrDefault(x => x.EntityId == entity.Entity.EntityId);
            if (gameEntity != null)
            {
                gameEntity.SetPosition(entity.Entity.Vector3Position);
            }
        }

        /// <summary>
        /// Game Entity Removed
        /// </summary>
        /// <param name="a_entity"></param>
        public void GameEntityRemoved(object a_entity)
        {
            EntityModel entity = (EntityModel)a_entity;
            if (m_gameEntities.Any(x => x.EntityId == entity.Entity.EntityId))
            {
                m_gameEntities.Remove(entity.Entity);
            }
        }

        public void Dispose()
        {

        }

        #endregion


    }
}
