using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	public class GladMonoBehaviourDataController : IBehaviourDataController
	{
		public IEditorDrawable RegisterModel(IDataStoreModel model)
		{
			if (model is CollectionDataStoreModel)
				return RegisterModel(model as CollectionDataStoreModel);
			else
				return null;
		}

		public IEditorDrawable RegisterModel(CollectionDataStoreModel model)
		{
			//create the view for lists
			CollectionView view = new CollectionView(model.SerializedObject.dataStoreCollection, "Test");

			//setup the view to send updates back to the model
			view.OnEditorValueChanged += (s, args) => model.Update(args.ChangedValue);

			return view;
		}
	}
}
