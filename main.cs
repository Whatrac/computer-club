using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Program
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CompClub compClub = new CompClub(8);
            compClub.Work();
        }
    }

    class CompClub
    {
        private int _money = 0;

        private List<Comp> _comps = new List<Comp>();
        public Queue<Client> _clients = new Queue<Client>();

        public CompClub(int countComps)
        {
            Random rand = new Random();
            for (int i = 0; i < countComps; i++)
            {
                _comps.Add(new Comp(rand.Next(1, 11)));
            }

            CreateNewClient(25, rand);
        }

        public void CreateNewClient(int count, Random rand)
        {
            for (int i = 0; i < count; i++)
            {
                _clients.Enqueue(new Client(rand.Next(50, 201), rand));
            }
        }

        public void Work()
        {
            bool IsOut = true;
            while (_clients.Count > 0 && IsOut)
            {
                Client NewClient = _clients.Dequeue();
                System.Console.WriteLine($"Добро пожаловать в комп клуб! Баланс нашего клуба - {_money} рублей. Ждем нового клиента. Для выхода напишите пароль: 333");
                System.Console.WriteLine($"У нас новый клиент и он хочет купить {NewClient.Minutes} минут");
                ShowAllCompsStats();

                System.Console.WriteLine("\nВыберите компьютер под номером: ");
                string userInput = Console.ReadLine();
                if (userInput == "333")
                {
                    IsOut = false;
                    System.Console.WriteLine("Завершение работы");
                    Console.ReadKey();
                    Console.Clear();

                }
                else
                {

                    if (int.TryParse(userInput, out int compIndex))
                    {
                        compIndex = compIndex - 1;

                        if (compIndex >= 0 && compIndex < _comps.Count)
                        {
                            if (_comps[compIndex].IsOpen)
                            {
                                System.Console.WriteLine("Вы пытаетесь посадить клиента за занятый компьютер.");
                            }
                            else
                            {
                                if (NewClient.ChekSolvensy(_comps[compIndex]))
                                {
                                    _comps[compIndex].BecamTaken(NewClient);
                                    _money += NewClient.Pay();
                                    System.Console.WriteLine($"Клиент сел за компьютер №{compIndex + 1}. Осталось минут: {NewClient.Minutes}.");
                                    _money += NewClient.Pay();
                                    _comps[compIndex].BecamTaken(NewClient);
                                    System.Console.WriteLine($"Баланс клуба: {_money}");
                                }
                                else
                                {
                                    System.Console.WriteLine("У клиента недостаточно денег для оплаты. Клиент ушел.");
                                }
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("Вы сами не знаете за какой комп хотите посадить клиента.");
                        }
                    }
                    else
                    {
                        CreateNewClient(1, new Random());
                        System.Console.WriteLine("Неверный формат ввода. Попробуйте снова.");
                    }
                    System.Console.WriteLine("чтобы перейти к следующему клиенту нажмите любую клавишу...");
                    Console.ReadKey();
                    Console.Clear();
                    SpendOneMinute();
                }

            }
        }

        public void ShowAllCompsStats()
        {
            System.Console.WriteLine("\nСписок компьютеров в клубе:");
            for (int i = 0; i < _comps.Count; i++)
            {
                Console.Write(i + 1 + " - ");
                _comps[i].ShowState();
            }
        }

        private void SpendOneMinute()
        {
            foreach(var comp in _comps)
            {
                comp.SpendOneMinute();
            }
        }
        


    }

    class Comp
    {
        private Client _client;
        private int _minutesRemain;
        public int PricePerMinute { get; private set; }
        public bool IsOpen
        {
            get
            {
                return _minutesRemain > 0;
            }
        }
        public void Open()
        {

        }

        public Comp(int pricePerMinute)
        {
            PricePerMinute = pricePerMinute;

        }

        public void BecamTaken(Client client)
        {
            _client = client;
            _minutesRemain = client.Minutes;
        }

        public void BecamFree()
        {
            _client = null;
        }

        public void SpendOneMinute()
        {
            _minutesRemain--;
        }

        public void ShowState()
        {
            if (IsOpen)
            {
                Console.WriteLine($"Компьютер занят клиентом, осталось минут: {_minutesRemain}");
            }
            else
            {
                Console.WriteLine($"Компьютер свободен, цена за минуту {PricePerMinute}");
            }
        }
    }

    class Client
    {
        private int _money;
        private int _moneyToPay;
        public int Minutes { get; private set; }

        public Client(int money, Random rand)
        {
            _money = money;
            Minutes = rand.Next(1, 61);
        }

        public bool ChekSolvensy(Comp comp)
        {
            _moneyToPay = Minutes * comp.PricePerMinute;
            if (_money >= _moneyToPay)
            {
                return true;
            }
            else
            {
                _moneyToPay = 0;
                return false;
            }
        }

        public int Pay()
        {
            _money -= _moneyToPay;
            return _moneyToPay;
        }

    }
}

    
    
    

