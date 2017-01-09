using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// View for collection type data models.
	/// </summary>
	public class CollectionView : IEditorDrawable, IView
	{
		/// <summary>
		/// Event to subscribe to to be notified when 
		/// the value of a collection has changed.
		/// </summary>
		public event OnValueChanged OnEditorValueChanged;

		/// <summary>
		/// Collection for the view to display.
		/// </summary>
		private readonly IList<UnityEngine.Object> listData;

		/// <summary>
		/// Display name for the collection.
		/// </summary>
		private readonly string headerDisplayName;

		/// <summary>
		/// Type of the inner object collection.
		/// </summary>
		private readonly Type serializedObjectType;

		/// <summary>
		/// View/GUI collection that Unity renders.
		/// </summary>
		private readonly ReorderableList list;

		public CollectionView(/*yep Unity uses IList*/IList<UnityEngine.Object> currentCollection, string propName, Type objectType)
		{
			listData = currentCollection;
			headerDisplayName = propName;
			serializedObjectType = objectType;

			list = new ReorderableList((IList)listData, typeof(UnityEngine.Object), true, true, true, true); //should be a list of MonoBehaviour

			//This is copy pasted from ExtendedEvent
			//Required to setup the list
			list.draggable = false;
			list.elementHeight *= 2;
			list.drawHeaderCallback = DrawHeaderInternal;
			list.drawElementCallback = DrawElementInternal;
			list.onAddCallback = AddInternal;
			list.onRemoveCallback = RemoveInternal;
		}

		/// <summary>
		/// Draws the View.
		/// </summary>
		public void Draw()
		{
			EditorGUI.BeginChangeCheck();

			list.DoLayoutList();

			if(EditorGUI.EndChangeCheck()) //check if the editor changed since drawing the list
			{
				OnEditorValueChanged?.Invoke(this, new GladBehaviourValueChangedArgs(list.list));

				//WARNING: This call is needed to mark a scene dirty. There are cases where a scene
				//won't save the data if not marked dirty.
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}
		}

		/// <summary>
		/// <see cref="ReorderableList"/> delegate function for drawing header.
		/// </summary>
		/// <param name="rect">Rect of the header.</param>
		private void DrawHeaderInternal(Rect rect)
		{
			EditorGUI.LabelField(rect, GetLabelName(headerDisplayName));
		}

		/// <summary>
		/// <see cref="ReorderableList"/> delegate function for drawing the contained elements.
		/// </summary>
		/// <param name="rect">Rect for the element.</param>
		/// <param name="index">Index of the element</param>
		/// <param name="isActive">Indicates if the list is active.</param>
		/// <param name="isFocused">Indicates if the list is focused.</param>
		private void DrawElementInternal(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.yMin += 3f;
			rect.yMax -= 7f;

			var thirdWidth = rect.width / 3;
			var halfHeight = rect.height / 2;

			//swap GO Field Rect for the dropdown in ExtendedEvent source
			//var gameObjectRect = new Rect(rect.x + thirdWidth, rect.y, thirdWidth * 2, halfHeight);
			//var gameObjectRect = new Rect(rect.x + thirdWidth, rect.y, rect.width * 4, halfHeight);
			//var dropdownRect = new Rect(rect.x, rect.y, thirdWidth, halfHeight);
			var gameObjectRect = new Rect(rect.x, rect.y, rect.width, halfHeight);

			/*var gameObjectRect = new Rect(rect.x, rect.y, thirdWidth, halfHeight);
			var dropdownRect = new Rect(rect.x + thirdWidth, rect.y, thirdWidth * 2, halfHeight);
			var bottomRect = new Rect(rect.x, rect.y + halfHeight, rect.width, halfHeight);*/

			EditorGUI.BeginChangeCheck();
			//GUI.Box(dropdownRect, "");

			StringBuilder builder = new StringBuilder();

			builder.AppendFormat("{0}. {1}->{2}", index, listData[index]?.name, listData[index]?.GetType()?.Name);

			listData[index] = (UnityEngine.Object)EditorGUI.ObjectField(gameObjectRect, new GUIContent(builder.ToString()), listData[index], serializedObjectType, true);

			if (EditorGUI.EndChangeCheck())
			{
				//Maybe, do the value changed thing here.
				//listener.Reset();
			}
		}

		//I actually don't know. This is in the ExtendedEvents source code
		private void AddInternal(ReorderableList list)
		{
			listData.Add(null); //this is trequired to start off the list
			//eEvent.Listeners.Add(new ExtendedEvent.GameObjectContainer());
		}

		/// <summary>
		/// <see cref="ReorderableList"/> delegate function for removing elements.
		/// </summary>
		/// <param name="list"><see cref="ReorderableList"/> instance.</param>
		private void RemoveInternal(ReorderableList list)
		{
			listData.RemoveAt(list.index);
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
