using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GladBehaviour.Common
{
	public interface IReflectionStrategy
	{
		//Field
		//non generic attribute
		IEnumerable<FieldInfo> Fields(Type t, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null);
		FieldInfo Field(Type t, string name, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null);

		IEnumerable<FieldInfo> Fields<TAttributeType>(Type t, BindingFlags flags = BindingFlags.Default)
			where TAttributeType : Attribute;

		FieldInfo Field<TAttributeType>(Type t, string name, BindingFlags flags = BindingFlags.Default)
			where TAttributeType : Attribute;
		FieldInfo Field<TAttributeType>(Type t, BindingFlags flags = BindingFlags.Default)
			where TAttributeType : Attribute;

		//Property
		//non generic attribute
		IEnumerable<PropertyInfo> Properties(Type t, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null);
		PropertyInfo Property(Type t, string name, BindingFlags flags = BindingFlags.Default, Type withAttributeType = null);

		IEnumerable<PropertyInfo> Properties<TAttributeType>(Type t, BindingFlags flags = BindingFlags.Default)
			where TAttributeType : Attribute;

		PropertyInfo Property<TAttributeType>(Type t, string name, BindingFlags flags = BindingFlags.Default)
			where TAttributeType : Attribute;

		//getters and setters for reflection
		object GetValue(object instance, FieldInfo info);

		object GetValue(object instance, PropertyInfo info);

		void SetValue(object instance, FieldInfo info, object value);

		object SetValue(object instance, PropertyInfo info, object value);
	}
}
