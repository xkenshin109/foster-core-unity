using FosterServer.Core.Pathfinding;
using FosterServer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.UnityCore.Controller
{
    public class AnimatorController : MonoBehaviour
    {
        private Animator animator;
        public float moveSpeed;
        public List<Vector3> waypoint;
        Vector2 initialPosition;
        public float currentTimeOnPath, time;
        string[] animations;
        public Vector3 moveTo;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            initialPosition = transform.position;
            moveSpeed = 0.8f;
            animations = new string[] { "attack", "jump", "shoot", "dying", "run", "attack2" };
        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            if (!moveTo.EqualsTo(gameObject.transform.position) && waypoint.Count() == 0)
            {
                waypoint.AddRange(gameObject.transform.position.Round().RunPathFindingWorkflow(moveTo, 10, .5f));
            }
            walk();
        }
        public void StartAnimation()
        {

        }
        private void look(Vector2 direction)
        {
            if (direction.x > transform.position.x) // To the right
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

            }
            else if (direction.x < transform.position.x) // To the left
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

            }
        }
        void walk()
        {
            animator.Play("walk");
            Vector3 startPosition = initialPosition;
            Vector3 endPosition = waypoint.FirstOrDefault();
            if (endPosition == null)
            {
                //At the end
                return;
            }
            // 2 
            float pathLength = Vector3.Distance(startPosition, endPosition);
            float totalTimeForPath = pathLength / moveSpeed;
            //float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
            currentTimeOnPath += 1 * Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
            // 3 
            if (gameObject.transform.position.EqualsTo(endPosition))
            {
                initialPosition = transform.position;
                waypoint.Remove(endPosition);
                currentTimeOnPath = 0;
                look(waypoint.FirstOrDefault());

            }
        }
    }
}
