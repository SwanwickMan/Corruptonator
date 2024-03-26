class Program
{
    static string getFilePath(string[] args)
    {
        string filepath;
        while (true)
        {
            if (args.Length > 0) { filepath = args[0]; }
            else
            {
                Console.Write("Input file path >>> ");
                filepath = Console.ReadLine();
            }

            if (File.Exists(filepath)) { return filepath; }
            else { Console.WriteLine("File not found"); args = new string[] { }; }
        }
    }

    public enum Type
    {
        PERCENT,
        NUMBER
    }

    static (double factor, Type type) getCorruptionFactor()
    {
        while (true)
        {
            try
            {
                Console.Write("Input corruption factor >>> ");
                string input = Console.ReadLine();
                if (input.EndsWith("%"))
                {
                    return (Convert.ToDouble(input.Trim('%')) / 100, Type.PERCENT);
                }
                else
                {
                    return (Convert.ToDouble(input), Type.NUMBER);
                }
            }
            catch { Console.WriteLine("Bad input given"); }
        }
    }

    static byte[] readFile(string filepath)
    {
        byte[] file = File.ReadAllBytes(filepath);
        return file;
    }

    static HashSet<int> getBytesToCorrupt(double corruptionFactor, byte[] file, Type type)
    {
        HashSet<int> indices = new HashSet<int>();
        Random random = new Random();
        int amountToCorrupt;

        if (type == Type.PERCENT)
        {
            corruptionFactor = Math.Min(corruptionFactor, 1); // ensure maximum if file size is selected
            amountToCorrupt = Convert.ToInt32(file.Length * corruptionFactor);
        }
        else
        {
            corruptionFactor = Math.Min(corruptionFactor, file.Length);
            amountToCorrupt = Convert.ToInt32(corruptionFactor);
        }

        while (indices.Count() < amountToCorrupt)
        {
            indices.Add(random.Next(0, file.Length));
        }

        return indices;
    }

    static void corruptFile(double corruptionFactor, byte[] file, string filepath, Type type)
    {
        Random random = new Random();
        HashSet<int> indicesToCorrupt = getBytesToCorrupt(corruptionFactor, file, type);
        foreach (int i in indicesToCorrupt)
        {
            file[i] = (byte)random.Next(0, 256); // start to corrupt stuff
        }

        File.WriteAllBytes(filepath, file);
    }



    static void Main(string[] args)
    {
        string filepath = getFilePath(args);
        byte[] file = readFile(filepath);
        var result = getCorruptionFactor();


        corruptFile(result.factor, file, filepath, result.type);
    }
}
