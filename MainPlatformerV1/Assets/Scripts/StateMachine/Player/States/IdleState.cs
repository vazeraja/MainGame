using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/IdleState")]
    public class IdleState : State<Player> {
        public IdleState(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}
        
        public override void OnEnter(Player player){
        }

        public override void LogicUpdate(Player player){

        }

        public override void OnExit(Player player){
        }

    }
}
