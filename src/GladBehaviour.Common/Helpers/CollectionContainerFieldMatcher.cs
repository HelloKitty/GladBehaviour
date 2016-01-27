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

		//TODO: Implement Lazy<T> for this field. Maybe use the crappy NetEssentials lib version
		private Dictionary<string, FieldInfo> sortedFieldInfo;

		private readonly object syncObj = new object();

		public CollectionContainerFieldMatcher(Type typeToParse)
		{
			if (typeToParse == null)
				throw new ArgumentNullException(nameof(typeToParse), "Cannot parse a null type.");

			parseTarget = typeToParse;
        }

		public FieldInfo FindMatch(ISerializableContainer container)
		{
			//double check locking
			if(sortedFieldInfo == null)
				lock(syncObj)
					if (sortedFieldInfo == null)
					{
						//prepare the dictionary
						//It'll help speed things up with O(1) lookup
						sortedFieldInfo = new Dictionary<string, FieldInfo>();
						
						foreach(FieldInfo fi in SerializedTypeManipulator.GetCollectionFields(parseTarget))
						{
							sortedFieldInfo.Add(fi.Name, fi);
						}
					}
						

			if (sortedFieldInfo.ContainsKey(container.SerializedName))
				if (SerializedTypeManipulator.isInterfaceCollectionType(sortedFieldInfo[container.SerializedName].Type()))
					if (container.SerializedType == sortedFieldInfo[container.SerializedName].Type().GetGenericArguments().First())
					{
						return sortedFieldInfo[container.SerializedName];
                    }

			return null;
        }

		public IEnumerable<FieldInfo> FindUnContainedFields<TSerializableContainerType>(IEnumerable<TSerializableContainerType> containers) where TSerializableContainerType : ISerializableContainer
		{
			throw new NotImplementedException();
		}

		public bool hasMatch(ISerializableContainer container)
		{
			throw new NotImplementedException();
		}

		private void EnsureDataInit()
		{

		}
	}
}
