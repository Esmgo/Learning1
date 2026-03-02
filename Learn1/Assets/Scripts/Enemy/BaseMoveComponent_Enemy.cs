using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMoveComponent_Enemy : MonoBehaviour, IMove
{
    private Stat moveSpeed;
    public Stat MoveSpeed => moveSpeed;

    private Rigidbody2D rb;
    private ICharacter character;

    [SerializeField] private GameObject body;

    public void Init(EnemyConfiguration config)
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = new Stat(config.moveSpeed, 0.01f);
        character = CharacterManager.Instance.currentCharacter;

        body = transform.Find("Body").gameObject;
        if (body == null) Debug.LogError("Œ¥…Ë÷√÷˜ÃÂ");
    }

    private void FixedUpdate()
    {
        if(character != null && character.gameObject != null)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector2 direction = (character.gameObject.transform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed.FinalValue;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rb.rotation = angle; 
        body.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
