using UnityEngine;
using UnityEngine.Events;

namespace MainGame {

    [System.Serializable] public struct Void {}
    [System.Serializable] public class UnityVoidEvent : UnityEvent<Void> {}

    public class VoidListener : GameEventListener<Void, VoidEvent, UnityVoidEvent> {}

}
