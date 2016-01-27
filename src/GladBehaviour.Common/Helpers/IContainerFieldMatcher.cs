using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Common
{
	public interface IContainerFieldMatcher
	{
		bool hasMatch(ISerializableContainer container);

		FieldInfo FindMatch(ISerializableContainer container);

		IEnumerable<FieldInfo> FindUnContainedFields(IEnumerable<ISerializableContainer> containers);
	}
}
