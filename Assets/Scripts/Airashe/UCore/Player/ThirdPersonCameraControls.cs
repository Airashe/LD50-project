using Airashe.UCore.Common.Structures;
using Airashe.UCore.Common.Extensions;
using UnityEngine;

namespace Airashe.UCore.Player
{
    /// <summary>
    /// Управление камерой - третье лицо.
    /// </summary>
    public class ThirdPersonCameraControls : MonoBehaviour
    {
        /// <summary>
        /// Целевые сферические углы - игрок контролирует это значение, чтобы поворачивать 
        /// камеру вокруг объекта наблюдения.
        /// </summary>
        public Vector2 SphericalAngles
        {
            get => new Vector2(targetSphericalAngleX, targetSphericalAngleY);
            set
            {
                targetSphericalAngleX.Value = value.x;
                targetSphericalAngleY.Value = value.y;
            }
        }

        ///<summary>
        /// Целевое отдаление камеры от точки наблюдения - игрок контролирует это значение, чтобы 
        /// приближать и отдалять камеру.
        ///</summary>
        public float Distance
        {
            get => targetDistance;
            set => targetDistance.Value = value;
        }

        /// <summary>
        /// Целевой объект, за которым наблюдает камера.
        /// </summary>
        [Header("Target")]
        [SerializeField]
        private GameObject targetObject;
        /// <summary>
        /// Point that camera looking at.
        /// </summary>
        [SerializeField]
        private Vector3 worldTargetPoint;

        /// <summary>
        /// Целевые сферические углы (значение X) - игрок контролирует это значение, чтобы поворачивать 
        /// камеру вокруг объекта наблюдения.
        /// </summary>
        [Header("Position")]
        [SerializeField]
        private BorderedValue<float> targetSphericalAngleX;
        /// <summary>
        /// Целевые сферические углы (значение Y) - игрок контролирует это значение, чтобы поворачивать 
        /// камеру вокруг объекта наблюдения.
        /// </summary>
        [SerializeField]
        private BorderedValue<float> targetSphericalAngleY;
        /// <summary>
        /// Current sphere angles wich used for camera position calculation.
        /// </summary>
        private Vector2 currentSphericalAngles;

        ///<summary>
        /// Целевое отдаление камеры от точки наблюдения - игрок контролирует это значение, чтобы 
        /// приближать и отдалять камеру.
        ///</summary>
        [Header("Distances")]
        [SerializeField]
        private BorderedValue<float> targetDistance;
        /// <summary>
        /// Максимально возможное отдаление камеры от точки наблюдения (если нет никаких препятствий).
        /// </summary>
        [SerializeField]
        private float defaultMaximumDistance;
        /// <summary>
        /// Текущее отдаление камеры от точки наблюдения.
        /// </summary>
        private float currentDistance;
        /// <summary>
        /// Дистанция, на которую осуществяется проверка прохождения 
        /// сквозь стены.
        /// </summary>
        [SerializeField]
        private float сameraClippingDistance = 1.5f;

        /// <summary>
        /// Скорость камеры, вокруг точки наблюдения.
        /// </summary>
        [Header("Values change speed")]
        [SerializeField]
        private float sphericalAngelsChangeSpeed;
        /// <summary>
        /// Скорость приближения / отдаления камеры.
        /// </summary>
        [SerializeField]
        private float distanceChangeSpeed;

        /// <summary>
        /// Скорость вращения камеры, вокруг точки наблюдения 
        /// при считывании ввода игрока.
        /// </summary>
        [Header("Player controls speed")]
        [SerializeField]
        private float cameraRotationSpeed;
        /// <summary>
        /// Скорость приближения / отдаления камеры при считывании 
        /// ввода игрока.
        /// </summary>
        [SerializeField]
        private float cameraZoomSpeed;

        /// <summary>
        /// Флаг обозначающий то, что игрок в данный момент контролирует поворот камеры.
        /// </summary>
        [Header("Player input")]
        private bool rotationByPlayer;

        private void Update()
        {
            CalculateCurrentMaximumDistance();
            ShiftCurrentSphericalAnglesToTarget();
            ShiftCurrentDistanceToTarget();
            ReadAndApplyPlayerInput();

            Vector3 targetPosition = GetTargetPosition();
            Vector3 localCameraPosition = GetLocalPositionFromShericalAngles();
            ChangeCameraPositionAndRotation(localCameraPosition, targetPosition);
        }

        private void OnDrawGizmos()
        {
            // Сфера, отображающая дистанцию, которую задает игрок.
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(worldTargetPoint, targetDistance);
            // Сфера, с текущей дистанцией камеры.
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(worldTargetPoint, currentDistance);
            // Сфера максимальной дистанции камеры.
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(worldTargetPoint, targetDistance.Maximum);
            // Отрисовать луч, который ограничивает максимальную дистанцию камеры.
            Gizmos.DrawRay(worldTargetPoint, (transform.forward * -1).normalized * (targetDistance.Value + сameraClippingDistance));
        }

        /// <summary>
        /// Высчитать текущую максимальную дистанцию камеры. 
        /// Позволяет избежать застревания камеры внутри объектов уровня.
        /// </summary>
        private void CalculateCurrentMaximumDistance()
        {
            Vector3 backward = (transform.forward * -1).normalized;
            Vector3 startCheckPosition = worldTargetPoint;
            float checkDistance = targetDistance.Value + сameraClippingDistance;
            Ray backwardRay = new Ray(startCheckPosition, backward);

            if (Physics.Raycast(backwardRay, out var hitInfo, checkDistance))
            {
                if (targetDistance.Maximum > targetDistance.Minimum)
                    targetDistance.Maximum -= Mathf.Abs(Vector3.Distance(startCheckPosition, hitInfo.point) - checkDistance);
                else
                    targetDistance.Maximum = targetDistance.Minimum;
            }
            else
            {
                targetDistance.Maximum = defaultMaximumDistance;
            }
        }

        /// <summary>
        /// Плавное изменение текущего значения сферических углов (<see cref="currentSphericalAngles"/>), в сторону 
        /// значения которое задает игрок (<see cref="SphericalAngles"/>).
        /// </summary>
        private void ShiftCurrentSphericalAnglesToTarget()
        {
            Vector2 delta = Vector2.zero;
            delta.x = FloatExtensions.GetDelta(currentSphericalAngles.x, SphericalAngles.x, sphericalAngelsChangeSpeed);
            delta.y = FloatExtensions.GetDelta(currentSphericalAngles.y, SphericalAngles.y, sphericalAngelsChangeSpeed);
            currentSphericalAngles += delta;
        }

        /// <summary>
        /// Плавное изменение текущего отдаления камеры (<see cref="currentDistance"/>), в сторону 
        /// значения которое задает игрок (<see cref="targetDistance"/>).
        /// </summary>
        private void ShiftCurrentDistanceToTarget()
        {
            float delta = FloatExtensions.GetDelta(currentDistance, targetDistance, distanceChangeSpeed);
            currentDistance += delta;
        }

        /// <summary>
        /// Прочитать текущий ввод игрока и применить его к свойствам камеры.
        /// </summary>
        private void ReadAndApplyPlayerInput()
        {
            rotationByPlayer = Input.GetKey(KeyCode.Mouse2);
            if (rotationByPlayer)
            {
                var newSphericalAnglesX = SphericalAngles.x - Input.GetAxis("Mouse X") * Time.deltaTime * cameraRotationSpeed;
                var newSphericalAnglesY = SphericalAngles.y - Input.GetAxis("Mouse Y") * Time.deltaTime * cameraRotationSpeed;
                SphericalAngles = new Vector2(newSphericalAnglesX, newSphericalAnglesY);
            }
            Distance -= Input.mouseScrollDelta.y * Time.deltaTime * cameraZoomSpeed;
        }

        /// <summary>
        /// Получить глобальные координаты точки, за которой наблюдает камера.
        /// </summary>
        /// <returns>
        /// Возвращает глобальные координаты центра сферы.
        /// </returns>
        private Vector3 GetTargetPosition()
        {
            if (targetObject != null)
                worldTargetPoint = targetObject.transform.position;

            return worldTargetPoint;
        }

        /// <summary>
        /// Получить локальные декардовые координаты камеры из начений 
        /// сферических углов камеры.
        /// </summary>
        /// <returns>
        /// Локальные координаты позиции камеры на сфере.
        /// </returns>
        private Vector3 GetLocalPositionFromShericalAngles()
        {
            Vector3 localPosition = Vector3.zero;

            localPosition.x = Mathf.Cos(currentSphericalAngles.x) * Mathf.Cos(currentSphericalAngles.y) * currentDistance;
            localPosition.z = Mathf.Sin(currentSphericalAngles.x) * Mathf.Cos(currentSphericalAngles.y) * currentDistance;
            localPosition.y = Mathf.Sin(currentSphericalAngles.y) * currentDistance;

            return localPosition;
        }

        /// <summary>
        /// Установить позицию камеры на сфере.
        /// </summary>
        /// <param name="newLocalPosition">Локальные координаты камеры, на сфере.</param>
        /// <param name="targetWorldPosition">Глобальные координаты центра сферы.</param>
        private void ChangeCameraPositionAndRotation(Vector3 newLocalPosition, Vector3 targetWorldPosition)
        {
            Vector3 worldCameraPosition = targetWorldPosition + newLocalPosition;
            transform.position = worldCameraPosition;
            transform.LookAt(worldTargetPoint);
        }
    }
}