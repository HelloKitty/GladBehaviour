using GladBehaviour.Common;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		public static void Test_isInterfaceCollectionType_Returns_False_On_UnExpected_Types(Type t)
		{
			//arrange
			bool result = false;

			//act
			result = SerializedTypeManipulator.isInterfaceCollectionType(t);

			//assert
			Assert.IsFalse(result, "UnExpected Type: {0} was considered an interface collection", t.FullName);
		}
	}
}
