using GladBehaviour.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// Implemented is a controller that manages models and views for behaviours.
	/// </summary>
	public interface IBehaviourDataController
	{
		/// <summary>
		/// Registers a given <see cref="IDataStoreModel"/> and generates a <see cref="IEditorDrawable"/>
		/// view for the model.
		/// </summary>
		/// <param name="model">Model to register.</param>
		/// <returns>A new view setup from the model.</returns>
		IEditorDrawable RegisterModel(DataStoreModel<CollectionComponentDataStore> model);

		/// <summary>
		/// Registers a given <see cref="IDataStoreModel"/> and generates a <see cref="IEditorDrawable"/>
		/// view for the model.
		/// </summary>
		/// <param name="model">Model to register.</param>
		/// <returns>A new view setup from the model.</returns>
		IEditorDrawable RegisterModel(DataStoreModel<SingleComponentDataStore> model);

		/// <summary>
		/// Registers a given <see cref="IDataStoreModel"/> and generates a <see cref="IEditorDrawable"/>
		/// view for the model.
		/// </summary>
		/// <param name="model">Model to register.</param>
		/// <returns>A new view setup from the model.</returns>
		IEditorDrawable RegisterModel(IDataStoreModel model);
	}
}
