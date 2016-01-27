using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;
using UnityEngine;

namespace GladBehaviour.Common
{
	//Unity doesn't really allow us to inject things so
	//the next best thing to do is sadly make things static
	public static class SerializedTypeManipulator
	{
		public static bool isInterfaceCollectionType(Type type)
		{
			if (type != null)
				if (typeof(IEnumerable).IsAssignableFrom(type))
					if (type.IsGenericType) //if it's a generic type like IEnumerable<ISomething> then we're good
					{
						if (type.GetGenericArguments().First().IsInterface)
							return true;
					}
					else
					{
						//If it's not a generic type it may be an array type.
						//Array types aren't considered generic types
						if (type.IsArray)
							if (type.GetElementType().IsInterface)
								return true;
					}
					

			return false;
		}

		public static IEnumerable<FieldInfo> GetCollectionFields(Type t)
		{
			if (t == null)
				throw new ArgumentNullException(nameof(t), "Type to be parsed for collection fields must not be null.");

			return t.FieldsWith(Flags.InstanceAnyVisibility, typeof(SerializeField))
				.Where(x => !x.HasAttribute<HideInInspector>()) //don't want hidden members
				.Where(x => isInterfaceCollectionType(x.Type()));
		}

		public static Type CollectionInferaceType(Type t)
		{
			if (t == null)
				throw new ArgumentNullException(nameof(t), "The type cannot be null.");

			if (!isInterfaceCollectionType(t))
				throw new InvalidOperationException("Cannot find the collection Type of a non interface collection.");

			if (t.IsGenericType) //has a generic arg then
				return t.GetGenericArguments().First();
			else
			{
				//maybe it's an array
				if (t.IsArray)
					return t.GetElementType();
			}

			throw new InvalidOperationException(t.FullName + " is not a valid interface collection type.");
		}
	}
}
