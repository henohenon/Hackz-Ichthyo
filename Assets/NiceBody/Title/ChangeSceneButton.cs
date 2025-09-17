using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Title
{
    public sealed class ChangeSceneButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string sceneToLoad = "GameScene";



        public void OnPointerClick(PointerEventData eventData)
        {
            // クリックされたらシーンを切り替える
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}