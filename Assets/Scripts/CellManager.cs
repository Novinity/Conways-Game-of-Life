using UnityEngine;
using UnityEngine.Events;

public class CellManager : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;

    public int GRID_SIZE = 20;
    public Cell[,] grid;
    [SerializeField] private float _interval = 1;

    public UnityEvent CellUpdateEvent = new UnityEvent();
    public UnityEvent CellResetEvent = new UnityEvent();
    public UnityEvent CellClearEvent = new UnityEvent();

    private float _timer;
    public bool _running;

    void Start()
    {
        grid = new Cell[GRID_SIZE, GRID_SIZE];

        _populateGrid();
    }

    private void _populateGrid()
    {
        int gridSizeHalf = GRID_SIZE / 2;
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int y = 0; y < GRID_SIZE; y++)
            {
                GameObject newCell = Instantiate(_cellPrefab);
                Cell cell = newCell.GetComponent<Cell>();
                cell.Initialize(this, new Cell.Position(x, y));
                newCell.transform.position = new Vector2(x - gridSizeHalf, y - gridSizeHalf);
                grid[x, y] = cell;
            }
        }
    }

    void Update()
    {
        if (!_running) return;

        _timer += Time.deltaTime;
        if (_timer >= _interval)
        {
            _timer = 0;
            CellUpdateEvent.Invoke();
        }
    }

    public void StartSim()
    {
        _timer = 0;
        _running = true;
    }

    public void StopSim()
    {
        _running = false;
    }

    public void ResetSim()
    {
        CellResetEvent.Invoke();
    }

    public void ClearSim()
    {
        CellClearEvent.Invoke();
    }
}
