using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class ActionExtensions
    {
        /// <summary>
        /// Runs an action and catches an expected exception.
        /// </summary>
        /// <typeparam name="T">The type of exception that will be caught. All others will be rethrown.</typeparam>
        /// <param name="action">Action to generate the exception</param>
        /// <param name="message">Optional string to compare the exception message to.</param>
        /// <returns>Returns the expected exception so further testing on exception properties
        /// can be performed.</returns>
        /// <example>
        ///     //Define the action that will throw the expected exception
        ///     Action action = () => { throw new ArgumentNullException("paramname"); };
        ///     //Call the helper and pass in the action
        ///     var ex = ActionExtensions.AssertException<ArgumentNullException>(action);
        ///     //The helper will check the exception type but you can do more analysis if you need to
        ///     Assert.AreEqual("paramname", ex.ParamName);
        /// </example>
        public static T AssertException<T>(this Action action, string message = null)
            where T : Exception
        {
            if (action == null)
                throw new ArgumentNullException("action");

            T exception = null;

            try
            {
                action();
            }
            catch (T ex)
            {
                //Verify that the types are equal and not just assignable
                if (ex.GetType() != typeof(T))
                    throw;
                exception = ex;
            }
            catch (UnitTestAssertException)
            {
                //if the action has an Assert then rethrow the assert exception so the test framework can capture it.
                throw;
            }

            Assert.IsNotNull(exception, "Exception of type {0} should be thrown. No exception was thrown.", typeof(T));

            if (message != null)
            {
                Assert.AreEqual(message.Trim(), exception.Message.Trim());
            }
            return exception;
        }

        /// <summary>
        /// Ignores any exceptions thrown by invoking the action. Primarily used in finally blocks.
        /// </summary>
        /// <param name="action">Get invoked in the method</param>
        public static void IgnoreException(this Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.GetExceptionMessage());
            }
        }
    }
}