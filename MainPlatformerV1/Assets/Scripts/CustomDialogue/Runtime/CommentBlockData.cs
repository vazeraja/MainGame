﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.DialogueGraph {

    [Serializable]
    public class CommentBlockData {
        public List<string> ChildNodes = new List<string>();
        public Vector2 Position;
        public string Title = "Comment Block";
    }
}
