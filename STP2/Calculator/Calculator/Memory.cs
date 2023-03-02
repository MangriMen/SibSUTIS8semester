namespace Calculator;

public static class Memory
{
    public enum Actions
    {
        Store,
        Read,
        Add,
        Subtract,
        Clear,
    }
}

public class Memory<T>
    where T : new()
{
    private T _number = new();
    private bool _isOn = true;

    public T Number => _number;
    public bool IsOn => _isOn;

    public Memory()
    {
        Clear();
    }

    public void Store(T obj)
    {
        _isOn = true;
        _number = obj;
    }

    public T Read()
    {
        _isOn = true;
        return _number;
    }

    public void Add(T obj)
    {
        _isOn = true;
        _number = (dynamic?)_number + (dynamic?)obj;
    }

    public void Subtract(T obj)
    {
        _isOn = true;
        _number = (dynamic?)_number - (dynamic?)obj;
    }

    public void Clear()
    {
        _isOn = false;
        _number = new();
    }
}
