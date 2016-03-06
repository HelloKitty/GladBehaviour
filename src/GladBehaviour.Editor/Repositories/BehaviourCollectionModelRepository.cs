using GladBehaviour.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// Repository that provides access to <see cref="IDataStoreModel"/>s.
	/// </summary>
	public class BehaviourCollectionModelRepository : IBehaviourRepository
	{
		/// <summary>
		/// <see cref="GladMonoBehaviour"/> instance that <see cref="IDataStoreModel"/> instances should be
		/// generates from.
		/// </summary>
		private readonly GladMonoBehaviour dataBehaviour;

		/// <summary>
		/// Strategy for reflection.
		/// </summary>
		private readonly IReflectionStrategy reflectionStrat;

		/// <summary>
		/// Threading lock/sync object.
		/// </summary>
		private readonly object syncObj = new object();

		/// <summary>
		/// Creates a <see cref="IBehaviourRepository"/> based on the given <see cref="GladMonoBehaviour"/>
		/// instance provided and uses <see cref="IReflectionStrategy"/> for parsing the Type info.
		/// </summary>
		/// <param name="behaviour">Instance to parse.</param>
		/// <param name="strat">Strategy for parsing.</param>
		public BehaviourCollectionModelRepository(GladMonoBehaviour behaviour, IReflectionStrategy strat)
		{
			if (behaviour == null)
				throw new ArgumentNullException(nameof(behaviour), "Cannot deal with null " + nameof(MonoBehaviour) + ".");

			if (strat == null)
				throw new ArgumentNullException(nameof(strat), "Cannot handle serialization with a null " + nameof(IReflectionStrategy) + " strat.");

			dataBehaviour = behaviour;
			reflectionStrat = strat;
		}

		IEnumerable<IDataStoreModel> BuildSingleModels()
		{
			FieldInfo info = reflectionStrat.Field<SingleCollectionSerializationAttribute>(typeof(GladMonoBehaviour), BindingFlags.NonPublic | BindingFlags.Instance);

			if (info == null)
				throw new InvalidOperationException("Unable to find the collection data. Should be marked with " + nameof(SingleCollectionSerializationAttribute));

			object objValue = reflectionStrat.GetValue(dataBehaviour, info);

			if (objValue == null)
				throw new InvalidOperationException("Unable to get the value of the collection data. FieldInfo was found but value was not found.");

			List<SingleComponentDataStore> dataStoreCollection = objValue as List<SingleComponentDataStore>;

			if (dataStoreCollection == null)
				throw new InvalidOperationException("Unexpected Type of " + nameof(objValue) + " expected Type " + nameof(List<SingleComponentDataStore>));

			return dataStoreCollection.Select(x => new DataStoreModel<SingleComponentDataStore>(x)).Cast<IDataStoreModel>();
		}

		IEnumerable<IDataStoreModel> BuildCollectionModels()
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

			return dataStoreCollection.Select(x => new DataStoreModel<CollectionComponentDataStore>(x)).Cast<IDataStoreModel>();
		}

		IEnumerable<IDataStoreModel> IBehaviourRepository.BuildModels()
		{
			return Enumerable.Concat(BuildCollectionModels(), BuildSingleModels());
		}
	}
}
