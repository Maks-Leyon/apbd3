namespace c3s30792;

abstract class Container
{
    protected static int counterL = 1;
    protected static int counterG = 1;
    protected static int counterC = 1;
    public string SerialNumber { get; protected set; }
    public double Height { get; }
    public double Weight { get; }
    public double Depth { get; }
    public double MaxLoad { get; }
    public double CurrentLoad { get; protected set; }

    protected Container(double height, double weight, double depth, double maxLoad)
    {
        Height = height;
        Weight = weight;
        Depth = depth;
        MaxLoad = maxLoad;
    }

    public virtual void Load(double weight)
    {
        if (CurrentLoad + weight > MaxLoad)
            throw new OverfillException("Za duża masa ładunku!");
        CurrentLoad += weight;
    }

    public virtual void Unload() => CurrentLoad = 0;

    public override string ToString() => $"{SerialNumber} (Masa ładunku: {CurrentLoad}kg/{MaxLoad}kg) | Wysokość: {Height}cm, Waga własna: {Weight}kg, Głębokość: {Depth}cm";
}

class LContainer : Container, IHazardNotifier
{
    private bool isHazardous;

    public LContainer(double height, double weight, double depth, double maxLoad, bool isHazardous) : base(height, weight, depth, maxLoad)
    {
        this.isHazardous = isHazardous;
        SerialNumber = $"KON-L-{counterL++}";
    }

    public override void Load(double weight)
    {
        double limit = isHazardous ? MaxLoad * 0.5 : MaxLoad * 0.9;
        if (CurrentLoad + weight > MaxLoad)
            throw new OverfillException("Za duża masa ładunku!");
        if (weight > limit)
            Notify("UWAGA! Niebezpieczna operacja podczas ładowania!");
        base.Load(weight);
    }

    public void Notify(string message) => Console.WriteLine($"{SerialNumber}: {message}");
}

class GContainer : Container, IHazardNotifier
{
    public double Pressure { get; }

    public GContainer(double height, double weight, double depth, double maxLoad, double pressure) : base(height, weight, depth, maxLoad)
    {
        Pressure = pressure;
        SerialNumber = $"KON-G-{counterG++}";
    }

    public override void Unload() => CurrentLoad *= 0.05;

    public void Notify(string message) => Console.WriteLine($"{SerialNumber}: {message}");
    
    public override string ToString() => $"{SerialNumber} (Masa ładunku: {CurrentLoad}kg/{MaxLoad}kg) | Wysokość: {Height}cm, Waga własna: {Weight}kg, Głębokość: {Depth}cm, Ciśnienie: {Pressure}atm";
}

class ChContainer : Container
{
    public string ProductType { get; }
    public double Temperature { get; }
     
    Dictionary<string, double> listaProduktow = new Dictionary<string, double>
    {
        { "Bananas", 13.3 },
        { "Chocolate", 18 },
        { "Fish", 2 },
        { "Meat", -15 },
        { "Ice cream", -18 },
        { "Frozen pizza", -30 },
        { "Cheese", 7.2 },
        { "Sausages", 5 },
        { "Butter", 20.5 },
        { "Eggs", 19 }
    };

    public ChContainer(double height, double weight, double depth, double maxLoad, string productType, double temperature) : base(height, weight, depth, maxLoad)
    {
        if (!listaProduktow.ContainsKey(productType))
        {
            Console.WriteLine("Nieznany produkt.");
            return;
        } if (temperature < listaProduktow[productType])
        {
            Console.WriteLine("Temperatura kontenera nie jest odpowiednia dla podanego produktu.");
            return;
        }
        ProductType = productType;
        Temperature = temperature;
        SerialNumber = $"KON-C-{counterC++}";
    }
    
    public override string ToString() => $"{SerialNumber} (Masa ładunku: {CurrentLoad}kg/{MaxLoad}kg) | Wysokość: {Height}cm, Waga własna: {Weight}kg, Głębokość: {Depth}cm, Temperatura: {Temperature}\u00b0C, Przechowywany produkt: {ProductType}";
}