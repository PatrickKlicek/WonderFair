using System.Collections;
using System.Linq;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    public MoleController[] moles;

    [Header("Difficulty")]
    public float minInterval   = 0.5f;
    public float maxInterval   = 1f;
    public float minActiveTime = 1.0f;
    public float maxActiveTime = 2f;
    public int   maxMolesUp    = 1;

    private bool _running;

    public void StartGame() { _running = true;  StartCoroutine(SpawnLoop()); }
    public void StopGame()  { _running = false; StopAllCoroutines(); }

    IEnumerator SpawnLoop()
    {
        while (_running)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            var available = moles.Where(m => m != null && !m.IsUp && !m.IsWhacked).ToList();
            int currentUp = moles.Count(m => m != null && m.IsUp);

            if (available.Count > 0 && currentUp < maxMolesUp)
            {
                var chosen = available[Random.Range(0, available.Count)];
                chosen.Activate(Random.Range(minActiveTime, maxActiveTime));
            }
        }
    }
}