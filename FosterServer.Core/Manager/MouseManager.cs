using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Manager
{
    [RequireComponent(typeof(EventsManager))]
    public class MouseManager : MonoBehaviour
    {
        public void ConnectListeners()
        {

        }
        private void Update()
        {
            //Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //EventsManager.Instance.TriggerEvent(EventsManager.MouseEventHighlightedTile, point);
        }
    }
}
