using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [SerializeField] private Slider health;
        [SerializeField] private Text bulletsCount;

        private Player.Player player;

        private void Start()
        {
            player = FindObjectOfType<Player.Player>();
            health.maxValue = player.ReturnHealth();
            player.onPlayerHealth += ChangePlayerSlider;
            bulletsCount.text = " " + player.ReturnBulletCount();
            player.onBulletsCountChange += ChangeBulletsCount;
        }
        private void ChangePlayerSlider()
        {        
            health.value = player.ReturnHealth();

        }
        private void ChangeBulletsCount()
        {
            bulletsCount.text = " " + player.ReturnBulletCount();
        }
    }
}
