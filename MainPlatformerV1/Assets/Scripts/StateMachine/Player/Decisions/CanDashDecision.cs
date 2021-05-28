using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDecision")]
    public class CanDashDecision : Decision<MainPlayer> {

        public override bool Decide(MainPlayer mainPlayer){
            return mainPlayer.DashInput;
        }
    }
}
