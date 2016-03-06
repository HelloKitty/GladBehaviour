using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Fasterflect;

namespace GladBehaviour.Common
{
	/// <summary>
	/// Serializable mutable collection of components that persist type data.
	/// Provides the ability to transform the underlying collection to the desired type.
	/// </summary>
	[Serializable]
	public class CollectionComponentDataStore : IEnumerable<UnityEngine.Object>, IDataUpdatable<List<UnityEngine.Object>>, ISerializableContainer
	{
		//Unity editor does not promise any of this will run on the main thread.
		//Better to be safe
		private readonly object syncObj = new object();

		/// <summary>
		/// Serializable mutable collection field for serialized components.
		/// </summary>
		[SerializeField]
		public List<UnityEngine.Object> dataStoreCollection;

		/// <summary>
		/// Serializable Type name data in assembly qualified string form.
		/// </summary>
		[SerializeField]
		private string serializedTypeName;

		//caching should make this quicker
		private Type cachedType = null;
		/// <summary>
		/// Inner type this collection can map to.
		/// </summary>
		public Type SerializedType
		{
			get
			{
				lock (syncObj)
					return cachedType == null ? cachedType = Type.GetType(serializedTypeName, true, false) : cachedType;
			}
		}

		/// <summary>
		/// Serializable Type name data for the collection tpye in assembly qualified string form.
		/// </summary>
		[SerializeField]
		private string serializedCollectionTypeName;

		//caching should make this quicker
		private Type cachedCollectionType = null;
		/// <summary>
		/// Collection Type this collection can map to. (Ex. IEnumerable{ISomething})
		/// </summary>
		public Type SerializedCollectionType
		{
			get
			{
				lock(syncObj)
					return cachedCollectionType == null ? cachedCollectionType = Type.GetType(serializedCollectionTypeName, true, false) : cachedCollectionType;
			}
		}

		/// <summary>
		/// Serializable member name this collection is serializing for.
		/// </summary>
		[SerializeField]
		private string serializedMemberName;

		/// <summary>
		/// Serializable member name this collection is serializing for.
		/// </summary>
		public string SerializedName { get { return serializedMemberName; } }

		/// <summary>
		/// Indicates if the Type persisted can be loaded.
		/// </summary>
		public bool canLoadType
		{
			get
			{
				lock (syncObj)
					return serializedTypeName != null && Type.GetType(serializedTypeName) != null;
			}
		}

		/// <summary>
		/// Should only be called by Unity. Do not invoke this constructor manually.
		/// </summary>
		private CollectionComponentDataStore()
		{

		}

		/// <summary>
		/// Creates an empty collection with the specific inner type, collection type and name.
		/// </summary>
		/// <param name="dataType">The type of data this collection should contain.</param>
		/// <param name="collectionType">The collection type this collection is persisting for.</param>
		/// <param name="name">Name of the field/prop this collection is persisting for.</param>
		public CollectionComponentDataStore(Type dataType, Type collectionType, string name)
			: this(Enumerable.Empty<UnityEngine.Object>(), dataType, collectionType, name)
		{

		}

		/// <summary>
		/// Creates an empty collection of the specified size with the specific inner type, collection type and name.
		/// </summary>
		/// <param name="initialSize">Non-negative initial size for the collection.</param>
		/// <param name="dataType">The type of data this collection should contain.</param>
		/// <param name="collectionType">The collection type this collection is persisting for.</param>
		/// <param name="name">Name of the field/prop this collection is persisting for.</param>
		public CollectionComponentDataStore(int initialSize, Type dataType, Type collectionType, string name)
			: this(new List<UnityEngine.Object>(initialSize), dataType, collectionType, name)
		{
			if (initialSize < 0)
				throw new ArgumentException(nameof(initialSize) + " must not be negative.", nameof(initialSize));
		}

		/// <summary>
		/// Creates a collection with the provided internal collection with the specific inner type, collection type and name.
		/// </summary>
		/// <param name="data">Collection to store internally.</param>
		/// <param name="dataType">The type of data this collection should contain.</param>
		/// <param name="collectionType">The collection type this collection is persisting for.</param>
		/// <param name="name">Name of the field/prop this collection is persisting for.</param>
		public CollectionComponentDataStore(IEnumerable<UnityEngine.Object> data, Type dataType, Type collectionType, string name)
		{
			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType), "Type of data cannot be null.");

			if (collectionType == null)
				throw new ArgumentNullException(nameof(collectionType), "Type of the collection cannot be null.");

			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The member name must not be empty or null.", nameof(dataType));

			if (data == null)
				throw new ArgumentNullException(nameof(data), "The provided collection must not be null. At least must be empty.");
			

			dataStoreCollection = data.ToList();

			serializedMemberName = name;
			serializedTypeName = dataType.AssemblyQualifiedName;
			serializedCollectionTypeName = collectionType.AssemblyQualifiedName;
		}
		
		/// <summary>
		/// Provides iterable interface.
		/// </summary>
		/// <returns>An enumerable object.</returns>
		public IEnumerator<UnityEngine.Object> GetEnumerator()
		{
			lock (syncObj)
				return dataStoreCollection?.GetEnumerator();
		}

		/// <summary>
		/// Updates the internal collection.
		/// </summary>
		/// <param name="newValue">Collection to set as the new internal collection.</param>
		public void Update(List<UnityEngine.Object> newValue)
		{
			//This might be a bad idea. Maybe we should swap the elements out.
			//It's likely we're getting the same list reference back but this still seems like a shakey idea
			lock(syncObj)
				dataStoreCollection = newValue;		
		}

		/// <summary>
		/// Provides iterable interface.
		/// </summary>
		/// <returns>An enumerable object.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			lock (syncObj)
				return dataStoreCollection?.GetEnumerator();
		}

		/// <summary>
		/// Updates the internal collection.
		/// </summary>
		/// <param name="newValue">Collection to set as the new internal collection.</param>
		public void Update(object newValue)
		{
			lock(syncObj)
				dataStoreCollection = ((IList)newValue).Cast<UnityEngine.Object>().ToList();
		}

		/// <summary>
		/// Transforms the internal collection from <see cref="UnityEngine.Object"/> list to <see cref="SerializedType"/> list.
		/// </summary>
		/// <returns>A enumerable collection of type IEnumerable{T} where T is <see cref="SerializedType"/>.</returns>
		public IEnumerable ToCollectionType()
		{

			//TODO: Refactor this and maybe move it into a dependency
			lock(syncObj)
			{
				//check if it's generic first
				if (SerializedCollectionType.IsGenericType)
				{
					//We need the IEnumerable<T> version not the IEnumerable<ISomething> type
					Type t = SerializedCollectionType.GetGenericTypeDefinition();

					//Check if it matches the current internal collection which is a List<T>
					if (t == typeof(List<>) || t == typeof(IList<>) || t == typeof(ICollection<>) || t == typeof(IEnumerable<>))
					{
						return this.CallMethod(new Type[] { typeof(UnityEngine.Object), SerializedType }, nameof(ConvertToList), new object[] { this.dataStoreCollection }) as IEnumerable;
					}
					else
					{
						//Otherwise we need to handle the odd collections here like Stack, Queue and etc.
						throw new InvalidOperationException("Unable to handle type: " + SerializedCollectionType.GetGenericTypeDefinition().Name);
					}
				}
				else
				{
					//It could be an array
					if(SerializedCollectionType.IsArray)
					{
						return this.CallMethod(new Type[] { typeof(UnityEngine.Object), SerializedType }, nameof(ConvertToArray), new object[] { this.dataStoreCollection }) as IEnumerable;
					}
				}

				throw new InvalidOperationException("Unable to handle type: " + SerializedCollectionType);
			}
		}

		/// <summary>
		/// Converts a collection from <typeparamref name="TSourceType"/> to <typeparamref name="TDestinationType"/>.
		/// </summary>
		/// <typeparam name="TSourceType">Source type of the collection.</typeparam>
		/// <typeparam name="TDestinationType">Desired destination type.</typeparam>
		/// <param name="source"></param>
		/// <returns>List collection of <typeparamref name="TDestinationType"/> for {T}.</returns>
		private List<TDestinationType> ConvertToList<TSourceType, TDestinationType>(IEnumerable<TSourceType> source)
		{
			lock (syncObj)
				return source.Cast<TDestinationType>().ToList(); //casts and then converts to a list.
		}

		/// <summary>
		/// Converts a collection from <typeparamref name="TSourceType"/> to <typeparamref name="TDestinationType"/>.
		/// </summary>
		/// <typeparam name="TSourceType">Source type of the collection.</typeparam>
		/// <typeparam name="TDestinationType">Desired destination type.</typeparam>
		/// <param name="source"></param>
		/// <returns>Array collection of <typeparamref name="TDestinationType"/> for {T}.</returns>
		private TDestinationType[] ConvertToArray<TSourceType, TDestinationType>(IEnumerable<TSourceType> source)
		{
			lock (syncObj)
				return source.Cast<TDestinationType>().ToArray(); //casts and then converts to an array
		}
	}
}
