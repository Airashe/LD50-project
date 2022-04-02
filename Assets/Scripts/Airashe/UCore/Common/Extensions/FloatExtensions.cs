using System;
using UnityEngine;

namespace Airashe.UCore.Common.Extensions
{
    /// <summary>
    /// Класс, содержащий методы расширения для типа <see cref="float"/>
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Отсекаемое значение, при сравнении <see cref="float"/>.
        /// </summary>
        private const float Treshold = 0.1f;

        /// <summary>
        /// Получить дельту изменения <paramref name="current"/> в сторону <paramref name="target"/> с скоростью <paramref name="changeSpeed"/>, 
        /// за время прошедшее с прошлого кадра.
        /// </summary>
        /// <param name="current">
        /// Текущее значение переменной.
        /// </param>
        /// <param name="target">
        /// Целевое значение переменной.
        /// </param>
        /// <param name="changeSpeed">
        /// Скорость изменения переменной.
        /// </param>
        /// <param name="treshold">
        /// (<i>Опционально</i>)
        /// Пороговое значение разницы, между <paramref name="current"/> и <paramref name="target"/>, чтобы 
        /// метод считал, что дельта должна быть высчитана.
        /// </param>
        /// <returns>
        /// Возвращает дельту-велечину, на которую изменится <paramref name="current"/> в сторону 
        /// <paramref name="target"/>, за время прошедшее, с прошлого кадра.
        /// </returns>
        public static float GetDelta(float current, float target, float changeSpeed, float treshold = Treshold)
        {
            var needChange = Mathf.Abs(current - target) > treshold;
            if (!needChange)
                return 0;

            return (target - current) * (Time.deltaTime * changeSpeed);
        }
    }
}