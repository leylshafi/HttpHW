using Client.Command;
using Client.Model;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace Client;

public partial class MainWindow : Window
{
    public ICommand GetCommand { get; set; }
    public ICommand PutCommand { get; set; }
    public ICommand PostCommand { get; set; }

    private HttpClient httpClient;
    

    public int Value
    {
        get { return (int)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(int), typeof(MainWindow));



    public string Key
    {
        get { return (string)GetValue(KeyProperty); }
        set { SetValue(KeyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register("Key", typeof(string), typeof(MainWindow));

    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;

        httpClient = new();

        GetCommand = new RelayCommand(ExecuteGetCommand);
        // PostCommand = new RelayCommand(ExecutePostCommand);
        // PutCommand = new RelayCommand(ExecutePutCommand);
    }
    

    private async void ExecuteGetCommand(object? obj)
    {
        if (Key is not null)
        {
            var response = await httpClient.GetAsync($"http://localhost:27001/?key={Key}");

            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var keyValue = JsonSerializer.Deserialize<KeyValue>(content);
                Value = keyValue.Value;
                Key = string.Empty;
                Key += keyValue.Key;
            }
            else
                MessageBox.Show(response.StatusCode.ToString());
        }
    }
}
