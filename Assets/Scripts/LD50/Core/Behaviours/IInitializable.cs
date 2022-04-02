using UnityEngine;

namespace LD50.Core
{
    public interface IInitializable
    {
        public void Initialize();
        public bool IsInitialized { get; }
    }
}
