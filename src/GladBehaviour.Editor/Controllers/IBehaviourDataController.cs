using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	public interface IBehaviourDataController
	{
		IEditorDrawable RegisterModel(CollectionDataStoreModel model);

		IEditorDrawable RegisterModel(IDataStoreModel model);
	}
}
