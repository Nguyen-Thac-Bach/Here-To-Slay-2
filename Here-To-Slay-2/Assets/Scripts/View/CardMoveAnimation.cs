using System;
using UnityEngine;
namespace View
{
    public class CardMoveAnimation : MonoBehaviour
    {
        [SerializeField]
        float timerValue = 10.0f;

        Vector2 initialPos;
        float timer = 0;
        bool isMoving;

        // Update is called once per frame
        void Update()
        {
            if (transform.localPosition.magnitude > 1.0f || !isMoving)
            {
                isMoving = true;
                initialPos = transform.localPosition;
                timer = timerValue;
            }

            if (isMoving)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    isMoving = false;
                }
                else
                {
                    transform.localPosition = Vector2.Lerp(Vector2.zero, initialPos, timer / timerValue);
                }
            }
        }
    }
}

