using MadeInKawaru.Presenter.Menu;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MadeInKawaru.Installer.Menu
{
    public sealed class OptionInstaller : LifetimeScope
    {
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<OptionPresenter>();
            builder.RegisterComponent(_button);
            builder.RegisterComponent(_canvasGroup);
        }
    }
}