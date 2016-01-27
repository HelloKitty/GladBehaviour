using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace GladBehaviour.Common
{
	public class CollectionContainerFieldMatcher : IContainerFieldMatcher
	{
		private readonly Type parseTarget;

		private readonly Lazy<Dictionary<string, FieldInfo>> sortedFieldInfo;

		public CollectionContainerFieldMatcher(Type typeToParse)
		{
			if (typeToParse == null)
				throw new ArgumentNullException(nameof(typeToParse), "Cannot parse a null type.");

			parseTarget = typeToParse;

			sortedFieldInfo = new Lazy<Dictionary<string, FieldInfo>>(CreateDictionary, true);
        }

		public FieldInfo FindMatch(ISerializableContainer container)
		{
			//if we find an exact matching field then we return it as the match
			if(hasMatch(container))
			{
				return sortedFieldInfo.Value[container.SerializedName];
            }

			return null;
        }

		public IEnumerable<FieldInfo> FindUnContainedFields<TSerializableContainerType>(IEnumerable<TSerializableContainerType> containers) where TSerializableContainerType : ISerializableContainer
		{
			throw new NotImplementedException();
		}

		public bool hasMatch(ISerializableContainer container)
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

		private Dictionary<string, FieldInfo> CreateDictionary()
		{
			Dictionary<string, FieldInfo> dict = new Dictionary<string, FieldInfo>();

			//prepare the dictionary
			//It'll help speed things up with O(1) lookup
			foreach (FieldInfo fi in SerializedTypeManipulator.GetCollectionFields(parseTarget))
			{
				dict.Add(fi.Name, fi);
			}

			return dict;
		}
	}
}
