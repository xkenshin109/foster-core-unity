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
    [DebuggerDisplay("GameEntity: Id({EntityId}) Position({X},{Y}) Size({EntitySize.Width},{EntitySize.Height}) Passable({CanEntityPassThrough})")]
    public class GameEntity : IDisposable
    {
        #region Private Members

        private Guid m_entityId;
        private bool m_isInteractable = false;
        private bool m_canEntityPassThrough = false;
        private float m_rotation = 0.0f;
        private Size m_entitySize;
        private GridPoint m_gridPoint;

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
                return m_canEntityPassThrough;
            }
            set
            {
                if (m_canEntityPassThrough != value)
                {
                    m_canEntityPassThrough = value;
                }
            }
        }

        /// <summary>
        /// Is Game Entity Interactable
        /// </summary>
        public bool IsInteractable
        {
            get
            {
                return m_isInteractable;
            }
            set
            {
                if (m_isInteractable != value)
                {
                    m_isInteractable = value;
                }
            }
        }

        /// <summary>
        /// Game Entity's Size
        /// </summary>
        public Size EntitySize
        {
            get
            {
                return m_entitySize;
            }
            set
            {
                if (m_entitySize != value)
                {
                    m_entitySize = value;
                }
            }
        }

        /// <summary>
        /// Game Entity's Rotation
        /// </summary>
        public float Rotation
        {
            get
            {
                return m_rotation;
            }
            set
            {
                if (m_rotation != value)
                {
                    m_rotation = value;
                }
            }
        }

        /// <summary>
        /// Game Entity's Size Width
        /// </summary>
        public float EntityWidth
        {
            get
            {
                if (m_entitySize == null)
                {
                    return 0;
                }
                return m_entitySize.Width;
            }
        }

        /// <summary>
        /// Game Entity's Size Height
        /// </summary>
        public float EntityHeight
        {
            get
            {
                if (m_entitySize == null)
                {
                    return 0;
                }
                return m_entitySize.Height;
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

        #region Constructors

        public GameEntity()
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
            EntitySize = a_entitySize;
            Rotation = a_rotation;
            IsInteractable = a_isInteractable;
            CanEntityPassThrough = a_canEntityPassThrough;
            m_gridPoint = new GridPoint(a_X, a_Y, a_canEntityPassThrough, a_isInteractable, a_entitySize, a_rotation);
        }

        /// <summary>
        /// Toggles Entities Ability to Pass Through
        /// </summary>
        public void ToggleCanEntityBePassedThrough()
        {
            CanEntityPassThrough = !CanEntityPassThrough;
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
            m_rotation = a_rotate;
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

        public void Dispose()
        {

        }

        #endregion

    }
}
