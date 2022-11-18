using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] int startPage = 0;
    [SerializeField] GameObject[] pages = null;

    int activePage;

    private void Awake()
    {
        activePage = startPage;
        DisableAll();
        SwitchPage(startPage);
    }
    
    public void SwitchPage(int page)
    {
        if (page < 0 || page >= pages.Length)
            return;

        pages[activePage].SetActive(false);
        pages[page].SetActive(true);
        activePage = page;
    }

    void DisableAll()
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(false);
    }
}
