using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using lab15.Models;
using lab15.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace lab15.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private readonly AbonentList _abonents;

    private string _abonentName = "";
    private string _abonentPhone = "";

    public struct Abonent
    {
        public string Name
        {
            get; set;
        }

        public int Number
        {
            get; set;
        }
    }

    public ReadOnlyObservableCollection<Abonent> Abonents
    {
        get
        {
            ILookup<string, List<int>> abonents;
            if (string.IsNullOrWhiteSpace(FindQuery))
            {
                abonents = _abonents.Abonents;
            }
            else
            {
                abonents = _abonents.Find(FindQuery);
            }

            var listAbonents = new ObservableCollection<Abonent>();
            foreach (var abonent in abonents)
            {
                foreach (var phone in abonent.Single())
                {
                    listAbonents.Add(new Abonent() { Name = abonent.Key, Number = phone });
                }
            }
            return new ReadOnlyObservableCollection<Abonent>(listAbonents);
        }
    }

    public string AbonentName
    {
        get => _abonentName;
        set
        {
            _abonentName = value;
            OnPropertyChanged(nameof(AbonentName));
        }
    }
    public string AbonentPhone
    {
        get => _abonentPhone;
        set
        {
            _abonentPhone = value;
            OnPropertyChanged(nameof(AbonentPhone));
        }
    }

    public string FindQuery
    {
        get; set;
    } = "";

    public object? SelectedItem
    {
        get; set;
    }

    public MainViewModel()
    {
        _abonents = new(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "abonents.list");
        _abonents.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == "Abonents")
            {
                OnPropertyChanged(nameof(Abonents));
            }
        };
    }

    public void AddHandler(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AbonentName) || string.IsNullOrWhiteSpace(AbonentPhone))
        {
            return;
        }

        int parsedInt;
        if (!int.TryParse(AbonentPhone, out parsedInt))
        {
            return;
        }

        _abonents.Add(AbonentName, parsedInt);
    }

    public void EditHandler(object sender, DataGridCellEditEndingEventArgs e)
    {
        var columnName = e.Column.Header;
        var textBox = (TextBox)e.EditingElement;

        var abonent = (Abonent)e.Row.DataContext;

        switch (columnName)
        {
            case "Номер":
                int parsedAbonentNumber;
                if (!int.TryParse(textBox.Text, out parsedAbonentNumber))
                {
                    return;
                }
                _abonents.Edit(abonent.Name, abonent.Number, abonent.Name, parsedAbonentNumber);
                break;
            case "Имя":
                _abonents.Edit(abonent.Name, abonent.Number, textBox.Text, abonent.Number);
                break;
        }
    }

    public void RemoveHandler(object sender, RoutedEventArgs e)
    {
        if (SelectedItem == null)
        {
            return;
        }

        var abonent = (Abonent)SelectedItem;

        _abonents.Remove(abonent.Name, abonent.Number);
    }

    public void ClearHandler(object sender, RoutedEventArgs e)
    {
        _abonents.Clear();
    }

    public async void FindHandler(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        async Task<bool> UserKeepsTyping()
        {
            var txt = ((TextBox)sender).Text;
            await Task.Delay(500);
            return txt != ((TextBox)sender).Text;
        }

        if (await UserKeepsTyping())
        {
            return;
        }

        FindQuery = sender.Text;
        OnPropertyChanged(nameof(Abonents));
    }

    public void SaveHandler(object sender, RoutedEventArgs e)
    {
        _abonents.Save();
    }
}
