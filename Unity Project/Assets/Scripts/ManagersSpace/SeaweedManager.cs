using UnityEngine;

namespace ManagersSpace
{
	public class SeaweedManager : MagazineManager<Seaweed.Type, Seaweed>
	{
		//static
		
		
		//public/inspector
		//Todo: jeżeli możliwe zmienić na wielką literę
		[field: SerializeField] public ushort currentAmount { get; private set; } = 0;

		[SerializeField] private float passiveIncomeTimer;

		//unity methods

		protected override void Start()
		{
			base.Start();
			Seaweed.OnSeaweedClick.AddListener(OnSeaweedClick);
		}

		private void Update()
		{
			if(BattleManager.GameStopped) return;

			if(Input.GetMouseButtonUp(0))
			{
				if(Seaweed.selectedSeaweed == null)
					return;

				if(Seaweed.selectedSeaweed.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
					Seaweed.OnSeaweedClick.Invoke(Seaweed.selectedSeaweed);
				Seaweed.selectedSeaweed = null;
			}
			
			passiveIncomeTimer += Time.deltaTime;
			if(passiveIncomeTimer < 9)
				return;
			passiveIncomeTimer = 0.0f;
			GeneratePassiveIncome();
		}

		//public methods
		public bool CanBuy(ushort cost)
		{
			return cost <= currentAmount;
		}

		public void Buy(ushort cost)
		{
			currentAmount -= cost;
		}

		public void Add(ushort amount)
		{
			currentAmount += amount;
		}

		//private methods
		private void OnSeaweedClick(Seaweed seaweed)
		{
			Managers.Statistics.IncreaseStatisticValue("gainedSeaweed", seaweed.value);
			currentAmount += seaweed.value;
			seaweed.GoToStorage();
		}

		private void GeneratePassiveIncome()
		{
			Seaweed seaweed = Managers.Seaweed.GetElement(Seaweed.Type.Standard);
			//Todo: Skąd ta 8f?
			float x = Random.Range(0f, 8f);
			seaweed.transform.position = new Vector3(x, 2, -198);
			seaweed.destination = new Vector3(x, Random.Range(-1f, -6f), -198);
		}
	}
}