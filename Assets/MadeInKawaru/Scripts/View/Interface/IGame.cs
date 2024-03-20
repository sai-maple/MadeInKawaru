using System.Threading;
using Cysharp.Threading.Tasks;

namespace MadeInKawaru.View.Interface
{
    public interface IGame
    {
        UniTask<bool> PlayAsync(float time, float speed, int level, CancellationToken token = default);
    }
}