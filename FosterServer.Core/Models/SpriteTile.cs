using FosterServer.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FosterServer.Core.Models
{
    public class SpriteTile
    {
        private Tile m_TileSprite;
        public Sprite SpriteImage { get; set; }
        public Tile TileSprite
        {
            get
            {
                if (SpriteImage == null) return null;
                if (m_TileSprite == null)
                {
                    m_TileSprite = ScriptableObject.CreateInstance<Tile>();
                    m_TileSprite.sprite = SpriteImage;
                }
                return m_TileSprite;
            }
        }
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsPassable { get; set; }

        public GameEntityEnum Entity { get; set; }

        public SpriteTile()
            : this(null, GameEntityEnum.Empty, 0, 0)
        {

        }
        public SpriteTile(Sprite spriteImage, GameEntityEnum entity)
            : this(spriteImage, entity, 0, 0)
        {
            Entity = entity;
        }
        public SpriteTile(Sprite spriteImage, GameEntityEnum entity, float x, float y)
        {
            SpriteImage = spriteImage;
            X = x;
            Y = y;
            Entity = entity;
        }
        /// <summary>
        /// Set X, Y Coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
