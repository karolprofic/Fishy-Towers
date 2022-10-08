using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace ManagersSpace
{
	public abstract class MagazineManager<TE, TM> : Manager where TE : Enum where TM : MovableBehaviour
	{
		//public/inspector
		[SerializeField] protected ElementPackage<TE, TM>[] elementsPackages;

		//private
		protected readonly Dictionary<TE, Magazine<TM>> magazines = new();

		//unity methods
		protected virtual void Start()
		{
			foreach (var (type, element) in elementsPackages)
				magazines.TryAdd(type, new Magazine<TM>(element, Managers.Game.storageMapLocation));
		}

		//public methods
		public TM GetElement(TE type)
		{
			TM element = magazines[type].NextElement();
			element.GetFromStorage();
			return element;
		}
	}
}