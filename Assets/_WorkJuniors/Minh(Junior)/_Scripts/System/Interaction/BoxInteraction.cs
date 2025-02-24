using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private ItemData _requiredItem;
    private Renderer _renderer;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    
    private void OnEnable()
    {
        EventBus.Instance.onItemUsed += onItemUsed;
    }

    private void OnDisable()
    {
        EventBus.Instance.onItemUsed -= onItemUsed;
    }

    private void onItemUsed(ItemData item)
    {
        if (Vector3.Distance(FPSController.Instance.transform.position, transform.position) < 50)
        {
            if (item == _requiredItem)
            {
                DialoguePrinter.Instance.PrintDialougeLine("You used the key on this cube for some reason.", 0.06f, () => _renderer.material.color = new Color(1, 0, 0, 1));
            }
        }
    }
}
