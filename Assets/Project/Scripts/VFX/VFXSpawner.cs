using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _enemyDestroyedEffectPrefab;

    public void SpawnEffect(Vector3 position)
    {
        var vfx = Instantiate(_enemyDestroyedEffectPrefab, position, Quaternion.identity);
        var ps = vfx.GetComponent<ParticleSystem>();
        ps.Play();
        Destroy(vfx, ps.main.duration);
    }
}

