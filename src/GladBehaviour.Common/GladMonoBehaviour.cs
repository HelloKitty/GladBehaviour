using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	public class GladMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
	{


		public void OnAfterDeserialize()
		{
			throw new NotImplementedException();
		}

		public void OnBeforeSerialize()
		{
			throw new NotImplementedException();
		}
	}
}
