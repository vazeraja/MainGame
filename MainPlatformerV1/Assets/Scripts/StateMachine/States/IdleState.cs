using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/IdleState")]
    public class IdleState : State {

        public override void OnEnter(Player player) {
            Debug.Log("entered idle");
        }

        public override void LogicUpdate(Player player) {
            Idle(player);
        }

        public override void OnExit(Player player) {
            Debug.Log("exited idle");
        }

        private void Idle(Player player) {
            player.Anim.Play("player_idle");
        }

    }
}
