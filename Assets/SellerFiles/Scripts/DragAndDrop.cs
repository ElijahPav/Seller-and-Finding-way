using UnityEngine;
using Image = UnityEngine.UI.Image;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Vector3 _defaultScale = Vector3.one;
    private Vector3 _inDragScale = new Vector3(1.3f, 1.3f, 1.3f);



    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }

    public void BeginDrag()
    {
        transform.localScale = _inDragScale;
    }

    public void Drag()
    {
        var pos =_camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }

    public void EndDrag()
    {
        transform.localScale = Vector3.one;
    }
    public void Drop()
    {
        transform.localScale = _defaultScale;
    }
}
