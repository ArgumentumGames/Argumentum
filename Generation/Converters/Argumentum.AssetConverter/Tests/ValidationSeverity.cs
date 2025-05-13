using System;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Niveaux de sévérité pour les validations
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// Information - niveau le plus bas
        /// </summary>
        Info,
        
        /// <summary>
        /// Avertissement - problème mineur
        /// </summary>
        Warning,
        
        /// <summary>
        /// Erreur - problème majeur
        /// </summary>
        Error,
        
        /// <summary>
        /// Critique - problème bloquant
        /// </summary>
        Critical
    }
}