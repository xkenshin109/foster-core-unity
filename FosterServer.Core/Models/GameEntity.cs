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
        private List<GridPoint> m_gridPoints = new List<GridPoint>();
        private Vector3 m_vector3position;
        #endregion

        #region Properties

        public Guid EntityId { 
            get 
            { 
                if(m_entityId == Guid.Empty)
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
        public int EntityWidth
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
        public int EntityHeight
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

        /// <summary>
        /// Points where this entity exists on Grid
        /// </summary>
        public List<GridPoint> GridPoints { get { return m_gridPoints; } }

        /// <summary>
        /// X Position of Entity
        /// </summary>
        public float X => m_vector3position.x;

        /// <summary>
        /// Y Position of Entity
        /// </summary>
        public float Y => m_vector3position.y;

        /// <summary>
        /// Position of Game Entity
        /// </summary>
        public Vector2Int Vector2IntPosition 
        { 
            get
            {
                return new Vector2Int((int) m_vector3position.x, (int) m_vector3position.y);
            }
        }

        /// <summary>
        /// Vector 2 Position
        /// </summary>
        public Vector2 Vector2Position
        {
            get
            {
                return new Vector2(m_vector3position.x, m_vector3position.y);
            }
        }

        /// <summary>
        /// Vector 3 Position
        /// </summary>
        public Vector3 Vector3Position
        {
            get
            {
                return m_vector3position;
            }
            set
            {
                if (m_vector3position != value)
                {
                    m_vector3position = value;
                }
            }
        }

        /// <summary>
        /// Vector 3 Int Position
        /// </summary>
        public Vector3Int Vector3IntPosition
        {
            get
            {
                return new Vector3Int((int)m_vector3position.x, (int)m_vector3position.y, (int)m_vector3position.z);
            }
        }

        #endregion

        #region Constructors

        public GameEntity()
        {

        }

        public GameEntity(int a_x, int a_y)
            :this(a_x, a_y, new Size(1,1))
        {

        }

        public GameEntity(int a_x, int a_y, Size a_entitySize)
            : this(a_x, a_y, a_entitySize, 0)
        {

        }

        public GameEntity(int a_x, int a_y, Size a_entitySize, float a_rotation)
            : this(a_x, a_y, a_entitySize, a_rotation, false, false)
        {

        }

        public GameEntity(int a_x, int a_y, Size a_entitySize, float a_rotation, bool a_isInteractable = false, bool a_canEntityPassThrough = false)
        {
            Initialize(a_x, a_y, a_entitySize, a_rotation, a_isInteractable, a_canEntityPassThrough);
        }

        #endregion

        #region Private Methods

        private void SetGridPoints()
        {
            for (float height = Vector3Position.y; height < Vector3Position.y + EntitySize.Height; height+=1)
            {
                for (float width = Vector3Position.x; width < Vector3Position.x + EntitySize.Width; width+=1)
                {
                    GridPoint entityPoint = new GridPoint((int)width, (int)height);
                    if (!GridPoints.Contains(entityPoint))
                    {
                        m_gridPoints.Add(entityPoint);
                    }
                }
            }
        }

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
        public void Initialize(int a_X, int a_Y, Size a_entitySize, float a_rotation = 0, bool a_isInteractable = false, bool a_canEntityPassThrough = false)
        {
            Vector3Position = new Vector3(a_X, a_Y);
            EntitySize = a_entitySize;
            Rotation = a_rotation;
            IsInteractable = a_isInteractable;
            CanEntityPassThrough = a_canEntityPassThrough;
            SetGridPoints();
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
            m_vector3position = a_position;
        }

        /// <summary>
        /// Set Position of Entity
        /// </summary>
        /// <param name="a_x"></param>
        /// <param name="a_y"></param>
        public void SetPosition(int a_x, int a_y, int a_z = 1)
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
            bool bHasGridPointIntersect = false;
            foreach (GridPoint point in GridPoints)
            {
                if (point.EqualsTo(a_sourcePoint))
                {
                    bHasGridPointIntersect = true;
                }
            }
            return bHasGridPointIntersect;
        }

        public void Dispose()
        {
            
        }

        #endregion

    }
}
