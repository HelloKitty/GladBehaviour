using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	[Serializable]
	public class SingleComponentDataStore : IDataUpdatable<UnityEngine.Object>, ISerializableContainer
	{
		[SerializeField]
		private UnityEngine.Object storedComponent;

		public UnityEngine.Object StoredComponent { get { return storedComponent; } }

		[SerializeField]
		private string serializedTypeName;

		//caching should make this quicker
		private Type cachedType = null;
		public Type SerializedType { get { return cachedType == null ? cachedType = Type.GetType(serializedTypeName, true, false) : cachedType; } }

		[SerializeField]
		private string serializedMemberName;

		public string SerializedName { get { return serializedMemberName; } }

		public bool canLoadType
		{
			get
			{
				return serializedTypeName != null && Type.GetType(serializedTypeName) != null;
			}
		}

		private SingleComponentDataStore()
		{

		}

		public SingleComponentDataStore(Type dataType, string name)
		{
			serializedTypeName = dataType.AssemblyQualifiedName;
			serializedMemberName = name;
		}


		public SingleComponentDataStore(UnityEngine.Object com, Type dataType, string name)
		{
			serializedTypeName = dataType.AssemblyQualifiedName;
			serializedMemberName = name;
			storedComponent = com;
		}

		public void Update(UnityEngine.Object newValue)
		{
			storedComponent = newValue;
		}

		public void Update(object newValue)
		{
			storedComponent = newValue as UnityEngine.Object;

			if (storedComponent == null && newValue != null)
				throw new InvalidOperationException("Component was null but newValue wasn't.");
		}
	}
}
