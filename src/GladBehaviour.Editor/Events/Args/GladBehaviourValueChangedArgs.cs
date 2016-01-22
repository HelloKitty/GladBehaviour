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

		//public readonly string MemberName;

		//public readonly MemberTypes MemberType;

		public GladBehaviourValueChangedArgs(object newValue, int registeredID)//, string name, MemberTypes type)
			: base()
		{
			ChangedValue = newValue;
			//MemberName = name;
			//MemberType = type;
		}
	}
}
