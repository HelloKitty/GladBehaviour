using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Common
{
	public interface ISerializableContainer
	{
		bool canLoadType { get; }

		Type SerializedType { get; }

		string SerializedName { get; }
	}
}
