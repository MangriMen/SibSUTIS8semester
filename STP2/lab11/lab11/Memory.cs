namespace lab11;

public class Memory<T> where T : new()
{
    public T FNumber = new();

    bool _isOn;
    public bool IsOn { get => _isOn; set => _isOn = value; }

    public Memory()
    {
        Clear();
    }

    public void Storage(T obj)
    {
        _isOn = true;
        FNumber = obj;
    }

    public T Read()
    {
        _isOn = true;
        return FNumber;
    }

    public void Add(T obj)
    {
        _isOn = true;
        FNumber = (dynamic?)FNumber + (dynamic?)obj;
    }

    public void Subtract(T obj)
    {
        _isOn = true;
        FNumber = (dynamic?)FNumber - (dynamic?)obj;
    }

    public void Clear()
    {
        _isOn = false;
        FNumber = new();
    }

    public string GetState()
    {
        return _isOn ? "On" : "Off";
    }

    public T GetNumber()
    {
        return FNumber;
    }
}
