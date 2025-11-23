using UnityEngine;
using UnityEngine.SceneManagement;
using Work.JYG.Code;

public class RetryNow : MonoBehaviour
{
    public void RetryRightNow()
    {
        StatManager.Instance.ResetDatas();
        SaveManager.Instance.DeleteSave();
        SceneManager.LoadScene(0);
    }
}
