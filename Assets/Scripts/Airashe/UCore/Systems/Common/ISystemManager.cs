using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airashe.UCore.Systems
{
    /// <summary>
    /// Интерфейс менеджера системы.
    /// </summary>
    public interface ISystemManager
    {
        /// <summary>
        /// Инициализирован ли менеджер.
        /// </summary>
        public bool Initialized { get; }
        /// <summary>
        /// Инициализировать менеджер системы.
        /// </summary>
        public void InitializeManager();
    }
}
