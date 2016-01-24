using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Common
{
	/// <summary>
	/// Implementer allows for the updating of an internal data of type <typeparamref name="TType"/>.
	/// </summary>
	/// <typeparam name="TType">Data type</typeparam>
	public interface IDataUpdatable<TType> : IDataUpdatable
	{
		/// <summary>
		/// Updates a value.
		/// </summary>
		/// <param name="newValue">Value to update to.</param>
		void Update(TType newValue);
	}

	/// <summary>
	/// Implementer allows for the updating of an internal data of type object.
	/// </summary>
	public interface IDataUpdatable
	{
		/// <summary>
		/// Updates a value.
		/// </summary>
		/// <param name="newValue">Value to update to.</param>
		void Update(object newValue);
	}
}
