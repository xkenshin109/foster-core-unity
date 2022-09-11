using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace FosterServer.Core.GUIActions
{
    [RequireComponent(typeof(Button))]
    public class NewGame : MonoBehaviour
    {
        public string LevelToLoad;
        public string ButtonLabel = "New Game";

        /// <summary>
        /// Button Associated for New Game
        /// </summary>
        public Button Button { get; set; }

        void Awake()
        {
            Button = this.gameObject.GetComponent<Button>();
            Button.clicked += StartNewGame;
        }
         
        void StartNewGame()
        {
            SceneManager.LoadScene(LevelToLoad, LoadSceneMode.Single);
        }
        void OnGUI()
        {

        }
    }
}
