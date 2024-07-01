namespace Contracts;

public abstract class Base
{
    public abstract List<Shortcut> Shortcuts { get; set; }
    
    public abstract List<Button> Buttons { get; set; }

    public abstract void AddParams();
}