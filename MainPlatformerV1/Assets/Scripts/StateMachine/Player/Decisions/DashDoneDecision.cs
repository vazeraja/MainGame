using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDoneDecision")]
    public class DashDoneDecision : Decision<MainPlayer> {
        public DashDoneDecision(PlayerInputData playerInputData) : base(playerInputData){}

        [SerializeField] private DashState _dashState = null;

        public override bool Decide(MainPlayer mainPlayer){
            return _dashState.isAbilityDone;
        }
    }
}
