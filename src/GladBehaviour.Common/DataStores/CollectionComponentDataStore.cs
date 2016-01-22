using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	[Serializable]
	public class CollectionComponentDataStore : IEnumerable<SingleComponentDataStore>
	{
		[SerializeField]
		List<SingleComponentDataStore> dataStoreCollection;

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
			dataStoreCollection = data.ToList();
		}

		public IEnumerator<SingleComponentDataStore> GetEnumerator()
		{
			return dataStoreCollection?.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dataStoreCollection?.GetEnumerator();
		}
	}
}
