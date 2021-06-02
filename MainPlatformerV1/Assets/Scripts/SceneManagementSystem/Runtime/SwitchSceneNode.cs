using Aarthificial.Reanimation.Common;
using UnityEngine;


namespace MainGame {
    
    [CreateAssetMenu(fileName = "switch", menuName = "SceneSwitchSystem/Switch", order = 400)]
    public class SwitchSceneNode : SceneSystemNode {
        [SerializeField] private DriverDictionary drivers = new DriverDictionary();
    }
}
