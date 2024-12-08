using System;

public static class GeneticHelper
{
    #region Attributes
    /// <summary>
    /// probability of swapping a gene during crossover
    /// </summary>
    private static float swapProb = 0.75f;

    /// <summary>
    /// probability of a gene being mutated
    /// </summary>
    private static float mutationProb = 0.1f;

    private static Random rand = new Random();

    /// <summary>
    /// range of how much mutation can change maximally
    /// </summary>
    private static float mutationRange = 1f;

    /// <summary>
    /// Centers mutation so it has the same probability of increasing or decreasing the value of a gene
    /// </summary>
    private static float mutationShift = mutationRange / 2;

    /// <summary>
    /// probability of crossover occurring 
    /// </summary>
    private static float crossOverRate = 0.75f;

    /// <summary>
    /// probability an agent skips genome mutation
    /// </summary>
    private static float skipMutation = 0.15f;

    #endregion


    /// <summary>
    /// Crossover between two genomes, swapping some genes randomly to create two children genomes
    /// </summary>
    /// <param name="parent1">inputted parent 1</param>
    /// <param name="parent2">inputted parent 2</param>
    /// <param name="child1">children of crossover 1</param>
    /// <param name="child2">children of crossover 2</param>
    public static void Crossover(Genome parent1, Genome parent2, out Genome child1, out Genome child2)
    {
        double[] genes1 = parent1.CopyGenes();
        double[] genes2 = parent2.CopyGenes();

        if (rand.NextDouble() < crossOverRate)
        {
            for (int i = 0; i < genes1.Length; i++)
            {
                if (rand.NextDouble() < swapProb)
                {
                    genes1[i] = genes2[i];
                    genes2[i] = parent1.Genes[i];
                }

            }
        }
        child1 = new Genome(genes1);
        child2 = new Genome(genes2);
    }
    public static void Crossover(Genome parent1, Genome parent2, out Genome child1)
    {
        double[] genes1 = parent1.CopyGenes();
        double[] genes2 = parent2.CopyGenes();

        if (rand.NextDouble() < crossOverRate)
        {
            for (int i = 0; i < genes1.Length; i++)
            {
                if (rand.NextDouble() < swapProb)
                {
                    genes1[i] = genes2[i];
                }

            }
        }
        child1 = new Genome(genes1);
    }
    /// <summary>
    /// Mutates some genes randomly, changing some value
    /// </summary>
    /// <param name="_">Genome to mutate values of</param>
    /// <returns>New genome with mutated genes</returns>
    public static Genome Mutate(Genome _)
    {
        double[] genes = _.Genes;
        if (rand.NextDouble() < skipMutation)
            return _;
        for (int i = 0; i < genes.Length; i++)
            if (rand.NextDouble() < mutationProb)
                genes[i] += rand.NextDouble() * mutationRange - mutationShift;

        Genome genome = new Genome(genes);
        return genome;
    }
}
