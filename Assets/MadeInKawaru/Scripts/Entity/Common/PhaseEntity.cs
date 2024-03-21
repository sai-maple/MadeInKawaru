using System;
using MadeInKawaru.Enums;
using UniRx;

namespace MadeInKawaru.Entity.Common
{
    public sealed class PhaseEntity : IDisposable
    {
        private readonly ReactiveProperty<Phase> _phase = new();

        public IObservable<Phase> OnPhaseChangedAsObservable()
        {
            return _phase;
        }
        
        public void OnNext(Phase phase)
        {
            _phase.Value = phase;
        }
        
        public void Dispose()
        {
            _phase?.Dispose();
        }
    }
}