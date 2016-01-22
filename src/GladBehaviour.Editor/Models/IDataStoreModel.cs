using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Editor
{
	public interface IDataStoreModel
	{
		object SerializedObject { get; }

		void Update(object newValue);
	}
}
