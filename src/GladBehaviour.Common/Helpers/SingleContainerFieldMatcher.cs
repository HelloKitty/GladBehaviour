using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;
using UnityEngine;

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
			IEnumerable<FieldInfo> fields = GetSerializedInterfaceFields(parseTarget.Fields(Flags.InstanceAnyVisibility));

			//the above where finds private fields marked SerializeField or public fields that aren't marked HideInInspector
			//This can help ignore/weedout fields that are private for DI/IoC frameworks or for other purposes.

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

			return GetSerializedInterfaceFields(sortedFieldInfo.Value.Values)
				.Where(x => !tempDictionary.ContainsKey(x.Name)); //where we don't already have a mapped name
		}

		private IEnumerable<FieldInfo> GetSerializedInterfaceFields(IEnumerable<FieldInfo> fields)
		{
			IEnumerable<FieldInfo> parsedFields = fields.Where(fi => fi.Type().IsInterface) //find the interface fields
				.Where(x => !SerializedTypeManipulator.isInterfaceCollectionType(x.Type())) //where it's NOT an interface collection type
				.Where(fi => !SerializedTypeManipulator.isInterfaceCollectionType(fi.Type())) //exclude the collection ones
				.Where(fi => ((fi.IsPrivate || fi.IsFamily) && fi.GetCustomAttribute<SerializeField>(true) != null) || fi.IsPublic && fi.GetCustomAttribute<HideInInspector>(true) == null)
				.Where(fi => !fi.Type().IsValueType); //ignore value types.

			IEnumerable<FieldInfo> backingFieldsToProps = parseTarget.Properties(Flags.InstanceAnyVisibility)
				.Where(pi => pi.Type().IsInterface) //find the interface properties
				.Where(pi => !SerializedTypeManipulator.isInterfaceCollectionType(pi.Type())) //exclude the collection ones
				.Where(pi => pi.GetCustomAttribute<SerializeField>(true) != null)
				.Where(pi => !pi.Type().IsValueType) //ignore value types.
				.Select(pi => parseTarget.Field($"<{pi.Name}>k__BackingField")); //add the backing fields for the properties

			return parsedFields.Concat(backingFieldsToProps);
		}

		public override bool hasMatch(ISerializableContainer container)
		{
			if (sortedFieldInfo.Value.ContainsKey(container.SerializedName))
				if (!SerializedTypeManipulator.isInterfaceCollectionType(sortedFieldInfo.Value[container.SerializedName].Type())) //make sure it's NOT something like IEnumerable<ISomething>
					if (container.SerializedTypeName.Contains(sortedFieldInfo.Value[container.SerializedName].Type().Name)) //makes sure the Type is still approximately the same
					{
						return true;
					}

			return false;
		}
	}
}
