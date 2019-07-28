using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadSceneDemo1()
    {
        SceneManager.LoadScene("SparklesDemo1");
    }
    public void LoadSceneDemo2()
    {
        SceneManager.LoadScene("SparklesDemo2");
    }
    public void LoadSceneDemo3()
    {
        SceneManager.LoadScene("SparklesDemo3");
    }
    public void LoadSceneDemo4()
    {
        SceneManager.LoadScene("SparklesDemo4");
    }
}