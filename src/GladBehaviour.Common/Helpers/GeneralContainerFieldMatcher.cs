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

		protected Lazy<Dictionary<string, FieldInfo>> sortedFieldInfo { get; private set; }

		public GeneralContainerFieldMatcher(Type typeToParse)
		{
			if (typeToParse == null)
				throw new ArgumentNullException(nameof(typeToParse), "Cannot parse a null type.");

			parseTarget = typeToParse;

			sortedFieldInfo = new Lazy<Dictionary<string, FieldInfo>>(CreateDictionary, true);
		}

		public abstract FieldInfo FindMatch(ISerializableContainer container);

		public abstract IEnumerable<FieldInfo> FindUnContainedFields<TSerializableContainerType>(IEnumerable<TSerializableContainerType> containers) 
			where TSerializableContainerType : ISerializableContainer;

		public abstract bool hasMatch(ISerializableContainer container);

		private Dictionary<string, FieldInfo> CreateDictionary()
		{
			Dictionary<string, FieldInfo> dict = new Dictionary<string, FieldInfo>();

			//prepare the dictionary
			//It'll help speed things up with O(1) lookup
			foreach (FieldInfo fi in SerializedTypeManipulator.GetCollectionFields(parseTarget))
			{
				dict.Add(fi.Name, fi);
			}

			return dict;
		}
	}
}
