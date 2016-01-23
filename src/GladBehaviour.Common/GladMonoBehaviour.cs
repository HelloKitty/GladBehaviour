using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Fasterflect;
using System.Reflection;
using System.Collections;

namespace GladBehaviour.Common
{
	public class GladMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
	{
		[HideInInspector]
		[SerializeField]
		[SingleCollectionSerialization]
		private List<SingleComponentDataStore> singleDataStoreCollection;

		[HideInInspector]
		[SerializeField]
		[ListCollectionSerialization]
		private List<CollectionComponentDataStore> collectionDataStoreCollection;

		public void OnAfterDeserialize()
		{
			//This is:
			//Called once after a recompile
			//Called once after the scene is loaded
			//Called once in an actual build(in my simple test)
			//according to: http://answers.unity3d.com/questions/782872/onbeforeserialize-is-getting-called-rapidly.html#answer-796853

			//Therefore this is the perfect time to do the following:
			//Init null collections
			//Check if class structure has changed
			//Initialization member values

			//Check if the collections are empty

			CheckAndInitEmptyDataStores();

			//TODO: We should do a check if this is a compiled build. If it is we don't want the inefficiency of doing all the stuff below. We should just set values
			InitializeSerializedCollectionContainers();

			//Write the data to the fields
			//All fields should be valid so we don't have to worry about trying to write to non-existant fields
			foreach(SingleComponentDataStore scd in singleDataStoreCollection)
			{
				if (scd.StoredComponent == null)
					continue;

				//Debug.Log("Dealing with: " + scd.SerializedName);

				FieldInfo fi = this.GetType().GetField(scd.SerializedName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				//Grabs the field info and writes the component being stored to it

				//Debug.Log(scd.StoredComponent.GetType().ToString());

				fi.SetValue(this, scd.StoredComponent);
			}

			//Debug.Log("Count of Collections: " + collectionDataStoreCollection.Count);

			foreach(CollectionComponentDataStore ccd in collectionDataStoreCollection)
			{
				//Debug.Log("Dealing with: " + ccd.SerializedName);

				if (ccd.dataStoreCollection == null)
					continue;

				FieldInfo fi = this.GetType().GetField(ccd.SerializedName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				//Grabs the field info and writes the component being stored to it

				fi.SetValue(this, ccd.ToCollectionType());
			}
        }

		private void InitializeSerializedCollectionContainers()
		{
			//Find the removed interfaces and remove them from the collection
			RemoveSerializedContainers(FindRemovedInterfaceMembers(), singleDataStoreCollection);
			//Find the new members and create serialized containers for them
			singleDataStoreCollection.AddRange(FindNewInterfaceMembers().Select(x => new SingleComponentDataStore(x.Type(), x.Name)));

			//At this point all non-collection interfaces have containers in the serializable collection
			//We must now do the same for Collection types
			RemoveSerializedContainers(FindRemovedCollectionsMembers(), collectionDataStoreCollection);
			collectionDataStoreCollection.AddRange(FindNewCollectionMembers().Select(x => new CollectionComponentDataStore(x.Type().GetGenericArguments().First(), x.Type(), x.Name)));
		}

		private IEnumerable<FieldInfo> FindNewCollectionMembers()
		{
			List<FieldInfo> newFields = new List<FieldInfo>();

			//Cache this outside so it's not done N times.
			IEnumerable<FieldInfo> fields = GetCollectionFields();

			foreach (FieldInfo fi in fields)
			{
				bool foundMatch = false;

				foreach (CollectionComponentDataStore ccd in collectionDataStoreCollection)
				{
					if (ccd.SerializedName == fi.Name)
						if(isInterfaceCollectionType(fi.Type()))
                            if (ccd.SerializedType == fi.Type().GetGenericArguments().First())
							{
								foundMatch = true;
								break;
							}
				}

				//If a match wasn't found it's a new field and we should add it
				if (!foundMatch)
					newFields.Add(fi);
			}

			return newFields;
		}

		//TODO: Find a way to merge code with interface one
		private IEnumerable<CollectionComponentDataStore> FindRemovedCollectionsMembers()
		{
			List<CollectionComponentDataStore> staleFields = new List<CollectionComponentDataStore>();

			//if it's empty we don't need to do searches
			if (collectionDataStoreCollection.Count == 0)
				return staleFields;

			//Cache this outside so it's not done N times.
			IEnumerable<FieldInfo> fields = GetCollectionFields();

			//WARNING: This is O(n^2) so may need improvements
			foreach (CollectionComponentDataStore ccd in collectionDataStoreCollection)
			{
				//If the Type is no longer in an assembly/changed then we remove it without checking
				if (!ccd.canLoadType)
				{
					//Debug.Log("Found stale type.");

					staleFields.Add(ccd);
					continue;
				}

				bool foundTheField = false;

				//If we get here we need to look at 
				foreach (FieldInfo fi in fields)
				{
					if (ccd.SerializedName == fi.Name)
					{
						//Check if it's a collection type we handle
						if(isInterfaceCollectionType(fi.Type()))
						{
							//We could handle less derived type found but this complicates the logic
							if (ccd.SerializedType == fi.Type().GetGenericArguments().First())
								foundTheField = true;
						}

						//We break after we find a match
						break;
					}
				}

				//We didn't find its match
				if (!foundTheField)
				{
					staleFields.Add(ccd);
				}
					
			}

			return staleFields;
		}

		private bool isInterfaceCollectionType(Type type)
		{
			return type != null && type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type) && !type.GetGenericArguments().First().IsPrimitive;
		}

		private void RemoveSerializedContainers<TSerializedContainerType>(IEnumerable<TSerializedContainerType> toRemove, ICollection<TSerializedContainerType> collection)
			where TSerializedContainerType : ISerializableContainer
		{
			foreach(TSerializedContainerType o in toRemove)
			{
				//Debug.Log("Removing: " + o?.ToString() + " with storetype: " + typeof(TSerializedContainerType).ToString());
				collection.Remove(o);
			}
		}

		private IEnumerable<SingleComponentDataStore> FindRemovedInterfaceMembers()
		{
			List<SingleComponentDataStore> staleFields = new List<SingleComponentDataStore>();

			//if it's empty we don't need to do searches
			if (singleDataStoreCollection.Count == 0)
				return staleFields;

			//Cache this outside so it's not done N times.
			IEnumerable<FieldInfo> fields = GetInterfaceFields();

			//WARNING: This is O(n^2) so may need improvements
			foreach(SingleComponentDataStore scd in singleDataStoreCollection)
			{
				//If the Type is no longer in an assembly/changed then we remove it without checking
				if (!scd.canLoadType)
				{
					staleFields.Add(scd);
					continue;
				}

				bool foundTheField = false;

				//If we get here we need to look at 
				foreach (FieldInfo fi in fields)
				{
					if (scd.SerializedName == fi.Name)
					{
						//We could handle less derived type found but this complicates the logic
						if (scd.SerializedType == fi.Type())
							foundTheField = true;

						//We break after we find a match
						break;
					}
				}
				
				//We didn't find its match
				if (!foundTheField)
					staleFields.Add(scd);
			}

			return staleFields;
		}

		private IEnumerable<FieldInfo> GetCollectionFields()
		{
			return GetType().FieldsWith(Flags.InstanceAnyVisibility, typeof(SerializeField))
				.Where(x => !x.HasAttribute<HideInInspector>()) //don't want hidden members
				.Where(x => typeof(IEnumerable).IsAssignableFrom(x.Type())) //we don't want collection types
				.Where(x => x.Type().IsGenericType) //we want generic collections where T is interface
				.Where(x => x.Type().GetGenericArguments().First().IsInterface)
				.Where(x => !x.Type().GetGenericArguments().First().IsPrimitive);
		}

		private IEnumerable<FieldInfo> FindNewInterfaceMembers()
		{
			List<FieldInfo> newFields = new List<FieldInfo>();

			//Cache this outside so it's not done N times.
			IEnumerable<FieldInfo> fields = GetInterfaceFields();

			foreach(FieldInfo fi in fields)
			{
				bool foundMatch = false;

				foreach(SingleComponentDataStore scd in singleDataStoreCollection)
				{
					if (scd.SerializedName == fi.Name)
						if (scd.SerializedType == fi.Type())
						{
							foundMatch = true;
							break;
						}
				}

				//If a match wasn't found it's a new field and we should add it
				if (!foundMatch)
					newFields.Add(fi);
            }

			return newFields;
		}

		private IEnumerable<FieldInfo> GetInterfaceFields()
		{
			return GetType().FieldsWith(Flags.InstanceAnyVisibility, typeof(SerializeField))
				.Where(x => x.Type().IsInterface) //we want interface types only
				.Where(x => !typeof(IEnumerable).IsAssignableFrom(x.Type())) //we don't want collection types
				.Where(x => !x.HasAttribute<HideInInspector>()); //don't want hidden members
		}

		public void OnBeforeSerialize()
		{
			//Must call this here too so that we have empty data for editor
			//And if it is true, that they were null, then that means we need to fully initialize them too
			if(CheckAndInitEmptyDataStores())
			{
				InitializeSerializedCollectionContainers();
			}
		}

		private bool CheckAndInitEmptyDataStores()
		{
			bool result = false;

			if (singleDataStoreCollection == null)
			{
				result = true;
				singleDataStoreCollection = new List<SingleComponentDataStore>();
			}

			if (collectionDataStoreCollection == null)
			{
				result = true;
				collectionDataStoreCollection = new List<CollectionComponentDataStore>();
			}

			return result;
		}
	}
}
