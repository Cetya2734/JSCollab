using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class FPS_Controller2 : MonoBehaviour
{
    public float speed = 6f;
    public float mouseSensitivity = 200f;
    public Transform cam;
    public Animator animator;

    CharacterController controller;
    public GameObject flashSprite;
    public GameObject bubble;


    float xRotation = 0f;


    public GameObject objectToSpawn;
    public GameObject objectToSpawn1;
    public GameObject flashObject;
    public Transform spawnPoint;
    public Transform spawnPoint1;

    public Vector3 startScale1 = new Vector3(1, 1, 1);
    public Vector3 startScale2 = new Vector3(1, 1, 1);
    public float shrinkTime = 0.3f;
    public float flashTime = 0.3f;
    public float flashTimeLight = 0.3f;

    public float shrinkTime1;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        flashObject.SetActive(false);

    }

    void Update()
    {
        Look();
        Move();
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Shoot");
            StartCoroutine(Flash());
            Spawn();
            SpawnFlash();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            animator.SetTrigger("Equip");
        }
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // detect ground normal
        RaycastHit hit;
        Vector3 groundNormal = Vector3.up;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
            groundNormal = hit.normal;

        move = Vector3.ProjectOnPlane(move, groundNormal);
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);
        controller.Move(Vector3.down * 2f * Time.deltaTime);

        // animation bool
        bool moving = move.magnitude > 0.1f;
        animator.SetBool("Moving", moving);



    }


    #region MuzzleFlash
    IEnumerator Flash()
    {
        flashObject.SetActive(true);
        yield return new WaitForSeconds(flashTimeLight);
        flashObject.SetActive(false);
    }
    public void Spawn()
    {
        GameObject obj = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        obj.transform.localScale = startScale1;

        StartCoroutine(Shrink(obj));
    }
    public void SpawnFlash()
    {
        GameObject obj = Instantiate(objectToSpawn1, spawnPoint1.position, spawnPoint1.rotation);
        obj.transform.localScale = startScale2;

        StartCoroutine(Shrink2(obj));
        StartCoroutine(Shrink3(obj));
        StartCoroutine(Shrink4(obj));

    }
    IEnumerator Shrink(GameObject obj)
    {
        float t = 0;


        Renderer rend = obj.GetComponent<Renderer>();
        Material mat = rend.material;

        while (t < shrinkTime)
        {
            t += Time.deltaTime;

            float value = Mathf.Lerp(10f, 0f, t / shrinkTime);
            mat.SetFloat("_NoiseScale", value); // property name must match shader

            yield return null;
        }
        Destroy(obj);

    }
    IEnumerator Shrink2(GameObject obj)
    {
        float t = 0;


        Renderer rend = obj.GetComponent<Renderer>();
        Material mat = rend.material;
        Color c = mat.GetColor("_FresnelColour");
        while (t < flashTime)
        {
            t += Time.deltaTime;

            c *= Mathf.Lerp(1f, 0f, t / flashTime);
            mat.SetColor("_FresnelColour", c);

            yield return null;
        }
    

    }
    IEnumerator Shrink3(GameObject obj)
    {
        float t = 0;


        Renderer rend = obj.GetComponent<Renderer>();
        Material mat = rend.material;

        while (t < shrinkTime1)
        {
            t += Time.deltaTime;

            float value = Mathf.Lerp(10f, 0f, t / shrinkTime1);
            mat.SetFloat("_NoiseScale", value); // property name must match shader

            yield return null;
        }
        Destroy(obj);

    }
    IEnumerator Shrink4(GameObject obj)
    {
        float t = 0;


        Renderer rend = obj.GetComponent<Renderer>();
        Material mat = rend.material;

        while (t < flashTime)
        {
            t += Time.deltaTime;

            float value = Mathf.Lerp(2f, 70f, t / flashTime);
            mat.SetFloat("_FresnelPower", value); // property name must match shader

            yield return null;
        }
       

    }
    #endregion
}
