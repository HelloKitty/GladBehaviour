using GladBehaviour.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	public interface IBehaviourDataController
	{
		IEditorDrawable RegisterModel(DataStoreModel<CollectionComponentDataStore> model);

		IEditorDrawable RegisterModel(DataStoreModel<SingleComponentDataStore> model);

		IEditorDrawable RegisterModel(IDataStoreModel model);
	}
}
