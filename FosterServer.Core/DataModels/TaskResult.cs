using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FosterServer.Core.DataModels
{
    /// <summary>
    /// Result Class for returning void Methods. Used for verifying if Methods executed properly
    /// </summary>
    public class Result 
    {
        /// <summary>
        /// Validation Error enums
        /// </summary>
        public enum ValidationError
        {
            Error,
            NotFound,
            NotImplemented,
            Invalid,
            None
        }
        private List<string> m_errorMessages;

        /// <summary>
        /// Success Flag from Result
        /// </summary>
        public bool IsSuccess { get; set; }
        
        /// <summary>
        /// Validation Error from Result
        /// </summary>
        public ValidationError Validation { get; set; }

        /// <summary>
        /// Errors Messages from Result
        /// </summary>
        public List<string> ErrorMessage 
        { 
            get 
            { 
                if(m_errorMessages == null)
                {
                    m_errorMessages = new List<string>();
                }
                return m_errorMessages; 
            } 
        }

        /// <summary>
        /// Latest Error Message
        /// </summary>
        /// 
        public string Message
        {
            get
            {
                return ErrorMessage.LastOrDefault();
            }
        }

        public Result()
        {

        }

        /// <summary>
        /// Result Valid
        /// </summary>
        /// <returns></returns>
        public static Result Valid()
        {
            return new Result
            {
                IsSuccess = true,
                Validation = ValidationError.None
            };
        }

        /// <summary>
        /// Result Error
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result Error(string message = "")
        {
            var result = new Result
            {
                IsSuccess = false,
                Validation = ValidationError.Error
            };
            if (!string.IsNullOrEmpty(message))
            {
                result.ErrorMessage.Add(message);
            }            

            return result;
        }

        /// <summary>
        /// Result Invalid
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result Invalid(string message = "")
        {
            var result = new Result
            {
                IsSuccess = false,
                Validation = ValidationError.Invalid
            };

            if (!string.IsNullOrEmpty(message))
            {
                result.ErrorMessage.Add(message);
            }

            return result;
        }

        /// <summary>
        /// Result Not Found
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result NotFound(string message = "")
        {
            var result = new Result
            {
                IsSuccess = false,
                Validation = ValidationError.NotFound
            };

            if (!string.IsNullOrEmpty(message))
            {
                result.ErrorMessage.Add(message);
            }

            return result;
        }

        /// <summary>
        /// Result Not Implemented
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result NotImplemented(string message = "")
        {
            var result = new Result
            {
                IsSuccess = false,
                Validation = ValidationError.NotImplemented
            };

            if (!string.IsNullOrEmpty(message))
            {
                result.ErrorMessage.Add(message);
            }

            return result;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        
        public T Value { get; set; }
        
        public static Result<T> Valid()
        {

            return new Result<T> { IsSuccess = true};
        }

        public static Result<T> Valid(T data)
        {
            var result = Valid();
            result.Value = data;
            
            return result;
        }
        public static Result<T> Error(T data, string message = null)
        {
            var result = Error(message) as Result<T>;
            result.Value = data;
            return result;
        }
    }

}
