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
    public bool playStartDialogue;

    private void Awake()
    {
        playStartDialogue = true;
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
        if (levelIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            levelIndex = 0;
        } else
        {
            levelIndex++;
        }
        playStartDialogue = true;
        StartCoroutine(LoadSpiral());
    }

    public void ReloadScene()
    {
        playStartDialogue = false;
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
            loadingVFX.material.SetFloat("SpiralPower_", Mathf.SmoothStep(10f, 0.0f, elapsed / spiralEaseTime));
            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("eased in");
        AsyncOperation load = SceneManager.LoadSceneAsync(levelIndex);
        load.allowSceneActivation = false;
        elapsed = 0.0f;
        while (elapsed <= loadWaitTimeMin && !load.isDone)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        load.allowSceneActivation = true;

        elapsed = 0.0f;
        while (elapsed <= spiralEaseTime)
        {
            loadingVFX.material.SetFloat("SpiralSpeed_", Mathf.SmoothStep(0.75f, 0.5f, elapsed / spiralEaseTime));
            loadingVFX.material.SetFloat("SpiralPower_", Mathf.SmoothStep(0.0f, 10f, elapsed / spiralEaseTime));
            elapsed += Time.deltaTime;
            yield return null;
        }
        loadingVFX.enabled = false;
    }

}
