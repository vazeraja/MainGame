using System;
using UnityEngine;
namespace MainGame {

    public class UISpawner : MonoBehaviour {
        public GameObject[] prefabs;

        private void Awake(){
            foreach (var prefab in prefabs) {
                Instantiate(prefab, transform);
            }
        }
    }
}
