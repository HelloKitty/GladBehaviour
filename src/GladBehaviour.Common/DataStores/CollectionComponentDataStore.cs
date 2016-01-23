using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Fasterflect;

namespace GladBehaviour.Common
{
	[Serializable]
	public class CollectionComponentDataStore : IEnumerable<UnityEngine.Object>, IDataUpdatable<List<UnityEngine.Object>>, ISerializableContainer
	{
		//Idk if the Unity editor will ever go threaded for serialization
		private readonly object syncObj = new object();

		[SerializeField]
		public List<UnityEngine.Object> dataStoreCollection;

		[SerializeField]
		private string serializedTypeName;

		//caching should make this quicker
		private Type cachedType = null;
		public Type SerializedType
		{
			get
			{
				lock (syncObj)
					return cachedType == null ? cachedType = Type.GetType(serializedTypeName, true, false) : cachedType;
			}
		}

		[SerializeField]
		private string serializedCollectionTypeName;

		//caching should make this quicker
		private Type cachedCollectionType = null;
		public Type SerializedCollectionType
		{
			get
			{
				lock(syncObj)
					return cachedCollectionType == null ? cachedCollectionType = Type.GetType(serializedCollectionTypeName, true, false) : cachedCollectionType;
			}
		}

		[SerializeField]
		private string serializedMemberName;

		public string SerializedName { get { return serializedMemberName; } }

		public bool canLoadType
		{
			get
			{
				lock (syncObj)
					return serializedTypeName != null && Type.GetType(serializedTypeName) != null;
			}
		}

		private CollectionComponentDataStore()
		{

		}

		public CollectionComponentDataStore(Type dataType, Type collectionType, string name)
		{
			dataStoreCollection = new List<UnityEngine.Object>();

			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType), "Type of data cannot be null.");

			serializedMemberName = name;

			serializedTypeName = dataType.AssemblyQualifiedName;
			serializedCollectionTypeName = collectionType.AssemblyQualifiedName;

			//Debug.Log("Added: " + serializedCollectionTypeName);
        }

		public CollectionComponentDataStore(int initialSize, Type dataType, Type collectionType, string name)
		{
			dataStoreCollection = new List<UnityEngine.Object>(initialSize);

			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType), "Type of data cannot be null.");

			serializedMemberName = name;

			serializedTypeName = dataType.AssemblyQualifiedName;
			serializedCollectionTypeName = collectionType.AssemblyQualifiedName;
		}

		public CollectionComponentDataStore(IEnumerable<UnityEngine.Object> data, Type dataType, Type collectionType, string name)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data), "Cannot init with a null collection.");

			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType), "Type of data cannot be null.");

			serializedMemberName = name;

			dataStoreCollection = data.ToList();

			serializedTypeName = dataType.AssemblyQualifiedName;
			serializedCollectionTypeName = collectionType.AssemblyQualifiedName;
		}

		public IEnumerator<UnityEngine.Object> GetEnumerator()
		{
			lock (syncObj)
				return dataStoreCollection?.GetEnumerator();
		}

		public void Update(List<UnityEngine.Object> newValue)
		{
			//Debug.Log("New collection. Size: " + newValue.Count());

			lock(syncObj)
				dataStoreCollection = newValue;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			lock (syncObj)
				return dataStoreCollection?.GetEnumerator();
		}

		public void Update(object newValue)
		{
			lock(syncObj)
				dataStoreCollection = ((IList)newValue).Cast<UnityEngine.Object>().ToList();
		}

		public IEnumerable ToCollectionType()
		{
			lock(syncObj)
			{
				//check if it's generic first
				if (SerializedCollectionType.IsGenericType)
				{
					Type t = SerializedCollectionType.GetGenericTypeDefinition();

					if (t == typeof(List<>) || t == typeof(IList<>) || t == typeof(ICollection<>) || t == typeof(IEnumerable<>))
					{
						return this.CallMethod(new Type[] { typeof(UnityEngine.Object), SerializedType }, nameof(Convert), new object[] { this.dataStoreCollection }) as IEnumerable;
					}
				}

				throw new InvalidOperationException("Unable to handle type: " + SerializedCollectionType.GetGenericTypeDefinition().Name);
			}
		}

		private IEnumerable<TDestinationType> Convert<TSourceType, TDestinationType>(IEnumerable<TSourceType> source)
		{
			lock (syncObj)
				return source.Cast<TDestinationType>().ToList();
		}
	}
}
