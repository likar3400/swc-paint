using System.Linq;
namespace SWCPaint.Core.Commands;

public class HistoryManager : IHistoryManager
{
    private readonly Stack<IUndoableCommand> _undoStack = new();
    private readonly Stack<IUndoableCommand> _redoStack = new();
    public event Action? HistoryChanged;

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

   public void Execute(IUndoableCommand command)
{
    command.Execute();
    _undoStack.Push(command);
    _redoStack.Clear();
    if (_undoStack.Count > _maxHistorySize)
        TrimHistory();

    HistoryChanged?.Invoke();
}

    public void Undo()
    {
        if (!CanUndo) return;

        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);
        HistoryChanged?.Invoke();
    }

    public void Redo()
    {
        if (!CanRedo) return;

        var command = _redoStack.Pop();
        command.Execute();
        _undoStack.Push(command);
        HistoryChanged?.Invoke();
    }
    private void TrimHistory()
    {
        var items = _undoStack.ToArray();
        _undoStack.Clear();

        foreach (var item in items.Take(_maxHistorySize).Reverse())
        {
            _undoStack.Push(item);
        }
    }
}

