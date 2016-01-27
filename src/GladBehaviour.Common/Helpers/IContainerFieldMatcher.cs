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

		IEnumerable<FieldInfo> FindUnContainedFields<TSerializableContainerType>(IEnumerable<TSerializableContainerType> containers) //yes we could just use ISerializableContainerType but for then we'd need a cast if contravariance is not supported
			where TSerializableContainerType : ISerializableContainer;
	}
}
