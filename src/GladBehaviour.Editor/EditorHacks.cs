using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;

namespace GladBehaviour.Editor
{
	public static class EditorHacks
	{
		/*internal delegate UnityEngine.Object ObjectFieldValidator(UnityEngine.Object[] references, Type objType, SerializedProperty property);

		/// <summary>
		///   <para>Make an object field. You can assign objects either by drag and drop objects or by selecting an object using the Object Picker.</para>
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="label">Optional label in front of the field.</param>
		/// <param name="obj">The object the field shows.</param>
		/// <param name="objType">The type of the objects that can be assigned.</param>
		/// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
		/// <returns>
		///   <para>The object that has been set by the user.</para>
		/// </returns>
		public static UnityEngine.Object ObjectField(Rect position, UnityEngine.Object.Object obj, Type objType, bool allowSceneObjects)
		{
			int controlID = GUIUtility.GetControlID(EditorGUI.s_ObjectFieldHash, EditorGUIUtility.native, position);
			return EditorHacks.DoObjectField(EditorGUI.IndentedRect(position), EditorGUI.IndentedRect(position), controlID, obj, objType, null, null, allowSceneObjects);
		}

		internal static UnityEngine.Object DoObjectField(Rect position, Rect dropRect, int id, UnityEngine.Object obj, Type objType, SerializedProperty property, EditorHacks.ObjectFieldValidator validator, bool allowSceneObjects)
		{
			return EditorHacks.DoObjectField(position, dropRect, id, obj, objType, property, validator, allowSceneObjects, EditorStyles.objectField);
		}

		internal static UnityEngine.Object ValidateObjectFieldAssignment(UnityEngine.Object[] references, Type objType, SerializedProperty property)
		{
			if ((int)references.Length > 0)
			{
				bool length = (int)DragAndDrop.objectReferences.Length > 0;
				bool flag = (references[0] == null ? false : references[0].GetType() == typeof(Texture2D));
				if (objType == typeof(Sprite) && flag && length)
				{
					return SpriteUtility.TextureToSprite(references[0] as Texture2D);
				}
				if (property == null)
				{
					if (references[0] != null && references[0].GetType() == typeof(GameObject) && typeof(Component).IsAssignableFrom(objType))
					{
						references = ((GameObject)references[0]).GetComponents(typeof(Component));
					}
					UnityEngine.Object[] objArray = references;
					for (int i = 0; i < (int)objArray.Length; i++)
					{
						UnityEngine.Object obj = objArray[i];
						if (obj != null && objType.IsAssignableFrom(obj.GetType()))
						{
							return obj;
						}
					}
				}
				else
				{
					if (references[0] != null && property.ValidateObjectReferenceValue(references[0]))
					{
						return references[0];
					}
					if ((property.type == "PPtr<Sprite>" || property.type == "PPtr<$Sprite>" || property.type == "vector") && flag && length)
					{
						return SpriteUtility.TextureToSprite(references[0] as Texture2D);
					}
				}
			}
			return null;
		}

		internal static UnityEngine.Object DoObjectField(Rect position, Rect dropRect, int id, UnityEngine.Object obj, Type objType, SerializedProperty property, EditorHacks.ObjectFieldValidator validator, bool allowSceneObjects, GUIStyle style)
		{
			Rect rect;
			GUIContent sMixedValueContent;
			if (validator == null)
			{
				validator = new EditorHacks.ObjectFieldValidator(EditorHacks.ValidateObjectFieldAssignment);
			}
			Event curEvent = Event.current;
			EventType eventType = curEvent.type;
			if (!GUI.enabled && GUIClip.enabled && Event.current.rawType == EventType.MouseDown)
			{
				eventType = Event.current.rawType;
			}
			bool flag = EditorGUIUtility.HasObjectThumbnail(objType);
			EditorGUI.ObjectFieldVisualType objectFieldVisualType = EditorGUI.ObjectFieldVisualType.IconAndText;
			if (flag && position.height <= 18f && position.width <= 32f)
			{
				objectFieldVisualType = EditorGUI.ObjectFieldVisualType.MiniPreivew;
			}
			else if (flag && position.height > 16f)
			{
				objectFieldVisualType = EditorGUI.ObjectFieldVisualType.LargePreview;
			}
			Vector2 iconSize = EditorGUIUtility.GetIconSize();
			if (objectFieldVisualType == EditorGUI.ObjectFieldVisualType.IconAndText)
			{
				EditorGUIUtility.SetIconSize(new Vector2(12f, 12f));
			}
			else if (objectFieldVisualType == EditorGUI.ObjectFieldVisualType.LargePreview)
			{
				EditorGUIUtility.SetIconSize(new Vector2(64f, 64f));
			}
			EventType eventType1 = eventType;
			switch (eventType1)
			{
				case EventType.KeyDown:
					{
						if (GUIUtility.keyboardControl == id)
						{
							if (curEvent.keyCode == KeyCode.Backspace || curEvent.keyCode == KeyCode.Delete)
							{
								if (property == null)
								{
									obj = null;
								}
								else
								{
									property.objectReferenceValue = null;
								}
								GUI.changed = true;
								curEvent.Use();
							}
							if (curEvent.MainActionKeyForControl(id))
							{
								ObjectSelector.@get.Show(obj, objType, property, allowSceneObjects);
								ObjectSelector.@get.objectSelectorID = id;
								curEvent.Use();
								GUIUtility.ExitGUI();
							}
						}
						break;
					}
				case EventType.Repaint:
					{
						if (EditorGUI.showMixedValue)
						{
							sMixedValueContent = EditorGUI.s_MixedValueContent;
						}
						else if (property == null)
						{
							sMixedValueContent = EditorGUIUtility.ObjectContent(obj, objType);
						}
						else
						{
							sMixedValueContent = EditorGUIUtility.TempContent(property.objectReferenceStringValue, AssetPreview.GetMiniThumbnail(property.objectReferenceValue));
							obj = property.objectReferenceValue;
							if (obj != null)
							{
								if (validator(new Object[] { obj }, objType, property) == null)
								{
									sMixedValueContent = EditorGUIUtility.TempContent("Type mismatch");
								}
							}
						}
						switch (objectFieldVisualType)
						{
							case EditorGUI.ObjectFieldVisualType.IconAndText:
								{
									EditorGUI.BeginHandleMixedValueContentColor();
									style.Draw(position, sMixedValueContent, id, DragAndDrop.activeControlID == id);
									EditorGUI.EndHandleMixedValueContentColor();
									break;
								}
							case EditorGUI.ObjectFieldVisualType.LargePreview:
								{
									EditorGUI.DrawObjectFieldLargeThumb(position, id, obj, sMixedValueContent);
									break;
								}
							case EditorGUI.ObjectFieldVisualType.MiniPreivew:
								{
									EditorGUI.DrawObjectFieldMiniThumb(position, id, obj, sMixedValueContent);
									break;
								}
							default:
								{
									throw new ArgumentOutOfRangeException();
								}
						}
						break;
					}
				case EventType.DragUpdated:
				case EventType.DragPerform:
					{
						if (dropRect.Contains(Event.current.mousePosition) && GUI.enabled)
						{
							Object obj1 = validator(DragAndDrop.objectReferences, objType, property);
							if (obj1 != null && !allowSceneObjects && !EditorUtility.IsPersistent(obj1))
							{
								obj1 = null;
							}
							if (obj1 != null)
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
								if (eventType != EventType.DragPerform)
								{
									DragAndDrop.activeControlID = id;
								}
								else
								{
									if (property == null)
									{
										obj = obj1;
									}
									else
									{
										property.objectReferenceValue = obj1;
									}
									GUI.changed = true;
									DragAndDrop.AcceptDrag();
									DragAndDrop.activeControlID = 0;
								}
								Event.current.Use();
							}
						}
						break;
					}
				case EventType.ExecuteCommand:
					{
						if (curEvent.commandName == "ObjectSelectorUpdated" && ObjectSelector.@get.objectSelectorID == id && GUIUtility.keyboardControl == id)
						{
							Object obj2 = validator(new Object[] { ObjectSelector.GetCurrentObject() }, objType, property);
							if (property != null)
							{
								property.objectReferenceValue = obj2;
							}
							GUI.changed = true;
							curEvent.Use();
							return obj2;
						}
						break;
					}
				case EventType.DragExited:
					{
						if (GUI.enabled)
						{
							HandleUtility.Repaint();
						}
						break;
					}
				default:
					{
						if (eventType1 != EventType.MouseDown)
						{
							break;
						}
						else if (Event.current.button == 0)
						{
							if (position.Contains(Event.current.mousePosition))
							{
								switch (objectFieldVisualType)
								{
									case EditorGUI.ObjectFieldVisualType.IconAndText:
										{
											rect = new Rect(position.xMax - 15f, position.y, 15f, position.height);
											break;
										}
									case EditorGUI.ObjectFieldVisualType.LargePreview:
										{
											rect = new Rect(position.xMax - 36f, position.yMax - 14f, 36f, 14f);
											break;
										}
									case EditorGUI.ObjectFieldVisualType.MiniPreivew:
										{
											rect = new Rect(position.xMax - 15f, position.y, 15f, position.height);
											break;
										}
									default:
										{
											throw new ArgumentOutOfRangeException();
										}
								}
								EditorGUIUtility.editingTextField = false;
								if (!rect.Contains(Event.current.mousePosition))
								{
									UnityEngine.Object obj3 = (property == null ? obj : property.objectReferenceValue);
									Component component = obj3 as Component;
									if (component)
									{
										obj3 = component.gameObject;
									}
									if (EditorGUI.showMixedValue)
									{
										obj3 = null;
									}
									if (Event.current.clickCount == 1)
									{
										GUIUtility.keyboardControl = id;
										if (obj3)
										{
											bool flag1 = (curEvent.shift ? true : curEvent.control);
											if (!flag1)
											{
												EditorGUIUtility.PingObject(obj3);
											}
											if (flag1 && obj3 is Texture)
											{
												PopupWindowWithoutFocus.Show((new RectOffset(6, 3, 0, 3)).Add(position), new ObjectPreviewPopup(obj3), new PopupLocationHelper.PopupLocation[] { PopupLocationHelper.PopupLocation.Left, PopupLocationHelper.PopupLocation.Below, PopupLocationHelper.PopupLocation.Right });
											}
										}
										curEvent.Use();
									}
									else if (Event.current.clickCount == 2)
									{
										if (obj3)
										{
											AssetDatabase.OpenAsset(obj3);
											GUIUtility.ExitGUI();
										}
										curEvent.Use();
									}
								}
								else if (GUI.enabled)
								{
									GUIUtility.keyboardControl = id;
									ObjectSelector.@get.Show(obj, objType, property, allowSceneObjects);
									ObjectSelector.@get.objectSelectorID = id;
									curEvent.Use();
									GUIUtility.ExitGUI();
								}
							}
							break;
						}
						else
						{
							break;
						}
					}
			}
			EditorGUIUtility.SetIconSize(iconSize);
			return obj;
			throw new ArgumentOutOfRangeException();
		}*/
	}
}
