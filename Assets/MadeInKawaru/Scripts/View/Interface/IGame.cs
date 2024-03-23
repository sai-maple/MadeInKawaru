using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MadeInKawaru.View.Interface
{
    public interface IGame
    {
        float Time => 5f;
        string Title { get; }
        IGame Create(Transform content);
        UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default);
        void Close();
    }
}