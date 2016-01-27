using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GladBehaviour.Tests
{
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
