using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Common
{
	/// <summary>
	/// Implementer is a serializable container and offers data about the containters state.
	/// </summary>
	public interface ISerializableContainer
	{
		bool canLoadType { get; }

		Type SerializedType { get; }

		string SerializedName { get; }
	}
}
