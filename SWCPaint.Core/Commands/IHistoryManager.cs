using System;
namespace SWCPaint.Core.Commands;

public interface IHistoryManager
{
    event Action? HistoryChanged;
    bool CanUndo { get; }
    bool CanRedo { get; }
    void Execute(IUndoableCommand command);
    void Undo();
    void Redo();
}
