using UnityEngine;

namespace MainGame {
    
    [CreateAssetMenu(fileName = "New Player Event Listener", menuName = "GameEvents/Event Listeners/Player Event Listener")]
    public class PlayerEventListenerSO : GameEventListenerSO<MainPlayer, PlayerEvent, UnityPlayerEvent> {}
    
}
