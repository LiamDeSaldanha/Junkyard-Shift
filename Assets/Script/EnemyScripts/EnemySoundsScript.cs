using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class EnemySoundsScript : MonoBehaviour
{
    public float minDelay = 1f;
    public float maxDelay = 3f;

    public bool onlyPlayWhenMoving = true;

    private AudioSource audioSource;
    private NavMeshAgent agent;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        // Ensure looping is OFF (we control it manually)
        audioSource.loop = false;

        // Optional variation so enemies sound different
        audioSource.pitch = Random.Range(0.95f, 1.05f);

        // Start with a random delay so enemies don’t sync
        StartCoroutine(PlayWithRandomDelay());
    }

    IEnumerator PlayWithRandomDelay()
    {
        // Initial random delay (important!)
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        while (true)
        {
            if (!onlyPlayWhenMoving || IsMoving())
            {
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }

            // Wait random delay before next play
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    bool IsMoving()
    {
        if (agent == null) return true; // fallback if no NavMeshAgent
        return agent.velocity.magnitude > 0.1f;
    }
}