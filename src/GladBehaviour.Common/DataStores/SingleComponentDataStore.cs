using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	[Serializable]
	public class SingleComponentDataStore
	{
		[SerializeField]
		public readonly UnityEngine.Component StoredComponent;

		public SingleComponentDataStore(UnityEngine.Component com)
		{
			StoredComponent = com;
		}
	}
}
