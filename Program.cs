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

    static double getCorruptionPercentage()
    {
        while (true)
        {
            try
            {
                Console.Write("Input corruption percentage >>> ");
                return Convert.ToDouble(Console.ReadLine()) / 100;
            }
            catch { Console.WriteLine("Bad input given"); }
        }
    }

    static byte[] readFile(string filepath)
    {
        byte[] file = File.ReadAllBytes(filepath);
        return file;
    }

    static HashSet<int> getBytesToCorrupt(double corruptionPercentage, byte[] file)
    {
        HashSet<int> indices = new HashSet<int>();
        Random random = new Random();

        corruptionPercentage = Math.Min(corruptionPercentage, 1); // ensure maximum if file size is selected
        int amountToCorrupt = Convert.ToInt32(file.Length * corruptionPercentage);

        while (indices.Count() < amountToCorrupt)
        {
            indices.Add(random.Next(0, file.Length));
        }

        return indices;
    }

    static void corruptFile(double corruptionPercentage, byte[] file, string filepath)
    {
        Random random = new Random();
        HashSet<int> indicesToCorrupt = getBytesToCorrupt(corruptionPercentage, file);
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
        double corruptionPercentage = getCorruptionPercentage();

        corruptFile(corruptionPercentage, file, filepath);
    }
}
