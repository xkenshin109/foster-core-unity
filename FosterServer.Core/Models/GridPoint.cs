using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Models
{
    [DebuggerDisplay("Pathfinding: F={this.F} G={this.G} H={this.H} Point: X={this.X} Y={this.Y}")]
    public class GridPoint
    {

        #region Private Members

        private Point m_position;
        private bool m_isInteractable = false;
        private bool m_canEntityPassThrough = false;
        private float m_rotation = 0.0f;
        private Size m_entitySize;
        private float m_totalMoveCost;
        private float m_distanceCurrentToStarting;
        private float m_distanceCurrentToEnding;
        private GridPoint m_parentGridPoint;
        #endregion

        #region Public Members
        /// <summary>
        /// Unity Position for Tilemap
        /// </summary>
        public Vector2Int? UnityPosition
        {
            get
            {
                if (Position != null)
                {
                    return new Vector2Int((int)X, (int)Y);
                }
                return null;
            }
        }

        /// <summary>
        /// Vector3 Position For Unity
        /// </summary>
        public Vector3? Vector3Position
        {
            get
            {
                if (Position != null)
                {
                    return new Vector3((float)X, (float)Y);
                }
                return null;
            }
        }

        /// <summary>
        /// Position of Game Entity
        /// </summary>
        public Point Position
        {
            get
            {
                return m_position;
            }
            set
            {
                if (m_position != value)
                {
                    m_position = value;
                }
            }
        }

        /// <summary>
        /// X-Coordinate of a Game Entity's Position
        /// </summary>
        public float? X
        {
            get
            {
                if (Position == null)
                {
                    return null;
                }
                return Position.X;
            }
        }

        /// <summary>
        /// Y-Coordinate of a Game Entity's Position
        /// </summary>
        public float? Y
        {
            get
            {
                if (Position == null)
                {
                    return null;
                }
                return Position.Y;
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

        /// <summary>
        /// F - Actual distance from Starting to End Point
        /// </summary>
        public float F
        {
            get
            {
                return m_totalMoveCost;
            }
            set
            {
                if (m_totalMoveCost != value)
                {
                    m_totalMoveCost = value;
                }
            }
        }

        /// <summary>
        /// G - Actual distance from Starting to Current Node
        /// </summary>
        public float G
        {
            get
            {
                return m_distanceCurrentToStarting;
            }
            set
            {
                if (m_distanceCurrentToStarting != value)
                {
                    m_distanceCurrentToStarting = value;
                }
            }
        }

        /// <summary>
        /// H - Estimated distance from Current to Ending Nod
        /// </summary>
        public float H
        {
            get
            {
                return m_distanceCurrentToEnding;
            }
            set
            {
                if (m_distanceCurrentToEnding != value)
                {
                    m_distanceCurrentToEnding = value;
                }
            }
        }

        /// <summary>
        /// Previous Node 
        /// </summary>
        public GridPoint Parent
        {
            get
            {
                return m_parentGridPoint;
            }
            set
            {
                if (m_parentGridPoint != value)
                {
                    m_parentGridPoint = value;
                }
            }
        }
        #endregion

        #region Constructors

        public GridPoint(int a_x, int a_y)
            : this(new Point(a_x, a_y))
        {

        }

        public GridPoint(int a_x, int a_y, bool a_canEntityPassThrough)
            : this(new Point(a_x, a_y), a_canEntityPassThrough)
        {

        }

        public GridPoint(int a_x, int a_y, bool a_canEntityPassThrough, bool a_isInteractable)
            : this(new Point(a_x, a_y), a_canEntityPassThrough, a_isInteractable)
        {

        }

        public GridPoint(int a_x, int a_y, bool a_canEntityPassThrough, bool a_isInteractable, Size a_entitySize)
            : this(new Point(a_x, a_y), a_canEntityPassThrough, a_isInteractable, a_entitySize)
        {

        }

        public GridPoint(int a_x, int a_y, bool a_canEntityPassThrough, bool a_isInteractable, Size a_entitySize, float rotation)
            : this(new Point(a_x, a_y), a_canEntityPassThrough, a_isInteractable, a_entitySize, rotation)
        {

        }

        public GridPoint()
            : this(new Point(0, 0))
        {

        }

        public GridPoint(Point a_point)
            : this(a_point, false)
        {
        }

        public GridPoint(Point a_point, bool a_canEntityPassThrough)
            : this(a_point, a_canEntityPassThrough, false)
        {

        }

        public GridPoint(Point a_point, bool a_canEntityPassThrough, bool a_isInteractable)
            : this(a_point, a_canEntityPassThrough, a_isInteractable, new Size(1, 1))
        {


        }

        public GridPoint(Point a_point, bool a_canEntityPassThrough, bool a_isInteractable, Size a_entitySize)
            : this(a_point, a_canEntityPassThrough, a_isInteractable, a_entitySize, 0)
        {

        }

        public GridPoint(Point a_point, bool a_canEntityPassThrough, bool a_isInteractable, Size a_entitySize, float a_rotation)
        {
            Position = a_point;
            CanEntityPassThrough = a_canEntityPassThrough;
            IsInteractable = a_isInteractable;
            EntitySize = a_entitySize;
            Rotation = a_rotation;
        }

        public GridPoint(float a_x, float a_y)
            : this(new Point(a_x, a_y))
        {

        }

        public GridPoint(float a_x, float a_y, bool a_canEntityPassThrough)
            : this(new Point(a_x, a_y), a_canEntityPassThrough)
        {

        }

        public GridPoint(float a_x, float a_y, bool a_canEntityPassThrough, bool a_isInteractable)
            : this(new Point(a_x, a_y), a_canEntityPassThrough, a_isInteractable)
        {

        }

        public GridPoint(float a_x, float a_y, bool a_canEntityPassThrough, bool a_isInteractable, Size a_entitySize)
            : this(new Point(a_x, a_y), a_canEntityPassThrough, a_isInteractable, a_entitySize)
        {

        }

        public GridPoint(float a_x, float a_y, bool a_canEntityPassThrough, bool a_isInteractable, Size a_entitySize, float a_rotation)
            : this(new Point(a_x, a_y), a_canEntityPassThrough, a_isInteractable, a_entitySize, a_rotation)
        {

        }

        #endregion

        #region Public Methods

        public void SetParent(GridPoint a_parentGridPoint)
        {
            Parent = a_parentGridPoint;
        }

        public List<GridPoint> GetPathgridPoints()
        {
            List<GridPoint> res = new List<GridPoint>();
            var destination = this;
            res.Add(destination);

            while (destination.Parent != null)
            {
                destination = destination.Parent;
                res.Add(destination);
            }

            res.Reverse();

            return res;
        }

        public List<Vector3> GetVector3GridPoints()
        {
            List<Vector3> res = new List<Vector3>();
            var destination = this;
            res.Add(new Vector3((float)destination.X, (float)destination.Y, 1));

            while (destination.Parent != null)
            {
                destination = destination.Parent;
                res.Add(new Vector3((float)destination.X, (float)destination.Y, 1));
            }

            res.Reverse();

            return res;
        }

        public List<Vector2> GetVector2GridPoints()
        {
            List<Vector2> res = new List<Vector2>();
            var destination = this;
            res.Add(new Vector2((float)destination.X, (float)destination.Y));

            while (destination.Parent != null)
            {
                destination = destination.Parent;
                res.Add(new Vector2((float)destination.X, (float)destination.Y));
            }

            res.Reverse();

            return res;
        }

        public void SetLocation(Vector3 a_position)
        {
            Position.SetPosition(a_position);
        }

        #endregion

        #region Overloads

        public bool EqualsTo(GridPoint a_targetGridPoint)
        {
            return (this.X == a_targetGridPoint.X) &&
                (this.Y == a_targetGridPoint.Y);
        }

        #endregion
    }
}
