using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDecision")]
    public class CanDashDecision : Decision<MainPlayer> {
        public CanDashDecision(PlayerInputData playerInputData) : base(playerInputData){}

        public override bool Decide(MainPlayer mainPlayer) => playerInputData.DashInput;
    }
}
