using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Editor
{
	public interface IDataStoreModel : ISerializedObjectReferenceProvider
	{
		/// <summary>
		/// Serialized data <see cref="Type"/> of the internal container.
		/// </summary>
		Type DataType { get; }

		/// <summary>
		/// Serialized name of the member this container is persisting for.
		/// </summary>
		string SerializedName { get; }

		/// <summary>
		/// Updates a value.
		/// </summary>
		/// <param name="newValue">Value to update to.</param>
		void Update(object newValue);
	}
}
