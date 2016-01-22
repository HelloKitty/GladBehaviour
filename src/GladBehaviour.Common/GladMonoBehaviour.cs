using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	public class GladMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField]
		[SingleCollectionSerialization]
		private List<SingleComponentDataStore> singleDataStoreCollection;

		[SerializeField]
		[SingleCollectionSerialization]
		private List<CollectionComponentDataStore> collectionDataStoreCollection;

		public void OnAfterDeserialize()
		{
			//throw new NotImplementedException();
		}

		public void OnBeforeSerialize()
		{
			//throw new NotImplementedException();
		}
	}
}
