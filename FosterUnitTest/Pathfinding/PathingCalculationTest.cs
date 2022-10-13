using FosterServer.Core.Models;
using FosterServer.Core.Pathfinding;
using FosterServer.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterUnitTest.Pathfinding
{
    [TestClass]
    public class PathingCalculationTest
    {
        [TestMethod]
        public void PathingTest()
        {
            //SETUP
            var startingPoint = new GridPoint(5, 5);
            var endingPoint = new GridPoint(10, 10);
            var expectedSteps = 2;
            var expectedPath = new List<GridPoint>()
            {
                new GridPoint(5,5),
                new GridPoint(5,6),
                new GridPoint(5,7)
            };
            Calculation.ClearMap();

            //ACT
            var result = startingPoint.RunPathfindingWorkflow(endingPoint, expectedSteps);
            
            //ASSERT
            Assert.AreEqual(expectedSteps, result.Count() - 1);
            Assert.IsTrue(ValidateList(expectedPath, result));
        }

        [TestMethod]
        public void PathingTestWithEntities()
        {
            //SETUP
            var startingPoint = new GridPoint(0, 0);
            var endingPoint = new GridPoint(5, 5);
            Calculation.ClearMap();
            Calculation.AddEntities(new List<Bounds> {
                new Bounds(new Vector3(1f,0f),new Vector3(1,1)),
                new Bounds(new Vector3(0f,1f),new Vector3(1,1)),
            });
            var expectedPath = new List<GridPoint>()
            {
                new GridPoint(0, 0),
                new GridPoint(0, -1),
                new GridPoint(1, -1),
                new GridPoint(2, -1),
                new GridPoint(2, 0),
                new GridPoint(2, 1)
            };

            //ACT

            var result = startingPoint.RunPathfindingWorkflow(endingPoint, 5);

            //ASSERT
            Assert.AreEqual(expectedPath.Count(), result.Count());
            Assert.IsTrue(ValidateList(expectedPath, result));
        }

        [TestMethod]
        public void PathingTestSteps()
        {
            //SETUP
            var startingPoint = new GridPoint(0, 0);
            var endingPoint = new GridPoint(5, 5);
            var expectedPath = new List<GridPoint>()
            {
                new GridPoint(0, 0),
                new GridPoint(0, .5f),
                new GridPoint(0, 1),
                new GridPoint(0, 1.5f),
                new GridPoint(0, 2)

            };

            //ACT
            Calculation.ClearMap();
            var result = startingPoint.RunPathfindingWorkflow(endingPoint, 4, 0.5f);

            //ASSERT
            Assert.AreEqual(expectedPath.Count(), result.Count());
            Assert.IsTrue(ValidateList(expectedPath, result));
        }

        [TestMethod]
        public void PathingTestStepsWithEntities()
        {
            //SETUP
            var startingPoint = new GridPoint(0, 0);
            var endingPoint = new GridPoint(5, 5);
            Calculation.ClearMap();
            Calculation.AddEntities(new List<Bounds> {
                new Bounds(new Vector3(1f,0f),new Vector3(1,1)),
                new Bounds(new Vector3(0f,1f),new Vector3(1,1)),
            });
            var expectedPath = new List<GridPoint>()
            {
                new GridPoint(0, 0),
                new GridPoint(0, 0.5f),
                new GridPoint(.5f, .5f),
                new GridPoint(.5f,1),
                new GridPoint(.5f,1.5f),
                new GridPoint(.5f,2),

            };

            //ACT

            var result = startingPoint.RunPathfindingWorkflow(endingPoint, 5, .5f);

            //ASSERT
            Assert.AreEqual(expectedPath.Count(), result.Count());
            Assert.IsTrue(ValidateList(expectedPath, result));
        }

        private bool ValidateList(IEnumerable<GridPoint> pointsA, IEnumerable<GridPoint> pointsB)
        {
            var totalInList = pointsA.Count();

            if(pointsB.Count() != totalInList)
            {
                return false;
            }

            bool isEqual = true;
            for(var x = 0; x < totalInList; x++)
            {
                var a = pointsA.ElementAt(x);
                var b = pointsB.ElementAt(x);
                if (!a.EqualsTo(b))
                {
                    isEqual = false;
                }                
            }
            return isEqual;
        }
    }
}
