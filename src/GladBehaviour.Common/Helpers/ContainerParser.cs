using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace GladBehaviour.Common
{
	public class ContainerParser<TSerializedContainerType>
		where TSerializedContainerType : class, ISerializableContainer
    {
		private readonly IEnumerable<TSerializedContainerType> containerCollection;

		private readonly IContainerFieldMatcher matchStrategy;

		public ContainerParser(IEnumerable<TSerializedContainerType> collection, IContainerFieldMatcher matchStrat)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection), "Cannot parse a null " + nameof(ISerializableContainer) + " collection.");

			if (matchStrat == null)
				throw new ArgumentNullException(nameof(matchStrat), "Cannot parse container collection with a null " + nameof(IContainerFieldMatcher));

			containerCollection = collection;
			matchStrategy = matchStrat;
        }

		public IEnumerable<TSerializedContainerType> ComputeStaleContainers()
		{
			List<TSerializedContainerType> staleFields = new List<TSerializedContainerType>(containerCollection.Count()); //Too large an allocation is probably better than multiple

			//if it's empty we don't need to do searches
			if (containerCollection.Count() == 0)
				return staleFields;

			foreach (TSerializedContainerType con in containerCollection)
			{
				//If it doesn't have a match we should
				//add it to the collection so it can be removed
				if (!matchStrategy.hasMatch(con))
					staleFields.Add(con);
			}

			return staleFields;
		}

		public IEnumerable<FieldInfo> FindNewCollectionMembers()
		{
			return matchStrategy.FindUnContainedFields(containerCollection);
		}
	}
}
