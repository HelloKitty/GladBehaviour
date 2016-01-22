using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GladBehaviour.Editor
{
	public class CollectionView : IEditorDrawable, IView
	{
		public event OnValueChanged OnEditorValueChanged;

		private readonly IList<UnityEngine.Object> listData;

		private readonly string headerDisplayName;

		public CollectionView(/*yep Unity uses IList*/IList<UnityEngine.Object> currentCollection, string propName)
		{
			listData = currentCollection;
			headerDisplayName = propName;
        }

		public void Draw()
		{
			EditorGUI.BeginChangeCheck();

			ReorderableList list = new ReorderableList((IList)listData, typeof(UnityEngine.Object)); //should be a list of MonoBehaviour

			//This is copy pasted from ExtendedEvent
			//Required to setup the list
			list.draggable = false;
			list.elementHeight *= 2;
			list.drawHeaderCallback = DrawHeaderInternal;
			list.drawElementCallback = DrawElementInternal;
			list.onAddCallback = AddInternal;
			list.onRemoveCallback = RemoveInternal;

			list.DoLayoutList();

			if(EditorGUI.EndChangeCheck()) //check if the editor changed since drawing the list
			{
				if (OnEditorValueChanged != null)
					OnEditorValueChanged(this, new GladBehaviourValueChangedArgs(list.list));
			}
		}

		private void DrawHeaderInternal(Rect rect)
		{
			EditorGUI.LabelField(rect, headerDisplayName);

			/*if (serializedProperty.isInstantiatedPrefab)
			{
				if (GUI.Button(new Rect(rect.x + rect.width * 0.85f, rect.y, rect.width * 0.15f, rect.height), "Apply"))
				{
					EditorUtility.SetDirty(serializedProperty.serializedObject.targetObject);
				}
			}*/
		}

		private void DrawElementInternal(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.yMin += 3f;
			rect.yMax -= 7f;

			var thirdWidth = rect.width / 3;
			var halfHeight = rect.height / 2;

			//swap GO Field Rect for the dropdown in ExtendedEvent source
			var gameObjectRect = new Rect(rect.x + thirdWidth, rect.y, thirdWidth * 2, halfHeight);  
			var dropdownRect = new Rect(rect.x, rect.y, thirdWidth, halfHeight);

			/*var gameObjectRect = new Rect(rect.x, rect.y, thirdWidth, halfHeight);
			var dropdownRect = new Rect(rect.x + thirdWidth, rect.y, thirdWidth * 2, halfHeight);
			var bottomRect = new Rect(rect.x, rect.y + halfHeight, rect.width, halfHeight);*/

			EditorGUI.BeginChangeCheck();
			GUI.Box(gameObjectRect, "");

			listData[index] = (UnityEngine.Object)EditorGUI.ObjectField(gameObjectRect, listData[index], typeof(UnityEngine.Object), true);

			if (EditorGUI.EndChangeCheck())
			{
				//Maybe, do the value changed thing here.
				//listener.Reset();
			}
		}

		private void AddInternal(ReorderableList list)
		{
			//eEvent.Listeners.Add(new ExtendedEvent.GameObjectContainer());
		}

		private void RemoveInternal(ReorderableList list)
		{
			listData.RemoveAt(list.index);
		}
	}
}
