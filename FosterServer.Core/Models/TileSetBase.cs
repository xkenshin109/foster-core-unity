using FosterServer.Core.Enumerations;
using FosterServer.Core.Interface;
using FosterServer.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FosterServer.Core.Models
{
    /// <summary>
    /// TileMapBase 
    /// Retrieves Tileset and parses into usable Sprites
    /// </summary>
    public abstract class TileSetBase : ITileSet
    {

        public string TemplateName { get; }
        public Sprite[] SpriteTemplate { get; set; }
        public Tilemap TilePalette { get; set; }
        public GameObject PaletteObject { get; set; }
        public string Name { get; }
        public string Description { get; }

        public Dictionary<object, Sprite> SpriteDictonary = new Dictionary<object, Sprite>();

        public int Layer { get; set; }

        public TileSetBase()
        {

        }
        
        public TileSetBase(string templateName, string name, string description)
            : this()
        {
            TemplateName = templateName;
            Name = name;
            Description = description;

        }
        public void Awake()
        {
            FosterLog.Log($"{Name} - {TemplateName} loading...");
            string location = TemplateName + "/" + Name;
            PaletteObject = Resources.Load<GameObject>( location + "_palette");
            int failedCount = 2; //Total Tasks
            if(PaletteObject != null)
            {
                failedCount--;
                FosterLog.Log($"...Palette loaded");
                TilePalette = PaletteObject.GetComponentInChildren<Tilemap>();
                
                if (TilePalette != null)
                {
                    failedCount--;
                    FosterLog.Log($"....Tile Palette loaded");
                    List<Sprite> sprites = new List<Sprite>();

                    failedCount += TilePalette.GetUsedSpritesCount();

                    for (var i = 0; i < TilePalette.GetUsedSpritesCount(); i++)
                    {
                        failedCount--;
                        FosterLog.Log($".....Sprite {(i + 1) + " (" + i + ", 0, 0)"} loaded");
                        var sprite = TilePalette.GetSprite(new Vector3Int(i, 0, 0));
                        sprites.Add(sprite);
                    }

                    SpriteTemplate = sprites.ToArray();
                    
                }
                else
                {
                    FosterLog.Error(".....No Tile Palette Found");
                }
            }
            else
            {
                FosterLog.Error(".....No Tile Palette Found");
            }

            if(failedCount == 0)
            {
                Debug.Log($"{Name} has been initialized");
            }
            else
            {
                Debug.Log($"{Name} failed to initialize ({failedCount})");
            }
            
        }
        public abstract void Initialize();
    }
}
