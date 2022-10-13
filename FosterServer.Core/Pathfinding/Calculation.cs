using FosterServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Pathfinding
{
    public static class Calculation
    {
        #region Private Members

        private static List<Bounds> m_entities;
        private static float m_movement = 1;
        #endregion

        #region Public Members

        public static List<Bounds> Entities
        {
            get
            {
                if (m_entities == null)
                {
                    m_entities = new List<Bounds>();
                }
                return m_entities;
            }
        }

        #endregion

        #region Public Method
        /// <summary>
        /// Adds a collection of Entities to the map
        /// </summary>
        /// <param name="a_entities"></param>
        public static void AddEntities(IEnumerable<Bounds> a_entities)
        {
            foreach (var item in a_entities)
            {
                if (!Entities.Contains(item))
                {
                    AddEntity(item);
                }
            }
        }

        /// <summary>
        /// Adds a single entity to the map
        /// </summary>
        /// <param name="a_entity"></param>
        public static void AddEntity(Bounds a_entity)
        {
            Entities.Add(a_entity);
        }

        /// <summary>
        /// Clears entities on the map
        /// </summary>
        public static void ClearMap()
        {
            Entities.Clear();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Run Main core to finding quickest Path A*
        /// </summary>
        /// <param name="a_startingPoint"></param>
        /// <param name="a_destination"></param>
        /// <param name="a_totalSteps"></param>
        /// <returns></returns>
        public static GridPoint RunWorkflow(this GridPoint a_startingPoint, GridPoint a_destination, int a_totalSteps = 5, float a_movement = 1)
        {
            m_movement = a_movement;
            //Variable definition
            List<GridPoint> liOpenList = new List<GridPoint>();
            List<GridPoint> liClosedList = new List<GridPoint>();

            //F, G, H
            //Calculate Values for node
            a_startingPoint.SetValues(a_destination);

            //Add Starting Point to Closed list
            liOpenList.Add(a_startingPoint);

            //Find Best Option Node
            GridPoint currentNode = liOpenList.FindBestOption();

            while (liClosedList.FirstOrDefault(x => x.EqualsTo(a_destination)) == null && a_totalSteps >= 0)
            {
                currentNode = liOpenList.FindBestOption();
                liOpenList.Remove(currentNode);
                liClosedList.Add(currentNode);
                liOpenList = liOpenList.AddAdjacentPoints(liClosedList, currentNode, a_destination);
                a_totalSteps--;
            }

            return currentNode;
        }

        /// <summary>
        /// Calculates and sets Grid Point for Node
        /// </summary>
        /// <param name="a_gridPoint"></param>
        /// <param name="a_destinationPoint"></param>
        private static void SetValues(this GridPoint a_gridPoint, GridPoint a_destinationPoint)
        {
            if (a_gridPoint.Parent != null)
            {
                a_gridPoint.G = a_gridPoint.Parent.G + m_movement;
            }
            a_gridPoint.H = a_gridPoint.CalculateDistance(a_destinationPoint);
            a_gridPoint.F = a_gridPoint.G + a_gridPoint.H;
        }

        /// <summary>
        /// Calculate the distance between two points
        /// </summary>
        /// <param name="a_pointA"></param>
        /// <param name="a_pointB"></param>
        /// <returns></returns>
        private static float CalculateDistance(this GridPoint a_pointA, GridPoint a_pointB)
        {
            float xDistance = Math.Abs((float)a_pointB.X - (float)a_pointA.X);
            float yDistance = Math.Abs((float)a_pointB.Y - (float)a_pointA.Y);

            return xDistance + yDistance;
        }

        /// <summary>
        /// With a given list find best possible node route
        /// </summary>
        /// <param name="a_PointList"></param>
        /// <returns></returns>
        private static GridPoint FindBestOption(this List<GridPoint> a_PointList)
        {
            var lowest = a_PointList.OrderBy(x => x.F).ThenByDescending(x => x.G).FirstOrDefault(x => !x.Interesects());
            var lowestList = a_PointList.Where(x => x.F == lowest.F);
            if (lowestList.Count() > 1)
            {
                lowest = lowestList.OrderBy(x => x.H).FirstOrDefault(x=>!x.Interesects());
            }
            return lowest;
        }

        /// <summary>
        /// Adds Grid Points to an Open List.
        /// </summary>
        /// <param name="a_openList"></param>
        /// <param name="a_currentNode"></param>
        /// <returns></returns>
        private static List<GridPoint> AddAdjacentPoints(this List<GridPoint> a_openList, List<GridPoint> a_closedList, GridPoint a_currentNode, GridPoint a_destinationNode)
        {

            //This is where we will perform Map Entity/Wall check
            var up = new GridPoint(a_currentNode.Position.X, a_currentNode.Position.Y - m_movement);
            up.SetParent(a_currentNode);
            up.SetValues(a_destinationNode);
            if (!a_closedList.Any(x => x.EqualsTo(up)))
            {
                a_openList.ValidatePointAndUpdateOrAddPointToList(a_currentNode, up, a_destinationNode);
            }

            var down = new GridPoint(a_currentNode.Position.X, a_currentNode.Position.Y + m_movement);
            down.SetParent(a_currentNode);
            down.SetValues(a_destinationNode);
            if (!a_closedList.Any(x => x.EqualsTo(down)))
            {
                a_openList.ValidatePointAndUpdateOrAddPointToList(a_currentNode, down, a_destinationNode);
            }

            var left = new GridPoint(a_currentNode.Position.X - m_movement, a_currentNode.Position.Y);
            left.SetParent(a_currentNode);
            left.SetValues(a_destinationNode);
            if (!a_closedList.Any(x => x.EqualsTo(left)))
            {
                a_openList.ValidatePointAndUpdateOrAddPointToList(a_currentNode, left, a_destinationNode);
            }

            var right = new GridPoint(a_currentNode.Position.X + m_movement, a_currentNode.Position.Y);
            right.SetParent(a_currentNode);
            right.SetValues(a_destinationNode);
            if (!a_closedList.Any(x => x.EqualsTo(right)))
            {
                a_openList.ValidatePointAndUpdateOrAddPointToList(a_currentNode, right, a_destinationNode);
            }

            return a_openList;
        }

        private static List<GridPoint> ValidatePointAndUpdateOrAddPointToList(this List<GridPoint> a_list, GridPoint a_currentNode, GridPoint a_newNode, GridPoint a_destination)
        {
            GridPoint pointInOpenList = null;
            pointInOpenList = a_list.ContainsGridPoint(a_newNode);
            if (pointInOpenList != null)
            {
                if (pointInOpenList.G > a_newNode.G)
                {
                    pointInOpenList.SetParent(a_currentNode);
                    pointInOpenList.SetValues(a_destination);
                    a_list[a_list.IndexOf(pointInOpenList)] = pointInOpenList;
                }
            }
            else
            {
                a_list.Add(a_newNode);
            }
            return a_list;
        }

        private static GridPoint ContainsGridPoint(this List<GridPoint> a_list, GridPoint target)
        {
            return a_list.FirstOrDefault(x => x.EqualsTo(target) && !target.Interesects());
        }

        private static bool Interesects(this GridPoint a_point)
        {
            //Bounds b = new Bounds(new Vector3((float)a_point.X, (float)a_point.Y), new Vector3(a_point.EntitySize.Width / 2 , a_point.EntitySize.Height / 2));

            bool intersects = Entities.Any(x => {
                return x.center.Equals(a_point.Vector3Position);
                //var bound = x;
                ////return b.center.Equals(x.center);
                //return b.Intersects(x);
            });
            return intersects;
        }

        #endregion
    }
}
