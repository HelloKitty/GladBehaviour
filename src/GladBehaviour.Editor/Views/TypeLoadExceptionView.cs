using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace GladBehaviour.Editor
{
	public class TypeLoadExceptionView : IEditorDrawable, IView
	{
		public event OnValueChanged OnEditorValueChanged;

		private string SerializedTypeName { get; }

		public TypeLoadExceptionView(string serializedTypeName)
		{
			SerializedTypeName = serializedTypeName;
		}

		public void Draw()
		{
			EditorGUILayout.LabelField($"The Type: {SerializedTypeName} is not found. Check box to remove reference.");

			//if the user clicks the box we should null out the reference.
			if(EditorGUILayout.Toggle(false))
			{

			}
		}
	}
}
