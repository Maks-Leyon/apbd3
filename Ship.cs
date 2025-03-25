namespace c3s30792;

class Ship
{
    public string Name { get; }
    public int MaxContainers { get; }
    public int Speed { get; }
    public double MaxWeight { get; }
    public List<Container> containers { get; }

    public Ship(string name, int maxContainers, double maxWeight, int speed)
    {
        Name = name;
        MaxContainers = maxContainers;
        MaxWeight = maxWeight;
        Speed = speed;

        containers = new List<Container>();
    }

    public void LoadContainer(Container container)
    {
        if (containers.Count >= MaxContainers || GetTotalWeight() + container.MaxLoad > MaxWeight)
            throw new Exception("Statek jest przeciążony!");
        containers.Add(container);
    }
    
    public void LoadContainers(List<Container> containerList)
    {
        foreach (var container in containerList)
        {
            if (containers.Count >= MaxContainers || GetTotalWeight() + container.MaxLoad > MaxWeight)
                throw new Exception("Statek jest przeciążony!");
            containers.Add(container);
        }
    }

    public void RemoveContainer(string serialNumber)
    {
        containers.RemoveAll(c => c.SerialNumber == serialNumber);
    }

    public void ReplaceContainer(string serialNumber, Container newContainer)
    {
        RemoveContainer(serialNumber);
        LoadContainer(newContainer);
    }

    public double GetTotalWeight() => containers.Sum(c => c.MaxLoad);
    public override string ToString()
    {
        string containerStrings = "";
        foreach (var container in containers)
        {
            containerStrings += "\n\t" + container;
        }

        return $"{Name} (Kontenery: {containers.Count}/{MaxContainers}, Waga: {GetTotalWeight()/1000} ton/{MaxWeight/1000} ton), Prędkość: {Speed} węzłów{containerStrings}";
    }
    
}