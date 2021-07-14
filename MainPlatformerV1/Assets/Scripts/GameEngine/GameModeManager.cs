using System;
using System.Collections;
using UnityEngine;


public interface IGameMode { }

public class GameModeManager : MonoBehaviour {
    // [SerializeField] private InputReader inputReader;
    //
    // private SasukeController player;
    //
    // private void OnEnable() {
    //     inputReader.JumpEvent += SpawnPlayer;
    // }
    //
    // private void OnDisable() {
    //     inputReader.JumpEvent -= SpawnPlayer;
    // }
    //
    // private void SpawnPlayer() {
    //     player = FindObjectOfType<SasukeController>();
    //     if (player != null) return;
    //     Instantiate(Resources.Load<SasukeController>("Player_Sasuke"));
    //     inputReader.JumpEvent -= SpawnPlayer;
    // }
    //
    // private IEnumerator SwitchMode(IGameMode mode) {
    //     yield return new WaitUntil(() => player == null);
    // }
}