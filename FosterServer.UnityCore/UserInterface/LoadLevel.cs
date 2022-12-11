using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace FosterServer.UnityCore.UserInterface
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

        public void PerformAction()
        {
            if (SceneManager.GetSceneByName(LevelToLoad) == null)
            {
                Debug.LogError($"No Scene named {LevelToLoad}. Verify Level is added to project");
                return;
            }
            SceneManager.LoadScene(LevelToLoad, LoadSceneMode.Single);
        }
        void OnGUI()
        {

        }
    }
}
