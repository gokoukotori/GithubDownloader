using System;
using System.Collections;
using System.Collections.Generic;

namespace GKsWpfLib.Plugins
{
	public class VisualPlugins : IVisualPlugins
	{
		private IVisualPlugin[] _Array = Array.Empty<IVisualPlugin>();
		public IVisualPlugin this[int index]
		{
			get
			{
				if (_Array == null) throw new InvalidOperationException(nameof(_Array) + " == null");
				if (index < 0 || _Array.Length <= index) throw new ArgumentException(nameof(index) + " < 0 || " + nameof(_Array) + ".Length <= " + nameof(index));
				return _Array[index];
			}
			set
			{
				if (_Array == null) throw new InvalidOperationException(nameof(_Array) + " == null");
				if (index < 0 || _Array.Length <= index) throw new ArgumentNullException(nameof(index) + " < 0 || " + nameof(_Array) + ".Length <= " + nameof(index));
				_Array[index] = value;
			}
		}

		public int Count => _Array == null ? throw new InvalidOperationException(nameof(_Array) + " == null") : _Array.Length;

		public bool IsReadOnly => false;

		public void Add(IVisualPlugin item)
		{
			if (_Array == null) throw new InvalidOperationException(nameof(_Array) + " == null");
			var temporary = new IVisualPlugin[_Array.Length + 1];
			_Array.CopyTo(temporary, 0);
			temporary[^1] = item;
			_Array = temporary;
		}
		public void Clear() => _Array = Array.Empty<IVisualPlugin>();
		public bool Contains(IVisualPlugin item)
		{
			if (_Array == null) throw new InvalidOperationException(nameof(_Array) + " == null");
			if (item == null) throw new ArgumentNullException(nameof(item) + " == null");
			return Array.IndexOf(_Array, item) != -1;
		}
		public void CopyTo(IVisualPlugin[] array, int arrayIndex)
		{
			if (_Array == null) throw new InvalidOperationException("this." + nameof(array) + " == null");
			if (array == null) throw new ArgumentNullException(nameof(array) + " == null");
			if (arrayIndex < 0 || array.Length <= arrayIndex) throw new ArgumentException(nameof(arrayIndex) + " < 0 || " + nameof(array) + ".Length <= " + nameof(arrayIndex));
			_Array.CopyTo(array, arrayIndex);
		}
		public IEnumerator<IVisualPlugin> GetEnumerator()
		{
			foreach (var item in _Array) yield return item;
		}
		public int IndexOf(IVisualPlugin item) => _Array == null ? throw new ArgumentNullException(nameof(_Array) + " == null") : Array.IndexOf(_Array, item);
		public void Insert(int index, IVisualPlugin item)
		{
			if (_Array == null) throw new InvalidOperationException(nameof(_Array) + " == null");
			if (index < 0 || _Array.Length <= index) throw new ArgumentException(nameof(index) + " < 0 || " + nameof(_Array) + ".Length <= " + nameof(index));
			if (item == null) throw new ArgumentNullException(nameof(item) + " == null");
			var temporary = new IVisualPlugin[_Array.Length + 1];
			Array.Copy(_Array, 0, temporary, 0, index);
			Array.Copy(_Array, index, temporary, index + 1, _Array.Length - index);
			temporary[index] = item;
			_Array = temporary;
		}
		public bool Remove(IVisualPlugin item)
		{
			if (_Array == null) throw new InvalidOperationException(nameof(_Array) + " == null");
			if (item == null) throw new ArgumentNullException(nameof(item) + " == null");
			if (_Array.Length == 0) throw new ArgumentNullException(nameof(_Array) + ".Length == 0");
			int index = Array.BinarySearch(_Array, item);
			if (index < 0) return false;

			RemoveAt(index);

			return true;
		}
		public void RemoveAt(int index)
		{
			if (_Array == null) throw new InvalidOperationException(nameof(_Array) + " == null");
			if (index < 0 || _Array.Length <= index) throw new ArgumentException(nameof(index) + " < 0 || " + nameof(_Array) + ".Length <= " + nameof(index));
			var temporary = new IVisualPlugin[_Array.Length - 1];
			Array.Copy(_Array, 0, temporary, 0, index);
			Array.Copy(_Array, index + 1, temporary, index, _Array.Length - index - 1);
			_Array = temporary;
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
