using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Common
{
	public abstract class GeneralContainerFieldMatcher : IContainerFieldMatcher
	{
		protected Type parseTarget { get; private set; }

		public GeneralContainerFieldMatcher(Type typeToParse)
		{
			if (typeToParse == null)
				throw new ArgumentNullException(nameof(typeToParse), "Cannot parse a null type.");

			parseTarget = typeToParse;
		}

		public abstract FieldInfo FindMatch(ISerializableContainer container);

		public abstract IEnumerable<FieldInfo> FindUnContainedFields<TSerializableContainerType>(IEnumerable<TSerializableContainerType> containers) 
			where TSerializableContainerType : ISerializableContainer;

		public abstract bool hasMatch(ISerializableContainer container);
	}
}
