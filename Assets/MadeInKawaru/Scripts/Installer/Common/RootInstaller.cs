using MadeInKawaru.Entity.Common;
using MadeInKawaru.Entity.Game;
using VContainer;
using VContainer.Unity;

namespace MadeInKawaru.Installer.Common
{
    public sealed class RootInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PhaseEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LifeEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<StageEntity>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}