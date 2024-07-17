using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    #region Attributes
    /// <summary>
    /// Stores a list of genes for this member 
    /// </summary>
    public double[] Genes {  get; private set; }

    /// <summary>
    /// Fitness of this member
    /// </summary>
    public float Fitness;
    #endregion

    #region Constructor
    /// <summary>
    /// Creates genome of a member using the given Genes
    /// </summary>
    /// <param name="Genes">Initial Genes of this member</param>
    public Genome(double[] Genes)
    {
        this.Genes = Genes;
        Fitness = 0;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Returns a deep copy of the genes array
    /// </summary>
    /// <returns>Copied genes</returns>
    public double[] CopyGenes()
    {
        double[] returnable = new double[Genes.Length];
        for (int i = 0; i < returnable.Length; i++)
            returnable[i] = Genes[i];
        return returnable;
    }

    /// <summary>
    /// Sets current genes to inputted genes
    /// </summary>
    /// <param name="Genes">Genes to change to</param>
    /// <exception cref="ArgumentException">Incorrect length of inputted Genes</exception>
    public void SetNewGenes(double[] Genes)
    {
        if (Genes.Length != this.Genes.Length)
            throw new ArgumentException();
        this.Genes = Genes;
    }
    #endregion
}
