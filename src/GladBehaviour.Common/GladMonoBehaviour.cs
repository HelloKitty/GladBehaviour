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
			ContainerParser<CollectionComponentDataStore> collectionContainerParser = 
				new ContainerParser<CollectionComponentDataStore>(collectionDataStoreCollection, new CollectionContainerFieldMatcher(this.GetType()));

			ContainerParser<SingleComponentDataStore> singleContainerParser =
				new ContainerParser<SingleComponentDataStore>(singleDataStoreCollection, new SingleContainerFieldMatcher(this.GetType()));

			//Find the removed interfaces and remove them from the collection
			RemoveSerializedContainers(singleContainerParser.ComputeStaleContainers(), singleDataStoreCollection);
			//Find the new members and create serialized containers for them
			singleDataStoreCollection.AddRange(singleContainerParser.FindNewCollectionMembers().Select(x => new SingleComponentDataStore(x.Type(), x.Name)));

			//At this point all non-collection interfaces have containers in the serializable collection
			//We must now do the same for Collection types
			RemoveSerializedContainers(collectionContainerParser.ComputeStaleContainers(), collectionDataStoreCollection);
			collectionDataStoreCollection.AddRange(collectionContainerParser.FindNewCollectionMembers().Select(x => new CollectionComponentDataStore(SerializedTypeManipulator.CollectionInferaceType(x.Type()), x.Type(), x.Name)));
		}

		private void RemoveSerializedContainers<TSerializedContainerType>(IEnumerable<TSerializedContainerType> toRemove, ICollection<TSerializedContainerType> collection)
			where TSerializedContainerType : ISerializableContainer
		{
			foreach (TSerializedContainerType o in toRemove)
			{
				//Debug.Log("Removing: " + o?.ToString() + " with storetype: " + typeof(TSerializedContainerType).ToString());
				collection.Remove(o);
			}
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
