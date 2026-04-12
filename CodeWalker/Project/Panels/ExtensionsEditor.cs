using CodeWalker.GameFiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CodeWalker.Project.Panels
{
    /// <summary>
    /// Helpers for editing extension MetaWrapper arrays in the ymap/ytyp editors.
    /// Extensions are instances of MCExtensionDef* classes (sub-classes of MetaWrapper).
    /// Each wrapper stores a value-type struct as a public field named "_Data".
    /// PropertyGrid does not natively edit fields, so we expose the struct's properties
    /// via an ICustomTypeDescriptor adapter which reads/writes back into the wrapper.
    /// </summary>
    public static class ExtensionsEditor
    {
        /// <summary>
        /// The list of extension wrapper types that the editor knows how to create.
        /// Order is the order shown in the Add dropdown.
        /// </summary>
        public static readonly Type[] KnownExtensionTypes = new[]
        {
            typeof(MCExtensionDefDoor),
            typeof(MCExtensionDefLightEffect),
            typeof(MCExtensionDefSpawnPoint),
            typeof(MCExtensionDefSpawnPointOverride),
            typeof(MCExtensionDefLightShaft),
            typeof(MCExtensionDefLadder),
            typeof(MCExtensionDefBuoyancy),
            typeof(MCExtensionDefAudioCollisionSettings),
            typeof(MCExtensionDefAudioEmitter),
            typeof(MCExtensionDefExpression),
            typeof(MCExtensionDefParticleEffect),
            typeof(MCExtensionDefExplosionEffect),
            typeof(MCExtensionDefProcObject),
            typeof(MCExtensionDefWindDisturbance),
        };

        public static string GetDisplayName(Type t)
        {
            // Strip leading "MC" to show e.g. "ExtensionDefDoor".
            var n = t.Name;
            if (n.StartsWith("MC")) n = n.Substring(2);
            return n;
        }

        public static string GetDisplayName(MetaWrapper wrapper)
        {
            if (wrapper == null) return "<null>";
            return GetDisplayName(wrapper.GetType());
        }

        /// <summary>
        /// Create a new extension instance of the given MetaWrapper subclass.
        /// </summary>
        public static MetaWrapper CreateExtension(Type wrapperType)
        {
            if (wrapperType == null) return null;
            return Activator.CreateInstance(wrapperType) as MetaWrapper;
        }

        /// <summary>
        /// Return an object that can be shown in a PropertyGrid to edit the
        /// struct data of the given extension wrapper. Mutations are written
        /// back into the wrapper's _Data field.
        /// </summary>
        public static object GetEditObject(MetaWrapper wrapper)
        {
            if (wrapper == null) return null;
            var dataField = wrapper.GetType().GetField("_Data", BindingFlags.Public | BindingFlags.Instance);
            if (dataField == null)
            {
                // No struct data field - just return the wrapper itself so the
                // PropertyGrid shows whatever properties it has.
                return wrapper;
            }
            return new MetaWrapperDataAdapter(wrapper, dataField);
        }

        /// <summary>
        /// Append an extension to an existing MetaWrapper[] array and return the new array.
        /// </summary>
        public static MetaWrapper[] AddExtension(MetaWrapper[] existing, MetaWrapper toAdd)
        {
            if (toAdd == null) return existing;
            var list = existing != null ? new List<MetaWrapper>(existing) : new List<MetaWrapper>();
            list.Add(toAdd);
            return list.ToArray();
        }

        /// <summary>
        /// Remove an extension from an existing MetaWrapper[] array and return the new array.
        /// </summary>
        public static MetaWrapper[] RemoveExtension(MetaWrapper[] existing, MetaWrapper toRemove)
        {
            if (existing == null || toRemove == null) return existing;
            var list = new List<MetaWrapper>(existing);
            list.Remove(toRemove);
            return list.Count == 0 ? null : list.ToArray();
        }
    }

    /// <summary>
    /// ICustomTypeDescriptor that exposes the public properties of an extension
    /// wrapper's internal struct field as editable PropertyGrid properties.
    /// </summary>
    internal sealed class MetaWrapperDataAdapter : ICustomTypeDescriptor
    {
        private readonly MetaWrapper _wrapper;
        private readonly FieldInfo _dataField;
        private readonly Type _structType;
        private readonly PropertyDescriptorCollection _props;

        public MetaWrapperDataAdapter(MetaWrapper wrapper, FieldInfo dataField)
        {
            _wrapper = wrapper;
            _dataField = dataField;
            _structType = dataField.FieldType;

            var pds = new List<PropertyDescriptor>();
            foreach (var pi in _structType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!pi.CanRead) continue;
                if (pi.GetIndexParameters().Length > 0) continue;
                // Hide the "UnusedN" padding fields - they're not meaningful to edit.
                if (pi.Name.StartsWith("Unused", StringComparison.OrdinalIgnoreCase)) continue;
                pds.Add(new StructFieldPropertyDescriptor(pi));
            }
            _props = new PropertyDescriptorCollection(pds.ToArray());
        }

        public object GetDataBoxed()
        {
            return _dataField.GetValue(_wrapper);
        }

        public void SetDataBoxed(object boxed)
        {
            _dataField.SetValue(_wrapper, boxed);
        }

        public override string ToString()
        {
            return ExtensionsEditor.GetDisplayName(_wrapper);
        }

        // ICustomTypeDescriptor implementation
        public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);
        public string GetClassName() => _structType.Name;
        public string GetComponentName() => ExtensionsEditor.GetDisplayName(_wrapper);
        public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);
        public EventDescriptor GetDefaultEvent() => null;
        public PropertyDescriptor GetDefaultProperty() => _props.Count > 0 ? _props[0] : null;
        public object GetEditor(Type editorBaseType) => null;
        public EventDescriptorCollection GetEvents() => EventDescriptorCollection.Empty;
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => EventDescriptorCollection.Empty;
        public PropertyDescriptorCollection GetProperties() => _props;
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => _props;
        public object GetPropertyOwner(PropertyDescriptor pd) => this;

        /// <summary>
        /// PropertyDescriptor that reads/writes a property of the wrapper's
        /// boxed struct and copies the modified struct back into the wrapper.
        /// </summary>
        private sealed class StructFieldPropertyDescriptor : PropertyDescriptor
        {
            private readonly PropertyInfo _pi;

            public StructFieldPropertyDescriptor(PropertyInfo pi)
                : base(pi.Name, (Attribute[])pi.GetCustomAttributes(typeof(Attribute), true))
            {
                _pi = pi;
            }

            public override Type ComponentType => typeof(MetaWrapperDataAdapter);
            public override bool IsReadOnly => !_pi.CanWrite;
            public override Type PropertyType => _pi.PropertyType;

            public override bool CanResetValue(object component) => false;
            public override void ResetValue(object component) { }
            public override bool ShouldSerializeValue(object component) => false;

            public override object GetValue(object component)
            {
                var adapter = component as MetaWrapperDataAdapter;
                if (adapter == null) return null;
                var boxed = adapter.GetDataBoxed();
                return _pi.GetValue(boxed, null);
            }

            public override void SetValue(object component, object value)
            {
                var adapter = component as MetaWrapperDataAdapter;
                if (adapter == null) return;
                if (!_pi.CanWrite) return;
                var boxed = adapter.GetDataBoxed();
                _pi.SetValue(boxed, value, null);
                adapter.SetDataBoxed(boxed);
            }
        }
    }
}
