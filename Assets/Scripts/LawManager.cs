using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SquirrelLaw
{
    None,
    GoUp,
    GoDown,
    Attack,
    Scatter,
}

public enum ToadLaw
{
    None,
    GoUp,
    GoDown,
    Attack,
}

public enum InputState
{
    Empty,
    Q,
    W,
    E
}

public class Laws : MonoBehaviour
{
    public static Laws Instance { get; private set; }
    public List<SquirrelLaw> squirrelLaws = new List<SquirrelLaw>();
    public List<ToadLaw> toadLaws = new List<ToadLaw>();
    private int maxRoom = 5;
    public int currentRoom = 0;
    public InputState state = InputState.Empty;
    public Transform qHigh;
    public Transform wHigh;
    public bool inMenu = true;
    public GameObject hackMenu;
    public int health = 100;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        Instance = this;
        for (int ix = 0; ix < maxRoom; ix += 1)
        {
            this.squirrelLaws.Add(SquirrelLaw.None);
            this.toadLaws.Add(ToadLaw.None);
        }
    }

    void Update()
    {
        if (inMenu)
        {
            Time.timeScale = 0.0f;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                inMenu = false;
            }
            else
            {
                return;
            }
        }
        Time.timeScale = 1.0f;
        if (hackMenu != null)
        {
            Destroy(hackMenu);
        }
        // Check for input to set the state
        if (Input.GetKeyDown(KeyCode.Q))
        {
            state = InputState.Q;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            state = InputState.W;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            state = InputState.E;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = InputState.Empty;
        }

        switch (state)
        {
            case InputState.Empty:
                qHigh.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                wHigh.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case InputState.Q:
                wHigh.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                SquirrelLaw currentLaw = squirrelLaws[currentRoom];
                // Check for input to set the state
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (currentLaw == SquirrelLaw.GoUp)
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.None;
                    }
                    else
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.GoUp;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (currentLaw == SquirrelLaw.GoDown)
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.None;
                    }
                    else
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.GoDown;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (currentLaw == SquirrelLaw.Attack)
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.None;
                    }
                    else
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.Attack;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    if (currentLaw == SquirrelLaw.Scatter)
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.None;
                    }
                    else
                    {
                        squirrelLaws[currentRoom] = SquirrelLaw.Scatter;
                    }
                }

                switch (squirrelLaws[currentRoom])
                {
                    case SquirrelLaw.None:
                        qHigh.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                        break;
                    case SquirrelLaw.GoUp:
                        qHigh.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        qHigh.localPosition = new Vector3(-0.72f, 0.318f, 0.0f);
                        break;
                    case SquirrelLaw.GoDown:
                        qHigh.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        qHigh.localPosition = new Vector3(-0.72f, 0.164f, 0.0f);
                        break;
                    case SquirrelLaw.Attack:
                        qHigh.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        qHigh.localPosition = new Vector3(-0.72f, 0.0f, 0.0f);
                        break;
                    case SquirrelLaw.Scatter:
                        qHigh.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        qHigh.localPosition = new Vector3(-0.72f, -0.154f, 0.0f);
                        break;
                }

                break;
            case InputState.W:
                qHigh.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                ToadLaw currentLaw2 = toadLaws[currentRoom];
                // Check for input to set the state
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (currentLaw2 == ToadLaw.GoUp)
                    {
                        toadLaws[currentRoom] = ToadLaw.None;
                    }
                    else
                    {
                        toadLaws[currentRoom] = ToadLaw.GoUp;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (currentLaw2 == ToadLaw.GoDown)
                    {
                        toadLaws[currentRoom] = ToadLaw.None;
                    }
                    else
                    {
                        toadLaws[currentRoom] = ToadLaw.GoDown;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (currentLaw2 == ToadLaw.Attack)
                    {
                        toadLaws[currentRoom] = ToadLaw.None;
                    }
                    else
                    {
                        toadLaws[currentRoom] = ToadLaw.Attack;
                    }
                }

                switch (toadLaws[currentRoom])
                {
                    case ToadLaw.None:
                        wHigh.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                        break;
                    case ToadLaw.GoUp:
                        wHigh.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        wHigh.localPosition = new Vector3(-0.10f, 0.318f, 0.0f);
                        break;
                    case ToadLaw.GoDown:
                        wHigh.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        wHigh.localPosition = new Vector3(-0.10f, 0.164f, 0.0f);
                        break;
                    case ToadLaw.Attack:
                        wHigh.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        wHigh.localPosition = new Vector3(-0.10f, 0.0f, 0.0f);
                        break;
                }

                break;
            default:
                break;
        }

        // Change rooms
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentRoom < maxRoom)
            {
                currentRoom += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentRoom > 0)
            {
                currentRoom -= 1;
            }
        }
    }
}