using System.Collections.Generic;

public static class EvolutionManager
{
    //This class handles much of the evolution process, can be tweaked to try new genetic algorithms

    #region Methods
    /// <summary>
    /// Evolves the current generation, creating a new generation. Uses elitism
    /// </summary>
    /// <param name="Population">Sorted list of genomes based on fitness</param>
    /// <returns></returns>
    /// <remarks>the two fittest genomes are unchanged</remarks>
    public static Genome[] Evolve(Genome[] Population)
    {
        Genome[] newPopulation;
        LeaveTwo(Population, out Genome best1, out Genome best2);

        List<Genome> curPopulation = new List<Genome>(Population);
        curPopulation.Remove(best1);
        curPopulation.Remove(best2);
        Population = curPopulation.ToArray();

        newPopulation = Crossover(Population, best1, best2);
        newPopulation = Mutate(newPopulation);

        curPopulation = new List<Genome>(newPopulation);
        curPopulation.Add(best1);
        curPopulation.Add(best2);

        newPopulation = curPopulation.ToArray();
        return newPopulation;
    }

    /// <summary>
    /// Mutates a population, returning the new mutated version
    /// </summary>
    /// <param name="CurrentPopulation">population to mutate</param>
    /// <returns>new population</returns>
    private static Genome[] Mutate(Genome[] CurrentPopulation)
    {
        for (int i = 0; i < CurrentPopulation.Length; i++)
        {
            CurrentPopulation[i] = GeneticHelper.Mutate(CurrentPopulation[i]);
        }
        return CurrentPopulation;
    }

    /// <summary>
    /// Crossover between a population with the best or second best genome, creating a new population with each genome 
    /// in it being a child of either best1 or best2, and another genome
    /// </summary>
    /// <param name="Population">current population</param>
    /// <param name="best1">fittest genome</param>
    /// <param name="best2">second fittest genome</param>
    /// <returns>a new population of genomes</returns>
    private static Genome[] Crossover(Genome[] Population, Genome best1, Genome best2)
    {
        for (int i = 0; i < Population.Length; i++)
        {
            GeneticHelper.Crossover(Population[i], i % 2 == 0 ? best1 : best2, out Population[i]);
        }
        return Population;
    }

    /// <summary>
    /// Finds the two best genes of this population
    /// </summary>
    /// <param name="Population">Population to search</param>
    /// <param name="best1">Outputted fittest genome</param>
    /// <param name="best2">Outputted second fittest genome</param>
    /// <exception cref="System.Exception">Population length less than 2</exception>
    private static void LeaveTwo(Genome[] Population, out Genome best1, out Genome best2)
    {
        if (Population.Length < 2)
            throw new System.Exception("population too small for leave two");

        float bestFitness1 = 0;
        float bestFitness2 = 0;

        best1 = Population[0];
        best2 = Population[1];
        foreach (Genome gen in Population)
        {
            if (gen.Fitness > bestFitness1)
            {
                bestFitness2 = bestFitness1;
                best2 = best1;
                best1 = gen;
                bestFitness1 = gen.Fitness;
            }
            else if (gen.Fitness > bestFitness2)
            {
                bestFitness2 = gen.Fitness;
                best2 = gen;
            }

        }
    }
    #endregion
}
