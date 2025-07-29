using UnityEngine;

namespace Project.Scripts.VFX
{
    public class VFXSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _enemyDestroyedEffectPrefab;

        public void SpawnEffect(Vector3 position)
        {
            var ps = Instantiate(_enemyDestroyedEffectPrefab, position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration);
        }
    }
}

