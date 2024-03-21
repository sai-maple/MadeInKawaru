using System;
using UnityEngine;

namespace MadeInKawaru.Entity.Game
{
    public sealed class StageEntity
    {
        public int Stage { get; private set; }
        public float Speed { get; private set; }
        public bool IsSpeedUp { get; private set; }

        public void Initialize()
        {
            Stage = 1;
            Speed = 1;
            IsSpeedUp = false;
        }

        public void OnClear(bool result)
        {
            if (result) Stage++;
            var speed = 1 + Mathf.FloorToInt(Stage / 5f) * 0.1f;
            IsSpeedUp = Math.Abs(Speed - speed) > 0.1f;
            Speed = speed;
        }
    }
}