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
		/// <summary>
		/// Indicates if the Type persisted can be loaded.
		/// </summary>
		bool canLoadType { get; }

		/// <summary>
		/// The serialized Type that is being contained.
		/// </summary>
		Type SerializedType { get; }

		/// <summary>
		/// Serializable member name this container is serializing for.
		/// </summary>
		string SerializedName { get; }
	}
}
