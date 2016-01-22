using GladBehaviour.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	public class CollectionDataStoreModel : IDataStoreModel
	{
		private CollectionComponentDataStore serializedObject;

		public CollectionDataStoreModel(CollectionComponentDataStore datas)
		{
			serializedObject = datas;
        }

		object IDataStoreModel.SerializedObject
		{
			get { return serializedObject; }
		}

		public CollectionComponentDataStore SerializedObject { get { return serializedObject; } }

		public void Update(object newValue)
		{
			//Idk what if Unity uses List<UnityEngine.Component> under the IList or what
			//So we need to LINQ cast
			serializedObject.Update(((IList)newValue).Cast<UnityEngine.Object>().ToList());
		}
	}
}
