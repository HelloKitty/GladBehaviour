using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Common
{
	/// <summary>
	/// Meta-data marker for a serializable list of single component containers.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class SingleCollectionSerializationAttribute : Attribute
	{
		//we don't need any properties
	}
}
