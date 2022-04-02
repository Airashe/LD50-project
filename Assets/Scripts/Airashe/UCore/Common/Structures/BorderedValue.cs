using System;
using UnityEngine;

namespace Airashe.UCore.Common.Structures
{
    /// <summary>
    /// Переменная, ограниченная верхей и нижний максимальной границей.
    /// </summary>
    [Serializable]
    public struct BorderedValue<T> where T : IComparable<T>
    {
        /// <summary>
        /// Текущее значение переменной.
        /// </summary>
        public T Value
        {
            get => currentValue;
            set
            {
                if (value.CompareTo(minimalValue) < 0)
                {
                    currentValue = loopValue ? maximumValue : minimalValue;
                    return;
                }
                if (value.CompareTo(maximumValue) > 0)
                {
                    currentValue = loopValue ? minimalValue : maximumValue;
                    return;
                }
                currentValue = value;
            }
        }

        /// <summary>
        /// Максимальное возможное значение переменной.
        /// </summary>
        public T Maximum
        {
            get => maximumValue;
            set
            {
                if (maximumValue.CompareTo(minimalValue) < 0)
                {
                    maximumValue = minimalValue;
                    return;
                }
                maximumValue = value;
            }
        }

        /// <summary>
        /// Минимально возможное значение переменной.
        /// </summary>
        public T Minimum
        {
            get => minimalValue;
            set
            {
                if (minimalValue.CompareTo(maximumValue) > 0)
                {
                    minimalValue = maximumValue;
                    return;
                }
                minimalValue = value;
            }
        }
        /// <summary>
        /// Текущее значение переменной.
        /// </summary>
        [SerializeField]
        private T currentValue;
        /// <summary>
        /// Минимально возможное значение переменной.
        /// </summary>
        [SerializeField]
        private T minimalValue;
        /// <summary>
        /// Максимально возможное значение переменной.
        /// </summary>
        [SerializeField]
        private T maximumValue;
        /// <summary>
        /// Параметр указывающий на то, что значение переменной зацикленно. 
        /// Т.е. при выход за минимальный порог, значение установится от максимального. 
        /// А при достижении максимального значения, порог установится от минимального.
        /// </summary>
        [SerializeField]
        private bool loopValue;

        /// <summary>
        /// Переменная, ограниченная верхей и нижний максимальной границей.
        /// </summary>
        /// <param name="currentValue">Значение, которым инициализируется переменная.</param>
        /// <param name="minimalValue">Минимальное возможное значение переменной.</param>
        /// <param name="maximumValue">Макимальное возможное значение переменной.</param>
        /// <param name="loopValue">Значение переменной зацикленно.</param>
        public BorderedValue(T currentValue, T minimalValue, T maximumValue, bool loopValue = false)
        {
            this.minimalValue = minimalValue;
            this.currentValue = currentValue;
            this.maximumValue = maximumValue;
            this.loopValue = loopValue;
        }

        public static implicit operator T(BorderedValue<T> borderedValue) => borderedValue.Value;
        public static explicit operator BorderedValue<T>(T value) => new BorderedValue<T>(value, value, value);
    }

}