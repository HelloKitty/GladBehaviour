using GladBehaviour.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Tests.UnitTests
{
	[TestFixture]
	public static class ContainerParserTests
	{
		[Test(Author = "Andrew Blakely", Description = "Verifies that an unexpected type is not considered interface collection type.", TestOf = typeof(SerializedTypeManipulator))]
		public static void Test_ContainerParser_Contructor_Throws_On_Null_Dependencies()
		{
			//assert
			Assert.Throws<ArgumentNullException>(() => new ContainerParser<ISerializableContainer>(null, Mock.Of<IContainerFieldMatcher>()));
			Assert.Throws<ArgumentNullException>(() => new ContainerParser<ISerializableContainer>(Mock.Of<IEnumerable<ISerializableContainer>>(), null));
		}

		[Test(Author = "Andrew Blakely", Description = "Verifies that an unexpected type is not considered interface collection type.", TestOf = typeof(SerializedTypeManipulator))]
		public static void Test_ContainerParser_Generates_Expected_Stale_Values_When_All_Stale()
		{
			//arrange
			List<CollectionComponentDataStore> collections = new List<CollectionComponentDataStore>();
			collections.Add(new CollectionComponentDataStore(typeof(IDisposable), typeof(ICollection<IDisposable>), nameof(TestClass.Collection1)));

			//Setup a mock object that'll return false for any container type
			Mock<IContainerFieldMatcher> matcherMock = new Mock<IContainerFieldMatcher>(MockBehavior.Strict);
			matcherMock.Setup(x => x.hasMatch(It.IsAny<ISerializableContainer>())).Returns(false);

			ContainerParser<CollectionComponentDataStore> containerParser = new ContainerParser<CollectionComponentDataStore>(collections, matcherMock.Object);

			//act
			IEnumerable<CollectionComponentDataStore> toRemove = containerParser.ComputeStaleContainers();

			//assert
			Assert.NotNull(toRemove);
			Assert.IsTrue(toRemove.Count() == collections.Count);
			Assert.Contains(collections.First(), (ICollection)toRemove, "Expected to remove to contain all values in the collection.");
        }

		[Test(Author = "Andrew Blakely", Description = "Verifies that an unexpected type is not considered interface collection type.", TestOf = typeof(SerializedTypeManipulator))]
		public static void Test_ContainerParser_Generates_Expected_Stale_Values_When_None_Stale()
		{
			//arrange
			List<CollectionComponentDataStore> collections = new List<CollectionComponentDataStore>();
			collections.Add(new CollectionComponentDataStore(typeof(IDisposable), typeof(ICollection<IDisposable>), nameof(TestClass.Collection1)));

			//Setup a mock object that'll return true for any container type and thus won't return removals
			Mock<IContainerFieldMatcher> matcherMock = new Mock<IContainerFieldMatcher>(MockBehavior.Strict);
			matcherMock.Setup(x => x.hasMatch(It.IsAny<ISerializableContainer>())).Returns(true);

			ContainerParser<CollectionComponentDataStore> containerParser = new ContainerParser<CollectionComponentDataStore>(collections, matcherMock.Object);

			//act
			IEnumerable<CollectionComponentDataStore> toRemove = containerParser.ComputeStaleContainers();

			//assert
			Assert.NotNull(toRemove);
			Assert.IsFalse(toRemove.Count() == collections.Count);
			Assert.IsTrue(!toRemove.Contains(collections.First()), "Expected no values to be contained in the removal collection.");
		}

		[Test(Author = "Andrew Blakely", Description = "Verifies that an unexpected type is not considered interface collection type.", TestOf = typeof(SerializedTypeManipulator))]
		public static void Test_ContainerParser_Generates_Expected_Stale_Values_Whent_Collection_Empty()
		{
			//arrange
			//Setup a mock object that'll return false for any container type
			Mock<IContainerFieldMatcher> matcherMock = new Mock<IContainerFieldMatcher>(MockBehavior.Strict);
			matcherMock.Setup(x => x.hasMatch(It.IsAny<ISerializableContainer>())).Returns(false);

			ContainerParser<CollectionComponentDataStore> containerParser = new ContainerParser<CollectionComponentDataStore>(Enumerable.Empty<CollectionComponentDataStore>(), matcherMock.Object);

			//act
			IEnumerable<CollectionComponentDataStore> toRemove = containerParser.ComputeStaleContainers();

			//assert
			Assert.NotNull(toRemove);
			Assert.IsTrue(toRemove.Count() == 0);
		}
	}
}
