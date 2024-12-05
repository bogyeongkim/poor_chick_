using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class farm_GameManager : MonoBehaviour
{
    public int maxLives = 3;          // �ִ� ��� ��
    private int currentLives;         // ���� ��� ��

    public GameObject[] traps;        // ���� ������Ʈ �迭
    public GameObject[] items;        // ������ ������Ʈ �迭
    public GameObject[] platforms;    // �÷��� ������Ʈ �迭

    private Vector3 respawnPosition;  // ������ ��ġ
    private bool[] platformTouched;   // �� �÷����� ��Ҵ��� ����

    void Start()
    {
        currentLives = maxLives;      // �������� ���� �� �ִ� ��� ���� ����
        respawnPosition = transform.position; // ���� ��ġ�� �ʱ� ������ ��ġ�� ����
        platformTouched = new bool[platforms.Length]; // �� �÷����� ��Ҵ��� �����ϱ� ���� �迭 �ʱ�ȭ

        Debug.Log("Game Started. Current Lives: " + currentLives);


        // ��� ������ ������Ʈ�� Ʈ���� �ݹ� ����
        foreach (var item in items)
        {
            var itemCollider = item.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.isTrigger = true;
            }
        }

        // ��� �÷��� ������Ʈ�� Ʈ���� �ݹ� ����
        foreach (var platform in platforms)
        {
            var platformCollider = platform.GetComponent<BoxCollider>();
            if (platformCollider != null)
            {
                platformCollider.isTrigger = true;
            }
        }
    }

    void Update()
    {
        // �߶� �� ������ ��ġ üũ
        TrackPlayerHeight();
    }

    // ��� ���� �Լ� (�������� �Ծ��� �� ȣ��)
    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            Debug.Log("Life added. Current Lives: " + currentLives);
        }
        else
        {
            Debug.Log("Lives are already at max. Current Lives: " + currentLives);
        }
    }

    // ��� ���� �Լ� (�÷��� �Ʒ��� �������ų� ������ ����� �� ȣ��)
    public void LoseLife(bool respawn = false)
    {
        if (currentLives > 0)
        {
            currentLives--;
            Debug.Log("Life lost. Current Lives: " + currentLives);

            if (currentLives == 0)
            {
                GameOver(); // ����� 0�� �Ǹ� ���� ����
            }
            else if (respawn)
            {
                Respawn(); // �߶� �� ������
            }
        }
    }

    // ������ ó��
    void Respawn()
    {
        transform.position = respawnPosition;
        Debug.Log("Respawning at: " + respawnPosition);
    }

    // ���� ���� �Լ�
    void GameOver()
    {
        Debug.Log("Game Over! No lives remaining.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Ʈ���� �浹 ó��
    void OnTriggerEnter(Collider other)
    {
        // ���� ó��
        foreach (var trap in traps)
        {
            if (other.gameObject == trap) // �浹�� ������Ʈ�� �������� Ȯ��
            {
                LoseLife(); // ������ ���� ����� ����
                return;
            }
        }

        // ������ ó��
        foreach (var item in items)
        {
            if (other.gameObject == item) // �浹�� ������Ʈ�� ���������� Ȯ��
            {
                AddLife();
                Destroy(other.gameObject); // �������� ���� �� �ı�
                return;
            }
        }

        // �÷����� ����� �� ������ ��ġ ����
        for (int i = 0; i < platforms.Length; i++)
        {
            if (other.gameObject == platforms[i] && !platformTouched[i]) // ù ��°�� �ش� �÷����� �������
            {
                platformTouched[i] = true;  // �÷����� ��Ҵٰ� ǥ��
                SetRespawnPosition(i);      // �ش� �÷����� �´� ������ ��ġ ����
                Debug.Log("Platform " + i + " touched. Respawn position set.");
                return;
            }
        }
    }

    // �÷��̾� ���� ����
    void TrackPlayerHeight()
    {
        float playerHeight = transform.position.y;

        if (playerHeight < -30.0f)
        {
            LoseLife(respawn: true); // �߶� �� ������ ��ġ�� �̵�
        }
    }

    // ������ ��ġ ����
    void SetRespawnPosition(int platformIndex)
    {
        if (platformIndex >= 0 && platformIndex < platforms.Length)
        {
            Vector3 platformPosition = platforms[platformIndex].transform.position;

            respawnPosition = new Vector3(platformPosition.x, platformPosition.y + 3.0f, platformPosition.z);

            Debug.Log($"������ ���� : {respawnPosition}");
        }
        else
        {
            Debug.LogWarning("������ ����Ʈ ����");
        }
    }
}