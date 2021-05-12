using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDecision")]
    public class CanDashDecision : Decision<Player> {

        public override bool Decide(Player player) => player.DashInput;
    }
}
