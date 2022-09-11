using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Manager
{
    /// <summary>
    /// Player Controller
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class PlayerManager : MonoBehaviour
    {
        public float Speed = .1f;
        private float X => transform.position.x;
        private float Y => transform.position.y;
        private float Z => transform.position.z;
        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        private void LateUpdate()
        {
            bool Left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            bool Right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            bool Down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            bool Up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            if (Left)
            {
                transform.Translate(Vector3.left * Speed * Time.deltaTime);
            }
            if (Right)
            {
                transform.Translate(Vector3.right * Speed * Time.deltaTime);
            }
            if (Down)
            {
                transform.Translate(Vector3.down * Speed * Time.deltaTime);
            }
            if (Up)
            {
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
            }
        }
    }
}
