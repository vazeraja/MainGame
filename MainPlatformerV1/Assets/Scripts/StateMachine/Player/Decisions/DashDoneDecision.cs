using UnityEngine;
namespace MainGame {
    
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDoneDecision")]
    public class DashDoneDecision : Decision<Player> {

        [SerializeField] private DashState _dashState = null;
        
        public override bool Decide(Player player) => _dashState.isAbilityDone;
    }
}
