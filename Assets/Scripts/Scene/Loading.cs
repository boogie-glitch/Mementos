using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private float waitTimeAfterLoad = 5f; // Thời gian chờ thêm (giây)
    [SerializeField] private float minLoadingTime = 2f; // Thời gian tối thiểu hiển thị màn hình loading
    [SerializeField] private float loadingBarMaxValue = 4.81f; // Giá trị tối đa mới cho slider

    public void LoadLevelBtn(string leverToload)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(leverToload));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        float timer = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);
        operation.allowSceneActivation = false;

        // Chạy slider từ 0 đến 0.9 trong minLoadingTime giây hoặc đến khi operation.progress đạt 0.9
        while (timer < minLoadingTime || operation.progress < 0.9f)
        {
            timer += Time.deltaTime;
            float timeProgress = Mathf.Clamp01(timer / minLoadingTime);
            float loadProgress = Mathf.Clamp01(operation.progress / 0.9f);
            float displayProgress = Mathf.Min(timeProgress, loadProgress);
            loadingBar.value = displayProgress * loadingBarMaxValue;
            yield return null;
        }

        // Đảm bảo thanh tiến trình đầy khi tải xong và đủ thời gian
        loadingBar.value = loadingBarMaxValue;
        yield return new WaitForSeconds(0.2f); // Cho hiệu ứng đầy thanh

        operation.allowSceneActivation = true;
    }
}
