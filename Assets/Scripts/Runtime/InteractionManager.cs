using UnityEngine;

namespace Tetras
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private GameObject _touchObstaclePrefab;
        private GameObject _touchObstacleRef;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10f;

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                if (_touchObstacleRef != null)
                {
                    _touchObstacleRef.SetActive(true);
                    _touchObstacleRef.transform.position = worldPos;
                }
                else
                {
                    GameObject go = Instantiate(_touchObstaclePrefab, worldPos, Quaternion.identity);
                    _touchObstacleRef = go;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_touchObstacleRef != null)
                {
                    _touchObstacleRef.SetActive(false);
                }
            }
        }

    }
}