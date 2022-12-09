using FosterServer.Core.DataModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterUnitTest.Models
{
    [TestClass]
    public class ResultModelTest
    {
        [TestMethod]
        public void Result_valid_test_returns_true()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.Valid();

            //ASSERT
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 0);
            Assert.IsTrue(result.Validation == Result.ValidationError.None);
        }

        [TestMethod]
        public void Result_error_test_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.Error();

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 0);
            Assert.IsTrue(result.Validation == Result.ValidationError.Error);
        }

        [TestMethod]
        public void Result_error_test_with_message_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.Error("Execution errored out");

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 1);
            Assert.IsTrue(result.Validation == Result.ValidationError.Error);
        }

        [TestMethod]
        public void Result_invalid_test_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.Invalid();

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 0);
            Assert.IsTrue(result.Validation == Result.ValidationError.Invalid);
        }

        [TestMethod]
        public void Result_invalid_test_with_message_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.Invalid("Invalid Result");

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 1);
            Assert.IsTrue(result.Validation == Result.ValidationError.Invalid);
        }

        [TestMethod]
        public void Result_not_found_test_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.NotFound();

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 0);
            Assert.IsTrue(result.Validation == Result.ValidationError.NotFound);
        }

        [TestMethod]
        public void Result_not_found_with_message_test_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.NotFound("NotFound Result");

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 1);
            Assert.IsTrue(result.Validation == Result.ValidationError.NotFound);
        }

        [TestMethod]
        public void Result_not_implemented_test_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.NotImplemented();

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 0);
            Assert.IsTrue(result.Validation == Result.ValidationError.NotImplemented);
        }

        [TestMethod]
        public void Result_not_implemented_with_message_test_returns_false()
        {
            //SETUP
            Result result;

            //ACT
            result = Result.NotImplemented("Not Implemented");

            //ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Count == 1);
            Assert.IsTrue(result.Validation == Result.ValidationError.NotImplemented);
        }
    }
}
