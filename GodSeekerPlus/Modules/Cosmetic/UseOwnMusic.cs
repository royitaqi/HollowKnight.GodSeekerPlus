using UnityEngine.Audio;

namespace GodSeekerPlus.Modules.Cosmetic;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class UseOwnMusic : Module {
	private static readonly Lazy<AudioMixerSnapshot> silent = new(() =>
		Resources.FindObjectsOfTypeAll<AudioMixerSnapshot>().First(i => i.name == "Silent")
	);

	private static readonly Dictionary<string, Action<Scene>> sceneDict = new() {
		{ "GG_Vengefly", ModifySceneManagerGGMusicControl },
		{ "GG_Vengefly_V", ModifySceneManagerGGMusicControl },
		{ "GG_Gruz_Mother", ModifySceneManagerGGMusicControl },
		{ "GG_Gruz_Mother_V", ModifySceneManagerGGMusicControl },
		{ "GG_False_Knight", ModifySceneManagerGGMusicControl },
		{ "GG_Mega_Moss_Charger", FuncUtil.Combine(StopMusic, RemoveSceneManagerFSM, ModifyMegaMossChargerFSM) },
		{ "GG_Hornet_1", ModifySceneManagerFSM },

		{ "GG_Ghost_Gorb", ModifySceneManagerFSM },
		{ "GG_Ghost_Gorb_V", ModifySceneManagerGGMusicControl },
		{ "GG_Dung_Defender", ModifySceneManagerFSM },
		{ "GG_Mage_Knight", FuncUtil.Combine(StopMusic, ModifySceneManagerFSM) },
		{ "GG_Mage_Knight_V", ModifySceneManagerFSM },
		{ "GG_Brooding_Mawlek", ModifySceneManagerFSM },
		// { "GG_Brooding_Mawlek_V", No-op },
		// { "GG_Nailmasters", No-op },

		{ "GG_Nosk_Hornet", ModifyWingedNoskBattleControlFSM }
	};

	public UseOwnMusic() => _ = silent.Value;

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += ModifyMusic;

	private protected override void Unload() =>
		OsmiHooks.SceneChangeHook -= ModifyMusic;

	private static void ModifyMusic(Scene prev, Scene next) {
		if (BossSequenceController.IsInSequence && sceneDict.TryGetValue(next.name, out Action<Scene> action)) {
			action.Invoke(next);
			Logger.LogDebug("GG Music modified");
		}
	}


	private static void StopMusic(Scene scene) =>
		silent.Value.TransitionTo(0f);

	private static void ModifySceneManagerGGMusicControl(Scene scene) => scene
		.GetRootGameObjects()
		.First(go => go.name == "_SceneManager")
		.LocateMyFSM("gg_music_control")
		.ChangeTransition("Init", FsmEvent.Finished.Name, "Wait");

	private static void ModifySceneManagerFSM(Scene scene) => scene
		.GetRootGameObjects()
		.First(go => go.name == "_SceneManager")
		.LocateMyFSM("FSM")
		.ChangeTransition("Init", FsmEvent.Finished.Name, "Wait");

	private static void RemoveSceneManagerFSM(Scene scene) => UObject.Destroy(
		scene
			.GetRootGameObjects()
			.First(go => go.name == "_SceneManager")
			.LocateMyFSM("FSM")
	);

	private static void ModifyMegaMossChargerFSM(Scene scene) => scene
		.GetRootGameObjects()
		.First(go => go.name == "Mega Moss Charger")
		.LocateMyFSM("Mossy Control")
		.RemoveAction("Music", 0);

	private static void ModifyWingedNoskBattleControlFSM(Scene scene) => scene
		.GetRootGameObjects()
		.First(go => go.name == "Battle Scene")
		.LocateMyFSM("Battle Control")
		.ChangeTransition("Music Type", "PANTHEON", "Orig Music");
}
