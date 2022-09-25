using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace FosterServer.UnityCore.UserInterface.Menu
{
    [RequireComponent(typeof(Button))]
    public class LoadLevel : MonoBehaviour
    {
        public string LevelToLoad;

        /// <summary>
        /// Button Associated for New Game
        /// </summary>
        public Button Button;

        void Awake()
        {
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(LevelToLoad, LoadSceneMode.Single);
        }
        void OnGUI()
        {

        }
    }
}
