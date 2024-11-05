using System;

namespace VAF_ConfigPropertyMapping.Helper
{
    public static class Assert
    {
        /// <summary>
        /// Verifies that the specified value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the value being checked.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="message">An optional message that describes the error.</param>
        /// <exception cref="Exception">Thrown when the value is null.</exception>
        public static void IsNotNull<T>(T value, string message = null) where T : class
        {
            if (value is null)
            {
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Verifies that the specified string is not null or empty.
        /// </summary>
        /// <param name="value">The string to be checked.</param>
        /// <param name="message">An optional message that describes the error.</param>
        /// <exception cref="Exception">Thrown when the string is null or empty.</exception>
        public static void IsNotNullOrEmpty(string value, string message = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Verifies that the specified argument is not null.
        /// </summary>
        /// <param name="argument">The argument to be checked.</param>
        /// <param name="argumentName">The name of the argument, used in the exception message.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        public static void ArgumentIsNotNull(object argument, string argumentName)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Verifies that the specified argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of the argument being checked.</typeparam>
        /// <param name="argument">The argument to be checked.</param>
        /// <param name="argumentName">The name of the argument, used in the exception message.</param>
        /// <param name="message">An optional message that describes the error.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        public static void ArgumentIsNotNull<T>(T argument, string argumentName, string message = null) where T : class
        {
            if (argument is null)
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }

        /// <summary>
        /// Verifies that the specified string argument is not null or empty.
        /// </summary>
        /// <param name="stringArgument">The string argument to be checked.</param>
        /// <param name="argumentName">The name of the argument, used in the exception message.</param>
        /// <exception cref="ArgumentException">Thrown when the string argument is null or empty.</exception>
        public static void ArgumentIsNotNullOrEmpty(string stringArgument, string argumentName)
        {
            if (string.IsNullOrEmpty(stringArgument))
            {
                throw new ArgumentException($"The specified string argument '{argumentName}' must not be null or empty.", argumentName);
            }
        }

        /// <summary>
        /// Verifies that the specified integer argument is positive.
        /// </summary>
        /// <param name="numberArgument">The integer argument to be checked.</param>
        /// <param name="argumentName">The name of the argument, used in the exception message.</param>
        /// <exception cref="ArgumentException">Thrown when the integer argument is negative.</exception>
        public static void IsPositive(int numberArgument, string argumentName)
        {
            if (numberArgument < 0)
            {
                throw new ArgumentException($"The specified integer argument '{argumentName}' must not be negative.", argumentName);
            }
        }
    }

}
