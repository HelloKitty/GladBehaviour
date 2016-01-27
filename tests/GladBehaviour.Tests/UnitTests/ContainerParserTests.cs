using GladBehaviour.Common;
using Moq;
using NUnit.Framework;
using System;
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
	}
}
