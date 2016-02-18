using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace GladBehaviour.Common
{
	public class SingleContainerFieldMatcher : GeneralContainerFieldMatcher, IContainerFieldMatcher
	{
		private Lazy<Dictionary<string, FieldInfo>> sortedFieldInfo;

		private Dictionary<string, FieldInfo> CreateDictionary()
		{
			Dictionary<string, FieldInfo> dict = new Dictionary<string, FieldInfo>();

			//prepare the dictionary
			//It'll help speed things up with O(1) lookup
			IEnumerable<FieldInfo> fields = parseTarget.Fields(Flags.InstanceAnyVisibility)
				.Where(fi => fi.Type().IsInterface) //find the interface fields
				.Where(fi => !SerializedTypeManipulator.isInterfaceCollectionType(fi.Type())); //exclude the collection ones

			foreach (FieldInfo fi in fields)
			{
				dict.Add(fi.Name, fi);
			}

			return dict;
		}

		public SingleContainerFieldMatcher(Type typeToParse)
			: base(typeToParse)
		{
			if (typeToParse == null)
				throw new ArgumentNullException(nameof(typeToParse), "Cannot parse a null type.");

			sortedFieldInfo = new Lazy<Dictionary<string, FieldInfo>>(CreateDictionary, true);
		}

		public override FieldInfo FindMatch(ISerializableContainer container)
		{
			//if we find an exact matching field then we return it as the match
			if(hasMatch(container))
			{
				return sortedFieldInfo.Value[container.SerializedName];
            }

			return null;
        }

		public override IEnumerable<FieldInfo> FindUnContainedFields<TSerializableContainerType>(IEnumerable<TSerializableContainerType> containers)
		{
			//This may not actually be quicker but the idea is to create an O(1) lookup table that'll allow us
			//to find members that we don't know yet
			Dictionary<string, TSerializableContainerType> tempDictionary = new Dictionary<string, TSerializableContainerType>(containers.Count());

			foreach (TSerializableContainerType con in containers)
				tempDictionary.Add(con.SerializedName, con);

			return sortedFieldInfo.Value.Values
				.Where(x => !tempDictionary.ContainsKey(x.Name)) //where we don't already have a mapped name
				.Where(x => !SerializedTypeManipulator.isInterfaceCollectionType(x.Type())); //where it's NOT an interface collection type
		}

		public override bool hasMatch(ISerializableContainer container)
		{
			if (sortedFieldInfo.Value.ContainsKey(container.SerializedName))
				if (!SerializedTypeManipulator.isInterfaceCollectionType(sortedFieldInfo.Value[container.SerializedName].Type())) //make sure it's NOT
					if (sortedFieldInfo.Value[container.SerializedName].Type() == container.SerializedType) //if true then it's something like IEnumerable<ISomething>
					{
						return true;
					}

			return false;
        }
	}
}
