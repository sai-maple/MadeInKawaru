using System.Collections.Generic;
using System.Linq;
using MadeInKawaru.Presenter.Game;
using MadeInKawaru.View.Game;
using MadeInKawaru.View.Interface;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MadeInKawaru.Installer.GameMenu
{
    public sealed class GameMenuInstaller : LifetimeScope
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameMenuView _gameMenuView;
        [SerializeField] private GameTimerView _timerView;
        [SerializeField] private ResultView _resultView;
        [SerializeField] private List<GameObject> _games;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GamePresenter>();
            builder.RegisterComponent(_canvasGroup);
            builder.RegisterComponent(_gameMenuView);
            builder.RegisterComponent(_timerView);
            builder.RegisterComponent(_resultView);
            builder.RegisterComponent(_games.Select(obj => obj.GetComponent<IGame>()).ToList());
        }
    }
}