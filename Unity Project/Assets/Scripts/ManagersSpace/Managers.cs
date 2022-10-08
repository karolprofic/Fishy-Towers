namespace ManagersSpace
{
	public static class Managers
	{
		//public static properties
		public static GameManager Game => _game;
		public static AudioManager Audio => _audio;
		public static UIManager UI => _ui;
		public static BattleManager Battle => _battle;
		public static BulletsManager Bullets => _bullets;
		public static UnitsManager Units => _units;
		public static SettingsManager Settings => _settings;
		public static AchievementsManager Achievements => _achievements;
		public static StatisticsManager Statistics => _statistics;
		public static FirebaseManager Firebase => _firebase;
		public static SeaweedManager Seaweed => _seaweed;
		public static ProgressionManager Progression => _progression;

		//private static
		private static GameManager _game;
		private static AudioManager _audio;
		private static UIManager _ui;
		private static BattleManager _battle;
		private static BulletsManager _bullets;
		private static UnitsManager _units;
		private static SettingsManager _settings;
		private static AchievementsManager _achievements;
		private static StatisticsManager _statistics;
		private static FirebaseManager _firebase;
		private static SeaweedManager _seaweed;
		private static ProgressionManager _progression;

		//public static methods
		public static void Register(Manager manager)
		{
			if (Register(ref _game, manager)) return;
			if (Register(ref _audio, manager)) return;
			if (Register(ref _ui, manager)) return;
			if (Register(ref _battle, manager)) return;
			if (Register(ref _bullets, manager)) return;
			if (Register(ref _units, manager)) return;
			if (Register(ref _settings, manager)) return;
			if (Register(ref _achievements, manager)) return;
			if (Register(ref _statistics, manager)) return;
			if (Register(ref _firebase, manager)) return;
			if (Register(ref _seaweed, manager)) return;
			if (Register(ref _progression, manager)) return;
		}

		public static void Unregister(Manager manager)
		{
			if (Unregister(ref _game, manager)) return;
			if (Unregister(ref _audio, manager)) return;
			if (Unregister(ref _ui, manager)) return;
			if (Unregister(ref _battle, manager)) return;
			if (Unregister(ref _bullets, manager)) return;
			if (Unregister(ref _units, manager)) return;
			if (Unregister(ref _achievements, manager)) return;
			if (Unregister(ref _statistics, manager)) return;
			if (Unregister(ref _firebase, manager)) return;
			if (Unregister(ref _seaweed, manager)) return;
			if (Unregister(ref _progression, manager)) return;
		}

		//private static methods
		private static bool Register<T>(ref T myManager, Manager manager) where T : Manager
		{
			if (manager is not T temp) return false;
			
			temp.OnlyOne(ref myManager);
			
			return true;
		}

		private static bool Unregister<T>(ref T myManager, Manager manager) where T : Manager
		{
			if (manager != myManager)
				return false;

			myManager = null;
			return true;
		}
	}
}