using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	public interface ISerializedObjectReferenceProvider
	{
		/// <summary>
		/// Serialized container object.
		/// </summary>
		object SerializedObject { get; }
	}
}
