using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace MadeInKawaru.Extensions
{
    public static class PlayableDirectorExtension
    {
        public static async UniTask PlayAsync(this PlayableDirector self, CancellationToken token = default)
        {
            self.Play();
            await UniTask.WaitWhile(() => self.state == PlayState.Playing, cancellationToken: token);
            if (token.IsCancellationRequested) self.Stop();
        }

        public static async UniTask PlayWithSpeedAsync(this PlayableDirector self, float speed = 1,
            CancellationToken token = default)
        {
            self.Play();
            self.playableGraph.GetRootPlayable(0).SetSpeed(speed);
            await UniTask.WaitWhile(() => self.state == PlayState.Playing, cancellationToken: token);
            if (token.IsCancellationRequested) self.Stop();
        }
    }
}