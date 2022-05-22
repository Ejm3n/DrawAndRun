using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game instance;
    private bool GameStarted;
    private bool GameEnded;

    [Header("Guys")]
    [SerializeField] private int GuyCount;
    [SerializeField] private Guy GuyPrefab;
    private List<Guy> Guys;
    private List<Vector3> Points;
    [Header("Components")]
    [SerializeField] private Transform GuyZone;
    [SerializeField] private SplineFollower SplineFollow;
    [HideInInspector]public int BonusCount;
    public delegate void OnBonusTake();
    public OnBonusTake onBonusTake;
    public delegate void OnLoseGame();
    public OnLoseGame onLoseGame;
    public delegate void OnStartGame();
    public OnStartGame onStartGame;
    public delegate void OnWinGame();
    public OnWinGame onWinGame;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Start()
    {
        
        Draw.OnDrawEnd += OnDraw;
        Guys = new List<Guy>();
        Points = new List<Vector3>();

        CreateStartGuys();
    }

    public void RemoveGuy(Guy guy)
    {
        bool noAlive = true ;
        foreach (Guy guyL in Guys)
        {
            if (guyL.Alive)
                noAlive = false;
        }
        if(Guys.Contains(guy))
            Guys.Remove(guy);
        if (Guys.Count == 0 || noAlive)
            Lose();
    }

    public void AddGuy(Guy guy)
    {
        Guys.Add(guy);
        guy.transform.parent = GuyZone;
    }

    public void AddBonus(int amount)
    {
        BonusCount += amount;
        onBonusTake?.Invoke();
    }

    private void CreateStartGuys()
    {
        for (int i = 0; i < GuyCount; i++)
        {
            Guys.Add(Instantiate(GuyPrefab, GuyZone.transform.position, Quaternion.identity, GuyZone));
        }
        BuildInRow();
    }

    private void BuildInRow()
    {
        List<Vector3> StartPos = new List<Vector3>();

        float Lenght = 20;
        float Width = 10;
        int GuyInRow = 10;
        for (int y = 0; y < (float)GuyCount / (float)GuyInRow; y++)
        {
            for (int x = 0; x < (float)GuyInRow; x++)
            {
                Vector3 Pos = new Vector3((x - GuyInRow / 2) * Lenght / 2 + 5, 0, (y - (float)GuyCount / (float)GuyInRow / 2) * Width);
                StartPos.Add(Pos);
            }
        }

        for (int i = 0; i < Guys.Count; i++)
        {
            Guys[i].LocalPosition = StartPos[i];
        }
    }

    private void OnDraw(List<Vector3> NewPoints)
    {
        Points.Clear();
        Points = NewPoints;
        float Delta = (float)NewPoints.Count / (float)Guys.Count;
        if (Delta >= 1)
        {
            int Increase = Mathf.CeilToInt(Delta);
            for (int i = 0; i < Guys.Count; i++)
            {
                if (i * Increase < Points.Count)
                {
                    Guys[i].MoveTo(Points[i * Increase]);
                }
                else
                {
                    Guys[i].MoveTo(Points[Random.Range(0, Points.Count)]);
                }
            }
        }
        else
        {
            for (int i = 0; i < Guys.Count; i++)
            {
                Guys[i].MoveTo(Points[Random.Range(0, Points.Count)]);
            }
        }

        if (!GameStarted)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        GameStarted = true;
        SplineFollow.follow = true;
        for (int i = 0; i < Guys.Count; i++)
        {
            Guys[i].anim.SetTrigger("Run");
        }
        onStartGame?.Invoke();
    }

    public void Win()
    {
        Debug.Log("WIN");
        for (int i = 0; i < Guys.Count; i++)
        {
            Guys[i].anim.SetTrigger("Win");
        }

        onWinGame?.Invoke();
    }

    private void Lose()
    {
        onLoseGame?.Invoke();
        Debug.Log("Lose");
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        Guys.Clear();
    }
}
