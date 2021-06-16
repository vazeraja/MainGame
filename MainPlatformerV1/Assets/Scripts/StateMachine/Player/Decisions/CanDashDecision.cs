using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDecision")]
    public class CanDashDecision : Decision<Player> {
        [SerializeField] private PlayerInputData playerInputData;

        public override bool Decide(Player player) => playerInputData.DashInput;
    }
}
