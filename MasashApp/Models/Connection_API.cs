using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public class Connection_API
    {
        public const string ip = "192.168.31.169";
        const int port = 5682;
        public const int HttpPort = 4362;

        TcpClient client = new TcpClient();

        public StreamReader? Reader = null;
        public StreamWriter? Writer = null;
        public NetworkStream? NetStream = null;

        private readonly HttpClient HttpClient = new HttpClient();

        public bool isConnect = false;

        public string GetPathHttp(string Url)
        {
            return $"http://{ip}:{HttpPort}{Url}";
        }

        public Connection_API()
        {
            try
            {
                InitGUID();
                client.Connect(ip, port); //подключение клиента
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                NetStream = client.GetStream();
                if (Writer is null || Reader is null) return;
                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(Reader));
                isConnect = true;
            }
            catch (Exception ex)
            {
                isConnect = false;
                Console.WriteLine($"ERROR:{ex.Message}");
            }
        }
        //response = {StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.NSUrlSessionHandler+NSUrlSessionDataTaskStreamContent, Headers:
        public async Task<Stream> Upload(string actionUrl, string paramString, string pathFile, string filename)
        {
            HttpContent stringContent = new StringContent(paramString);
            HttpContent fileStreamContent = new StreamContent(File.OpenRead(pathFile));
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            using (var http_client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(stringContent, "param1", "param1");
                formData.Add(fileStreamContent, name: "filedata", fileName: filename);
                var response = await http_client.PostAsync(GetPathHttp(actionUrl), formData);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return await response.Content.ReadAsStreamAsync();
            }
        }

        public async void InitGUID()
        {
            string mainDir = FileSystem.Current.AppDataDirectory;
            string path = mainDir + "/GUID.txt";
            string ID = "";
            if (StaticData.GUID_User == null)
            {
                if (!File.Exists(path))
                {
                    Guid guid = Guid.NewGuid();
                    ID = guid.ToString();
                    await WriteTextFile(path, ID);
                }
                else
                {
                    string file = await ReadTextFile(path);
                    if (file.IndexOf("\n") > -1)
                    {
                        file = file.Substring(0, file.IndexOf("\n"));
                    }
                    ID = file;
                }
                StaticData.GUID_User = ID;

                await SendMessageAsync("InitUser");

                string User_res = await POST("/getInfoUser", new Dictionary<string, string> 
                {
                    { "id", ID }
                });

                if(User_res == "error")
                {
                    StaticData.isAuth = false;
                }
                else
                {
                    StaticData.isAuth = true;

                    Dictionary<string, string> parsing = StaticData.API.ParsingData(User_res);

                    StaticData.User = new Model_user()
                    {
                        PathImg = parsing["pathImg"],
                        Name = parsing["name"],
                        Email = parsing["email"],
                        Date_birthday = StaticData.API.GetDateFromString(parsing["date_birthday"]),
                        Date_reg = StaticData.API.GetDateFromString(parsing["date_reg"]),
                        IsAdmin = Convert.ToBoolean(parsing["isAdmin"]),
                        IsMaster = Convert.ToBoolean(parsing["isMaster"]),
                        Phone = parsing["phone"]
                    };
                    StaticData.linkMainPage.UpdateImgUser();
                }
            }
        }

        public async Task<string> POST(string Url, Dictionary<string, string> body)
        {
            try
            {
                var content = new FormUrlEncodedContent(body);

                var response = await HttpClient.PostAsync($"http://{ip}:{HttpPort}{Url}", content);

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "error";
            }
            
        }
        //result = "{\"_id\":\"674339cbfb5adad57222c94a\",\"id\":\"628be78b-03b0-4214-af85-9593bd76125e\",\"Date_birthday\":\"\",\"Date_reg\":\"\",\"Email\":\"\",\"Name\":\"\",\"Phone\":\"380931029054\",\"IsAdmin\":false,\"IsMaster\":false}"
        public Dictionary<string, string> ParsingData(string data)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string temp = data.Substring(1, data.Length - 1);
            while (temp.Length > 0)
            {
                string key = GetValueString(temp,"\"", "\"");
                temp = temp.Remove(0, temp.IndexOf(":") + 1);
                string value = "";
                string symbol = "";
                if (temp.Length > 0)
                {
                    if (temp[0] == '"')
                    {
                        symbol = "\"";
                        value = GetValueString(temp, "\"", "\"");
                    }
                    else if(temp[0] == '{')
                    {
                        symbol = "}";
                        value = GetValueString(temp, "{", "}");
                    }
                    else if (temp[0] == '[')
                    {
                        symbol = "]";
                        value = GetValueLastString(temp, "[", "]");
                    }
                    else if (temp[0] == 'f')
                    {
                        symbol = ",";
                        value = "false";
                    }
                    else if (temp[0] == 't')
                    {
                        symbol = ",";
                        value = "true";
                    }
                }
                if(symbol != ",")
                {
                    temp = temp.Remove(0, 1);
                    temp = temp.Remove(0, temp.IndexOf(symbol) + 1);
                    temp = temp.Remove(0, 1);
                }
                else
                {
                    temp = temp.Remove(0, temp.IndexOf(symbol) + 1);
                }
                result[key] = value;
                if(temp.IndexOf(":") < 0)
                {
                    break;
                }
            }

            return result;
        }

        public List<Dictionary<string, string>> ParsingListData(string data)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            string temp = GetValueLastString(data, "[", "]");
            while (temp.Length > 0)
            {
                string value = GetValueString(temp, "{", "}");
                result.Add(ParsingData("{"+value+"}"));
                temp = temp.Remove(0, temp.IndexOf('}') + 1);
                if(temp.IndexOf('{') > -1)
                {
                    temp = temp.Remove(0, 1);
                }
            }
            return result;
        }

        public DateTime GetDateFromString(string date)
        {
            if(date == "")
            {
                return DateTime.MinValue;
            }
            var data = date.Split(" ");
            var data_date = data[0].Split(".");
            var data_time = data[1].Split(":");
            int day = Convert.ToInt32(data_date[0]);
            int month = Convert.ToInt32(data_date[1]);
            int year = Convert.ToInt32(data_date[2]);
            int hour = Convert.ToInt32(data_time[0]);
            int minute = Convert.ToInt32(data_time[1]);
            int seconds = Convert.ToInt32(data_time[2]);
            return new DateTime(year, month, day, hour, minute, seconds);
        }

        public string GetValueString(string target, string open, string close)
        {
            string temp = target.Substring(target.IndexOf(open) + 1);
            temp = temp.Substring(0, temp.IndexOf(close));
            return temp;
        }
        public string GetValueLastString(string target, string open, string close)
        {
            string temp = target.Substring(target.IndexOf(open) + 1);
            temp = temp.Substring(0, temp.LastIndexOf(close));
            return temp;
        }

        public async Task<string> ReadTextFile(string filePath)
        {
            try
            {
                using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
                using StreamReader reader = new StreamReader(fileStream);

                return await reader.ReadToEndAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"ERROR:{ex.Message}");
                return null;
            }
        }

        public async Task WriteTextFile(string filePath, string text)
        {
            try
            {
                // полная перезапись файла 
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    await writer.WriteLineAsync(text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{ex.Message}");
            }
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                await Writer.WriteAsync($"{StaticData.GUID_User}:{message}");
                await Writer.FlushAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"ERROR:{ex.Message}");
            }
        }

        public async Task SendFileAsync(FileResult fileResult)
        {
            try
            {
                var file_bytes = await File.ReadAllBytesAsync(fileResult.FullPath);
                await NetStream.WriteAsync(file_bytes);
                await NetStream.FlushAsync();
                Thread.Sleep(60 * 2);
                await NetStream.FlushAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public void Close()
        {
            Writer?.Close();
            Reader?.Close();
        }

        // получение сообщений
        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;
                    Console.WriteLine($"======================================================={message}======================================================================");//вывод сообщения
                    var values = message.Split(":");
                    if (values.Length == 2)
                    {
                        string nick_name = values[0];
                        string command = values[1];
                        if (command.IndexOf("|") > -1)
                        {
                            var v = command.Split("|");
                            string Action_str = v[0];
                            string Data = v[1];

                            if (Action_str.IndexOf("AddUser") > -1)
                            {

                            }
                            else if (Action_str.IndexOf("SetCountStartSum") > -1)
                            {

                            }

                        }
                        else if (command.IndexOf("Loaded") > -1)
                        {

                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR:{ex.Message}");
                }
            }
        }
    }
}
