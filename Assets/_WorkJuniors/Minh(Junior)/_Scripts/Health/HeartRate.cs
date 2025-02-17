using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeartRate : MonoBehaviour
{
    Image m_Image;

    [SerializeField] float scrollSpeed;
    [SerializeField] Sprite[] healthStatus;

    void Start()
    {
        m_Image = this.GetComponent<Image>();
        m_Image.color = new Color32(241, 247, 214, 255);
    }

    private void Update()
    {

        m_Image.material.mainTextureOffset = m_Image.material.mainTextureOffset + new Vector2(Time.deltaTime * (-scrollSpeed / 10), 0f);


        if (Input.GetKeyDown(KeyCode.J))
        {
            m_Image.color = new Color32(0, 255, 0, 255);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            m_Image.color = new Color32(255, 255, 0, 255);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            m_Image.color = new Color32(255, 0, 0, 255);
        }

    }
}
