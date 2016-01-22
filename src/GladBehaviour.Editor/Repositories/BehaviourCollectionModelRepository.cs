using GladBehaviour.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Editor
{
	public class BehaviourCollectionModelRepository : IBehaviourRepository
	{
		private readonly MonoBehaviour dataBehaviour;

		private readonly IReflectionStrategy reflectionStrat;

		private IEnumerable<CollectionDataStoreModel> cachedModels;

		private readonly object syncObj = new object();

		public BehaviourCollectionModelRepository(MonoBehaviour behaviour, IReflectionStrategy strat)
		{
			if (behaviour == null)
				throw new ArgumentNullException(nameof(behaviour), "Cannot deal with null " + nameof(MonoBehaviour) + ".");

			if (strat == null)
				throw new ArgumentNullException(nameof(strat), "Cannot handle serialization with a null " + nameof(IReflectionStrategy) + " strat.");

			dataBehaviour = behaviour;
			reflectionStrat = strat;
        }

		IEnumerable<CollectionDataStoreModel> BuildModels()
		{
			FieldInfo info = reflectionStrat.Field<ListCollectionSerializationAttribute>(typeof(GladMonoBehaviour), BindingFlags.NonPublic | BindingFlags.Instance);

			if (info == null)
				throw new InvalidOperationException("Unable to find the collection data. Should be marked with " + nameof(ListCollectionSerializationAttribute));

			object objValue = reflectionStrat.GetValue(dataBehaviour, info);

			if (objValue == null)
				throw new InvalidOperationException("Unable to get the value of the collection data. FieldInfo was found but value was not found.");

			List<CollectionComponentDataStore> dataStoreCollection = objValue as List<CollectionComponentDataStore>;

			if (dataStoreCollection == null)
				throw new InvalidOperationException("Unexpected Type of " + nameof(objValue) + " expected Type " + nameof(List<CollectionComponentDataStore>));

			//Double check locking is required. Idk what thread editor runs on
			if (cachedModels == null)
			{
				lock (syncObj)
					if (cachedModels == null)
						cachedModels = dataStoreCollection.Select(x => new CollectionDataStoreModel(x));
			}

			return dataStoreCollection.Select(x => new CollectionDataStoreModel(x));
        }

		IEnumerable<IDataStoreModel> IBehaviourRepository.BuildModels()
		{
			return this.BuildModels().Cast<IDataStoreModel>();
		}
	}
}
