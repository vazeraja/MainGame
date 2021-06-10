using UnityEngine;

namespace MainGame {
    
    [CreateAssetMenu(fileName = "New Void Event Listener", menuName = "GameEvents/Event Listeners/Void Event Listener")]
    public class VoidListenerSO : GameEventListenerSO<Void, VoidEvent, UnityVoidEvent> { }
}