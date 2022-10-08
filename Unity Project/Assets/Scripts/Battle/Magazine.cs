using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Battle
{
	public class Magazine<T> where T : MovableBehaviour
	{
		//properties
		private int Idx
		{
			get => _idx;
			set
			{
				_idx = value;
				if (_idx < 0 || _idx >= elements.Count)
					_idx = 0;
			}
		}

		public Vector3 Storage { get; }

		//private
		private readonly List<T> elements = new();
		private readonly T prefab;
		
		private int _idx;
		
		//constructors
		public Magazine(T element, Vector3 storage)
		{
			prefab = element;
			Storage = storage;
		}

		//public methods
		public T NextElement()
		{
			T element;
			for (int idx = Idx++; Idx != idx; ++Idx)
			{
				element = elements[Idx];
				if(element.IsActive)
					continue;
				return element;
			}

			element = Object.Instantiate(
				prefab,
				Storage,
				Quaternion.identity);

			elements.Add(element);
			Idx = elements.Count - 1;
			return element;
		}

		public void ForEach(Action<T> action)
		{
			for (int i = 0; i < elements.Count; i++)
				action.Invoke(elements[i]);
		}
	}
}