using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _hp;
    [SerializeField] private ProjecttailOperator _operator;
    private Vector3 _input;
    private float _actualHp;
    private void Update()
    {
        GatherInput();
        Look();
    }

    private void Awake()
    {
        _actualHp = _hp;
    }

    public void REciveDamage(float damage)
    {
        _actualHp = _actualHp - damage;
        _operator.UpdateHp(_actualHp / _hp);
    }


    private void FixedUpdate()
    {
        Move();
    }
    void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }
    void Look()
    {
        if (_input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0));
            var skewedInput = matrix.MultiplyPoint3x4(_input);

            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = rot;

        }
    }
    private void Move()
    {
        _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);

    }
}
