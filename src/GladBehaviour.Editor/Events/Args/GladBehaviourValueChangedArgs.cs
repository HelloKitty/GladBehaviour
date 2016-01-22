using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Editor
{
	public class GladBehaviourValueChangedArgs : EventArgs
	{
		public readonly object ChangedValue;

		public GladBehaviourValueChangedArgs(object newValue)
			: base()
		{
			ChangedValue = newValue;
		}
	}
}
