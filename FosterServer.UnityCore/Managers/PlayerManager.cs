using FosterServer.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.UnityCore.Managers
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

        private bool m_movementDisabled = false;
        private Guid m_playerId;
        public Guid PlayerId
        {
            get
            {
                if (m_playerId == Guid.Empty)
                {
                    m_playerId = Guid.NewGuid();
                }
                return m_playerId;
            }
        }
        private void Awake()
        {

        }

        private void Start()
        {
            StartListening();
        }

        private void Update()
        {

        }

        private void OnDestroy()
        {
            StopListening();
        }

        private void LateUpdate()
        {
            if (!m_movementDisabled)
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

        private void ToggleMovement(object data)
        {
            m_movementDisabled = !m_movementDisabled;
        }

        private void StartListening()
        {
            EventsManager.Instance.StartListening(EventManagerEvent.DisableMovement, ToggleMovement, PlayerId);
        }

        private void StopListening()
        {
            EventsManager.Instance.StopListening(EventManagerEvent.DisableMovement, ToggleMovement, PlayerId);
        }
    }
}
