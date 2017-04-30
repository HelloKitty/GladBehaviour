using GladBehaviour.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace GladBehaviour.Tests.UnitTests
{
	[TestFixture]
	public static class SingleContainerFieldMatcherTests
	{
		[Test]
		public static void Test_FindUncontainedFields_Returns_Uncontained_Fields()
		{
			//arrange
			SingleContainerFieldMatcher matcher = new SingleContainerFieldMatcher(typeof(TestClass));
			List<ISerializableContainer> containers = new List<ISerializableContainer>();

			//act
			IEnumerable<FieldInfo> infos = matcher.FindUnContainedFields(containers);

			//assert
			Assert.NotNull(infos);
			Assert.IsTrue(infos.Any());
			Assert.IsTrue(infos.Any(i => i.Type() == typeof(IDisposable)));
			Assert.IsTrue(!infos.Any(i => SerializedTypeManipulator.isInterfaceCollectionType(i.Type())));

			//Check and make sure the protected field is there
			Assert.IsTrue(infos.Any(i => i.Name == "TestSingle"));
		}
	}
}
