﻿using System;
using System.Collections.Generic;

namespace NoWoL.TestingUtilities.ObjectCreators
{
    /// <summary>
    /// Provides a way to create IEnumerable of a given type
    /// </summary>
    public class GenericIEnumerableCreator : IObjectCreator
    {
        /// <summary>
        /// Determines whether this instance can create the specified object type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <returns><c>true</c> if this instance can create the specified object type; otherwise, <c>false</c>.</returns>
        public bool CanHandle(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return IsIEnumerable(type);
        }

        internal static bool IsIEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        /// <summary>
        /// Create an instance of the specified object type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="objectCreators">A list of <see cref="IObjectCreator"/> to handle creation of sub objects.</param>
        /// <returns>The created object.</returns>
        public object Create(Type type, ICollection<IObjectCreator> objectCreators)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (objectCreators == null)
            {
                throw new ArgumentNullException(nameof(objectCreators));
            }

            if (CanHandle(type))
            {
                var itemType = type.GetGenericArguments()[0];
                return CreatorHelpers.CreateArray(itemType, objectCreators);
            }

            throw new UnsupportedTypeException("Expecting an IEnumerable<> type however received " + type.FullName);
        }
    }
}