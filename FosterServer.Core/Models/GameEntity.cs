using FosterServer.Core.Interface;
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
    public class GameEntity : IDisposable, IGameEntityManager
    {
        #region Private Members

        private Guid m_entityId;
        private GridPoint m_gridPoint;
        private float m_health = 100f;
        private float m_energy = 100f;
        private float m_mana = 100f;
        private float m_defense = 20f;
        private float m_attack = 20f;
        private int m_level = 1;
        private float m_experience = 0f;
        private float m_range = 1f;
        private float m_gold = 0;
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
        /// Get the Health of The Game Entity
        /// </summary>
        public float Health { get { return m_health; } }

        /// <summary>
        /// Get the Energy of the Game Entity
        /// </summary>
        public float Energy { get { return m_energy; } }

        /// <summary>
        /// Get the Mana of the Game Entity
        /// </summary>
        public float Mana { get { return m_mana; } }

        /// <summary>
        /// Get the Defense of the Game Entity
        /// </summary>
        public float Defense { get { return m_defense; } }

        /// <summary>
        /// Get the Attack of the Game Entity
        /// </summary>
        public float Attack { get { return m_attack; } }

        /// <summary>
        /// Get the Level of the Game Entity
        /// </summary>
        public float Level { get { return m_level; } }

        /// <summary>
        /// Get the Experience of the Game Entity
        /// </summary>
        public float Experience { get { return m_experience; } }

        /// <summary>
        /// Get the Attack Range of the Game Entity
        /// </summary>
        public float AttackRange { get { return m_range; } }

        /// <summary>
        /// Is Game Entity Dead
        /// </summary>
        public bool IsDead { get { return m_health <= 0; } }

        /// <summary>
        /// Money the Game Entity possesses
        /// </summary>
        public float Gold { get { return m_gold; } }

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
        }

        /// <summary>
        /// Initialize Base Stats for Game Entity
        /// </summary>
        /// <param name="a_health"></param>
        /// <param name="a_attack"></param>
        /// <param name="a_defense"></param>
        /// <param name="a_attackRange"></param>
        /// <param name="a_gold"></param>
        /// <param name="a_mana"></param>
        /// <param name="a_energy"></param>
        public void InitializeStats(float a_health = 100, float a_attack = 15, float a_defense = 15, float a_attackRange = 1, float a_gold = 1f, float a_mana = 20, float a_energy = 20)
        {
            m_health = a_health;
            m_attack = a_attack;
            m_defense = a_defense;
            m_range = a_attackRange;
            m_gold = a_gold;
            m_mana = a_mana;
            m_energy = a_energy;
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
        /// Sets Health to specified new Health
        /// </summary>
        /// <param name="a_health"></param>
        public void SetHealth(float a_health)
        {
            m_health = a_health;
        }

        /// <summary>
        /// Game Entity took damage. Subtract from Current Health
        /// </summary>
        /// <param name="a_damage"></param>
        public void TakeDamage(float a_damage)
        {
            m_health -= a_damage;
        }

        /// <summary>
        /// Heal Game Entity
        /// </summary>
        /// <param name="a_health"></param>
        public void Heal(float a_health)
        {
            m_health += a_health;
        }

        /// <summary>
        /// Change Attack for Game Entity
        /// </summary>
        /// <param name="a_attackDamage"></param>
        public void ChangeAttack(float a_attackDamage)
        {
            m_attack += a_attackDamage;
            if (m_attack < 0)
            {
                m_attack = 0;
            }
        }

        /// <summary>
        /// Change Defense for Game Entity
        /// </summary>
        /// <param name="a_defense"></param>
        public void ChangeDefense(float a_defense)
        {
            m_defense += a_defense;
            if(m_defense < 0)
            {
                m_defense = 0;
            }
        }

        /// <summary>
        /// Change Experience for Game Entity
        /// </summary>
        /// <param name="a_experience"></param>
        public void ChangeExperience(float a_experience)
        {
            m_experience += a_experience;
        }

        /// <summary>
        /// Change Attack Range for Game Entity
        /// </summary>
        /// <param name="a_range"></param>
        public void ChangeAttackRange(float a_range)
        {
            m_range += a_range;
            if(m_range <= 0)
            {
                m_range = 1;
            }
        }

        /// <summary>
        /// Change Gold for Game Entity
        /// </summary>
        /// <param name="a_gold"></param>
        public void ChangeGold(float a_gold)
        {
            m_gold += a_gold;
            if(m_gold <= 0)
            {
                m_gold = 0;
            }
        }

        public void Dispose()
        {

        }

        #endregion

    }
}
