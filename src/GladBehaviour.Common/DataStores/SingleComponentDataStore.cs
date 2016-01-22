using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	[Serializable]
	public class SingleComponentDataStore : IDataUpdatable<UnityEngine.Component>
	{
		[SerializeField]
		public UnityEngine.Component StoredComponent { get; private set; }

		public SingleComponentDataStore(UnityEngine.Component com)
		{
			StoredComponent = com;
		}

		public void Update(Component newValue)
		{
			StoredComponent = newValue;
		}
	}
}
