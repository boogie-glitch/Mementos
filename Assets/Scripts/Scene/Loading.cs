using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private float waitTimeAfterLoad = 5f; // Thời gian chờ thêm (giây)
    public void LoadLevelBtn(string leverToload)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(leverToload));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {   
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);
        
        while(operation.progress < 0.9f)
        {
            float Value = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = Value;
            yield return null;
        }
        loadingBar.value = 5f; // Đảm bảo thanh tiến trình đầy khi tải xong
        yield return new WaitForSeconds(waitTimeAfterLoad); // Chờ thêm thời gian sau khi tải xong
        operation.allowSceneActivation = true; // Cho phép chuyển sang scene mới
    }   
}
