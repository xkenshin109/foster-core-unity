using FosterServer.Core.Manager;
using FosterServer.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterUnitTest.Models
{
    [TestClass]
    public class GameEntityTest
    {

        public static GameEntity GameEntityPositioned()
        {
            return new GameEntity(5, 5);
        }

        public static GameEntity GameEntityPositionedSized() {
            return new GameEntity(5, 5, new Size(1, 1));
        }
        public static GameEntity GameEntity()
        {
            return new GameEntity();
        }
        public static GameEntity GameEntityRotation()
        {
            return new GameEntity(5, 5, new Size(1, 1), 80);
        }

        [TestMethod]
        public void GameEntity_no_properties()
        {
            //SETUP
            GameEngine.ClearGameProperties();
            
            //ACT
            GameEntity a = new GameEntity();

            //ASSERT
            Assert.IsTrue(a.GameProperties.Count() == 0);
        }

        [TestMethod]
        public void GameEntity_has_properties()
        {
            string propertyName = "TestProperty";
            int expectedResult = 100;

            //SETUP
            GameEngine.ClearGameProperties();
            GameEngine.AddGameProperty(propertyName, typeof(int), 100);

            //ACT
            GameEntity a = GameEntityTest.GameEntity();

            //ASSERT
            Assert.IsTrue(a.GameProperties.Count() == 1, "Properties Count are not in sync");
            Assert.IsTrue(a.GameProperties.ContainsKey(propertyName), "Property Missing");
            Assert.AreEqual(expectedResult, a.GameProperties[propertyName], "Property Value is not correct");
        }

        [TestMethod]
        public void GameEntity_has_properties_value_change()
        {
            string propertyName = "TestProperty";
            int expectedResult = 80;

            //SETUP
            GameEngine.ClearGameProperties();
            GameEngine.AddGameProperty(propertyName, typeof(int), 100);

            //ACT
            GameEntity a = GameEntityTest.GameEntity();
            a.SetGameProperty(propertyName, 80);

            //ASSERT
            Assert.IsTrue(a.GameProperties.Count() == 1, "Properties Count are not in sync");
            Assert.IsTrue(a.GameProperties.ContainsKey(propertyName), "Property Missing");
            Assert.AreEqual(expectedResult, a.GameProperties[propertyName], "Property Value is not correct");
        }

        [TestMethod]
        public void GameEntity_validate_positioning_rotation()
        {
            //SETUP
            Vector3 expectedResult = new Vector3(5, 5, 0);
            float expectedRotationResult = 80f;

            //ACT
            GameEntity a = GameEntityTest.GameEntityRotation();

            //ASSERT
            Assert.AreEqual(expectedResult, a.Vector3Position, "Position is incorrect");
            Assert.AreEqual(expectedRotationResult, a.Rotation, "Rotation is incorrect");
        }
    }
}
