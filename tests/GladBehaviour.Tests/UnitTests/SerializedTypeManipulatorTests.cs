using GladBehaviour.Common;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Tests
{
	[TestFixture]
	public static class SerializedTypeManipulatorTests
	{
		[Test(Author = "Andrew Blakely", Description = "Verifies that an expected type is considered an interface collection type.", TestOf = typeof(SerializedTypeManipulator))]
		[TestCase(typeof(ICollection<IDisposable>))]
		[TestCase(typeof(IList<IDisposable>))]
		[TestCase(typeof(List<IDisposable>))]
		[TestCase(typeof(IEnumerable<IDisposable>))]
		[TestCase(typeof(IDisposable[]))]
		public static void Test_isInterfaceCollectionType_Returns_True_On_Expected_Types(Type t)
		{
			//arrange
			bool result = false;

			//act
			result = SerializedTypeManipulator.isInterfaceCollectionType(t);

			//assert
			Assert.IsTrue(result, "Expected Type: {0} to ", nameof(SerializedTypeManipulator.isInterfaceCollectionType));
		}

		[Test(Author = "Andrew Blakely", Description = "Verifies that an unexpected type is not considered interface collection type.", TestOf = typeof(SerializedTypeManipulator))]
		[TestCase(typeof(ICollection<>))]
		[TestCase(typeof(IEnumerable<Has>))]
		[TestCase(typeof(ICollection<DataAttribute>))]
		[TestCase(typeof(ICollection<int>))]
		[TestCase(typeof(ICollection))]
		[TestCase(null)]
		[TestCase(typeof(Array))]
		[TestCase(typeof(ArrayList))]
		public static void Test_isInterfaceCollectionType_Returns_False_On_UnExpected_Types(Type t)
		{
			//arrange
			bool result = false;

			//act
			result = SerializedTypeManipulator.isInterfaceCollectionType(t);

			//assert
			Assert.IsFalse(result, "UnExpected Type: {0} was considered an interface collection", t?.FullName);
		}

		[Test(Author = "Andrew Blakely", Description = "Ensures collection parser throws when the type is null.", TestOf = typeof(SerializedTypeManipulator))]
		public static void Test_GetCollectionFields_Throws_When_Null()
		{
			//assert
			Assert.Throws<ArgumentNullException>(() => SerializedTypeManipulator.GetCollectionFields(null));
		}

		[Test(Author = "Andrew Blakely", Description = "Ensures collection parser returns expected results.", TestOf = typeof(SerializedTypeManipulator))]
		public static void Test_GetCollectionFields_Returns_Expected_FieldInfo()
		{
			//arrange
			Type typeToParse = typeof(TestClass);

			//act
			IEnumerable<FieldInfo> fieldInfos = SerializedTypeManipulator.GetCollectionFields(typeToParse);

			//assert
			Assert.IsNotEmpty(fieldInfos, "Expected to find collection types but failed.");

			Assert.IsTrue(fieldInfos.Where(x => x.Name == nameof(TestClass.Collection1)).Count() != 0);
			Assert.IsTrue(fieldInfos.Where(x => x.Name == nameof(TestClass.Collection2)).Count() != 0);
			Assert.IsTrue(fieldInfos.Where(x => x.Name == nameof(TestClass.Collection3)).Count() != 0);
			Assert.IsTrue(fieldInfos.Where(x => x.Name == nameof(TestClass.Collection4)).Count() != 0);

			//Should find these
			Assert.IsTrue(fieldInfos.Where(x => x.Name == "blahblahshouldnthave").Count() == 0, "Found an unexpected field.");
			Assert.IsTrue(fieldInfos.Where(x => x.Name == nameof(TestClass.Collection5)).Count() == 0, "Found an unexpected field.");
			Assert.IsTrue(fieldInfos.Where(x => x.Name == nameof(TestClass.Collection6)).Count() == 0, "Found an unexpected field.");
		}

		public class TestClass
		{
			[SerializeField]
			public ICollection<IDisposable> Collection1;

			[SerializeField]
			public IDisposable[] Collection2;

			[SerializeField]
			public List<IDisposable> Collection3;

			[SerializeField]
			public IDisposable[] Collection4;

			[HideInInspector]
			[SerializeField]
			public List<IDisposable> Collection5;

			public List<IDisposable> Collection6;
		}
	}
}
