namespace lab11;
public class Memory<T> where T : new()
{
    private T FNumber = new();
    public T Number => FNumber;

    private bool _isOn;
    public bool IsOn => _isOn;

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
}
