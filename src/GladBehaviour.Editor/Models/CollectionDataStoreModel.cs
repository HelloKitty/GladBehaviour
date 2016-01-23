using GladBehaviour.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	public class DataStoreModel<TSerializableContainerType> : IDataStoreModel
		where TSerializableContainerType : ISerializableContainer, IDataUpdatable
	{
		private TSerializableContainerType serializedObject;

		object ISerializedObjectReferenceProvider.SerializedObject
		{
			get { return serializedObject; }
		}

		public TSerializableContainerType SerializedObject { get { return serializedObject; } }

		public Type DataType { get { return serializedObject.SerializedType; } }

		public string SerializedName { get { return serializedObject.SerializedName; } }

		public DataStoreModel(TSerializableContainerType datas)
		{
			serializedObject = datas;
        }

		public void Update(object newValue)
		{
			serializedObject.Update(newValue);
		}
	}
}
