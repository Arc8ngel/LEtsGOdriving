using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreLoader : MonoBehaviour
{

    [SerializeField]
    private GameObject loadingOverlay_obj;

    [SerializeField]
    private UnityEngine.UI.Text loading_Percent_txt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        loadingOverlay_obj?.SetActive(true);
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Main");
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while( !asyncOperation.isDone )
        {
            //Output the current progress
            loading_Percent_txt.text = $"Loading...{(asyncOperation.progress * 100).ToString("0#")}%";

            // Check if the load has finished
            asyncOperation.allowSceneActivation = (asyncOperation.progress >= 0.9f);
            
            yield return null;
        }

        loadingOverlay_obj?.SetActive(false);
    }
}
