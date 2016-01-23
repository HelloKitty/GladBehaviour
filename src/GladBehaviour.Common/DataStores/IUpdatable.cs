using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Common
{
	public interface IDataUpdatable<TType> : IDataUpdatable
	{
		void Update(TType newValue);
	}

	public interface IDataUpdatable
	{
		void Update(object newValue);
	}
}
