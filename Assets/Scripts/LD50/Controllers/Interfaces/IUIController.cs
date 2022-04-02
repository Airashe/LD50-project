using Assets.Scripts.LD50.Interact.Items;
using LD50.Controllers.Interfaces;
using LD50.Interact.Items;

namespace LD50.Controllers
{
    public interface IUIController
    {
        public ILogicController LogicController { get; }
        public void DragDropBeginInvoke(Item item);
    }
}
