using UnityEngine;

public class SphereController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _transform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        // Bu işlem rigidbody componentini bulmak için yapılıyor. 
        _transform.position = new Vector3(0, 0, 0);
    }

    void Start()
    {
        _rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
