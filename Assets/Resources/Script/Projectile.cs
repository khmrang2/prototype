using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnDestroy()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.NotifyProjectileDestroyed();
        }
    }
}
