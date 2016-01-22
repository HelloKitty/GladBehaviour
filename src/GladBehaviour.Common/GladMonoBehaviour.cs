using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	public class GladMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
	{
		[HideInInspector]
		[SerializeField]
		[SingleCollectionSerialization]
		private List<SingleComponentDataStore> singleDataStoreCollection;

		[HideInInspector]
		[SerializeField]
		[ListCollectionSerialization]
		private List<CollectionComponentDataStore> collectionDataStoreCollection;

		public void OnAfterDeserialize()
		{
			singleDataStoreCollection = new List<SingleComponentDataStore>();
			singleDataStoreCollection.Add(new SingleComponentDataStore(this));

			collectionDataStoreCollection = new List<CollectionComponentDataStore>();
			collectionDataStoreCollection.Add(new CollectionComponentDataStore(new List<UnityEngine.Object>() { this }));
		}

		public void OnBeforeSerialize()
		{
			singleDataStoreCollection = new List<SingleComponentDataStore>();
			singleDataStoreCollection.Add(new SingleComponentDataStore(this));

			collectionDataStoreCollection = new List<CollectionComponentDataStore>();
			collectionDataStoreCollection.Add(new CollectionComponentDataStore(new List<UnityEngine.Object>() { this }));
		}
	}
}
