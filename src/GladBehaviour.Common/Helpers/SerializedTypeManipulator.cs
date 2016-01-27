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
	}
}
