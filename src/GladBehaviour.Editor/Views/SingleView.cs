using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// View for single component type data models.
	/// </summary>
	public class SingleView : IEditorDrawable, IView
	{
		/// <summary>
		/// Event to subscribe to to be notified when 
		/// the value of the component has changed.
		/// </summary>
		public event OnValueChanged OnEditorValueChanged;

		/// <summary>
		/// Component provider. Provides the object to be displayed.
		/// </summary>
		private Func<UnityEngine.Object> objectValueProvider;

		/// <summary>
		/// Type of the component serialized.
		/// </summary>
		private readonly Type serializedObjectType;

		/// <summary>
		/// Name of the serialized component.
		/// </summary>
		private readonly string memberName;

		/// <summary>
		/// Creates a view for a single component.
		/// </summary>
		/// <param name="refProvider">Provider for the component.</param>
		/// <param name="dataType">Type of the component.</param>
		/// <param name="propName">Name of the member.</param>
		public SingleView(Func<UnityEngine.Object> refProvider, Type dataType, string propName)
		{
			objectValueProvider = refProvider;
			serializedObjectType = dataType;
			memberName = propName;
		}

		/// <summary>
		/// Draws the View.
		/// </summary>
		public void Draw()
		{
			StringBuilder builder = new StringBuilder();

			UnityEngine.Object objectValue = objectValueProvider() as UnityEngine.Object;
				
			builder.AppendFormat("{0}: {1}->{2}", GetLabelName(memberName), objectValue != null ? objectValue.name : "", objectValue != null ? objectValue.GetType().Name : "");

			EditorGUI.BeginChangeCheck();
			objectValue = EditorGUILayout.ObjectField(new GUIContent(builder.ToString()), objectValue, serializedObjectType, true) as UnityEngine.Object;
			if (EditorGUI.EndChangeCheck())
			{
				OnEditorValueChanged?.Invoke(this, new GladBehaviourValueChangedArgs(objectValue));

				//WARNING: This call is needed to mark a scene dirty. There are cases where a scene
				//won't save the data if not marked dirty.
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}
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
