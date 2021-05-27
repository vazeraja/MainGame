using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/IdleState")]
    public class IdleState : State<Player> {
        public override void OnEnter(Player player){
        }

        public override void LogicUpdate(Player player){

        }

        public override void OnExit(Player player){
        }

    }
}
