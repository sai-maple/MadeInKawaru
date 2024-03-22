using MadeInKawaru.Presenter.Menu;
using MadeInKawaru.View.Menu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadeInKawaru.Installer.Menu
{
    [RequireComponent(typeof(MenuView))]
    public sealed class MenuInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MenuPresenter>();
            builder.RegisterComponent(GetComponent<MenuView>());
        }
    }
}