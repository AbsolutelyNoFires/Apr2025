using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*

namespace Neb25.Core.Galaxy.Generators
{
	internal class StarSysGen2
	{
		public StarSystem Gen2(Galaxy galaxy, Random seed)
		{

			// first generate a primary body
			string primaryBasicType = string.Empty;
			string primarySizeCode = string.Empty;
			int primarySizeCodeNum = -1;
			int primarySpectralClassNum = -1;
			double primaryTempKelvin = -1;
			double primarySolarMasses = -1;
			double primarySolarLuminosity = -1;
			double primarySolarRadius = -1;






			string binaryBasicType = string.Empty;
			string binarySizeCode = string.Empty;
			int binarySizeCodeNum = -1;
			int binarySpectralClassNum = -1;


			double binaryTempKelvin = -1;
			double binarySolarMasses = -1;
			double binarySolarLuminosity = -1;
			double binarySolarRadius = -1;





			double trinaryTempKelvin = -1;
			double trinarySolarMasses = -1;
			double trinarySolarLuminosity = -1;
			double trinarySolarRadius = -1;

			string trinaryBasicType = string.Empty;
			string trinarySizeCode = string.Empty;
			int? trinarySizeCodeNum;
			int? trinarySpectralClassNum;


			double quaternaryTempKelvin = -1;
			double quaternarySolarMasses = -1;
			double quaternarySolarLuminosity = -1;
			double quaternarySolarRadius = -1;

			string quaternaryBasicType = string.Empty;
			string quaternarySizeCode = string.Empty;
			int? quaternarySizeCodeNum;
			int? quaternarySpectralClassNum;








			int roll1d100_primary = seed.Next(1, 101);
			int roll1d10_primary_specification_roll = seed.Next(1, 11);
			int roll1d10_primary_spectral_class_num = seed.Next(1, 11);

			bool hasBinary = (seed.Next(1, 11) >= 7);
			bool hasTrinary = (hasBinary && seed.Next(1, 11) >= 7);
			bool hasQuaternary = (hasTrinary && seed.Next(1, 11) >= 7);


			if (roll1d100_primary == 1)
			{
				primaryBasicType = "Spectral Class A";
				primarySizeCode = "V";
				primarySizeCodeNum = 5;
				if (roll1d10_primary_specification_roll >= 7) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
			}
			else if (roll1d100_primary <= 4)
			{
				primaryBasicType = "Spectral Class F";
				primarySizeCode = "V";
				primarySizeCodeNum = 5;
				if (roll1d10_primary_specification_roll >= 9) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
			}
			else if (roll1d100_primary <= 12)
			{
				primaryBasicType = "Spectral Class G";
				primarySizeCode = "V";
				primarySizeCodeNum = 5;
				if (roll1d10_primary_specification_roll >= 10) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
			}
			else if (roll1d100_primary <= 26)
			{
				primaryBasicType = "Spectral Class K";
				primarySizeCode = "V";
				primarySizeCodeNum = 5;
			}
			else if (roll1d100_primary <= 36)
			{
				primaryBasicType = "White Dwarf";
				primarySizeCode = "VII";
				primarySizeCodeNum = 7;
			}
			else if (roll1d100_primary <= 85)
			{
				primaryBasicType = "Spectral Class M";
				primarySizeCode = "V";
				primarySizeCodeNum = 5;
			}
			else if (roll1d100_primary <= 98)
			{
				primaryBasicType = "Brown Dwarf";
				primarySizeCode = string.Empty;
				primarySizeCodeNum = -1;
			}
			else if (roll1d100_primary <= 99)
			{
				primarySizeCode = "III";
				primarySizeCodeNum = 3;
				if (roll1d10_primary_specification_roll <= 1) primaryBasicType = "Spectral Class F";
				else if (roll1d10_primary_specification_roll <= 2) primaryBasicType = "Spectral Class G";
				else if (roll1d10_primary_specification_roll <= 7) primaryBasicType = "Spectral Class K";
				else if (roll1d10_primary_specification_roll >= 8)
				{
					primaryBasicType = "Spectral Class K";
					primarySizeCode = "IV";
					primarySizeCodeNum = 4;
				}
			}
			else if (roll1d100_primary == 100)
			{
				primaryBasicType = "Rare Stellar Object";
			}


			if (roll1d10_primary_spectral_class_num == 10) roll1d10_primary_spectral_class_num = 0;
			if (primaryBasicType != "Brown Dwarf" && primaryBasicType != "White Dwarf") primarySpectralClassNum = roll1d10_primary_spectral_class_num;
			if (primaryBasicType == "Spectral Class K" && primarySizeCode == "IV") primarySpectralClassNum = 0;

			// we generate binaries


			if (hasBinary) {
				int roll1d10_binary_spectral_class_num = seed.Next(1, 11);
				int roll1d10_first_binary_roll = seed.Next(1, 11);
				int roll1d10_binary_specification_roll = seed.Next(1, 11);
				if (roll1d10_first_binary_roll <= 2)
				{
					binaryBasicType = primaryBasicType;
					binarySizeCode = primarySizeCode;
					binarySizeCodeNum = primarySizeCodeNum;




					if (roll1d10_binary_specification_roll >= roll1d10_primary_specification_roll)
					{

						if ((primaryBasicType == "Spectral Class A") && (primarySizeCode == "V"))
						{
							if (roll1d10_binary_specification_roll >= 7) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class F") && (primarySizeCode == "V"))
						{
							if (roll1d10_binary_specification_roll >= 9) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class G") && (primarySizeCode == "V"))
						{
							if (roll1d10_binary_specification_roll == 10) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if (primarySizeCodeNum == 3)
						{
							if (roll1d10_binary_specification_roll <= 1) primaryBasicType = "Spectral Class F";
							else if (roll1d10_binary_specification_roll <= 2) primaryBasicType = "Spectral Class G";
							else if (roll1d10_binary_specification_roll <= 7) primaryBasicType = "Spectral Class K";
							else if (roll1d10_binary_specification_roll >= 8)
							{
								primaryBasicType = "Spectral Class K";
								primarySizeCode = "IV";
								primarySizeCodeNum = 4;
							}

						}

					}
					else if (roll1d10_first_binary_roll >= 3)
					{
						int roll1d100_binary_basictype = seed.Next(1, 101);


						if (roll1d100_binary_basictype == 1)
						{
							binaryBasicType = "Spectral Class A";
							binarySizeCode = "V";
							binarySizeCodeNum = 5;
							if (roll1d10_binary_specification_roll >= 7) { binarySizeCode = "IV"; binarySizeCodeNum = 4; }
						}
						else if (roll1d100_binary_basictype <= 4)
						{
							binaryBasicType = "Spectral Class F";
							binarySizeCode = "V";
							binarySizeCodeNum = 5;
							if (roll1d10_binary_specification_roll >= 9) { binarySizeCode = "IV"; binarySizeCodeNum = 4; }
							if (primaryBasicType == "Spectral Class A")
							{
								binaryBasicType = "Brown Dwarf";
								binarySizeCode = string.Empty;
								binarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_binary_basictype <= 12)
						{
							binaryBasicType = "Spectral Class G";
							binarySizeCode = "V";
							binarySizeCodeNum = 5;
							if (roll1d10_binary_specification_roll >= 10) { binarySizeCode = "IV"; binarySizeCodeNum = 4; }
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F")
							{
								binaryBasicType = "Brown Dwarf";
								binarySizeCode = string.Empty;
								binarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_binary_basictype <= 26)
						{
							binaryBasicType = "Spectral Class K";
							binarySizeCode = "V";
							binarySizeCodeNum = 5;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G")
							{
								binaryBasicType = "Brown Dwarf";
								binarySizeCode = string.Empty;
								binarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_binary_basictype <= 36)
						{
							binaryBasicType = "White Dwarf";
							binarySizeCode = "VII";
							binarySizeCodeNum = 7;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G" || primaryBasicType == "Spectral Class K")
							{
								binaryBasicType = "Brown Dwarf";
								binarySizeCode = string.Empty; ;
								binarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_binary_basictype <= 85)
						{
							binaryBasicType = "Spectral Class M";
							binarySizeCode = "V";
							binarySizeCodeNum = 5;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G" || primaryBasicType == "Spectral Class K" || primaryBasicType == "White Dwarf")
							{
								binaryBasicType = "Brown Dwarf";
								binarySizeCode = string.Empty; ;
								binarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_binary_basictype >= 98)
						{
							binaryBasicType = "Brown Dwarf";
							binarySizeCode = string.Empty; ;
							binarySizeCodeNum = -1;
						}
					}
				}

				if (roll1d10_binary_spectral_class_num == 10) roll1d10_binary_spectral_class_num = 0;
				if (binaryBasicType != "Brown Dwarf" && binaryBasicType != "White Dwarf") binarySpectralClassNum = roll1d10_binary_spectral_class_num;
				if (binaryBasicType == "Spectral Class K" && binarySizeCode == "IV") binarySpectralClassNum = 0;

			}

			if (hasTrinary)
			{



				int roll1d10_trinary_spectral_class_num = seed.Next(1, 11);

				int roll1d10_first_trinary_roll = seed.Next(1, 11);
				int roll1d10_trinary_specification_roll = seed.Next(1, 11);
				if (roll1d10_first_trinary_roll <= 2)
				{
					trinaryBasicType = primaryBasicType;
					trinarySizeCode = primarySizeCode;
					trinarySizeCodeNum = primarySizeCodeNum;
					trinarySpectralClassNum = primarySpectralClassNum;



					if (roll1d10_trinary_specification_roll >= roll1d10_primary_specification_roll)
					{

						if ((primaryBasicType == "Spectral Class A") && (primarySizeCode == "V"))
						{
							if (roll1d10_trinary_specification_roll >= 7) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class F") && (primarySizeCode == "V"))
						{
							if (roll1d10_trinary_specification_roll >= 9) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class G") && (primarySizeCode == "V"))
						{
							if (roll1d10_trinary_specification_roll == 10) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if (primarySizeCodeNum == 3)
						{
							if (roll1d10_trinary_specification_roll <= 1) primaryBasicType = "Spectral Class F";
							else if (roll1d10_trinary_specification_roll <= 2) primaryBasicType = "Spectral Class G";
							else if (roll1d10_trinary_specification_roll <= 7) primaryBasicType = "Spectral Class K";
							else if (roll1d10_trinary_specification_roll >= 8)
							{
								primaryBasicType = "Spectral Class K";
								primarySizeCode = "IV";
								primarySizeCodeNum = 4;
							}

						}

					}
					else if (roll1d10_first_trinary_roll >= 3)
					{
						int roll1d100_trinary_basictype = seed.Next(1, 101);


						if (roll1d100_trinary_basictype == 1)
						{
							trinaryBasicType = "Spectral Class A";
							trinarySizeCode = "V";
							trinarySizeCodeNum = 5;
							if (roll1d10_trinary_specification_roll >= 7) { trinarySizeCode = "IV"; trinarySizeCodeNum = 4; }
						}
						else if (roll1d100_trinary_basictype <= 4)
						{
							trinaryBasicType = "Spectral Class F";
							trinarySizeCode = "V";
							trinarySizeCodeNum = 5;
							if (roll1d10_trinary_specification_roll >= 9) { trinarySizeCode = "IV"; trinarySizeCodeNum = 4; }
							if (primaryBasicType == "Spectral Class A")
							{
								trinaryBasicType = "Brown Dwarf";
								trinarySizeCode = string.Empty;
								trinarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_trinary_basictype <= 12)
						{
							trinaryBasicType = "Spectral Class G";
							trinarySizeCode = "V";
							trinarySizeCodeNum = 5;
							if (roll1d10_trinary_specification_roll >= 10) { trinarySizeCode = "IV"; trinarySizeCodeNum = 4; }
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F")
							{
								trinaryBasicType = "Brown Dwarf";
								trinarySizeCode = string.Empty;
								trinarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_trinary_basictype <= 26)
						{
							trinaryBasicType = "Spectral Class K";
							trinarySizeCode = "V";
							trinarySizeCodeNum = 5;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G")
							{
								trinaryBasicType = "Brown Dwarf";
								trinarySizeCode = string.Empty;
								trinarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_trinary_basictype <= 36)
						{
							trinaryBasicType = "White Dwarf";
							trinarySizeCode = "VII";
							trinarySizeCodeNum = 7;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G" || primaryBasicType == "Spectral Class K")
							{
								trinaryBasicType = "Brown Dwarf";
								trinarySizeCode = string.Empty;
								trinarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_trinary_basictype <= 85)
						{
							trinaryBasicType = "Spectral Class M";
							trinarySizeCode = "V";
							trinarySizeCodeNum = 5;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G" || primaryBasicType == "Spectral Class K" || primaryBasicType == "White Dwarf")
							{
								trinaryBasicType = "Brown Dwarf";
								trinarySizeCode = string.Empty;
								trinarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_trinary_basictype >= 98)
						{
							trinaryBasicType = "Brown Dwarf";
							trinarySizeCode = string.Empty;
							trinarySizeCodeNum = -1;
						}
					}
				}
				if (roll1d10_trinary_spectral_class_num == 10) roll1d10_trinary_spectral_class_num = 0;
				if (trinaryBasicType != "Brown Dwarf" && trinaryBasicType != "White Dwarf") trinarySpectralClassNum = roll1d10_trinary_spectral_class_num;
				if (trinaryBasicType == "Spectral Class K" && trinarySizeCode == "IV") trinarySpectralClassNum = 0;

			}

			if (hasQuaternary)
			{


				int roll1d10_quaternary_spectral_class_num = seed.Next(1, 11);

				int roll1d10_first_quaternary_roll = seed.Next(1, 11);
				int roll1d10_quaternary_specification_roll = seed.Next(1, 11);
				if (roll1d10_first_quaternary_roll <= 2)
				{
					quaternaryBasicType = primaryBasicType;
					quaternarySizeCode = primarySizeCode;
					quaternarySizeCodeNum = primarySizeCodeNum;
					quaternarySpectralClassNum = primarySpectralClassNum;



					if (roll1d10_quaternary_specification_roll >= roll1d10_primary_specification_roll)
					{

						if ((primaryBasicType == "Spectral Class A") && (primarySizeCode == "V"))
						{
							if (roll1d10_quaternary_specification_roll >= 7) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class F") && (primarySizeCode == "V"))
						{
							if (roll1d10_quaternary_specification_roll >= 9) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class G") && (primarySizeCode == "V"))
						{
							if (roll1d10_quaternary_specification_roll == 10) { primarySizeCode = "IV"; primarySizeCodeNum = 4; }
						}
						else if (primarySizeCodeNum == 3)
						{
							if (roll1d10_quaternary_specification_roll <= 1) primaryBasicType = "Spectral Class F";
							else if (roll1d10_quaternary_specification_roll <= 2) primaryBasicType = "Spectral Class G";
							else if (roll1d10_quaternary_specification_roll <= 7) primaryBasicType = "Spectral Class K";
							else if (roll1d10_quaternary_specification_roll >= 8)
							{
								primaryBasicType = "Spectral Class K";
								primarySizeCode = "IV";
								primarySizeCodeNum = 4;
							}

						}

					}
					else if (roll1d10_first_quaternary_roll >= 3)
					{
						int roll1d100_quaternary_basictype = seed.Next(1, 101);


						if (roll1d100_quaternary_basictype == 1)
						{
							quaternaryBasicType = "Spectral Class A";
							quaternarySizeCode = "V";
							quaternarySizeCodeNum = 5;
							if (roll1d10_quaternary_specification_roll >= 7) { quaternarySizeCode = "IV"; quaternarySizeCodeNum = 4; }
						}
						else if (roll1d100_quaternary_basictype <= 4)
						{
							quaternaryBasicType = "Spectral Class F";
							quaternarySizeCode = "V";
							quaternarySizeCodeNum = 5;
							if (roll1d10_quaternary_specification_roll >= 9) { quaternarySizeCode = "IV"; quaternarySizeCodeNum = 4; }
							if (primaryBasicType == "Spectral Class A")
							{
								quaternaryBasicType = "Brown Dwarf";
								quaternarySizeCode = string.Empty;
								quaternarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_quaternary_basictype <= 12)
						{
							quaternaryBasicType = "Spectral Class G";
							quaternarySizeCode = "V";
							quaternarySizeCodeNum = 5;
							if (roll1d10_quaternary_specification_roll >= 10) { quaternarySizeCode = "IV"; quaternarySizeCodeNum = 4; }
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F")
							{
								quaternaryBasicType = "Brown Dwarf";
								quaternarySizeCode = string.Empty;
								quaternarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_quaternary_basictype <= 26)
						{
							quaternaryBasicType = "Spectral Class K";
							quaternarySizeCode = "V";
							quaternarySizeCodeNum = 5;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G")
							{
								quaternaryBasicType = "Brown Dwarf";
								quaternarySizeCode = string.Empty;
								quaternarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_quaternary_basictype <= 36)
						{
							quaternaryBasicType = "White Dwarf";
							quaternarySizeCode = "VII";
							quaternarySizeCodeNum = 7;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G" || primaryBasicType == "Spectral Class K")
							{
								quaternaryBasicType = "Brown Dwarf";
								quaternarySizeCode = string.Empty;
								quaternarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_quaternary_basictype <= 85)
						{
							quaternaryBasicType = "Spectral Class M";
							quaternarySizeCode = "V";
							quaternarySizeCodeNum = 5;
							if (primaryBasicType == "Spectral Class A" || primaryBasicType == "Spectral Class F" || primaryBasicType == "Spectral Class G" || primaryBasicType == "Spectral Class K" || primaryBasicType == "White Dwarf")
							{
								quaternaryBasicType = "Brown Dwarf";
								quaternarySizeCode = string.Empty;
								quaternarySizeCodeNum = -1;
							}
						}
						else if (roll1d100_quaternary_basictype >= 98)
						{
							quaternaryBasicType = "Brown Dwarf";
							quaternarySizeCode = string.Empty;
							quaternarySizeCodeNum = -1;
						}
					}
				}
				if (roll1d10_quaternary_spectral_class_num == 10) roll1d10_quaternary_spectral_class_num = 0;
				if (quaternaryBasicType != "Brown Dwarf" && quaternaryBasicType != "White Dwarf") quaternarySpectralClassNum = roll1d10_quaternary_spectral_class_num;
				if (quaternaryBasicType == "Spectral Class K" && quaternarySizeCode == "IV") quaternarySpectralClassNum = 0;
			}



			//consult the big table 1.1.3 Basic Luminosity & Mass
			// assign luminosity, mass, kelvin and radius values from the big table

			var primaryDetails = GetStarDetails(primaryBasicType, primarySpectralClassNum, primarySizeCode);
			if (primaryDetails.HasValue)
			{
				primarySolarLuminosity = primaryDetails.Value.SolLuminosity;
				primarySolarMasses = primaryDetails.Value.SolMass;
				primaryTempKelvin = primaryDetails.Value.TempKelvin;
				primarySolarRadius = primaryDetails.Value.SolRadius;
			}
			if (hasBinary)
			{	 
				var binaryDetails = GetStarDetails(binaryBasicType, binarySpectralClassNum, binarySizeCode);
				if (binaryDetails.HasValue)
				{
					binarySolarLuminosity = binaryDetails.Value.SolLuminosity;
					binarySolarMasses = binaryDetails.Value.SolMass;
					binaryTempKelvin = binaryDetails.Value.TempKelvin;
					binarySolarRadius = binaryDetails.Value.SolRadius;
				}
			}
			if (hasTrinary)
			{
				var trinaryDetails = GetStarDetails(trinaryBasicType, trinarySpectralClassNum, trinarySizeCode);
				if (trinaryDetails.HasValue)
				{
					trinarySolarLuminosity = trinaryDetails.Value.SolLuminosity;
					trinarySolarMasses = trinaryDetails.Value.SolMass;
					trinaryTempKelvin = trinaryDetails.Value.TempKelvin;
					trinarySolarRadius = trinaryDetails.Value.SolRadius;
				}
			}
			if (hasQuaternary)
			{
				var quaternaryDetails = GetStarDetails(quaternaryBasicType, quaternarySpectralClassNum, quaternarySizeCode);
				if (quaternaryDetails.HasValue)
				{
					quaternarySolarLuminosity = quaternaryDetails.Value.SolLuminosity;
					quaternarySolarMasses = quaternaryDetails.Value.SolMass;
					quaternaryTempKelvin = quaternaryDetails.Value.TempKelvin;
					quaternarySolarRadius = quaternaryDetails.Value.SolRadius;
				}
			}

			// next, randomise subgiants according to the logic in table 1.1.3






		}

		// basic luminsity and mass
		private static (float SolLuminosity, float SolMass, float TempKelvin, float SolRadius)? GetStarDetails(string basicType, int spectralClassNum, string sizeCode)
		{
			if (StarDetailsMap.TryGetValue((basicType, spectralClassNum, sizeCode), out var details))
			{
				return details;
			}
			return null; // Return null if no match is found
		}






	}












				private static readonly Dictionary<(string BasicType, int SpectralClassNum, string SizeCode), (float SolLuminosity, float SolMass, float TempKelvin, float SolRadius)> StarDetailsMap = new() {
				//Table 1.1.3 Basic Luminosity & Mass
				{ ("Spectral Class B", 0, "V"), (13000, 17.5f, 28000, 4.9f) },
				{ ("Spectral Class B", 1, "V"), (7800, 15.1f, 25000, 4.8f) },
				{ ("Spectral Class B", 2, "V"), (4700, 13.0f, 22000, 4.8f) },
				{ ("Spectral Class B", 3, "V"), (2800, 11.1f, 19000, 4.8f) },
				{ ("Spectral Class B", 4, "V"), (1700, 9.5f, 17000, 4.8f) },
				{ ("Spectral Class B", 5, "V"), (1000, 8.2f, 15000, 4.7f) },
				{ ("Spectral Class B", 6, "V"), (600, 7.0f, 14000, 4.2f) },
				{ ("Spectral Class B", 7, "V"), (370, 6.0f, 13000, 3.8f) },
				{ ("Spectral Class B", 8, "V"), (220, 5.0f, 12000, 3.5f) },
				{ ("Spectral Class B", 9, "V"), (130, 4.0f, 11000, 3.2f) },
				{ ("Spectral Class A", 0, "V"), (80, 3.0f, 10000, 3) },
				{ ("Spectral Class A", 1, "V"), (62, 2.8f, 9750, 2.8f) },
				{ ("Spectral Class A", 2, "V"), (48, 2.6f, 9500, 2.6f) },
				{ ("Spectral Class A", 3, "V"), (38, 2.5f, 9250, 2.4f) },
				{ ("Spectral Class A", 4, "V"), (29, 2.3f, 9000, 2.2f) },
				{ ("Spectral Class A", 5, "V"), (23, 2.2f, 8750, 2.1f) },
				{ ("Spectral Class A", 6, "V"), (18, 2.0f, 8500, 2) },
				{ ("Spectral Class A", 7, "V"), (14, 1.9f, 8250, 1.8f) },
				{ ("Spectral Class A", 8, "V"), (11, 1.8f, 8000, 1.7f) },
				{ ("Spectral Class A", 9, "V"), (8.2f, 1.7f, 7750, 1.6f) },
				{ ("Spectral Class F", 0, "V"), (6.4f, 1.6f, 7500, 1.5f) },
				{ ("Spectral Class F", 1, "V"), (5.5f, 1.53f, 7350, 1.5f) },
				{ ("Spectral Class F", 2, "V"), (4.7f, 1.47f, 7200, 1.4f) },
				{ ("Spectral Class F", 3, "V"), (4, 1.42f, 7050, 1.4f) },
				{ ("Spectral Class F", 4, "V"), (3.4f, 1.36f, 6900, 1.3f) },
				{ ("Spectral Class F", 5, "V"), (2.9f, 1.31f, 6750, 1.3f) },
				{ ("Spectral Class F", 6, "V"), (2.5f, 1.26f, 6600, 1.2f) },
				{ ("Spectral Class F", 7, "V"), (2.16f, 1.21f, 6450, 1.2f) },
				{ ("Spectral Class F", 8, "V"), (1.85f, 1.17f, 6300, 1.2f) },
				{ ("Spectral Class F", 9, "V"), (1.58f, 1.12f, 6150, 1.1f) },
				{ ("Spectral Class G", 0, "V"), (1.36f, 1.08f, 6000, 1.1f) },
				{ ("Spectral Class G", 1, "V"), (1.21f, 1.05f, 5900, 1.1f) },
				{ ("Spectral Class G", 2, "V"), (1.09f, 1.02f, 5800, 1) },
				{ ("Spectral Class G", 3, "V"), (0.98f, 0.99f, 5700, 1) },
				{ ("Spectral Class G", 4, "V"), (0.88f, 0.96f, 5600, 1) },
				{ ("Spectral Class G", 5, "V"), (0.79f, 0.94f, 5500, 1) },
				{ ("Spectral Class G", 6, "V"), (0.71f, 0.92f, 5400, 1) },
				{ ("Spectral Class G", 7, "V"), (0.64f, 0.89f, 5300, 1) },
				{ ("Spectral Class G", 8, "V"), (0.57f, 0.87f, 5200, 0.9f) },
				{ ("Spectral Class G", 9, "V"), (0.51f, 0.85f, 5100, 0.9f) },
				{ ("Spectral Class K", 0, "V"), (0.46f, 0.82f, 5000, 0.9f) },
				{ ("Spectral Class K", 1, "V"), (0.39f, 0.79f, 4850, 0.9f) },
				{ ("Spectral Class K", 2, "V"), (0.32f, 0.75f, 4700, 0.9f) },
				{ ("Spectral Class K", 3, "V"), (0.27f, 0.72f, 4550, 0.8f) },
				{ ("Spectral Class K", 4, "V"), (0.23f, 0.69f, 4400, 0.8f) },
				{ ("Spectral Class K", 5, "V"), (0.19f, 0.66f, 4250, 0.8f) },
				{ ("Spectral Class K", 6, "V"), (0.16f, 0.63f, 4100, 0.8f) },
				{ ("Spectral Class K", 7, "V"), (0.14f, 0.61f, 3950, 0.8f) },
				{ ("Spectral Class K", 8, "V"), (0.11f, 0.56f, 3800, 0.8f) },
				{ ("Spectral Class K", 9, "V"), (0.1f, 0.49f, 3650, 0.8f) },
				{ ("Spectral Class M", 0, "V"), (0.08f, 0.46f, 3500, 0.8f) },
				{ ("Spectral Class M", 1, "V"), (0.04f, 0.38f, 3350, 0.6f) },
				{ ("Spectral Class M", 2, "V"), (0.02f, 0.32f, 3200, 0.5f) },
				{ ("Spectral Class M", 3, "V"), (0.012f, 0.26f, 3050, 0.4f) },
				{ ("Spectral Class M", 4, "V"), (0.006f, 0.21f, 2900, 0.3f) },
				{ ("Spectral Class M", 5, "V"), (0.003f, 0.18f, 2750, 0.25f) },
				{ ("Spectral Class M", 6, "V"), (0.0017f, 0.15f, 2600, 0.2f) },
				{ ("Spectral Class M", 7, "V"), (0.0009f, 0.12f, 2450, 0.17f) },
				{ ("Spectral Class M", 8, "V"), (0.0005f, 0.1f, 2300, 0.14f) },
				{ ("Spectral Class M", 9, "V"), (0.0002f, 0.08f, 2200, 0.11f) },
				{ ("Spectral Class A", 0, "IV"), (156, 6.0f, 9700, 4.5f) },
				{ ("Spectral Class A", 1, "IV"), (127, 5.1f, 9450, 4.2f) },
				{ ("Spectral Class A", 2, "IV"), (102, 4.6f, 9200, 4) },
				{ ("Spectral Class A", 3, "IV"), (83, 4.3f, 8950, 3.8f) },
				{ ("Spectral Class A", 4, "IV"), (67, 4.0f, 8700, 3.6f) },
				{ ("Spectral Class A", 5, "IV"), (54, 3.7f, 8450, 3.5f) },
				{ ("Spectral Class A", 6, "IV"), (44, 3.4f, 8200, 3.3f) },
				{ ("Spectral Class A", 7, "IV"), (36, 3.1f, 7950, 3.2f) },
				{ ("Spectral Class A", 8, "IV"), (29, 2.9f, 7700, 3.1f) },
				{ ("Spectral Class A", 9, "IV"), (23, 2.7f, 7500, 2.9f) },
				{ ("Spectral Class F", 0, "IV"), (19, 2.5f, 7300, 2.7f) },
				{ ("Spectral Class F", 1, "IV"), (16.9f, 2.4f, 7200, 2.7f) },
				{ ("Spectral Class F", 2, "IV"), (15.1f, 2.3f, 7100, 2.6f) },
				{ ("Spectral Class F", 3, "IV"), (13.4f, 2.2f, 6950, 2.6f) },
				{ ("Spectral Class F", 4, "IV"), (12, 2.1f, 6800, 2.5f) },
				{ ("Spectral Class F", 5, "IV"), (10.7f, 2.0f, 6650, 2.5f) },
				{ ("Spectral Class F", 6, "IV"), (9.5f, 1.95f, 6500, 2.5f) },
				{ ("Spectral Class F", 7, "IV"), (8.5f, 1.9f, 6350, 2.5f) },
				{ ("Spectral Class F", 8, "IV"), (7.6f, 1.8f, 6200, 2.4f) },
				{ ("Spectral Class F", 9, "IV"), (6.7f, 1.7f, 6050, 2.4f) },
				{ ("Spectral Class G", 0, "IV"), (6.2f, 1.6f, 5900, 2.4f) },
				{ ("Spectral Class G", 1, "IV"), (5.9f, 1.55f, 5750, 2.4f) },
				{ ("Spectral Class G", 2, "IV"), (5.6f, 1.52f, 5600, 2.5f) },
				{ ("Spectral Class G", 3, "IV"), (5.4f, 1.49f, 5450, 2.6f) },
				{ ("Spectral Class G", 4, "IV"), (5.2f, 1.47f, 5300, 2.7f) },
				{ ("Spectral Class G", 5, "IV"), (5, 1.45f, 5200, 2.8f) },
				{ ("Spectral Class G", 6, "IV"), (4.8f, 1.44f, 5100, 2.8f) },
				{ ("Spectral Class G", 7, "IV"), (4.6f, 1.43f, 5000, 2.9f) },
				{ ("Spectral Class G", 8, "IV"), (4.4f, 1.42f, 4900, 2.9f) },
				{ ("Spectral Class G", 9, "IV"), (4.2f, 1.41f, 4800, 3) },
				{ ("Spectral Class K", 0, "IV"), (4, 1.4f, 4700, 3) },
				{ ("Spectral Class A", 0, "III"), (280, 12.0f, 9500, 6.2f) },
				{ ("Spectral Class A", 1, "III"), (240, 11.5f, 9250, 6.1f) },
				{ ("Spectral Class A", 2, "III"), (200, 11.0f, 9000, 5.9f) },
				{ ("Spectral Class A", 3, "III"), (170, 10.5f, 8750, 5.7f) },
				{ ("Spectral Class A", 4, "III"), (140, 10.0f, 8500, 5.6f) },
				{ ("Spectral Class A", 5, "III"), (120, 9.6f, 8250, 5.5f) },
				{ ("Spectral Class A", 6, "III"), (100, 9.2f, 8000, 5.3f) },
				{ ("Spectral Class A", 7, "III"), (87, 8.9f, 7750, 5.2f) },
				{ ("Spectral Class A", 8, "III"), (74, 8.6f, 7500, 5.1f) },
				{ ("Spectral Class A", 9, "III"), (63, 8.3f, 7350, 4.9f) },
				{ ("Spectral Class F", 0, "III"), (53, 8.0f, 7200, 4.7f) },
				{ ("Spectral Class F", 1, "III"), (51, 7.0f, 7050, 4.8f) },
				{ ("Spectral Class F", 2, "III"), (49, 6.0f, 6900, 4.9f) },
				{ ("Spectral Class F", 3, "III"), (47, 5.2f, 6750, 5.1f) },
				{ ("Spectral Class F", 4, "III"), (46, 4.7f, 6600, 5.2f) },
				{ ("Spectral Class F", 5, "III"), (45, 4.3f, 6450, 5.4f) },
				{ ("Spectral Class F", 6, "III"), (46, 3.9f, 6300, 5.7f) },
				{ ("Spectral Class F", 7, "III"), (47, 3.5f, 6150, 6.1f) },
				{ ("Spectral Class F", 8, "III"), (48, 3.1f, 6000, 6.5f) },
				{ ("Spectral Class F", 9, "III"), (49, 2.8f, 5900, 6.8f) },
				{ ("Spectral Class G", 0, "III"), (50, 2.5f, 5800, 7.1f) },
				{ ("Spectral Class G", 1, "III"), (55, 2.4f, 5700, 7.7f) },
				{ ("Spectral Class G", 2, "III"), (60, 2.5f, 5600, 8.3f) },
				{ ("Spectral Class G", 3, "III"), (65, 2.5f, 5500, 9) },
				{ ("Spectral Class G", 4, "III"), (70, 2.6f, 5400, 9.7f) },
				{ ("Spectral Class G", 5, "III"), (77, 2.7f, 5250, 10.7f) },
				{ ("Spectral Class G", 6, "III"), (85, 2.7f, 5100, 11.9f) },
				{ ("Spectral Class G", 7, "III"), (92, 2.8f, 4950, 13.2f) },
				{ ("Spectral Class G", 8, "III"), (101, 2.8f, 4800, 14.7f) },
				{ ("Spectral Class G", 9, "III"), (110, 2.9f, 4650, 16.3f) },
				{ ("Spectral Class K", 0, "III"), (120, 3.0f, 4500, 18.2f) },
				{ ("Spectral Class K", 1, "III"), (140, 3.3f, 4400, 20.4f) },
				{ ("Spectral Class K", 2, "III"), (160, 3.6f, 4300, 22.8f) },
				{ ("Spectral Class K", 3, "III"), (180, 3.9f, 4200, 25.6f) },
				{ ("Spectral Class K", 4, "III"), (210, 4.2f, 4100, 28.8f) },
				{ ("Spectral Class K", 5, "III"), (240, 4.5f, 4000, 32.4f) },
				{ ("Spectral Class K", 6, "III"), (270, 4.8f, 3900, 36.5f) },
				{ ("Spectral Class K", 7, "III"), (310, 5.1f, 3800, 41.2f) },
				{ ("Spectral Class K", 8, "III"), (360, 5.4f, 3700, 46.5f) },
				{ ("Spectral Class K", 9, "III"), (410, 5.8f, 3550, 54) },
				{ ("Spectral Class M", 0, "III"), (470, 6.2f, 3400, 63) },
				{ ("Spectral Class M", 1, "III"), (600, 6.4f, 3200, 80) },
				{ ("Spectral Class M", 2, "III"), (900, 6.6f, 3100, 105) },
				{ ("Spectral Class M", 3, "III"), (1300, 6.8f, 3000, 135) },
				{ ("Spectral Class M", 4, "III"), (1800, 7.2f, 2800, 180) },
				{ ("Spectral Class M", 5, "III"), (2300, 7.4f, 2650, 230) },
				{ ("Spectral Class M", 6, "III"), (2400, 7.8f, 2500, 260) },
				{ ("Spectral Class M", 7, "III"), (2500, 8.3f, 2400, 290) },
				{ ("Spectral Class M", 8, "III"), (2600, 8.8f, 2300, 325) },
				{ ("Spectral Class M", 9, "III"), (2700, 9.3f, 2200, 360) }
			};











	} 
}


*/