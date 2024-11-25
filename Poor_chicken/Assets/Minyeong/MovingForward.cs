using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForward : MonoBehaviour
{
    public float speed = 2.0f; // 플랫폼의 이동 속도
    public float distance = 5.0f; // 이동할 거리
    public bool moveForward = true; // 이동 방향 (true: 오른쪽, false: 왼쪽)

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float journeyLength;
    private float startTime;

    void Start()
    {
        startPosition = transform.position;

        // 이동 방향에 따라 끝 위치 설정
        if (moveForward)
        {
            endPosition = startPosition + new Vector3(0, 0, distance);
        }
        else
        {
            endPosition = startPosition - new Vector3(0, 0, distance);
        }

        journeyLength = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        if (fractionOfJourney >= 1)
        {
            // 방향 전환
            Vector3 temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;
            startTime = Time.time; // 시간 초기화
        }

        transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
    }
}
