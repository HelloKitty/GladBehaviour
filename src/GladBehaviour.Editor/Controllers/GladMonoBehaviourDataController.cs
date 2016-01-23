using GladBehaviour.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Editor
{
	public class GladMonoBehaviourDataController : IBehaviourDataController
	{
		public IEditorDrawable RegisterModel(IDataStoreModel model)
		{
			if (model is DataStoreModel<CollectionComponentDataStore>)
				return RegisterModel(model as DataStoreModel<CollectionComponentDataStore>);
			else
				return RegisterModel(model as DataStoreModel<SingleComponentDataStore>);
		}

		public IEditorDrawable RegisterModel(DataStoreModel<SingleComponentDataStore> model)
		{
			SingleView view = new SingleView(() => model.SerializedObject?.StoredComponent, model.DataType, model.SerializedName);

			view.OnEditorValueChanged += (s, args) => model.Update(args.ChangedValue);

			return view;
		}

		public IEditorDrawable RegisterModel(DataStoreModel<CollectionComponentDataStore> model)
		{
			//create the view for lists
			CollectionView view = new CollectionView(model.SerializedObject.dataStoreCollection, model.SerializedName, model.DataType);

			//setup the view to send updates back to the model
			view.OnEditorValueChanged += (s, args) => model.Update(args.ChangedValue);

			return view;
		}
	}
}
