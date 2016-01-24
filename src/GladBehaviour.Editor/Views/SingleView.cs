using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GladBehaviour.Editor
{
	public class SingleView : IEditorDrawable, IView
	{
		public event OnValueChanged OnEditorValueChanged;

		private Func<UnityEngine.Object> objectValueProvider;

		private readonly Type serializedObjectType;

		private readonly string memberName;

		public SingleView(Func<UnityEngine.Object> refProvider, Type dataType, string propName)
		{
			objectValueProvider = refProvider;
			serializedObjectType = dataType;
			memberName = propName;
        }

		public void Draw()
		{
			StringBuilder builder = new StringBuilder();

			UnityEngine.Object objectValue = objectValueProvider() as UnityEngine.Object;
				
            builder.AppendFormat("{0} {1}->{2}", GetLabelName(memberName), objectValue != null ? objectValue.name : "", objectValue != null ? objectValue.GetType().Name : "");

			EditorGUI.BeginChangeCheck();
			objectValue = EditorGUILayout.ObjectField(new GUIContent(builder.ToString()), objectValue, serializedObjectType, true) as UnityEngine.Object;
			if (EditorGUI.EndChangeCheck())
				if (OnEditorValueChanged != null)
					OnEditorValueChanged(this, new GladBehaviourValueChangedArgs(objectValue));

		}

		/// <summary>
		/// Produces a spaced out string for names on the editor.
		/// </summary>
		/// <param name="s">String to modify.</param>
		/// <returns>Spaced out capitalized string.</returns>
		private string GetLabelName(string s)
		{
			//http://stackoverflow.com/questions/5796383/insert-spaces-between-words-on-a-camel-cased-token
			if (s == null)
				return "";
			else
				return Regex.Replace(s, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
        }
	}
}
