using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	[Serializable]
	public class CollectionComponentDataStore : IEnumerable<UnityEngine.Object>, IDataUpdatable<List<UnityEngine.Object>>
	{
		//Idk if the Unity editor will ever go threaded for serialization
		private readonly object syncObj = new object();

		[SerializeField]
		public List<UnityEngine.Object> dataStoreCollection;

		public CollectionComponentDataStore()
		{
			dataStoreCollection = new List<UnityEngine.Object>();
		}

		public CollectionComponentDataStore(int initialSize)
		{
			dataStoreCollection = new List<UnityEngine.Object>(initialSize);
		}

		public CollectionComponentDataStore(IEnumerable<UnityEngine.Object> data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data), "Cannot init with a null collection.");

			dataStoreCollection = data.ToList();
		}

		public IEnumerator<UnityEngine.Object> GetEnumerator()
		{
			return dataStoreCollection?.GetEnumerator();
		}

		public void Update(List<UnityEngine.Object> newValue)
		{
			lock(syncObj)
				dataStoreCollection = newValue;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dataStoreCollection?.GetEnumerator();
		}
	}
}
