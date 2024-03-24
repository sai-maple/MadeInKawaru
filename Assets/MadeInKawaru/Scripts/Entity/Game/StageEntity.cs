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
            var speed = 1 + Mathf.FloorToInt(Stage / 3f) * 0.1f;
            speed = Mathf.Min(2f, speed);
            IsSpeedUp = Math.Abs(Speed - speed) > 0.05f;
            Speed = speed;
        }
    }
}