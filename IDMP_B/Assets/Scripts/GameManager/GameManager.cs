using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int levelIndex;
    public float spiralEaseTime;
    public float loadWaitTimeMin;
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
        StartCoroutine(LoadSpiral());
    }

    public void ReloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    IEnumerator LoadSpiral()
    {
        float elapsed = 0.0f;
        loadingVFX.enabled = true;
        while (elapsed <= spiralEaseTime)
        {
            Debug.Log("smooth step = " + Mathf.SmoothStep(0.5f, 0.75f, elapsed / spiralEaseTime));
            loadingVFX.material.SetFloat("SpiralSpeed_", Mathf.SmoothStep(0.5f, 0.75f, elapsed / spiralEaseTime));
            loadingVFX.material.SetFloat("SpiralPower_", Mathf.SmoothStep(15f, 0.5f, elapsed / spiralEaseTime));
            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("eased in");

        loadingVFX.material.SetFloat("MaskPower_", 0f);
        SceneManager.LoadSceneAsync(levelIndex);
        elapsed = 0.0f;
        while (elapsed <= loadWaitTimeMin && !SceneManager.GetSceneByBuildIndex(levelIndex).isLoaded)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        loadingVFX.material.SetFloat("MaskPower_", 1.5f);

        elapsed = 0.0f;
        while (elapsed <= spiralEaseTime)
        {
            loadingVFX.material.SetFloat("SpiralSpeed_", Mathf.SmoothStep(0.75f, 0.5f, elapsed / spiralEaseTime));
            loadingVFX.material.SetFloat("SpiralPower_", Mathf.SmoothStep(0.5f, 15f, elapsed / spiralEaseTime));
            elapsed += Time.deltaTime;
            yield return null;
        }
        loadingVFX.enabled = false;
    }

}
