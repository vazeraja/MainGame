using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/IdleState")]
    public class IdleState : State {

        public override void OnEnter(Player player) {
        }

        public override void LogicUpdate(Player player) {
            Idle(player);
        }

        public override void OnExit(Player player) {
        }

        private void Idle(Player player) {
            player.Anim.Play("player_idle");
        }

    }
}
