using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Image gamePanel;
    
    private Player.Player player;

    private void Start()
    {       
        player = FindObjectOfType<Player.Player>();
        player.onPlayerDeath += RestartLevelPanel;
    }
   
    private void RestartLevelPanel()
    {
        gamePanel.gameObject.SetActive(true);
    }
    
}
