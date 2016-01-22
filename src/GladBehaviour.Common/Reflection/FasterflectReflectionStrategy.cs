using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace GladBehaviour.Common
{
	public class FasterflectReflectionStrategy : IReflectionStrategy
	{
		public FieldInfo Field(Type t, string name, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null)
		{
			if (withAttributeType == null)
				return t.Field(name, flags);
			else
				return t.FieldsWith(flags, withAttributeType).FirstOrDefault(x => x.Name == name);
		}

		public FieldInfo Field<TAttributeType>(Type t, BindingFlags flags = BindingFlags.Default) where TAttributeType : Attribute
		{
			return t.FieldsWith(flags, typeof(TAttributeType)).FirstOrDefault();
		}

		public FieldInfo Field<TAttributeType>(Type t, string name, BindingFlags flags = BindingFlags.Default) where TAttributeType : Attribute
		{
			return t.FieldsWith(flags, typeof(TAttributeType)).FirstOrDefault(x => name == x.Name);
		}

		public IEnumerable<FieldInfo> Fields(Type t, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null)
		{
			if (withAttributeType == null)
				return t.Fields(flags);
			else
				return t.Fields(flags).Where(x => x.HasAttribute(withAttributeType));
		}

		public IEnumerable<FieldInfo> Fields<TAttributeType>(Type t, BindingFlags flags = BindingFlags.Default) where TAttributeType : Attribute
		{
			return t.FieldsWith(flags, typeof(TAttributeType));
		}

		public IEnumerable<PropertyInfo> Properties(Type t, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null)
		{
			if (withAttributeType == null)
				return t.Properties(flags);
			else
				return t.Properties(flags).Where(x => x.HasAttribute(withAttributeType));
		}

		public IEnumerable<PropertyInfo> Properties<TAttributeType>(Type t, BindingFlags flags = BindingFlags.Default) where TAttributeType : Attribute
		{
			return t.PropertiesWith(flags, typeof(TAttributeType));
		}

		public PropertyInfo Property(Type t, string name, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null)
		{
			if (withAttributeType == null)
				return t.Properties(flags).FirstOrDefault();
			else
				return t.Properties(flags).Where(x => x.HasAttribute(withAttributeType)).FirstOrDefault();
		}

		public PropertyInfo Property<TAttributeType>(Type t, string name, BindingFlags flags = BindingFlags.Default) where TAttributeType : Attribute
		{
			return t.PropertiesWith(flags, typeof(TAttributeType)).FirstOrDefault();
		}

		public object SetValue(object instance, PropertyInfo info, object value)
		{
			return info.GetValue(instance, null);
		}

		public void SetValue(object instance, FieldInfo info, object value)
		{
			info.Set(value);
		}

		public object GetValue(object instance, PropertyInfo info)
		{
			return info.GetValue(instance, null);
		}

		public object GetValue(object instance, FieldInfo info)
		{
			return info.GetValue(instance);
		}
	}
}
