using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLettuce : MonoBehaviour
{
    #region //�C���X�y�N�^�[�Őݒ肷��
    [Header("���Z�X�R�A")] public int myScore = 500;
    [Header("�ړ����x")] public float speed = 4;
    [Header("�d��")] public float gravity = 5;
    [Header("��ʊO�ł��s�����邩")] public bool nonVisibleAct;
    [Header("�ڐG����")] public EnemyCollisionCheck1 checkCollision;
    [Header("���ꂽ���ɖ炷SE")] public AudioClip deadSE;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision1 oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;
    private bool isDeadSE;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    //�C���X�^���X��߂܂���  
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision1>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!oc.playerStepOn)   // �v���C���[�ɓ��܂ꂽ���ǂ���
        {
            if (sr.isVisible || nonVisibleAct)
            {
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);

            }
            else
            {
                rb.Sleep();     //��ʂɉf���Ă��Ȃ����������Z��؂�
            }
        }
        else
        {
            if (!isDead)
            {
                anim.Play("EnemyLettuceDead");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(deadSE);
                    GManager1.instance.score += myScore;
                }
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        //�U�����̓����蔻��
        if (collision.gameObject.tag == "Attack" || collision.gameObject.tag == "Syuriken")
        {
            if (!isDeadSE)
            {
                this.tag = "DeadEnemy";
                anim.Play("EnemyLettuceDead");
                isDead = true;
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(deadSE);
                    GManager1.instance.score += myScore;
                }
                isDeadSE = true;
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
