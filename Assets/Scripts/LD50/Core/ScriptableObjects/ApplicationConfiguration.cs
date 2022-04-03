using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LD50.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "LD50/Configuration/Applicaton Configuration", order = 1)]
    public class ApplicationConfiguration : ScriptableObject
    {
        [Header("Drag drop UI settings")]
        public Vector2 dragDropIconSize;
    }
}
