using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Hier müssen die API Daten eingegeben werden, welche auf der Website zu finden sind
        private readonly string apiKey = "bdc3deebe51e80fe56b280899ea94922";

        private string requestURL = "https://api.openweathermap.org/data/2.5/weather";


        public MainWindow()
        {
            InitializeComponent();

            UpdateUI("Dortmund");
        }

        public void UpdateUI(string city)
        {
            //Funktionsaufruf mit Stadtangabe
            WeatherMapResponse result = GetWeatherData(city);

            //Standartbild
            string finalImage = "sun.png";

            //Je nach Wetterlage wird ein passendes Bild als Hintergrund gesetzt
            if (result.weather[0].main.ToLower().Contains("cloud"))
            {
                finalImage = "cloud.png";
            }
            else if (result.weather[0].main.ToLower().Contains("rain"))
            {
                finalImage = "rain.png";
            }
            else if (result.weather[0].main.ToLower().Contains("snow"))
            {
                finalImage = "snow.png";
            }

            backgroundImage.ImageSource = new BitmapImage(new Uri("Images/" + finalImage, UriKind.Relative));

            //Temperaturanzeige
            labelTemperature.Content = result.main.temp + "°C";

            labelInfo.Content = result.weather[0].main;
        }



        public WeatherMapResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient();



            //Hier wird die finale URL zusammengebaut
            var finalUri = requestURL + "?q=" + city + "&appid=" + apiKey + "&units=metric";

            //Hiermit holen wir uns die Antwort vom Server
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUri).Result;

            //Hier speichern wir die Antwort lesbar als Zeichenkette
            string response = httpResponse.Content.ReadAsStringAsync().Result;

            //Hier wird die JSON-Antwort in eine C# verwertbare Antwort deserialisiert
            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);

            return weatherMapResponse;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = textBoxQuery.Text;
            UpdateUI(query);
        }
    }
}
