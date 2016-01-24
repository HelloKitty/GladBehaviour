using GladBehaviour.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// Generic data model that wraps an updatable container source.
	/// </summary>
	/// <typeparam name="TSerializableContainerType">Container type.</typeparam>
	public class DataStoreModel<TSerializableContainerType> : IDataStoreModel
		where TSerializableContainerType : ISerializableContainer, IDataUpdatable
	{
		/// <summary>
		/// Wrapped container object.
		/// </summary>
		private TSerializableContainerType serializedObject;

		/// <summary>
		/// Serialized container object.
		/// </summary>
		object ISerializedObjectReferenceProvider.SerializedObject
		{
			get { return serializedObject; }
		}

		/// <summary>
		/// Serialized container object.
		/// </summary>
		public TSerializableContainerType SerializedObject { get { return serializedObject; } }

		/// <summary>
		/// Serialized data <see cref="Type"/> of the internal container.
		/// </summary>
		public Type DataType { get { return serializedObject.SerializedType; } }

		/// <summary>
		/// Serialized name of the member this container is persisting for.
		/// </summary>
		public string SerializedName { get { return serializedObject.SerializedName; } }

		/// <summary>
		/// Creates a new model for the given container.
		/// </summary>
		/// <param name="datas"></param>
		public DataStoreModel(TSerializableContainerType datas)
		{
			if (datas == null)
				throw new ArgumentNullException(nameof(datas), "The container instance cannot be null.");

			serializedObject = datas;
        }

		/// <summary>
		/// Updates a value.
		/// </summary>
		/// <param name="newValue">Value to update to.</param>
		public void Update(object newValue)
		{
			serializedObject.Update(newValue);
		}
	}
}
