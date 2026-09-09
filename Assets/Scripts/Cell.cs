using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    private CellManager _cellManager;

    public bool state = false;
    private bool _defaultState = false;
    private Position _position = new();

    [SerializeField] private GameObject _onImage;

    private bool _justUpdated = false;
    private bool _nextState = false;

    public class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(int x)
        {
            this.x = x;
            y = 0;
        }
    }

    public void Initialize(CellManager cellManager, Position position)
    {
        _cellManager = cellManager;
        _position = position;

        cellManager.CellUpdateEvent.AddListener(CheckSurroundings);
        cellManager.CellResetEvent.AddListener(ResetState);
        cellManager.CellClearEvent.AddListener(ClearState);
    }

    public void CheckSurroundings()
    {
        int surroundingOn = 0;

        for (int x = _position.x - 1; x <= _position.x + 1; x++)
        {
            for (int y = _position.y - 1; y <= _position.y + 1; y++)
            {
                if (x == _position.x && y == _position.y) continue;
                if (x >= _cellManager.grid.GetLength(0) || x < 0 || y >= _cellManager.grid.GetLength(1) || y < 0)
                {
                    continue;
                }
                
                if (_cellManager.grid[x, y].state) surroundingOn++;
            }
        }

        _nextState = state;

        if (state && (surroundingOn < 2 || surroundingOn > 3)) _nextState = false;
        else if (!state && surroundingOn == 3) _nextState = true;

        _justUpdated = true;
    }

    void Update()
    {
        if (_justUpdated && _nextState != state)
        {
            state = _nextState;
            _justUpdated = false;
            _update();
        }
    }

    public void ResetState()
    {
        _nextState = _defaultState;
        _justUpdated = true;
    }

    public void ClearState()
    {
        _nextState = false;
        _defaultState = false;
        _justUpdated = true;
    }

    private void _update()
    {
        _onImage.SetActive(state);
    }

    public void ToggleState()
    {
        state = !state;
        _defaultState = state;
        
        _update();
    }
}
