using c3s30792;

class Program
{
    static List<Ship> ships = new List<Ship>();
    static List<Container> freeContainers = new List<Container>();
    static List<Container> allContainers = new List<Container>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n\nLista kontenerowców:");
            for (int i = 0; i < ships.Count; i++)
                Console.WriteLine($"{i}. {ships[i]}");
            Console.WriteLine("\nLista kontenerów:");
            for (int i = 0; i < allContainers.Count; i++)
                Console.WriteLine($"{i}. {allContainers[i]}");
            Console.WriteLine("\nMożliwe akcje:");
            Console.WriteLine("1. Stwórz kontenerowiec");
            if (ships.Any()){
                Console.WriteLine("2. Usuń kontenerowiec");
                Console.WriteLine("3. Dodaj kontener");
            }
            if (allContainers.Any())
            {
                Console.WriteLine("4. Usuń kontener");
                Console.WriteLine("5. Załaduj ładunek do kontenera");
                Console.WriteLine("6. Rozładuj kontener");
            }
            if (ships.Any() && freeContainers.Any())
            {
                Console.WriteLine("7. Załaduj kontener na statek");
                Console.WriteLine("8. Załaduj listę kontenerów do statku");
            }
            if (ships.Any(s => s.containers.Any()))
            {
                Console.WriteLine("9. Usuń kontener ze statku");
                if (freeContainers.Any())
                {
                    Console.WriteLine("10. Zamień kontener na statku");
                }

                if (ships.Count > 1)
                {
                    Console.WriteLine("11. Przenieś kontener między statkami");
                }
            }
            Console.WriteLine("0. Wyjście");
            
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Nazwa statku: ");
                    string name = Console.ReadLine();
                    Console.Write("Maksymalna liczba kontenerów: ");
                    int maxContainers = int.Parse(Console.ReadLine());
                    Console.Write("Maksymalna waga: ");
                    double maxWeight = double.Parse(Console.ReadLine());
                    Console.Write("Maksymalna prędkość w węzłach: ");
                    int maxSpeed = int.Parse(Console.ReadLine());
                    ships.Add(new Ship(name, maxContainers, maxWeight, maxSpeed));
                    break;
                case "2":
                {
                    Console.Write("Wybierz statek do usunięcia: ");
                    int shipIndex = int.Parse(Console.ReadLine());
                    if (shipIndex < 0 || shipIndex >= ships.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }

                    Ship ship = ships[shipIndex];
                    ships.Remove(ship);
                }
                    break;
                case "3":
                    Console.Write("Typ kontenera (L, G, C): ");
                    string type = Console.ReadLine();
                    Console.Write("Wysokość: ");
                    double height = double.Parse(Console.ReadLine());
                    Console.Write("Waga własna: ");
                    double weight = double.Parse(Console.ReadLine());
                    Console.Write("Głębokość: ");
                    double depth = double.Parse(Console.ReadLine());
                    Console.Write("Maksymalna waga: ");
                    double maxLoad = double.Parse(Console.ReadLine());
                    double pressure = 0.0;
                    string productType = "";
                    double temp = 0.0;
                    if (type == "G")
                    {
                        Console.Write("Ciśnienie: ");
                        pressure = double.Parse(Console.ReadLine());
                    } else if (type == "C")
                    {
                        Console.Write("Typ produktu: ");
                        productType = Console.ReadLine();
                        Console.Write("Temperatura: ");
                        temp = double.Parse(Console.ReadLine());
                    }

                    Container newContainer = type == "L" ? new LContainer(height, weight, depth, maxLoad, false) :
                        type == "G" ? new GContainer(height, weight, depth, maxLoad, pressure) :
                        new ChContainer(height, weight, depth, maxLoad, productType, temp);
                    freeContainers.Add(newContainer);
                    allContainers.Add(newContainer);
                    break;
                case "4":
                {
                    Console.Write("Wybierz kontener do usunięcia: ");
                    int containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= allContainers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container container = allContainers[containerIndex];
                    allContainers.Remove(container);
                    if (freeContainers.Contains(container)) freeContainers.Remove(container);
                    foreach (Ship s in ships)
                        if (s.containers.Contains(container))
                        {
                            s.RemoveContainer(container.SerialNumber);
                            break;
                        }
                }
                    break;
                case "5":
                {
                    Console.Write("Wybierz kontener do którego załadować ładunek: ");
                    int containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= allContainers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container container = allContainers[containerIndex];
                    if (container.SerialNumber.Contains("C"))
                    {
                        Console.Write("Podaj typ ładunku do załadowania: ");
                        string loadType = Console.ReadLine();
                        ChContainer r = container as ChContainer;
                        if (loadType != r.ProductType)
                        {
                            Console.WriteLine("Niepoprawny typ ładunku!");
                            break;
                        }
                    }
                    Console.Write("Podaj masę ładunku do załadowania: ");
                    double loadWeight = double.Parse(Console.ReadLine());
                    container.Load(loadWeight);
                }
                    break;
                    
                case "6":
                {
                    Console.Write("Wybierz kontener do rozładowania: ");
                    int containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= allContainers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container container = allContainers[containerIndex];
                    container.Unload();
                }
                    break;
                case "7":
                    Console.WriteLine("Wybierz statek:");
                {
                    int shipIndex = int.Parse(Console.ReadLine());
                    if (shipIndex < 0 || shipIndex >= ships.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Ship ship = ships[shipIndex];
                    Console.Write("Wybierz kontener do załadowania na statek: \n");
                    for (int i = 0; i < freeContainers.Count; i++)
                        Console.WriteLine($"{i}. {freeContainers[i].SerialNumber}");
                    int containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= freeContainers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container container = freeContainers[containerIndex];
                    ship.LoadContainer(container);
                    freeContainers.Remove(container);
                }
                    break;
                case "8":
                {
                    Console.WriteLine("Wybierz statek:");
                    int shipIndex = int.Parse(Console.ReadLine());
                    if (shipIndex < 0 || shipIndex >= ships.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Ship ship = ships[shipIndex];
                    Console.Write("Podaj początek zakresu indeksów kontenerów: ");
                    int startRange = int.Parse(Console.ReadLine());
                    Console.Write("Podaj koniec zakresu indeksów kontenerów: ");
                    int endRange = int.Parse(Console.ReadLine());

                    if (startRange < 0 || endRange >= freeContainers.Count || startRange > endRange)
                    {
                        Console.WriteLine("Niepoprawny zakres!");
                        break;
                    }
                    List<Container> selectedContainers = freeContainers.GetRange(startRange, endRange - startRange + 1);
                    try{
                        ship.LoadContainers(selectedContainers);
                    } catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                    freeContainers.RemoveAll(selectedContainers.Contains);
                }
                    break;
                case "9":
                    Console.WriteLine("Wybierz statek:");
                {
                    int shipIndex = int.Parse(Console.ReadLine());
                    if (shipIndex < 0 || shipIndex >= ships.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Ship ship = ships[shipIndex];
                    if (!ship.containers.Any())
                    {
                        Console.WriteLine("Statek jest pusty!");
                        break;
                    }
                    Console.Write("Wybierz kontener do usunięcia: \n");
                    for (int i = 0; i < ship.containers.Count; i++)
                        Console.WriteLine($"{i}. {ship.containers[i].SerialNumber}");
                    int containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= ship.containers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container container = ship.containers[containerIndex];
                    ship.RemoveContainer(container.SerialNumber);
                    freeContainers.Add(container);
                }
                    break;
                case "10":
                    {
                    Console.WriteLine("Wybierz statek:");
                    int shipIndex = int.Parse(Console.ReadLine());
                    if (shipIndex < 0 || shipIndex >= ships.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Ship ship = ships[shipIndex];
                    if (!ship.containers.Any())
                    {
                        Console.WriteLine("Statek jest pusty!");
                        break;
                    }
                    Console.Write("Wybierz kontener do zamiany: \n");
                    for (int i = 0; i < ship.containers.Count; i++)
                        Console.WriteLine($"{i}. {ship.containers[i].SerialNumber}");
                    int containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= ship.containers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container oldContainer = ship.containers[containerIndex];
                    freeContainers.Add(oldContainer);
                    Console.Write("Wybierz nowy kontener: \n");
                    for (int i = 0; i < freeContainers.Count; i++)
                        Console.WriteLine($"{i}. {freeContainers[i].SerialNumber}");
                    containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= freeContainers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container container = freeContainers[containerIndex];
                    try
                    {
                        ship.ReplaceContainer(oldContainer.SerialNumber, container);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                    freeContainers.Remove(oldContainer);
                }
                    break;
                case "11":
                    {
                    Console.WriteLine("Wybierz statek źródłowy:");
                    int shipIndex = int.Parse(Console.ReadLine());
                    if (shipIndex < 0 || shipIndex >= ships.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Ship ship = ships[shipIndex];
                    if (!ship.containers.Any())
                    {
                        Console.WriteLine("Statek jest pusty!");
                        break;
                    }
                    Console.Write("Wybierz kontener do zamiany: \n");
                    for (int i = 0; i < ship.containers.Count; i++)
                        Console.WriteLine($"{i}. {ship.containers[i].SerialNumber}");
                    int containerIndex = int.Parse(Console.ReadLine());
                    if (containerIndex < 0 || containerIndex >= ship.containers.Count)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Container container = ship.containers[containerIndex];
                    Console.WriteLine("Wybierz statek docelowy:");
                    int targetShipIndex = int.Parse(Console.ReadLine());
                    if (targetShipIndex < 0 || targetShipIndex >= ships.Count || targetShipIndex == shipIndex)
                    {
                        Console.WriteLine("Niepoprawny indeks!");
                        break;
                    }
                    Ship targetShip = ships[targetShipIndex];
                    try { targetShip.LoadContainer(container);}
                    catch (Exception e) {
                        Console.WriteLine(e.Message);
                        break;
                    }
                    ship.RemoveContainer(container.SerialNumber);
                }
                    break;
                case "0":
                    return;
            }
        }
    }
}