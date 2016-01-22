using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	[Serializable]
	public class CollectionComponentDataStore : IEnumerable<SingleComponentDataStore>, IDataUpdatable<List<UnityEngine.Component>>
	{
		//Idk if the Unity editor will ever go threaded for serialization
		private readonly object syncObj = new object();

		[SerializeField]
		public List<SingleComponentDataStore> dataStoreCollection;

		public CollectionComponentDataStore()
		{
			dataStoreCollection = new List<SingleComponentDataStore>();
		}

		public CollectionComponentDataStore(int initialSize)
		{
			dataStoreCollection = new List<SingleComponentDataStore>(initialSize);
		}

		public CollectionComponentDataStore(IEnumerable<SingleComponentDataStore> data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data), "Cannot init with a null collection.");

			dataStoreCollection = data.ToList();
		}

		public IEnumerator<SingleComponentDataStore> GetEnumerator()
		{
			return dataStoreCollection?.GetEnumerator();
		}

		public void Update(List<UnityEngine.Component> newValue)
		{
			lock(syncObj)
				dataStoreCollection = newValue.Select(x => new SingleComponentDataStore(x)).ToList();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dataStoreCollection?.GetEnumerator();
		}
	}
}
