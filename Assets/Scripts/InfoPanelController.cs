using UnityEngine;
using UnityEngine.UIElements;

public class InfoPanelController : MonoBehaviour
{
    public VisualElement root;
    public ScrollView infoScrollView;
    public string minigameTitle;

    public void ShowInfo()
    {
        string infoUxmlPath = $"UI/UXML/{minigameTitle}Info";
        VisualTreeAsset gameInfoTree = Resources.Load<VisualTreeAsset>(infoUxmlPath);

        if (gameInfoTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInfoTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
    }

    public void ShowContext()
    {
        string infoUxmlPath = $"UI/UXML/{minigameTitle}Context";
        VisualTreeAsset gameInfoTree = Resources.Load<VisualTreeAsset>(infoUxmlPath);


        if (gameInfoTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInfoTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
        else
        {
            Debug.Log($"{minigameTitle}Context.uxml file not found.");
        }
    }

    public UQueryBuilder<Label> GetAllTabs()
    {
        return root.Query<Label>(className: "tab");
    }

        private void SelectTab(Label tab)
    {
        tab.AddToClassList("selectedTab");
        if(tab.name == "Instructions")
        {
            ShowInfo();
        }
        else if(tab.name == "science-context")
        {
            ShowContext();
        }
    }

    public void UnselectTab(Label tab)
    {
        tab.RemoveFromClassList("selectedTab");
        Debug.Log("Unselected tab");
    }
}