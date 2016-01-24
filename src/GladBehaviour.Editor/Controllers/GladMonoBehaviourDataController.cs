using GladBehaviour.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// Controller that manages models and views for behaviours.
	/// Manages registeration of models and creation and setup of views.
	/// </summary>
	public class GladMonoBehaviourDataController : IBehaviourDataController
	{
		/// <summary>
		/// Registers a given <see cref="IDataStoreModel"/> and generates a <see cref="IEditorDrawable"/>
		/// view for the model.
		/// </summary>
		/// <param name="model">Model to register.</param>
		/// <returns>A new view setup to push messages back to the model.</returns>
		public IEditorDrawable RegisterModel(IDataStoreModel model)
		{
			if (model is DataStoreModel<CollectionComponentDataStore>)
				return RegisterModel(model as DataStoreModel<CollectionComponentDataStore>);
			else
				return RegisterModel(model as DataStoreModel<SingleComponentDataStore>);
		}

		/// <summary>
		/// Registers a given <see cref="DataStoreModel{SingleComponentDataStore}"/> and generates a <see cref="IEditorDrawable"/>
		/// view for the model.
		/// </summary>
		/// <param name="model">Model to register.</param>
		/// <returns>A new view setup to push messages back to the model.</returns>
		public IEditorDrawable RegisterModel(DataStoreModel<SingleComponentDataStore> model)
		{
			SingleView view = new SingleView(() => model.SerializedObject?.StoredComponent, model.DataType, model.SerializedName);

			view.OnEditorValueChanged += (s, args) => model.Update(args.ChangedValue);

			return view;
		}

		/// <summary>
		/// Registers a given <see cref="DataStoreModel{CollectionComponentDataStore}"/> and generates a <see cref="IEditorDrawable"/>
		/// view for the model.
		/// </summary>
		/// <param name="model">Model to register.</param>
		/// <returns>A new view setup to push messages back to the model.</returns>
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
