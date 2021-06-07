using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDecision")]
    public class CanDashDecision : Decision<Player> {
        public CanDashDecision(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}

        public override bool Decide(Player player) => playerInputData.DashInput;
    }
}
