using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(fileName = "New Void Event", menuName = "GameEvents/Void Event")]
    public class VoidEvent : BaseGameEvent<Void> {
        public void Raise() => Raise(new Void());

    }
}
