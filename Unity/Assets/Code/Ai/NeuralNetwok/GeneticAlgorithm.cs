using System;
using System.Collections;
using System.Collections.Generic;

public class GeneticAlgorithm
{
    #region Attributes

    private float swapProb = 0.7f;

    private float mutationProb = 0.1f;

    private static Random rand = new Random();

    private static float mutationRange = 1.0f;

    private static float mutationShift = mutationRange / 2;

    #endregion

    #region Constructor
    public GeneticAlgorithm() { }

    #endregion
    /// <summary>
    /// Crossover between two genomes, swapping some genes randomly to create two children genomes
    /// </summary>
    /// <param name="parent1">inputted parent 1</param>
    /// <param name="parent2">inputted parent 2</param>
    /// <param name="child1">children of crossover 1</param>
    /// <param name="child2">children of crossover 2</param>
    private void Crossover(Genome parent1, Genome parent2, out Genome child1, out Genome child2)
    {
        double[] genes1 = parent1.CopyGenes();
        double[] genes2 = parent2.CopyGenes();

        for (int i = 0; i < genes1.Length; i++)
        {
            if (rand.NextDouble() < swapProb)
            {
                genes1[i] = genes2[i];
                genes2[i] = parent1.Genes[i];
            }
                    
        }
        child1 = new Genome(genes1);
        child2 = new Genome(genes2);
    }

    private Genome mutate(Genome _)
    {
        double[] genes = _.Genes;
        for (int i = 0; i<genes.Length; i++)
            if (rand.NextDouble() < mutationProb)
                genes[i] += rand.NextDouble() * mutationRange - mutationShift;

        Genome genome = new Genome(genes);
        return genome;
    }
}
