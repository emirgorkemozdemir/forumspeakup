namespace Voice_Form.Extra_Classes
{
    public class VpnChecker
    {
        private const string ContactEmail = "emirozdemirdeneme@gmail.com";
        private const double MaxProbability = 0.99;
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

        public static int CheckIP(string ip)
        {
            using (HttpClient client = new HttpClient { Timeout = Timeout })
            {
                try
                {
                    string url = $"http://check.getipintel.net/check.php?ip={ip}&contact={ContactEmail}";
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        double content = double.Parse(response.Content.ReadAsStringAsync().Result);

                        if (content < 0)
                        {
                            Console.Error.WriteLine("An error occurred while querying GetIPIntel");
                        }

                        if (content > MaxProbability)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("An error occurred while querying GetIPIntel");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return 0; // Default return value in case of error
        }
    }
}
