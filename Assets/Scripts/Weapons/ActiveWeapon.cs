using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }
    public float attackAnimationDuration = 0.4f;

    [SerializeField] private Sword sword;
    //[SerializeField] private Knife knife;
    private void Awake()
    {
        Instance = this;
    }
    public  Sword GetActiveWeapon()
    {
        return sword;
    }
    public void Update()
    {
        FlipWeapon();
    }
    private void FlipWeapon()
    {
        if (GameInput.Instance.GetMovementVector().x == 0 && GameInput.Instance.GetMovementVector().y == 0)
        {
            return;
        }
        else if (GameInput.Instance.GetMovementVector().x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
