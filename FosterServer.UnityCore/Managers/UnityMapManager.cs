using FosterServer.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FosterServer.Core.Enumerations;
using FosterServer.Core.Models;
using FosterServer.Core.Logging;
using UnityEngine.Tilemaps;
using System.Diagnostics;
using FosterServer.UnityCore.Models;

namespace FosterServer.UnityCore.Managers
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(Grid))]
    [RequireComponent(typeof(TilemapRenderer))]
    [DebuggerDisplay("MapManager: MapId({MapId}) GameEntities({GameMap.TotalEntities})")]
    public class UnityMapManager : MonoBehaviour
    {
        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Properties

        /// <summary>
        /// Game Map Entity
        /// </summary>
        public MapManager GameMap = new MapManager();

        /// <summary>
        /// Map ID
        /// </summary>
        public Guid MapId => GameMap.MapId;

        #endregion

        #region Private Methods

        private void LoadPreloadedGameEntities()
        {
            string a_method = "LoadPreloadedGameEntities";
            foreach (Transform children in this.transform)
            {
                UnityEntity childEntity;
                if (children.TryGetComponent<UnityEntity>(out childEntity))
                {
                    FosterLog.Log($"{a_method} - Game Entity Found {children.name}");
                    GameMap.AddGameEntity(new EntityModel { Entity = childEntity.Entity});
                }
            }
        }

        private void Awake()
        {
            LoadPreloadedGameEntities();
            StartListening();
        }

        private void OnDestroy()
        {
            StopListening();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Map Manager - Attach Game Logic to Listeners
        /// Core Events for Game Engine
        /// </summary>
        public void StartListening()
        {
            EventsManager.Instance.StartListening(EventManagerEvent.Created, GameMap.AddGameEntity, MapId);
            EventsManager.Instance.StartListening(EventManagerEvent.Action, GameMap.GameEntityAction, MapId);
            EventsManager.Instance.StartListening(EventManagerEvent.Position, GameMap.GameEntityPosition, MapId);
            EventsManager.Instance.StartListening(EventManagerEvent.Removed, GameMap.GameEntityRemoved, MapId);
        }

        /// <summary>
        /// Map Manager - Attach Game Logic to Stop Listening
        /// </summary>
        public void StopListening()
        {
            EventsManager.Instance.StopListening(EventManagerEvent.Created, GameMap.AddGameEntity, MapId);
            EventsManager.Instance.StopListening(EventManagerEvent.Action, GameMap.GameEntityAction, MapId);
            EventsManager.Instance.StopListening(EventManagerEvent.Position, GameMap.GameEntityPosition, MapId);
            EventsManager.Instance.StopListening(EventManagerEvent.Removed, GameMap.GameEntityRemoved, MapId);
        }

        #endregion
    }
}
