using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    public float speed = 5f;
    
    void Update()
    {
        //���������̍��W�擾
        float horizontal = Input.GetAxis("Horizontal");
        //���������̍��W�擾
        float vertical = Input.GetAxis("Vertical");

        //�X�V�����ړ���̍��W�擾
        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        //���݈ʒu�̕ύX
        transform.position += movement * speed * Time.deltaTime;
    }
}
