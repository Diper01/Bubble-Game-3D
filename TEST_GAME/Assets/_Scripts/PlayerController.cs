using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Space(5)][Header("Player")][Space(10)]
    [SerializeField] private float _minSize = 0.2f;
    [SerializeField] private float _maxSize = 2.0f;
    [SerializeField] private float _sizeDecreaseRate = 0.1f;
    [SerializeField] private float _moveSpeed = 1.7f;
    [Space(20)] [Header("Other")] [Space(10)]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _projectilePreview;
    [SerializeField] private Animator _animator;
    
    private Transform _finish;
    private Vector3 _targetPosition;
    private bool _isShooting = false;
    private float _shootTime;
    private GameObject _obstacleSpawner;
    private bool _isMinSize = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _finish = GameObject.Find("finishBolok").transform;
        _obstacleSpawner = GameObject.Find("ObstacleSpawner");
        ResetObstacles();
        Application.targetFrameRate = 120;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        HandleShooting();
        UpdateProjectilePreview();
        MovePlayer();
    }

    private void ResetObstacles()
    {
        _obstacleSpawner.GetComponent<ObstacleSpawner>().enabled = false;
        foreach (Transform child in _obstacleSpawner.transform)
        {
            if (child.name != "ObstacleSpawner")
                Destroy(child.gameObject);
        }
    }

    private void HandleShooting()
    {
        if (_isMinSize)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isShooting = true;
                _shootTime = 0;
                _projectilePreview.SetActive(true);
            }

            if (Input.GetMouseButton(0) && _isShooting)
            {
                _shootTime += Time.deltaTime;
                float newSize = Mathf.Max(_minSize, transform.localScale.x - _sizeDecreaseRate * Time.deltaTime);
                transform.localScale = new Vector3(newSize, newSize, newSize);

                float previewSize = Mathf.Min(_maxSize, _shootTime);
                _projectilePreview.transform.localScale = new Vector3(previewSize, previewSize, previewSize);

                if (transform.localScale.x <= _minSize + 0.1f)
                    _isMinSize = false;
            }

            if (Input.GetMouseButtonUp(0) && _isShooting)
            {
                _isShooting = false;
                Shoot();
                _projectilePreview.SetActive(false);
            }
        }
        else
        {
            _projectilePreview.SetActive(false);
        }
    }

    private void UpdateProjectilePreview()
    {
        _projectilePreview.transform.localPosition = new Vector3(
            transform.position.x + transform.localScale.x / 2 + _projectilePreview.transform.localScale.x / 2,
            _projectilePreview.transform.localPosition.y,
            _projectilePreview.transform.localPosition.z
        );
    }

    private void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _finish.position, _moveSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        float projectileSize = Mathf.Min(_maxSize, _shootTime);
        Vector3 positionToSpawn = new Vector3(transform.position.x + transform.localScale.x / 2 + projectileSize / 2,
            transform.position.y, transform.position.z);
        GameObject projectile = Instantiate(_projectilePrefab, positionToSpawn, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();
        projScript.Initialize(_finish.position, projectileSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            UIManager.instance.OnLose();
        }
        else if (other.gameObject.layer == 9)
        {
            StartCoroutine(WinAnim());
        }
    }

    private IEnumerator WinAnim()
    {
        _animator.Play("finish");
        yield return new WaitForSeconds(6f);
        UIManager.instance.OnWin();
    }

    public void StartGame()
    {
        _obstacleSpawner.GetComponent<ObstacleSpawner>().enabled = false;
        transform.localScale = new Vector3(_maxSize, _maxSize, _maxSize);
        _obstacleSpawner.GetComponent<ObstacleSpawner>().enabled = true;
        transform.position = GameObject.Find("SpawnPoint").transform.position;
        _isMinSize = true;
        Time.timeScale = 1;
    }
}
