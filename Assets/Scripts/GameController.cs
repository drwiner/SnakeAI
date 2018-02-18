using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public int maxSize;
    public int currentSize;
    public int xBound;
    public int yBound;
    public int score;
    public int NESW;

    public GameObject foodPrefab;
    public GameObject currentFood;
    public GameObject snakePrefab;

    public Snake head;
    public Snake tail;
    
    public Vector2 nextPos;
    public Text scoreText;

    public float timer = .25f;

    private void OnEnable()
    {
        Snake.hit += hit;
    }

    // Use this for initialization
    void Start () {
        InvokeRepeating("TimerInvoke", 0f, timer);
        FoodFunction();
	}

    private void OnDisable()
    {
        Snake.hit -= hit;
    }

    // Update is called once per frame
    void Update () {
        ComChangeD();
	}

    void TimerInvoke()
    {
        Movement();
        StartCoroutine(CheckVisible());
        if (currentSize >= maxSize)
        {
            TailFunction();
        }
        else
        {
            currentSize++;
        }
    }

    void Movement()
    {
        GameObject temp;

        nextPos = head.transform.position;
        switch (NESW)
        {
            case 0: //up
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1: //right
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2: //down
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3: //left
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;
        }
        temp = Instantiate(snakePrefab, nextPos, transform.rotation) as GameObject;


        head.Next = temp.GetComponent<Snake>();
        head = temp.GetComponent<Snake>();
        
        return;
    }

    void ComChangeD()
    {
        // make sure we don't go backwards

        if (NESW !=2 && Input.GetKeyDown(KeyCode.W))
        {
            NESW = 0;
        }
        else if (NESW != 3 && Input.GetKeyDown(KeyCode.D))
        {
            NESW = 1;
        }
        else if (NESW != 0 && Input.GetKeyDown(KeyCode.S))
        {
            NESW = 2;

        }
        else if (NESW != 1 && Input.GetKeyDown(KeyCode.A))
        {
            NESW = 3;
        }
    }

    void TailFunction()
    {
        Snake tempSnake = tail;
        tail = tail.Next;
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currentFood = Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation) as GameObject;
        StartCoroutine(CheckRender(currentFood));
    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if (!IN.GetComponent<Renderer>().isVisible)
        {
            if (IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunction();
            }
        }
    }

    void hit(string WhatWasSent)
    {
        if (WhatWasSent == "Food")
        {
            FoodFunction();
            maxSize++;
            score++;
            scoreText.text = score.ToString();
        }

        if (WhatWasSent == "Snake")
        {
            
            CancelInvoke("TimerInvoke");
            int temp = PlayerPrefs.GetInt("HighScore");
            if (score > temp)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
            Exit();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    void wrap()
    {
        if (NESW == 0)
        {
            head.transform.position = new Vector2(head.transform.position.x, -(head.transform.position.y - 1));
        }
        else if (NESW == 1)
        {
            head.transform.position = new Vector2(-(head.transform.position.x - 1), head.transform.position.y);
        }
        else if (NESW == 2)
        {
            head.transform.position = new Vector2(head.transform.position.x, -(head.transform.position.y + 1));
        }
        else if (NESW == 3)
        {
            head.transform.position = new Vector2(-(head.transform.position.x + 1), head.transform.position.y);
        }
    }

    IEnumerator CheckVisible()
    {
        yield return new WaitForEndOfFrame();
        if (!head.GetComponent<Renderer>().isVisible)
        {
            wrap();
        }
    }
}
