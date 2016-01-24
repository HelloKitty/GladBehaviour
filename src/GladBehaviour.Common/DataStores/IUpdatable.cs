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
		void Update(TType newValue);
	}

	/// <summary>
	/// Implementer allows for the updating of an internal data of type object.
	/// </summary>
	public interface IDataUpdatable
	{
		void Update(object newValue);
	}
}
