using System.Collections;

using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    
    public float dissolveSpeed = 0.25f;
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private Renderer[] _renderers;
    public Material[] _material;
    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
           _material = new Material[_renderers[0].materials.Length];
        

        
        for (int i=0;i< _material.Length;i++)
        {
            _material[i] = _renderers[0].materials[i];
            _material[i].SetFloat(DissolveAmount, 0f);
        }
    }

    public void StartDissolveOut()
    {
        StartCoroutine(DissolveOut());
    }

    public void StartDissolveIn()
    {
        StartCoroutine(DissolveIn());
    }

    public IEnumerator DissolveOut()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * dissolveSpeed;

            // Lerp smoothly from 0 (visible) to 1 (dissolved)
            float threshold = Mathf.Lerp(0f, 1.1f, time);

            for (int i = 0; i < _material.Length; i++)
            {
                _material[i].SetFloat(DissolveAmount, threshold);
            }
            yield return null;
        }

        // Make sure it ends exactly at 1
        for (int i = 0; i < _material.Length; i++)
        {
            _material[i].SetFloat(DissolveAmount, 1.1f);
        }

        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }

    public IEnumerator DissolveIn()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * dissolveSpeed;

            // Lerp smoothly from 1 (dissolved) to 0 (visible)
            float threshold = Mathf.Lerp(1.1f, 0f, time);
            for (int i = 0; i < _material.Length; i++)
            {
                _material[i].SetFloat(DissolveAmount, threshold);
            }
            yield return null;
        }

        // Make sure it ends exactly at 0
        for (int i = 0; i < _material.Length; i++)
        {
            _material[i].SetFloat(DissolveAmount, 0f);
        }
    }
}