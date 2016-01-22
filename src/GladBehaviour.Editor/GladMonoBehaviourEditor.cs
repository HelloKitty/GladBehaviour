using GladBehaviour.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace GladBehaviour.Editor
{
	[CustomEditor(typeof(GladMonoBehaviour))]
    public class GladMonoBehaviourEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			//Draws the default for all fields we don't handle
			DrawDefaultInspector();
		}
	}
}
