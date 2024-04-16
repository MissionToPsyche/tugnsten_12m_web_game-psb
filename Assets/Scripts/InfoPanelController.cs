using UnityEngine;
using UnityEngine.UIElements;

public class InfoPanelController : MonoBehaviour
{
    public VisualElement root;
    public ScrollView infoScrollView;

    public void ShowInfo(string minigameTitle)
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

    public UQueryBuilder<Label> GetAllTabs()
    {
        return root.Query<Label>(className: "tab");
    }

    public void UnselectTab(Label tab)
    {
        tab.RemoveFromClassList("selectedTab");
        Debug.Log("Unselected tab");
    }
}