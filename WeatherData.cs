using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherReportApp
{
    public class WeatherData
    {
        public Main main { get; set; }
        public Wind wind { get; set; }
        public string name { get; set; }

        public class Main
        {
            public double temp { get; set; }
            public double pressure { get; set; }
            public int humidity { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
        }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter city name:");
                string cityName = Console.ReadLine();

                string apiKey = "6d4912d358923c2005bbcfe990e76ebb"; // Replace with your correct API key
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";

                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: Unable to fetch weather data (Status code: {response.StatusCode})");
                    return;
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                if (weatherData == null || weatherData.main == null || weatherData.wind == null)
                {
                    Console.WriteLine("Error: Unable to parse weather data.");
                    return;
                }

                Console.WriteLine($"Weather in {weatherData.name}:");
                Console.WriteLine($"Temperature: {weatherData.main.temp}°C");
                Console.WriteLine($"Pressure: {weatherData.main.pressure} hPa");
                Console.WriteLine($"Humidity: {weatherData.main.humidity}%");
                Console.WriteLine($"Wind Speed: {weatherData.wind.speed} m/s");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nAn error occurred:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
