using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/IdleState")]
    public class IdleState : State<MainPlayer> {
        public override void OnEnter(MainPlayer mainPlayer){
        }

        public override void LogicUpdate(MainPlayer mainPlayer){

        }

        public override void OnExit(MainPlayer mainPlayer){
        }

    }
}
