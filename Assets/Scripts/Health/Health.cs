
using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private GameManager theGM;

    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    //[SerializeField] private AudioClip lifeSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);


        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invulerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
                theGM.GameOver();

                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

   
    // Vita Extra

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        //SoundManager.instance.PlaySound(lifeSound);
    }

     private IEnumerator Invulerability()
     {
         Physics2D.IgnoreLayerCollision(8, 9, true);
         //invulnerability duration
         for (int i = 0; i < numberOfFlashes; i++)
         {
             spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
             spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
         }

         Physics2D.IgnoreLayerCollision(8, 9, false);
     }

    public void ResetPlayer()
    {
        anim.Play("Idle");
        currentHealth = startingHealth;
        GetComponent<PlayerMovement>().enabled = true;
    }
}