using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDoneDecision")]
    public class DashDoneDecision : Decision<Player> {
        public DashDoneDecision(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}

        [SerializeField] private DashState _dashState = null;

        public override bool Decide(Player player){
            return _dashState.isAbilityDone;
        }
    }
}
