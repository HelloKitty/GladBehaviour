using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace GladBehaviour.Common
{
	public class CollectionContainerFieldMatcher : GeneralContainerFieldMatcher, IContainerFieldMatcher
	{
		public CollectionContainerFieldMatcher(Type typeToParse)
			: base(typeToParse)
		{
			if (typeToParse == null)
				throw new ArgumentNullException(nameof(typeToParse), "Cannot parse a null type.");
		}

		public override FieldInfo FindMatch(ISerializableContainer container)
		{
			//if we find an exact matching field then we return it as the match
			if(hasMatch(container))
			{
				return sortedFieldInfo.Value[container.SerializedName];
            }

			return null;
        }

		public override IEnumerable<FieldInfo> FindUnContainedFields<TSerializableContainerType>(IEnumerable<TSerializableContainerType> containers)
		{
			throw new NotImplementedException();
		}

		public override bool hasMatch(ISerializableContainer container)
		{
			if (sortedFieldInfo.Value.ContainsKey(container.SerializedName))
				if (SerializedTypeManipulator.isInterfaceCollectionType(sortedFieldInfo.Value[container.SerializedName].Type()))
					if (sortedFieldInfo.Value[container.SerializedName].Type().IsGenericType) //if true then it's something like IEnumerable<ISomething>
					{	
						if (container.SerializedType == sortedFieldInfo.Value[container.SerializedName].Type().GetGenericArguments().First())
							return true;
					}
					else
					{
						//It could still be an array. Those aren't considered generic
						if (sortedFieldInfo.Value[container.SerializedName].Type().IsArray)
							if (container.SerializedType == sortedFieldInfo.Value[container.SerializedName].Type().GetElementType())
								return true;
					}

			return false;
        }
	}
}
