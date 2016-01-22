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

		private readonly IList listData;

		public CollectionView(/*yep Unity uses IList*/IList currentCollection)
		{
			listData = currentCollection;
		}

		public void Draw()
		{
			EditorGUI.BeginChangeCheck();

			ReorderableList list = new ReorderableList(listData, typeof(MonoBehaviour)); //should be a list of MonoBehaviour
			list.DoLayoutList();

			if(EditorGUI.EndChangeCheck()) //check if the editor changed since drawing the list
			{
				if (OnEditorValueChanged != null)
					OnEditorValueChanged(this, new GladBehaviourValueChangedArgs(list.list));
			}
		}
	}
}
