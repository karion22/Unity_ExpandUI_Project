using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eDockPanel : eElement
{
    public void UpdateLayout(bool immediately = true)
    {
        // 자식 객체들을 읽어온다.
        var children = transform.GetComponentsInChildren<RectTransform>();

        // 자식 객체들의 정렬 상태를 읽어온다.
        float top = 0f, bottom = 0f, left = 0f, right = 0f;

        foreach(var child in children)
        {
            if (child == transform) continue;
            Vector3 position = child.position;
            if (child.pivot.x == 0)
            {
                position.x += left;
                child.position = position;
                left += child.sizeDelta.x;
            }
            else if(child.pivot.x == 1)
            {
                position.x -= right;
                child.position = position;
                right += child.sizeDelta.x;
            }
            else
            {
                Vector2 size = child.sizeDelta;
                size.x -= (left + right);
                child.sizeDelta = size;
            }

            if(child.pivot.y == 0)
            {
                position.y += bottom;
                child.position = position;
                bottom += child.sizeDelta.y;
            }
            else if(child.pivot.y == 1)
            {
                position.y -= top;
                child.position = position;
                top += child.sizeDelta.y;
            }
            else
            {
                Vector2 size = child.sizeDelta;
                size.y -= (top + bottom);
                child.sizeDelta = size;
            }
        }
    }
}
