using GladBehaviour.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Tests.UnitTests
{
	[TestFixture]
	public static class CollectionContainerFieldMatcherTests
	{
		[Test]
		public static void Test_CollectionContainerFieldMatcher_Contructor_Throws_On_Null_Type()
		{
			//assert
			Assert.Throws<ArgumentNullException>(() => new CollectionContainerFieldMatcher(null));
		}

		[Test]
		public static void Test_CollectionContainerFieldMatcher_Produces_Expected_Results_With_Empty_Collection()
		{
			//arrange
			CollectionContainerFieldMatcher matcher = new CollectionContainerFieldMatcher(typeof(EmptyType));

			Mock<ISerializableContainer> container = new Mock<ISerializableContainer>(MockBehavior.Strict);

			//setup behaviour to return a test name
			container.Setup(x => x.SerializedName).Returns("Test");

			//act
			FieldInfo matchForSomething = matcher.FindMatch(container.Object);
			IEnumerable<FieldInfo> fieldsToAdd = matcher.FindUnContainedFields(Enumerable.Empty<CollectionComponentDataStore>());
			bool result = matcher.hasMatch(container.Object);

			//assert
			Assert.IsNull(matchForSomething, "Expected no valid field info to return."); //should find a fieldinfo
			Assert.IsEmpty(fieldsToAdd); //shouldn't be any valid fields to be added.
			Assert.IsFalse(result, "Shouldn't have found a match.");
		}

		public class EmptyType { }
	}
}
