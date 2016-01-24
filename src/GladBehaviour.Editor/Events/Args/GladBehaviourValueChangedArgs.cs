using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// Arguments for a value changing event.
	/// </summary>
	public class GladBehaviourValueChangedArgs : EventArgs
	{
		/// <summary>
		/// Thew new changed value.
		/// </summary>
		public readonly object ChangedValue;

		public GladBehaviourValueChangedArgs(object newValue)
			: base()
		{
			ChangedValue = newValue;
		}
	}
}
