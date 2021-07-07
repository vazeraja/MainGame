using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderNut.SceneManagement {

    [CreateAssetMenu(fileName = "ScenePassage", menuName = "World Graph/Scene Passage")]
    public class ScenePassage : ScriptableObject {

        public PassageElement passage;

    }
}