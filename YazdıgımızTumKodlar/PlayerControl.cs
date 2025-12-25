using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 5f; //zýplama yüksekliði tanýmlamasý
    public bool isGrounded;         //yer kontrolu


    private bool isGameOver = false;


    public float walkSpeed = 5f;      // Normal yürüme hýzý
    public float runSpeed = 9f;       // Shift'e basýnca çýkacak hýz
    private float currentSpeed;

    public CoinManager CoinManager; //Altýn Toplamak için

    public Transform groundCheck;   //raycast için 
    public float checkRadius = 0.2f; //raycast için(yer kontrolu)
    public LayerMask whatIsGround;      //raycast için

    float horizontalMove;  
    private Animator animator;

    private Vector3 originalScale;

    //TIRMANMA AYARLARI
    public float climbSpeed = 5f;
    public float verticalInput;
    private bool isLadder;   // Merdiven alanýnda mý?
    public bool isClimbing; // Þu an týrmanýyor mu?
    private float originalGravity; // Karakterin normal yerçekimi
    

    private void Start()
    {

        currentSpeed = walkSpeed;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        originalGravity = rb.gravityScale; 

        originalScale = transform.localScale;
    }

    void Update()
    {
        // Shift'e basýlýyorsa runSpeed'i, býrakýlýnca walkSpeed'i kullan
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * currentSpeed;       //yatay hýz a ve d giriþi

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);      //raycast için yer kontrolü

        verticalInput = Input.GetAxisRaw("Vertical"); //w ve s tuþlarý merdiven

        animator.SetFloat("speed", Mathf.Abs(horizontalMove) * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));        //koþma animasyonu.

        animator.SetFloat("yVelocity", rb.linearVelocity.y);        //zýplama animasyonlarý için
        animator.SetBool("isGrounded", isGrounded);                 //zýplama animasyonlarý için



        if (isLadder)
        {
            // Yerdeyken sadece yukarý basarsak týrmanma baþlasýn
            if (isGrounded)
            {
                if (verticalInput > 0.1f)
                    isClimbing = true;
                else
                    isClimbing = false; // Yerdeyken S'ye basýnca veya boþta týrmanma kapalý
            }
            // Havadaysak (zýplayýp merdivene deðdiysek veya týrmanýyorsak)
            else if (Mathf.Abs(verticalInput) > 0.1f)
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
        }




        if (isClimbing)                                     //BÖYLE BÝR DEÐÝÞKEN LAZIM ÇÜNKÜ MERDÝVENDE SAÐA SOLA DÖNÜYOR.
        {

        }
        else
        {
            if (horizontalMove > 0)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
            else if (horizontalMove < 0)                                                                //karakteri 180 derece döndürmesi için
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);            //zýplama giriþi space ile
        }
        
   


    }


    private void OnTriggerEnter2D(Collider2D other)             //TRÝGGER SÝSTEMÝ    
    {
        if (other.CompareTag("Coin"))
        {
            // 1. Sayacý artýr
            if (CoinManager != null) CoinManager.AddCoin();

            // 2. Altýný yok et
            Destroy(other.gameObject);

            // 3. (Ýsteðe baðlý) Buraya bir altýn toplama sesi ekleyebilirsin
        }

        if (other.CompareTag("Ladder"))
        {
            isLadder = true;
        }

    }
    private void OnTriggerExit2D(Collider2D other)      //MERDÝVENDEN ÇIKMA 
    {
        if (other.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false; // Merdivenden çýkýnca týrmanma modunu kapat
        }
    }

    private void FixedUpdate()
    {

        if (isClimbing)                                                                         //TIRMANMA FÝZÝK HAREKETLERÝ
        {
            animator.SetBool("isClimbing", true);

            if  (verticalInput != 0)
                animator.speed = 1f;                    //Týrmanma
            else 
                animator.speed = 0f; 


            rb.gravityScale = 0f; // Yerçekimini kapat ki aþaðý kaymasýn
            rb.linearVelocity = new Vector2(0, verticalInput * climbSpeed);
            
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontalMove, rb.linearVelocity.y);

            animator.SetBool("isClimbing", false);
            animator.speed = 1f;                                    //Týrmanma animasyon

            rb.gravityScale = originalGravity; // Merdivende deðilse yerçekimi normale dönsün
        }
    }

}
