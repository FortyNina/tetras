using System.Collections;
using UnityEngine;

namespace Tetras
{
    /// <summary>
    /// Item that can be dropped into the environment
    /// </summary>
    public class DroppableItem : MonoBehaviour
    {
        [SerializeField] private float lifespan = 5f;
        [SerializeField] private bool destroyAtEndOfLifespan = false;

        public System.Action OnReachedEndOfLifespan;

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(CorSimulateLifespan());
        }

        private IEnumerator CorSimulateLifespan()
        {
            yield return new WaitForSeconds(lifespan);
            OnReachedEndOfLifespan?.Invoke();
            if (destroyAtEndOfLifespan) Destroy(gameObject);
        }
    }
}