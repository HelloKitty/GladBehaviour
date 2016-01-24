using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Common
{
	//Unity doesn't really allow us to inject things so
	//the next best thing to do is sadly make things static
	public static class SerializedTypeManipulator
	{
		public static bool isInterfaceCollectionType(Type type)
		{
			return type != null && type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type) && !type.GetGenericArguments().First().IsPrimitive;
		}
	}
}
