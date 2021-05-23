using Healin.Shared.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace Healin.Shared.Utils
{
    public static class AssertionConcern
    {
        public static void AssertArgumentEquals(object object1, object object2, string message)
        {
            if (!object1.Equals(object2))
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentFalse(bool boolValue, string message)
        {
            if (boolValue)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentLength(string stringValue, int maximum, string message)
        {
            int length = stringValue.Length;
            if (length > maximum)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentLength(string stringValue, int minimum, int maximum, string message)
        {
            int length = stringValue.Trim().Length;
            if (length < minimum || length > maximum)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentExactLength(string stringValue, int exactLength, string message)
        {
            int length = stringValue.Trim().Length;
            if (length != exactLength)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentMatches(string pattern, string stringValue, string message)
        {
            if (stringValue is not null && !Regex.IsMatch(stringValue, pattern))
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentNotEmpty(string stringValue, string message)
        {
            if (stringValue == null || stringValue.Trim().Length == 0)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentNotEmpty(Guid value, string message)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentNotEquals(object object1, object object2, string message)
        {
            if (object1.Equals(object2))
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentNotNull(object object1, string message)
        {
            if (object1 == null)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentRange(double value, double minimum, double maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentRange(float value, float minimum, float maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentRange(int value, int minimum, int maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentRange(long value, long minimum, long maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentRange(DateTime value, DateTime minimum, DateTime maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertArgumentTrue(bool boolValue, string message)
        {
            if (!boolValue)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertStateFalse(bool boolValue, string message)
        {
            if (boolValue)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertStateTrue(bool boolValue, string message)
        {
            if (!boolValue)
            {
                throw new DomainException(message);
            }
        }

        public static void AssertIsEnum(object value, Type enumType, string message)
        {
            if (!Enum.IsDefined(enumType, value))
            {
                throw new DomainException(message);
            }
        }
    }
}
