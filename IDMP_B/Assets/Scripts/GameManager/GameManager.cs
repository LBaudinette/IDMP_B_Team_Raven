using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int levelIndex;
    public int spiralEaseTime;
    private RawImage loadingVFX;

    private void Awake()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        DontDestroyOnLoad(this.gameObject);
        loadingVFX = GetComponentInChildren<RawImage>();
        loadingVFX.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        levelIndex++;
        SceneManager.LoadSceneAsync(levelIndex);
    }

    public void ReloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    IEnumerator SpiralIn()
    {
        float elapsed = 0.0f;
        
        while (elapsed <= spiralEaseTime)
        {
            loadingVFX.material.SetFloat("SpiralSpeed_", Mathf.SmoothStep(0.5f, 0.75f, elapsed / spiralEaseTime));
            loadingVFX.material.SetFloat("SpiralPower_", Mathf.SmoothStep(15f, 0.001f, elapsed / spiralEaseTime));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;
        //while (elapsed <= spiral)
    }

    IEnumerator SpiralOut()
    {
        float elapsed = 0.0f;

        while (elapsed <= spiralEaseTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
