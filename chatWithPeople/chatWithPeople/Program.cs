using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace chatWithPeople
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> currentMessages = new List<string>();
            currentMessages.Add("Hi. Use data.json to send messages.");
            string lmid = "0";
            float id = MathF.Floor(new Random().Next(1, 1000));
            string username = "Guest";
            string baseLink = Other.getUserString();
            //Override
            id = 1;
            username = "???";
            
            //End Override

            void setColor(ConsoleColor color) {
                Console.ForegroundColor = color;
            }

            void start()
            {
                Console.WriteLine("Entering Codes And Encrypting Chat...");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Testing Uri.EscapeUriString()");
                Console.WriteLine(Uri.EscapeUriString("Test ∂+å_ç-π;∑"));
                if (Uri.EscapeUriString("Test ∂+å_ç-π;∑") == "Test%20%E2%88%82+%C3%A5_%C3%A7-%CF%80;%E2%88%91")
                {
                    Console.WriteLine("Test Successful.");
                }
                else
                {
                    Console.WriteLine("Test Failed. Stopping Program...");
                    return;
                }
                Console.WriteLine("Connection Test. Using https://www.github.com for connection...");
                try {
                    WebClient tc = new WebClient();
                    String netConnection = tc.DownloadString("https://www.github.com");
                    if (netConnection.Length > 0) {
                        Console.WriteLine("Connection Test Successful.");
                    } else {
                        throw new SystemException("DownloadString failed. Internet or other issue...");
                    }
                } catch (Exception e) {
                    setColor(ConsoleColor.Red);
                    Console.WriteLine(e);
                    Console.WriteLine("\nSomething happened. You're probally not connected to the internet. Stopping...");
                    return;
                }
                Console.WriteLine("Ready to Chat!");
                chat();
            }

            void chat()
            {
                WebClient client = new WebClient();
                while (true) 
                {
                    string nonParsedJSON = client.DownloadString(baseLink);
                    string lengthOfMessages = client.DownloadString(baseLink + "lengthofmessages").Replace(".", "");
                    dynamic items = Newtonsoft.Json.JsonConvert.DeserializeObject(nonParsedJSON);
                    for (int i = 0; i < Convert.ToInt32(lengthOfMessages); i++)
                    {
                        Console.WriteLine(items[i].m);
                        bool cm = false;
                        for (int j = 0; j < currentMessages.Count; j++)
                        {
                            if (currentMessages[j] == items[i].m.ToString())
                            {
                                cm = true;
                            }
                        }
                        if (!cm)
                        {
                            currentMessages.Add(items[i].m.ToString());
                            Console.WriteLine(items[i].m);
                        }
                    }

                    string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data.json"));
                    dynamic datajson = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    if (datajson["messageId"] != lmid) {
                        lmid = datajson["messageId"];
                        string newM = client.DownloadString(baseLink + username + "/" + id + "/" + datajson["message"]);
                        if (newM == "ewair423qur9fc8yqwe89r7cg0236o") { return; }
                    }

                    System.Threading.Thread.Sleep(100);
                }
            }



            start();
        }
    }
}
