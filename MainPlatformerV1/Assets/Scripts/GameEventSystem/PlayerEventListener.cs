using UnityEngine;
using UnityEngine.Events;

namespace MainGame {

    [System.Serializable] public class UnityPlayerEvent : UnityEvent<MainPlayer> {}

    public class PlayerEventListener : GameEventListener<MainPlayer, PlayerEvent, UnityPlayerEvent> {}
}
