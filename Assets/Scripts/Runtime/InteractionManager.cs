using UnityEngine;

namespace Tetras
{
    public class InteractionManager : MonoBehaviour
    {
        private enum InteractionType
        {
            None,
            DroppingLeaves,
            DroppingBakingSoda
        }

        public static InteractionManager Instance;

        private InteractionType _currentInteractionMode;

        [SerializeField] private GameObject _touchObstaclePrefab;
        private GameObject _touchObstacleRef;

        [SerializeField] private GameObject _leavesTemplate;
        [SerializeField] private GameObject _bakingSodaTemplate;

        private Pool<DroppableItem> _leavesPool = new Pool<DroppableItem>();
        private Pool<DroppableItem> _bakingSodaPool = new Pool<DroppableItem>();

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
        }

        private void Start()
        {
            _leavesPool.Initialize(_leavesTemplate, transform);
            _bakingSodaPool.Initialize(_bakingSodaTemplate,transform);
        }

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
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 20f;

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                if (_touchObstacleRef != null)
                {
                    _touchObstacleRef.SetActive(false);
                }

                if(_currentInteractionMode == InteractionType.DroppingLeaves)
                {
                    var leaves = _leavesPool.Get();
                    leaves.transform.position = worldPos;
                    leaves.OnReachedEndOfLifespan = () =>
                    {
                        _leavesPool.Return(leaves);
                    };
                    EnvironmentManager.Instance.UpdatePH(EnvironmentManager.Instance.PH - .5f);
                }

                if (_currentInteractionMode == InteractionType.DroppingBakingSoda)
                {
                    var bakingSoda = _bakingSodaPool.Get();
                    bakingSoda.transform.position = worldPos;
                    bakingSoda.OnReachedEndOfLifespan = () =>
                    {
                        _bakingSodaPool.Return(bakingSoda);
                    };
                    EnvironmentManager.Instance.UpdatePH(EnvironmentManager.Instance.PH + .5f);
                }
            }
        }

        public void BeginLeavesInteraction()
        {
            _currentInteractionMode = InteractionType.DroppingLeaves;
        }

        public void BeginBakingSodaInteraction()
        {
            _currentInteractionMode = InteractionType.DroppingBakingSoda;
        }

        public void EndInteraction()
        {
            _currentInteractionMode = InteractionType.None;
        }
    }
}