using GladBehaviour.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GladBehaviour.Editor
{
	[CustomEditor(typeof(GladMonoBehaviour), true)]
	public class GladMonoBehaviourEditor : UnityEditor.Editor
	{
		//We mostly don't make these ephemeral because it would lag the editor
		//it's better to cache this stuff so that it's available as long as the editor doesn't
		//switch windows
		IBehaviourRepository collectionRepo;
		IReflectionStrategy reflectionStrat;
		IBehaviourDataController controller;

		IEnumerable<IEditorDrawable> views;

		public override void OnInspectorGUI()
		{
			//Draws the default for all fields we don't handle
			DrawDefaultInspector();

			//You shouldn't change components before repaint
			if (Event.current.type == EventType.Layout)
			{
				if (views == null)
					Init();
			}

			//draws all views AKA the lists/fields
			foreach (IEditorDrawable v in views)
				v.Draw();
		}

		//Can't do this in constructor for some reason
		private void Init()
		{
			if (target == null)
				throw new ArgumentException(nameof(target), "Target is null for some reason.");

			reflectionStrat = new FasterflectReflectionStrategy();
			collectionRepo = new BehaviourCollectionModelRepository(target as GladMonoBehaviour, reflectionStrat);
			controller = new GladMonoBehaviourDataController();

			IEnumerable<IDataStoreModel> models = collectionRepo.BuildModels();

			List<IEditorDrawable> tempViewList = new List<IEditorDrawable>(models.Count() * 2);

			foreach (var dm in models)
				tempViewList.Add(controller.RegisterModel(dm));

			views = tempViewList;
		}
	}
}
