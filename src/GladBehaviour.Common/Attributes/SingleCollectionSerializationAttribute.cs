using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class SingleCollectionSerializationAttribute : Attribute
	{

	}
}
