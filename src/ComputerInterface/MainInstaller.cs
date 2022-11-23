using ComputerInterface.Interfaces;
using Zenject;

namespace BalloonFloater.ComputerInterface
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            // Bind your mod entry like this
            Container.Bind<IComputerModEntry>().To<BFEntry>().AsSingle();
        }
    }
}