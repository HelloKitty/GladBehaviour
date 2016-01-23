using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Editor
{
	public interface IDataStoreModel : ISerializedObjectReferenceProvider
	{
		Type DataType { get; }

		string SerializedName { get; }

		void Update(object newValue);
	}
}
