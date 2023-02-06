using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace lab15.Models;
public class AbonentList : INotifyPropertyChanged
{
    private readonly FileStream _abonentsFile;
    private readonly Dictionary<string, List<int>> _abonents = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public ILookup<string, List<int>> Abonents => _abonents.ToLookup(x => x.Key, x => x.Value);

    public AbonentList(string path)
    {
        if (File.Exists(path))
        {
            ReadAbonentsFromFile(path);
        }
        _abonentsFile = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }

    ~AbonentList()
    {
        _abonentsFile.Close();
    }

    private void ReadAbonentsFromFile(string path)
    {
        var thread = new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;

            lock (_abonents)
            {
                using var reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    var parsedLine = line.Split(":");
                    var name = parsedLine[0];
                    var phoneNumbers = parsedLine[1].Split(", ", StringSplitOptions.RemoveEmptyEntries);
                    _abonents.Add(name, new());
                    foreach (var item in phoneNumbers)
                    {
                        _abonents[name].Add(int.Parse(item));
                    }
                }
            }
        });

        thread.Start();
        thread.Join();
    }

    private void WriteAbonentsToFile()
    {
        var thread = new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;

            _abonentsFile.SetLength(0);
            _abonentsFile.Flush();
            lock (_abonents)
            {
                foreach (var item in _abonents)
                {
                    var name_ = item.Key;
                    var phones_ = string.Join(", ", item.Value);
                    var encodedRegistry = new UTF8Encoding(true).GetBytes($"{name_}:{phones_}\n");
                    _abonentsFile.Write(encodedRegistry, 0, encodedRegistry.Length);
                }
            }
            _abonentsFile.Flush();
        });

        thread.Start();
    }

    public void Add(string name, int phone)
    {
        _ = _abonents.TryAdd(name, new());
        if (_abonents[name].Contains(phone))
        {
            return;
        }
        _abonents[name].Add(phone);
        _abonents[name].Sort();

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abonents)));
    }

    public void Edit(string name, int phone, string newName, int newPhone)
    {
        if (!_abonents.ContainsKey(name))
        {
            return;
        }

        if (name == newName && phone == newPhone)
        {
            return;
        }

        if (name == newName)
        {
            _abonents[name].Remove(phone);
            _abonents[name].Add(newPhone);
            _abonents[name].Sort();
        }

        if (phone == newPhone)
        {
            _abonents.TryAdd(newName, _abonents[name]);
            _abonents.Remove(name);
        }
    }

    public void Remove(string name, int phone)
    {
        List<int>? registry;
        if (_abonents.TryGetValue(name, out registry))
        {
            registry.Remove(phone);
            registry.Sort();
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abonents)));
    }

    public void Clear()
    {
        _abonents.Clear();

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Abonents)));
    }

    public void Save()
    {
        WriteAbonentsToFile();
    }

    public ILookup<string, List<int>> Find(string name)
    {
        var foundedAbonents = new Dictionary<string, List<int>>();

        if (name != string.Empty)
        {
            foreach (var item in _abonents)
            {
                if (Regex.IsMatch(item.Key, $"(.?)+{name}(.?)*"))
                {
                    foundedAbonents.TryAdd(item.Key, item.Value);
                }
            }
        }

        return foundedAbonents.ToLookup(x => x.Key, x => x.Value);
    }
}
