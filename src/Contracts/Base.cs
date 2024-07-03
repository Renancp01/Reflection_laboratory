namespace Contracts;

public abstract class Base
{
    public abstract List<Shortcut> Shortcuts { get;  }
    
    public abstract List<Button> Buttons { get; }

    public abstract void AddParams();
}