using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jumpSpeed;
    public float fallMultiplier = 2.5f;
    public float jumpStartMultiplier = 2f;
    public Text diamondCountTxt;
    public Text glassesCountTxt;

    private Rigidbody rb;
    private Collider cd;
    private Animator anim;
    private float distToGround = 0f;
    private bool onGround = true;
    private bool onWall = false;
    private int currentLevel;
    private int glasses;
    private int diamonds;
    private int totalG;
    private int totalD;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        distToGround = cd.bounds.extents.y;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        glasses = 0;
        diamonds = 0;
        totalG = GameObject.FindGameObjectsWithTag("GlassesCol").Length;
        totalD = GameObject.FindGameObjectsWithTag("DiamondCol").Length;
        diamondCountTxt.text = diamonds.ToString() + "/" + totalD.ToString();
        glassesCountTxt.text = glasses.ToString() + "/" + totalG.ToString();
        // Debug.Log(distToGround);
    }

    void Update()
    {
        onGround = isGrounded();
        onWall = isOnWall();
        //Debug.Log(onWall);
        //Debug.Log(onGround);
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime; 
        }
        else if(rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (jumpStartMultiplier - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {

        if(Input.GetButton("Jump") && (onGround || onWall))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0);
        }

        if(Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if(moveHorizontal > 0.1)
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, 0);
            transform.Find("FINAL").gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            anim.enabled = true;
        }
        else if(moveHorizontal < -0.1)
        {
            rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
            transform.Find("FINAL").gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.enabled = true;
        }
        else if(moveHorizontal == 0 && onGround)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            anim.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("EndLamp"))
		{
            currentLevel++;
            if(currentLevel == 2)
            {
                SceneManager.LoadSceneAsync(2);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
            }
            if(currentLevel == 3)
            {
                SceneManager.LoadSceneAsync(3);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(3));
            }
            if(currentLevel == 4)
            {
                SceneManager.LoadSceneAsync(4);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(4));  
            }
            if(currentLevel >= SceneManager.sceneCount - 1)
            {
                SceneManager.LoadScene("StartMenu");
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartMenu"));
            }
		}

        if(other.gameObject.CompareTag("Trap"))
        {
            SceneManager.LoadScene(currentLevel);
        }

        if(other.gameObject.CompareTag("GlassesCol"))
        {
            glasses++;
            other.gameObject.SetActive(false);
            updateText();
        }

        if(other.gameObject.CompareTag("DiamondCol"))
        {
            diamonds++;
            other.gameObject.SetActive(false);
            updateText();
        }
	}

    void updateText()
    {
        diamondCountTxt.text = diamonds.ToString() + "/" + totalD.ToString();
        glassesCountTxt.text = glasses.ToString() + "/" + totalG.ToString();
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), -Vector3.up, distToGround - 0.5805f);
    }

    bool isOnWall()
    {
        return Physics.Raycast(transform.position, Vector3.left, 0.6f) ||
             Physics.Raycast(transform.position, Vector3.right, 0.6f);
    }
}
