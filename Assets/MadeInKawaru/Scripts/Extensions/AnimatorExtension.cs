using UnityEngine;

namespace MadeInKawaru.Extensions
{
    public static class AnimatorExtension
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        public static void Speed(this Animator self, float speed)
        {
            self.SetFloat(SpeedHash, speed);
        }
    }
}