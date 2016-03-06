using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	public interface IEditorDrawable
	{
		/// <summary>
		/// Draws the View.
		/// </summary>
		void Draw();

		/// <summary>
		/// Event to subscribe to to be notified when 
		/// the value the view is showing has changed.
		/// </summary>
		event OnValueChanged OnEditorValueChanged;
	}
}
