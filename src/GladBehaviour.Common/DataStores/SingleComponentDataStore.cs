using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Common
{
	/// <summary>
	/// Serializable mutable container of a component that persists type data.
	/// </summary>
	[Serializable]
	public class SingleComponentDataStore : IDataUpdatable<UnityEngine.Object>, ISerializableContainer
	{
		/// <summary>
		/// Serializable mutable object field for serialized component.
		/// </summary>
		[SerializeField]
		private UnityEngine.Object storedComponent;

		/// <summary>
		/// Serializable mutable object field for serialized component.
		/// </summary>
		public UnityEngine.Object StoredComponent { get { return storedComponent; } }

		/// <summary>
		/// Serializable Type name data in assembly qualified string form.
		/// </summary>
		[SerializeField]
		private string serializedTypeName;

		//caching should make this quicker
		private Type cachedType = null;
		/// <summary>
		/// Inner type this collection can map to.
		/// </summary>
		public Type SerializedType { get { return cachedType == null ? cachedType = Type.GetType(serializedTypeName, true, false) : cachedType; } }

		/// <summary>
		/// Serializable member name this container is serializing for.
		/// </summary>
		[SerializeField]
		private string serializedMemberName;

		/// <summary>
		/// Serializable member name this container is serializing for.
		/// </summary>
		public string SerializedName { get { return serializedMemberName; } }

		/// <summary>
		/// Indicates if the Type persisted can be loaded.
		/// </summary>
		public bool canLoadType
		{
			get
			{
				return serializedTypeName != null && Type.GetType(serializedTypeName) != null;
			}
		}

		public string SerializedTypeName { get { return serializedTypeName; } }

		/// <summary>
		/// Should only be called by Unity. Do not invoke this constructor manually.
		/// </summary>
		private SingleComponentDataStore()
		{

		}

		/// <summary>
		/// Creates a new serializable container of the given Type and 
		/// </summary>
		/// <param name="dataType">Type of the component.</param>
		/// <param name="name">Member name this container is persisting for.</param>
		public SingleComponentDataStore(Type dataType, string name)
			: this(null, dataType, name)
		{

		}

		/// <summary>
		/// Creates a new serializable container of the given Type and 
		/// </summary>
		/// <param name="com">Initial component reference.</param>
		/// <param name="dataType">Type of the component.</param>
		/// <param name="name">Member name this container is persisting for.</param>
		public SingleComponentDataStore(UnityEngine.Object com, Type dataType, string name)
		{
			//Test to make sure the type is actually valid
			//If the com object does not derive or implement dataType then it'll throw
			if (com != null)
				if (!dataType.IsAssignableFrom(com.GetType()))
					throw new ArgumentException("Invalid dataType given for the initial component. Component type does not derive or implement from dataType", nameof(dataType));

			serializedTypeName = dataType.AssemblyQualifiedName;
			serializedMemberName = name;
			storedComponent = com;
		}

		/// <summary>
		/// Updates the internal component.
		/// </summary>
		/// <param name="newValue">Component to set as the new internal component.</param>
		public void Update(UnityEngine.Object newValue)
		{
			storedComponent = newValue;
		}


		/// <summary>
		/// Updates the internal component.
		/// </summary>
		/// <param name="newValue">Component to set as the new internal component.</param>
		public void Update(object newValue)
		{
			storedComponent = newValue as UnityEngine.Object;

			if (storedComponent == null && newValue != null)
				throw new InvalidOperationException($"Provided component was not a valid {nameof(UnityEngine.Object)} and thus casting was null.");
		}
	}
}
