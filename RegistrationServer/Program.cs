using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RegistrationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Working();
            Console.ReadKey();
        }

        public async static void Working()
        {
            await Listen("http://localhost:80/");
        }

        private static async Task CreateDatabase()
        {
            using (RegistrationDbContext context = new RegistrationDbContext()) { }
        }

        private static async Task Listen(string prefixHttpListener)
        {
            using (HttpListener listener = new HttpListener())
            {
                try
                {
                    listener.Prefixes.Add(prefixHttpListener);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                listener.Start();
                Console.WriteLine("Server started work");

                while (true)
                {
                    var context = await listener.GetContextAsync();
                    var request = context.Request;

                    string[] urlRequest = request.RawUrl.Split("/");
                    var response = context.Response;
                    if (urlRequest[1] == "users")
                    {
                        var jsonData = await GetPostData(request.InputStream, request.ContentEncoding);
                        string incomingString = jsonData["Data"].ToString();
                        string[] incomingData = incomingString.Split("//");
                        string answer = String.Empty;
                        Console.WriteLine(incomingString);
                        if (urlRequest[2] == "auth")
                        {
                            string login = incomingData[0];
                            string password = incomingData[1];
                            using (RegistrationDbContext contextDb = new RegistrationDbContext())
                            {
                                var entUser = contextDb.Users.Where(x => x.Nickname.Equals(login) || x.PhoneNumber.Equals(login)).FirstOrDefault();
                                if (entUser == null)
                                {
                                    answer = "false//Неверные данные!";
                                }
                                else if (entUser.Password.Equals(password))
                                {
                                    answer = "true//Вход выполнен успешно!";
                                }
                                else
                                {
                                    answer = "false//Неверные данные!";
                                }
                            }
                            var answerBytes = System.Text.Encoding.UTF8.GetBytes(answer);
                            response.OutputStream.Write(answerBytes, 0, answerBytes.Length);
                            response.Close();

                        }
                        else if (urlRequest[2] == "signup")
                        {
                            User user = new User // {nickname}//{password}//{firstName}//{secondName}//{phoneNumber}//
                            {
                                FirstName = incomingData[2],
                                Nickname = incomingData[0],
                                Password = incomingData[1],
                                PhoneNumber = incomingData[4],
                                SecondName = incomingData[3],
                            };
                            DbManager manager = new DbManager();
                            if (await manager.IfExistsByNickname(user.Nickname))
                            {
                                answer = "false//Этот никнейм уже занят!";
                            }
                            else if (await manager.IfExistsByPhoneNumber(user.PhoneNumber))
                            {
                                answer = "false//Этот номер телефона уже занят!";
                            }
                            else
                            {
                                await manager.AddUser(user);
                                answer = "true//Аккаунт успешно зарегистрирован!";
                            }
                            Console.WriteLine($"{user.Nickname}||{user.Password}||{user.SecondName}||{user.PhoneNumber}||{user.Nickname}");
                            var answerBytes = System.Text.Encoding.UTF8.GetBytes(answer);
                            response.OutputStream.Write(answerBytes, 0, answerBytes.Length);
                            response.Close();
                        }
                    }
                }
            }
        }

        private static async Task<JObject> GetPostData(Stream stream, System.Text.Encoding encoding) // вернет json результат 
        {
            JObject result;
            using (StreamReader reader = new StreamReader(stream, encoding))
            {
                result = JObject.Parse(await reader.ReadToEndAsync());
            }
            return Task.FromResult(result).Result;
        }
    }
}
