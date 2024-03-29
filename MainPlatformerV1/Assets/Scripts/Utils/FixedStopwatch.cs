﻿using System;
using UnityEngine;

namespace Utils {
    [Serializable]
    public class FixedStopwatch
    {
        public bool IsFinished => Elapsed > duration;
        public bool IsReady => Elapsed > cooldown;
        public float Completion => Mathf.Clamp01(Elapsed / duration);

        public float _timestamp;
        [SerializeField] private float duration;
        [SerializeField] private float cooldown;
        public float Elapsed => Time.fixedTime - _timestamp;

        public void Reset()
        {
            _timestamp = Time.fixedTime - cooldown - duration - 1;
        }

        public void Split()
        {
            _timestamp = Time.fixedTime;
        }
    }
}