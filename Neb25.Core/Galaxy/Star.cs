using System.Runtime.InteropServices;
using System;

namespace Neb25.Core.Galaxy
{
	public class Star
	{

		/// <summary>
		/// Unique identifier for the star.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Name of the star (e.g., "Promixa Centauri - Alpha", "Proxima Centauri - Beta").
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Type of star (e.g., "Spectral Class G", "Spectral Class M", "Brown Dwarf").
		/// </summary>
		public string BasicSpectralType { get; set; }


		/// <summary>
		/// During generation, we roll 1d10 to determine some facts about this star. The dice roll is saved to be compared to later on.
		/// </summary>
		public int? SpecDiceRoll { get; set; }

		/// <summary>
		/// Size code, roman numeral.
		/// </summary>
		public string SizeCode { get; set; }
		/// <summary>
		/// Maps the V, IV to 5, 4 etc for M2-V, K3-IV etc
		/// </summary>
		public int SizeCodeNum { get; set; }
		/// <summary>
		/// Spectral class num is the num between the letters - G2-V, M4-V star, the 2 and 4 here.
		/// </summary>
		public int? SpectralClassNum { get; set; }


		/// <summary>
		/// Size category or radius (could be int or double).
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// Parent star system in which the star lives.
		/// </summary>
		public StarSystem ParentStarSystem { get; set; }

		/// <summary>
		/// List of planets in orbit of this star, if any.
		/// </summary>
		public List<Planet> Planets { get; set; }
		/// <summary>
		/// Whether or not the star rolled a value to become parent to later binary/trinaries.
		/// </summary>
		public bool IsParentToBinaries { get; set; }
		/// <sumarry>
		/// List of child stars if this is a parent to binaries.
		/// </sumarry>
		public List<Star> ChildStars { get; set; }
		/// <summary>
		/// Whether or not the star is a binary or trinary or so on.
		/// </summary>
		public bool IsChildStar {  get; set; }
		/// <sumarry>
		/// If this is a child star, this is the parent.
		/// </sumarry>
		public Star? ParentStar { get; set; }
		public double AgeInGY { get; set; }

		public double LifeInGY { get; set; }



		/// <summary>
		/// Luminosity measured in solar luminosities.
		/// </summary>
		public double SolarLuminosity { get; set; }
		public double TempKelvin { get; set; }
		public double SolarMasses { get; set; }
		public double SolarRadius { get; set; }




		/// <summary>
		/// Constructor for a Star.
		/// </summary>
		public Star(StarSystem parentStarSystem)
		{
			Id = Guid.NewGuid();
			int parentSystemNumStars = parentStarSystem.Stars.Count;
			int alphaStarNum = 65 + parentSystemNumStars;
			string newLetter = ((char)alphaStarNum).ToString();
			Name = parentStarSystem.Name+" ("+ newLetter+")";
			BasicSpectralType = "Brown Dwarf";

			SizeCode = "V";
			SizeCodeNum = 5;
			TempKelvin = 42069;
			SolarMasses = 1;
			SolarRadius = 1;

			SpectralClassNum = 1;

			Size = 42069;
			AgeInGY = 420;
			LifeInGY = 10000;
			ParentStarSystem = parentStarSystem;
			Planets = new List<Planet>();
			IsParentToBinaries = false;
			ChildStars = new List<Star>();
			IsChildStar = false;
			ParentStar = null;
			SolarLuminosity = 1;



			parentStarSystem.Stars.Add(this);
			if (parentSystemNumStars==0) { parentStarSystem.PrimaryStar = this; }

		}
		public override string ToString()
		{
			return $"{Name} - {Planets.Count} Planets";
		}

		/// <summary>
		/// Returns the string like 'M2-V', 'White Dwarf', 'K4-IV'
		/// </summary>
		public string StarCode()
		{
			// Initialize the result string
			string result;

			// Check if the star is of a special type
			if (BasicSpectralType == "Brown Dwarf" || BasicSpectralType == "White Dwarf" || BasicSpectralType == "Rare Stellar Object")
			{
				// If it's a special type, the result is just the BasicSpectralType
				result = BasicSpectralType;
			}
			else
			{
				// Otherwise, construct the star code
				string spectralClass = BasicSpectralType.Substring(BasicSpectralType.Length - 1); // Extract the last character
				result = $"{spectralClass}{SpectralClassNum}-{SizeCode}"; // Combine with SpectralClassNum and SizeCode
			}

			// Return the constructed string
			return result;
		}


		public string StarFacts()
		{
			string result;
			result = $"Lum: {Math.Round(SolarLuminosity,4)}, Rad: {Math.Round(SolarRadius,2)}, Temp: {Math.Round(TempKelvin,2)}, Mass: {Math.Round(SolarMasses,2)}, Size: {Size}, Age in billions: {AgeInGY}";


			if (AgeInGY < 1) { result += " - Young star"; }
			if (AgeInGY >= 12) { result += " - Ancient star"; }
			return result;

		}
	}
}
