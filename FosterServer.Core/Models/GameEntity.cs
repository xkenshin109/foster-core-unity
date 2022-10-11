using FosterServer.Core.Interface;
using FosterServer.Core.Logging;
using FosterServer.Core.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FosterServer.Core.Models
{
    [Serializable]
    [DebuggerDisplay("GameEntity: Id({EntityId}) Position({X},{Y}) Size({EntitySize.Width},{EntitySize.Height}) Passable({CanEntityPassThrough})")]
    public class GameEntity : IDisposable, IGameEntityManager
    {
        #region Private Members

        private Guid m_entityId;
        private GridPoint m_gridPoint;
        private Dictionary<string, object> m_gameProperties = new Dictionary<string, object>();
        #endregion

        #region Properties

        public Guid EntityId
        {
            get
            {
                if (m_entityId == Guid.Empty)
                {
                    m_entityId = Guid.NewGuid();
                }

                return m_entityId;
            }
        }

        /// <summary>
        /// Can an Entity Pass Through
        /// </summary>
        public bool CanEntityPassThrough
        {
            get
            {
                return Point.CanEntityPassThrough;
            }
        }

        /// <summary>
        /// Is Game Entity Interactable
        /// </summary>
        public bool IsInteractable
        {
            get
            {
                return Point.IsInteractable;
            }
        }

        /// <summary>
        /// Abstract Dictionary of Game Properties
        /// </summary>
        public Dictionary<string, object> GameProperties { get { return m_gameProperties; } }

        #region Position and Size Properties

        /// <summary>
        /// Game Entity's Size
        /// </summary>
        public Size EntitySize
        {
            get
            {
                return Point.EntitySize;
            }
        }

        /// <summary>
        /// Game Entity's Rotation
        /// </summary>
        public float Rotation
        {
            get
            {
                return Point.Rotation;
            }
        }

        ///// <summary>
        ///// Point where this entity exists on Grid
        ///// </summary>
        public GridPoint Point { get { return m_gridPoint; } }

        /// <summary>
        /// X Position of Entity
        /// </summary>
        public float? X => Point.X;

        /// <summary>
        /// Y Position of Entity
        /// </summary>
        public float? Y => Point.Y;

        /// <summary>
        /// Position of Game Entity
        /// </summary>
        public Vector2Int Vector2IntPosition
        {
            get
            {
                return new Vector2Int((int)Point.X, (int)Point.Y);
            }
        }

        /// <summary>
        /// Vector 2 Position
        /// </summary>
        public Vector2 Vector2Position
        {
            get
            {
                if (Point != null)
                {
                    return new Vector2((float)Point.X, (float)Point.Y);
                }
                return new Vector2(0, 0);
            }
        }

        /// <summary>
        /// Vector 3 Position
        /// </summary>
        public Vector3 Vector3Position
        {
            get
            {
                if (Point != null)
                {
                    return new Vector3((float)Point.X, (float)Point.Y);
                }
                return new Vector3(0, 0, 0);
            }
        }

        /// <summary>
        /// Vector 3 Int Position
        /// </summary>
        public Vector3Int Vector3IntPosition
        {
            get
            {
                if (Point != null)
                {
                    return new Vector3Int((int)Point.X, (int)Point.Y);
                }
                return new Vector3Int(0, 0, 0);
            }
        }
        #endregion

        #endregion

        #region Constructors

        public GameEntity()
            :this(0,0)
        {

        }

        public GameEntity(float a_x, float a_y)
            : this(a_x, a_y, new Size(1, 1))
        {

        }

        public GameEntity(float a_x, float a_y, Size a_entitySize)
            : this(a_x, a_y, a_entitySize, 0)
        {

        }
        
        public GameEntity(float a_x, float a_y, Size a_entitySize, float a_rotation)
            : this(a_x, a_y, a_entitySize, a_rotation, false, false)
        {

        }

        public GameEntity(float a_x, float a_y, Size a_entitySize, float a_rotation, bool a_isInteractable = false, bool a_canEntityPassThrough = false)
        {
            Initialize(a_x, a_y, a_entitySize, a_rotation, a_isInteractable, a_canEntityPassThrough);
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize Game Entity
        /// </summary>
        /// <param name="a_X"></param>
        /// <param name="a_Y"></param>
        /// <param name="a_entitySize"></param>
        /// <param name="a_rotation"></param>
        /// <param name="a_isInteractable"></param>
        /// <param name="a_canEntityPassThrough"></param>
        public void Initialize(float a_X, float a_Y, Size a_entitySize, float a_rotation = 0, bool a_isInteractable = false, bool a_canEntityPassThrough = false)
        {
            m_gridPoint = new GridPoint(a_X, a_Y, a_canEntityPassThrough, a_isInteractable, a_entitySize, a_rotation);

            var properties = GameEngine.GetProperties();

            foreach(var prop in properties.Keys)
            {
                AddGameProperty(prop, properties[prop]);
            }
        }

        /// <summary>
        /// Initializes Inventory for Game Entity - Loot Table
        /// </summary>
        public void InitializeInventory()
        {

        }

        /// <summary>
        /// Toggles Entities Ability to Pass Through
        /// </summary>
        public void ToggleCanEntityBePassedThrough()
        {
            Point.CanEntityPassThrough = !Point.CanEntityPassThrough;
        }

        /// <summary>
        /// Set Position of Entity
        /// </summary>
        /// <param name="a_position"></param>
        public void SetPosition(Vector3 a_position)
        {
            Point.SetLocation(a_position);
        }

        /// <summary>
        /// Set Position of Entity
        /// </summary>
        /// <param name="a_x"></param>
        /// <param name="a_y"></param>
        public void SetPosition(float a_x, float a_y, float a_z = 1)
        {
            SetPosition(new Vector3(a_x, a_y, a_z));
        }
        
        /// <summary>
        /// Set Rotation of Entity
        /// </summary>
        /// <param name="a_rotate"></param>
        public void SetRotation(float a_rotate)
        {
            Point.Rotation = a_rotate;
        }

        /// <summary>
        /// Validates if a Point has interesect this Entities Grid Points
        /// </summary>
        /// <param name="a_sourcePoint"></param>
        /// <returns></returns>
        public bool HasGridPointIntersect(GridPoint a_sourcePoint)
        {
            return a_sourcePoint.EqualsTo(Point);
        }

        /// <summary>
        /// Add Game Property for the GameEntity
        /// </summary>
        /// <param name="a_propertyName"></param>
        /// <param name="a_type"></param>
        /// <param name="a_defaultValue"></param>
        public void AddGameProperty(string a_propertyName, object a_defaultValue)
        {
            if (!GameProperties.ContainsKey(a_propertyName))
            {
                GameProperties.Add(a_propertyName, a_defaultValue);
            }
        }

        /// <summary>
        /// Set/Update Game Property for the Game Entity
        /// </summary>
        /// <param name="a_propertyName"></param>
        /// <param name="a_value"></param>
        public void SetGameProperty(string a_propertyName, object a_value)
        {
            Type type;
            if(GameEngine.GetPropertyTypes().TryGetValue(a_propertyName, out type) && GameProperties.ContainsKey(a_propertyName))
            {
                GameProperties[a_propertyName] = Convert.ChangeType(a_value, type);
            }
        }

        /// <summary>
        /// Change value for Game Entity Property
        /// </summary>
        /// <param name="a_propertyName"></param>
        /// <param name="a_value"></param>
        public void ChangeValueGameProperty(string a_propertyName, object a_value)
        {
            if (!GameProperties.ContainsKey(a_propertyName))
            {
                FosterLog.Log("No Property found on Game Entity");
                return;
            }
            object property = GameProperties[a_propertyName];

            Type type = property.GetType();
            if(type == typeof(int))
            {
                GameProperties[a_propertyName] = (int)property + (int)a_value;
            }
            else if(type == typeof(long))
            {
                GameProperties[a_propertyName] = (long)property + (long)a_value;
            }
            else if(type == typeof(short))
            {
                GameProperties[a_propertyName] = (short)property + (short)a_value;
            }
            else if(type == typeof(float))
            {
                GameProperties[a_propertyName] = (float)property + (float)a_value;
            }
            else if(type == typeof(double))
            {
                GameProperties[a_propertyName] = (double)property + (double)a_value;
            }
            else if(type == typeof(decimal))
            {
                GameProperties[a_propertyName] = (decimal)property + (decimal)a_value;
            }
            else
            {
                GameProperties[a_propertyName] = a_value;
            }
        }

        /// <summary>
        /// Get Game Property Value for the Game Entity
        /// </summary>
        /// <param name="a_propertyName"></param>
        /// <returns></returns>
        public object GetGameProperty(string a_propertyName)
        {
            object a_value = null;
            if(GameProperties.ContainsKey(a_propertyName))
            {
                a_value = GameProperties[a_propertyName];
            }
            return a_value;
        }
        
        public void Dispose()
        {

        }

        #endregion

    }
}
