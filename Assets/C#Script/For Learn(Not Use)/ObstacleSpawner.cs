using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; //��Q���I�u�W�F�N�g
    public float spawnInterval = 2f;  //�����Ԋu
    public float spawnHeight = 3f;    //�����̍������

    void Start()
    {
        //spawnInterval�̕b�����Ƃɐ�������(SpawnObstacle)���s��
        //InvokeRepeating(�֐���, n�b��, n�b�Ԋu)
        InvokeRepeating("SpawnObstacle", 1f, spawnInterval);
    }

    void SpawnObstacle()
    {
        //spawnHeight����-spawnHeight�͈̔͂Ő��l���o��
        float randomY = Random.Range(-spawnHeight, spawnHeight);
        //���W�̐ݒ�
        Vector3 spawnPosition = new Vector3(10f, randomY, 0f);
        //�I�u�W�F�N�g�̃C���X�^���X��
        //Quaternion.identity�Ƃ͉�]�Ȃ�(��ɓ����p�x)�Ƃ�������
        //�p�x�̎w����s�������Ƃ���Quaternion.Euler(0, 0, 45)��
        //float randomY = Random.Range(0f, 360f);Quaternion.Euler(0, randomY, 0)�Ƃ���
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}
