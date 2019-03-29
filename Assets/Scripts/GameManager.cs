using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject borderPrefab;
    [SerializeField] private GameObject borderBottomPrefab;
    [SerializeField] private List<GameObject> fallingPrefabs= new List<GameObject>() ;
    [SerializeField] private float timeBetween=1f;
    [SerializeField] private bool spawn=true;
    
    [SerializeField] private Vector3 startBallPosition;
    [SerializeField] private Vector3 startBallRotation;
    [SerializeField] private Vector3 startPlaneRotation;
    [SerializeField] private Vector3 startPlanePosition;
    [SerializeField] private Vector3 startBorderLeftPosition;
    [SerializeField] private Vector3 startBorderLeftRotation;
    [SerializeField] private Vector3 startBorderRightPosition;    
    [SerializeField] private Vector3 startBorderRightRotation;
    [SerializeField] private Vector3 startBorderBottomPosition;    
    [SerializeField] private Vector3 startBorderBottomRotation;
    [SerializeField] private int startProgressValue;

    [SerializeField] private Vector3 leftSpawnPoint;
    [SerializeField] private Vector3 rightSpawnPoint;

    private GameObject plane;
    private GameObject ball;
    private GameObject borderLeft;
    private GameObject borderRight;
    private GameObject borderBottom;
    
    private int progress;
    
    private List<GameObject> spawnedObjects =  new List<GameObject>();
   
    private  IEnumerator spawnCoro;

    private bool isMobile = false;
    
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }
    private void Awake()
    {
         
        InitLevel();
        StartLevel();
#if UNITY_ANDROID && !UNITY_EDITOR
        isMobile = true;
#endif
    }

    public bool IsMobile
    {
        get => isMobile;  
    }

    private void InitLevel()
    {
        ball = SimplePool.Spawn(ballPrefab, startBallPosition, Quaternion.Euler(startBallRotation));
         
        plane = SimplePool.Spawn(planePrefab, startPlanePosition, Quaternion.Euler(startPlaneRotation));
         
        borderLeft = SimplePool.Spawn(borderPrefab, startBorderLeftPosition, Quaternion.Euler(startBorderLeftRotation));
        borderRight = SimplePool.Spawn(borderPrefab, startBorderRightPosition, Quaternion.Euler(startBorderRightRotation));
        borderBottom = SimplePool.Spawn(borderBottomPrefab, startBorderBottomPosition, Quaternion.Euler(startBorderBottomRotation));
    }
    
    public void StartLevel()
    {
        ball.transform.position = startBallPosition;
        ball.transform.rotation = Quaternion.Euler(startBallRotation);
        var rigid = ball.GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            rigid.velocity= Vector2.zero;
            rigid.angularVelocity= 0;
        }

        plane.transform.position = startPlanePosition;
        plane.transform.rotation =  Quaternion.Euler(startPlaneRotation);
        
        borderLeft.transform.position = startBorderLeftPosition;
        borderLeft.transform.rotation =  Quaternion.Euler(startBorderLeftRotation);
        borderRight.transform.position = startBorderRightPosition;
        borderRight.transform.rotation =  Quaternion.Euler(startBorderRightRotation);
        borderBottom.transform.position = startBorderBottomPosition;
        borderBottom.transform.rotation =  Quaternion.Euler(startBorderBottomRotation);

        Progress = startProgressValue;
        foreach (var spawnedO in spawnedObjects)
        {
            SimplePool.Despawn(spawnedO);
        }
        
        spawnCoro = Spawn();
        StartCoroutine(spawnCoro);
    }

    public void RestartLevel()
    {
        StopCoroutine(spawnCoro);
        StartLevel();
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetween);
            if (spawn)
            {
                int randInd = Random.Range(0, fallingPrefabs.Count);

                Vector3 randPos = Vector3.Lerp(leftSpawnPoint, rightSpawnPoint, Random.value);

                var newGO = SimplePool.Spawn(fallingPrefabs[randInd], randPos, Quaternion.identity);

                spawnedObjects.Add(newGO);
            }
        }
        yield break;
    }


    public int Progress
    {
        get => progress;
        set
        {
            progress = value;
            InterfaceManager.Instance.SetProgress(progress.ToString());
        }
    }

    public void FallingBallEnded(FallingBall fallingBall)
    {
        if (spawnedObjects.Contains(fallingBall.gameObject))
        {
            spawnedObjects.Remove(fallingBall.gameObject);
        }
        SimplePool.Despawn(fallingBall.gameObject);
    }
}
