using UnityEngine;
using UnityEngine.EventSystems;

public class MapElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public/inspector
    [SerializeField] private Material onDeselected;
    [SerializeField] private Material onSelected;
    [SerializeField] private MeshRenderer meshRenderer;
    
    //unity methods
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        meshRenderer.material = onSelected;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        meshRenderer.material = onDeselected;
    }
}
