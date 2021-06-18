using UnityEngine;

namespace MainGame {
    
    [CreateAssetMenu(fileName = "EnterHitStateDecision", menuName = "PluggableAI/Decisions/Sasuke/EnterHitStateDecision")]
    public class EnterHitStateDecision : Decision<SasukeController> {
        public override bool Decide(SasukeController sasuke) {
            return sasuke.State == SasukeState.Hit;
        }
    }
}