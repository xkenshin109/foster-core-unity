using FosterServer.Core.Models;
using FosterServer.Core.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Utilities
{
    public static class UtilityExtensions
    {
        private static int m_rounding = 0;

        /// <summary>
        /// Calculate the Path to the Destination
        /// </summary>
        /// <param name="a_startingPoint"></param>
        /// <param name="a_destination"></param>
        /// <param name="a_totalSteps"></param>
        /// <returns></returns>
        public static List<GridPoint> RunPathfindingWorkflow(this GridPoint a_startingPoint, GridPoint a_destination, int a_totalSteps = 5, float a_movement = 1)
        {
            return a_startingPoint.RunWorkflow(a_destination, a_totalSteps, a_movement).GetPathgridPoints();
        }

        /// <summary>
        /// Calculate the Path to the Destination
        /// </summary>
        /// <param name="a_startingPoint"></param>
        /// <param name="a_destination"></param>
        /// <param name="a_totalSteps"></param>
        /// <returns></returns>
        public static List<Vector3> RunPathFindingWorkflow(this Vector3 a_startingPoint, Vector3 a_destination, int a_totalSteps = 5, float a_movement = 1)
        {
            return Calculation.RunWorkflow(new GridPoint(a_startingPoint.x, a_startingPoint.y), new GridPoint(a_destination.x, a_destination.y), a_totalSteps, a_movement).GetVector3GridPoints();
        }

        /// <summary>
        /// Calculate the Path to the Destination
        /// </summary>
        /// <param name="a_startingPoint"></param>
        /// <param name="a_destination"></param>
        /// <param name="a_totalSteps"></param>
        /// <returns></returns>
        public static List<Vector2> RunPathFindingWorkflow(this Vector2 a_startingPoint, Vector2 a_destination, int a_totalSteps = 5, float a_movement = 1)
        {
            return Calculation.RunWorkflow(new GridPoint(a_startingPoint.x, a_startingPoint.y), new GridPoint(a_destination.x, a_destination.y), a_totalSteps, a_movement).GetVector2GridPoints();
        }
        public static Vector3 CalculateMovement(this Vector3 a_startPosition, Vector3 a_endPosition, float a_timeDelta, float a_speed = 2f)
        {
            Vector3 a_distance = a_startPosition - a_endPosition;
            if (Math.Round(a_distance.x, m_rounding) > 0)
            {
                return new Vector3(-1 * a_timeDelta * a_speed, 0, 0);
            }
            if (Math.Round(a_distance.x, m_rounding) < 0)
            {
                return new Vector3(1 * a_timeDelta * a_speed, 0, 0);
            }
            if (Math.Round(a_distance.y, m_rounding) > 0)
            {
                return new Vector3(0, -1 * a_timeDelta * a_speed, 0);
            }
            if (Math.Round(a_distance.y, m_rounding) < 0)
            {
                return new Vector3(0, 1 * a_timeDelta * a_speed, 0);
            }
            if (Math.Round(a_distance.z, m_rounding) > 0)
            {
                return new Vector3(0, 0, -1 * a_timeDelta * a_speed);
            }
            if (Math.Round(a_distance.z, m_rounding) < 0)
            {
                return new Vector3(0, 0, 1 * a_timeDelta * a_speed);
            }
            return new Vector3(0, 0, 0);
        }

        public static Vector3 Round(this Vector3 a_position)
        {
            return new Vector3((float)Math.Round(a_position.x, m_rounding), (float)Math.Round(a_position.y, m_rounding), (float)Math.Round(a_position.x, m_rounding));
        }
        public static bool EqualsTo(this Vector3 a_positionA, Vector3 a_positionB)
        {
            return Math.Round(a_positionA.x, m_rounding) == Math.Round(a_positionB.x, m_rounding)
                && Math.Round(a_positionA.y, m_rounding) == Math.Round(a_positionB.y, m_rounding)
                && Math.Round(a_positionA.z, m_rounding) == Math.Round(a_positionB.z, m_rounding);
        }

        public static bool EqualsTo(this Vector3Int a_positionA, Vector3Int a_positionB)
        {
            return a_positionA.x == a_positionB.x
                && a_positionA.y == a_positionB.y
                && a_positionA.z == a_positionB.z;
        }

        public static bool EqualsTo(this Vector2 a_positionA, Vector2 a_positionB)
        {
            return Math.Round(a_positionA.x, m_rounding) == Math.Round(a_positionB.x, m_rounding)
                && Math.Round(a_positionA.y, m_rounding) == Math.Round(a_positionB.y, m_rounding);
        }

        public static bool EqualsTo(this Vector2Int a_positionA, Vector2Int a_positionB)
        {
            return a_positionA.x == a_positionB.x
                && a_positionA.y == a_positionB.y;
        }
    }
}
