using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	[Serializable]
	public class SingleComponentDataStore : IDataUpdatable<UnityEngine.Object>
	{
		[SerializeField]
		public UnityEngine.Object StoredComponent { get; private set; }

		public SingleComponentDataStore(UnityEngine.Object com)
		{
			StoredComponent = com;
		}

		public void Update(UnityEngine.Object newValue)
		{
			StoredComponent = newValue;
		}
	}
}
