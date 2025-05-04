using System.Numerics;

namespace Neb25.Core.Galaxy.Generators
{
	
	public class StarSystemGenerator
	{
		public static StarSystem GenerateStarSystem(Galaxy galaxy, Random seed)
		{
			StarSystem newStarSystem = new StarSystem(galaxy);
			float galaxyRadius = Utils.Constants.GalaxyRadius;



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
			int trinarySizeCodeNum = -1;
			int trinarySpectralClassNum = -1;


			double quaternaryTempKelvin = -1;
			double quaternarySolarMasses = -1;
			double quaternarySolarLuminosity = -1;
			double quaternarySolarRadius = -1;

			string quaternaryBasicType = string.Empty;
			string quaternarySizeCode = string.Empty;
			int quaternarySizeCodeNum = -1;
			int quaternarySpectralClassNum = -1;








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


			if (hasBinary)
			{
				int roll1d10_binary_spectral_class_num = seed.Next(1, 11);
				int roll1d10_first_binary_roll = seed.Next(1, 11);
				int roll1d10_binary_specification_roll = seed.Next(1, 11);
				
				// if the first binary roll is 1-2, then we have a set of rules about spectral type and size class
				if (roll1d10_first_binary_roll <= 2)
				{
					binaryBasicType = primaryBasicType;
					binarySizeCode = primarySizeCode;
					binarySizeCodeNum = primarySizeCodeNum;




					if (roll1d10_binary_specification_roll >= roll1d10_primary_specification_roll) // then we get to use the binary's spec roll for occasional bumping up
					{

						if ((primaryBasicType == "Spectral Class A") && (primarySizeCode == "V"))
						{
							if (roll1d10_binary_specification_roll >= 7) { binarySizeCode = "IV"; binarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class F") && (primarySizeCode == "V"))
						{
							if (roll1d10_binary_specification_roll >= 9) { binarySizeCode = "IV"; binarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class G") && (primarySizeCode == "V"))
						{
							if (roll1d10_binary_specification_roll == 10) { binarySizeCode = "IV"; binarySizeCodeNum = 4; }
						}
						else if (primarySizeCodeNum == 3)
						{
							if (roll1d10_binary_specification_roll <= 1) binaryBasicType = "Spectral Class F";
							else if (roll1d10_binary_specification_roll <= 2) binaryBasicType = "Spectral Class G";
							else if (roll1d10_binary_specification_roll <= 7) binaryBasicType = "Spectral Class K";
							else if (roll1d10_binary_specification_roll >= 8)
							{
								binaryBasicType = "Spectral Class K";
								binarySizeCode = "IV";
								binarySizeCodeNum = 4;
							}

						}

					}
					else if (roll1d10_first_binary_roll >= 3) 
						//: Second star is of random type, determined by a roll on Table 1.1.1. However, treat the Giant result and any result that would give a second star of
						//	a higher type than the original star as Brown Dwarf results.
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
							if (roll1d10_trinary_specification_roll >= 7) { trinarySizeCode = "IV"; trinarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class F") && (primarySizeCode == "V"))
						{
							if (roll1d10_trinary_specification_roll >= 9) { trinarySizeCode = "IV"; trinarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class G") && (primarySizeCode == "V"))
						{
							if (roll1d10_trinary_specification_roll == 10) { trinarySizeCode = "IV"; trinarySizeCodeNum = 4; }
						}
						else if (primarySizeCodeNum == 3)
						{
							if (roll1d10_trinary_specification_roll <= 1) trinaryBasicType = "Spectral Class F";
							else if (roll1d10_trinary_specification_roll <= 2) trinaryBasicType = "Spectral Class G";
							else if (roll1d10_trinary_specification_roll <= 7) trinaryBasicType = "Spectral Class K";
							else if (roll1d10_trinary_specification_roll >= 8)
							{
								trinaryBasicType = "Spectral Class K";
								trinarySizeCode = "IV";
								trinarySizeCodeNum = 4;
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
							if (roll1d10_quaternary_specification_roll >= 7) { quaternarySizeCode = "IV"; quaternarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class F") && (primarySizeCode == "V"))
						{
							if (roll1d10_quaternary_specification_roll >= 9) { quaternarySizeCode = "IV"; quaternarySizeCodeNum = 4; }
						}
						else if ((primaryBasicType == "Spectral Class G") && (primarySizeCode == "V"))
						{
							if (roll1d10_quaternary_specification_roll == 10) { quaternarySizeCode = "IV"; quaternarySizeCodeNum = 4; }
						}
						else if (primarySizeCodeNum == 3)
						{
							if (roll1d10_quaternary_specification_roll <= 1) quaternaryBasicType = "Spectral Class F";
							else if (roll1d10_quaternary_specification_roll <= 2) quaternaryBasicType = "Spectral Class G";
							else if (roll1d10_quaternary_specification_roll <= 7) quaternaryBasicType = "Spectral Class K";
							else if (roll1d10_quaternary_specification_roll >= 8)
							{
								quaternaryBasicType = "Spectral Class K";
								quaternarySizeCode = "IV";
								quaternarySizeCodeNum = 4;
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

			// next, randomise subgiants (IV) according to the logic in table 1.1.3
			int roll1d10_randomise_subgiant_p = seed.Next(1, 11);
			int roll1d10_randomise_subgiant_b = seed.Next(1, 11);
			int roll1d10_randomise_subgiant_t = seed.Next(1, 11);
			int roll1d10_randomise_subgiant_q = seed.Next(1, 11);

			if (primarySizeCode == "IV")
			{
				if (roll1d10_randomise_subgiant_p == 3)
				{
					primarySolarMasses *= 0.9;
					primarySolarLuminosity *= 0.8;
				}
				else if (roll1d10_randomise_subgiant_p == 4) {
					primarySolarMasses *= 0.8;
					primarySolarLuminosity *= 0.6;
				}
				else if (roll1d10_randomise_subgiant_p == 5)
				{
					primarySolarMasses *= 0.7;
					primarySolarLuminosity = 0.4;
				}
				else if (roll1d10_randomise_subgiant_p == 6)
				{
					primarySolarMasses *= 0.6;
					primarySolarLuminosity = 0.2;
				}
				else if (roll1d10_randomise_subgiant_p == 7)
				{
					primarySolarMasses *= 1.1;
					primarySolarLuminosity *= 1.2;
				}
				else if (roll1d10_randomise_subgiant_p == 8)
				{
					primarySolarMasses *= 1.2;
					primarySolarLuminosity *= 1.4;
				}
				else if (roll1d10_randomise_subgiant_p == 9)
				{
					primarySolarMasses *= 1.3;
					primarySolarLuminosity *= 1.6;
				}
				else if (roll1d10_randomise_subgiant_p == 10)
				{
					primarySolarMasses *= 1.4;
					primarySolarLuminosity *= 1.8;
				}
				// after randomising the subgiants we have to recalculate the radius
				// the radius is derived from ... solar radiuses, radii  iguess ? = (Luminosity ^ 0.5) * (5800 / temp in kelvin)^2
				primarySolarRadius = (Math.Sqrt(primarySolarLuminosity) * Math.Pow((5800 / primaryTempKelvin), 2));
			}

			if (binarySizeCode == "IV")
			{
				if (roll1d10_randomise_subgiant_b == 3)
				{
					binarySolarMasses *= 0.9;
					binarySolarLuminosity *= 0.8;
				}
				else if (roll1d10_randomise_subgiant_b == 4)
				{
					binarySolarMasses *= 0.8;
					binarySolarLuminosity *= 0.6;
				}
				else if (roll1d10_randomise_subgiant_b == 5)
				{
					binarySolarMasses *= 0.7;
					binarySolarLuminosity = 0.4;
				}
				else if (roll1d10_randomise_subgiant_b == 6)
				{
					binarySolarMasses *= 0.6;
					binarySolarLuminosity = 0.2;
				}
				else if (roll1d10_randomise_subgiant_b == 7)
				{
					binarySolarMasses *= 1.1;
					binarySolarLuminosity *= 1.2;
				}
				else if (roll1d10_randomise_subgiant_b == 8)
				{
					binarySolarMasses *= 1.2;
					binarySolarLuminosity *= 1.4;
				}
				else if (roll1d10_randomise_subgiant_b == 9)
				{
					binarySolarMasses *= 1.3;
					binarySolarLuminosity *= 1.6;
				}
				else if (roll1d10_randomise_subgiant_b == 10)
				{
					binarySolarMasses *= 1.4;
					binarySolarLuminosity *= 1.8;
				}
				binarySolarRadius = (Math.Sqrt(binarySolarLuminosity) * Math.Pow((5800 / binaryTempKelvin), 2));
			}
			if (trinarySizeCode == "IV")
			{
				if (roll1d10_randomise_subgiant_t == 3)
				{
					trinarySolarMasses *= 0.9;
					trinarySolarLuminosity *= 0.8;
				}
				else if (roll1d10_randomise_subgiant_t == 4)
				{
					trinarySolarMasses *= 0.8;
					trinarySolarLuminosity *= 0.6;
				}
				else if (roll1d10_randomise_subgiant_t == 5)
				{
					trinarySolarMasses *= 0.7;
					trinarySolarLuminosity = 0.4;
				}
				else if (roll1d10_randomise_subgiant_t == 6)
				{
					trinarySolarMasses *= 0.6;
					trinarySolarLuminosity = 0.2;
				}
				else if (roll1d10_randomise_subgiant_t == 7)
				{
					trinarySolarMasses *= 1.1;
					trinarySolarLuminosity *= 1.2;
				}
				else if (roll1d10_randomise_subgiant_t == 8)
				{
					trinarySolarMasses *= 1.2;
					trinarySolarLuminosity *= 1.4;
				}
				else if (roll1d10_randomise_subgiant_t == 9)
				{
					trinarySolarMasses *= 1.3;
					trinarySolarLuminosity *= 1.6;
				}
				else if (roll1d10_randomise_subgiant_t == 10)
				{
					trinarySolarMasses *= 1.4;
					trinarySolarLuminosity *= 1.8;
				}
				trinarySolarRadius = (Math.Sqrt(trinarySolarLuminosity) * Math.Pow((5800 / trinaryTempKelvin), 2));
			}
			if (quaternarySizeCode == "IV")
			{
				if (roll1d10_randomise_subgiant_q == 3)
				{
					quaternarySolarMasses *= 0.9;
					quaternarySolarLuminosity *= 0.8;
				}
				else if (roll1d10_randomise_subgiant_q == 4)
				{
					quaternarySolarMasses *= 0.8;
					quaternarySolarLuminosity *= 0.6;
				}
				else if (roll1d10_randomise_subgiant_q == 5)
				{
					quaternarySolarMasses *= 0.7;
					quaternarySolarLuminosity = 0.4;
				}
				else if (roll1d10_randomise_subgiant_q == 6)
				{
					quaternarySolarMasses *= 0.6;
					quaternarySolarLuminosity = 0.2;
				}
				else if (roll1d10_randomise_subgiant_q == 7)
				{
					quaternarySolarMasses *= 1.1;
					quaternarySolarLuminosity *= 1.2;
				}
				else if (roll1d10_randomise_subgiant_q == 8)
				{
					quaternarySolarMasses *= 1.2;
					quaternarySolarLuminosity *= 1.4;
				}
				else if (roll1d10_randomise_subgiant_q == 9)
				{
					quaternarySolarMasses *= 1.3;
					quaternarySolarLuminosity *= 1.6;
				}
				else if (roll1d10_randomise_subgiant_q == 10)
				{
					quaternarySolarMasses *= 1.4;
					quaternarySolarLuminosity *= 1.8;
				}
				quaternarySolarRadius = (Math.Sqrt(quaternarySolarLuminosity) * Math.Pow((5800 / quaternaryTempKelvin), 2));
			}


			// now randomise giant class stars
			int roll1d10_randomise_giant_p = seed.Next(1, 11);
			int roll1d10_randomise_giant_b = seed.Next(1, 11);
			int roll1d10_randomise_giant_t = seed.Next(1, 11);
			int roll1d10_randomise_giant_q = seed.Next(1, 11);

			if (primarySizeCode == "V")
			{
				if (roll1d10_randomise_giant_p == 1) { primarySolarMasses *= 0.3; primarySolarLuminosity *= 0.3; }
				if (roll1d10_randomise_giant_p == 2) { primarySolarMasses *= 0.4; primarySolarLuminosity *= 0.4; }
				if (roll1d10_randomise_giant_p == 3) { primarySolarMasses *= 0.5; primarySolarLuminosity *= 0.5; }
				if (roll1d10_randomise_giant_p == 4) { primarySolarMasses *= 0.6; primarySolarLuminosity *= 0.6; }
				if (roll1d10_randomise_giant_p == 5) { primarySolarMasses *= 0.7; primarySolarLuminosity *= 0.7; }
				if (roll1d10_randomise_giant_p == 6) { primarySolarMasses *= 0.8; primarySolarLuminosity *= 0.8; }
				if (roll1d10_randomise_giant_p == 7) { primarySolarMasses *= 0.9; primarySolarLuminosity *= 0.9; }
				if (roll1d10_randomise_giant_p == 8) { primarySolarMasses *= 1.0; primarySolarLuminosity *= 1.0; }
				if (roll1d10_randomise_giant_p == 9) { primarySolarMasses *= 1.25; primarySolarLuminosity *= 1.5; }
				if (roll1d10_randomise_giant_p == 10) { primarySolarMasses *= 1.5; primarySolarLuminosity *= 2; }
				primarySolarRadius = (Math.Sqrt(primarySolarLuminosity) * Math.Pow((5800 / primaryTempKelvin), 2));
			}

			if (binarySizeCode == "V")
			{
				if (roll1d10_randomise_giant_b == 1) { binarySolarMasses *= 0.3; binarySolarLuminosity *= 0.3; }
				if (roll1d10_randomise_giant_b == 2) { binarySolarMasses *= 0.4; binarySolarLuminosity *= 0.4; }
				if (roll1d10_randomise_giant_b == 3) { binarySolarMasses *= 0.5; binarySolarLuminosity *= 0.5; }
				if (roll1d10_randomise_giant_b == 4) { binarySolarMasses *= 0.6; binarySolarLuminosity *= 0.6; }
				if (roll1d10_randomise_giant_b == 5) { binarySolarMasses *= 0.7; binarySolarLuminosity *= 0.7; }
				if (roll1d10_randomise_giant_b == 6) { binarySolarMasses *= 0.8; binarySolarLuminosity *= 0.8; }
				if (roll1d10_randomise_giant_b == 7) { binarySolarMasses *= 0.9; binarySolarLuminosity *= 0.9; }
				if (roll1d10_randomise_giant_b == 8) { binarySolarMasses *= 1.0; binarySolarLuminosity *= 1.0; }
				if (roll1d10_randomise_giant_b == 9) { binarySolarMasses *= 1.25; binarySolarLuminosity *= 1.5; }
				if (roll1d10_randomise_giant_b == 10) { binarySolarMasses *= 1.5; binarySolarLuminosity *= 2; }
				binarySolarRadius = (Math.Sqrt(binarySolarLuminosity) * Math.Pow((5800 / binaryTempKelvin), 2));
			}

			if (trinarySizeCode == "V")
			{
				if (roll1d10_randomise_giant_t == 1) { trinarySolarMasses *= 0.3; trinarySolarLuminosity *= 0.3; }
				if (roll1d10_randomise_giant_t == 2) { trinarySolarMasses *= 0.4; trinarySolarLuminosity *= 0.4; }
				if (roll1d10_randomise_giant_t == 3) { trinarySolarMasses *= 0.5; trinarySolarLuminosity *= 0.5; }
				if (roll1d10_randomise_giant_t == 4) { trinarySolarMasses *= 0.6; trinarySolarLuminosity *= 0.6; }
				if (roll1d10_randomise_giant_t == 5) { trinarySolarMasses *= 0.7; trinarySolarLuminosity *= 0.7; }
				if (roll1d10_randomise_giant_t == 6) { trinarySolarMasses *= 0.8; trinarySolarLuminosity *= 0.8; }
				if (roll1d10_randomise_giant_t == 7) { trinarySolarMasses *= 0.9; trinarySolarLuminosity *= 0.9; }
				if (roll1d10_randomise_giant_t == 8) { trinarySolarMasses *= 1.0; trinarySolarLuminosity *= 1.0; }
				if (roll1d10_randomise_giant_t == 9) { trinarySolarMasses *= 1.25; trinarySolarLuminosity *= 1.5; }
				if (roll1d10_randomise_giant_t == 10) { trinarySolarMasses *= 1.5; trinarySolarLuminosity *= 2; }
				trinarySolarRadius = (Math.Sqrt(trinarySolarLuminosity) * Math.Pow((5800 / trinaryTempKelvin), 2));
			}

			if (quaternarySizeCode == "V")
			{
				if (roll1d10_randomise_giant_q == 1) { quaternarySolarMasses *= 0.3; quaternarySolarLuminosity *= 0.3; }
				if (roll1d10_randomise_giant_q == 2) { quaternarySolarMasses *= 0.4; quaternarySolarLuminosity *= 0.4; }
				if (roll1d10_randomise_giant_q == 3) { quaternarySolarMasses *= 0.5; quaternarySolarLuminosity *= 0.5; }
				if (roll1d10_randomise_giant_q == 4) { quaternarySolarMasses *= 0.6; quaternarySolarLuminosity *= 0.6; }
				if (roll1d10_randomise_giant_q == 5) { quaternarySolarMasses *= 0.7; quaternarySolarLuminosity *= 0.7; }
				if (roll1d10_randomise_giant_q == 6) { quaternarySolarMasses *= 0.8; quaternarySolarLuminosity *= 0.8; }
				if (roll1d10_randomise_giant_q == 7) { quaternarySolarMasses *= 0.9; quaternarySolarLuminosity *= 0.9; }
				if (roll1d10_randomise_giant_q == 8) { quaternarySolarMasses *= 1.0; quaternarySolarLuminosity *= 1.0; }
				if (roll1d10_randomise_giant_q == 9) { quaternarySolarMasses *= 1.25; quaternarySolarLuminosity *= 1.5; }
				if (roll1d10_randomise_giant_q == 10) { quaternarySolarMasses *= 1.5; quaternarySolarLuminosity *= 2; }
				quaternarySolarRadius = (Math.Sqrt(quaternarySolarLuminosity) * Math.Pow((5800 / quaternaryTempKelvin), 2));
			}


			// now randomise the white dwarfs
			int roll1d10_whitedwarf_randomise_p = seed.Next(1, 11);
			int roll1d10_whitedwarf_randomise_b = seed.Next(1, 11);
			int roll1d10_whitedwarf_randomise_t = seed.Next(1, 11);
			int roll1d10_whitedwarf_randomise_q = seed.Next(1, 11);

			int roll1d10_whitedwarf_randomise_p_temp = seed.Next(1, 11);
			int roll1d10_whitedwarf_randomise_b_temp = seed.Next(1, 11);
			int roll1d10_whitedwarf_randomise_t_temp = seed.Next(1, 11);
			int roll1d10_whitedwarf_randomise_q_temp = seed.Next(1, 11);


			if (primaryBasicType == "White Dwarf") {
				if (roll1d10_whitedwarf_randomise_p == 1) { primarySolarMasses = 1.3; primarySolarRadius = 0.004; }
				if (roll1d10_whitedwarf_randomise_p == 2) { primarySolarMasses = 1.1; primarySolarRadius = 0.007; }
				if (roll1d10_whitedwarf_randomise_p == 3) { primarySolarMasses = 0.9; primarySolarRadius = 0.009; }
				if (roll1d10_whitedwarf_randomise_p == 4) { primarySolarMasses = 0.7; primarySolarRadius = 0.010; }
				if (roll1d10_whitedwarf_randomise_p == 5) { primarySolarMasses = 0.6; primarySolarRadius = 0.011; }
				if (roll1d10_whitedwarf_randomise_p == 6) { primarySolarMasses = 0.55; primarySolarRadius = 0.012; }
				if (roll1d10_whitedwarf_randomise_p == 7) { primarySolarMasses = 0.5; primarySolarRadius = 0.013; }
				if (roll1d10_whitedwarf_randomise_p == 8) { primarySolarMasses = 0.45; primarySolarRadius = 0.014; }
				if (roll1d10_whitedwarf_randomise_p == 9) { primarySolarMasses = 0.4; primarySolarRadius = 0.015; }
				if (roll1d10_whitedwarf_randomise_p == 10) { primarySolarMasses = 0.35; primarySolarRadius = 0.016; }
				

			}
			if (binaryBasicType == "White Dwarf") {
				if (roll1d10_whitedwarf_randomise_b == 1) { binarySolarMasses = 1.3; binarySolarRadius = 0.004; }
				if (roll1d10_whitedwarf_randomise_b == 2) { binarySolarMasses = 1.1; binarySolarRadius = 0.007; }
				if (roll1d10_whitedwarf_randomise_b == 3) { binarySolarMasses = 0.9; binarySolarRadius = 0.009; }
				if (roll1d10_whitedwarf_randomise_b == 4) { binarySolarMasses = 0.7; binarySolarRadius = 0.010; }
				if (roll1d10_whitedwarf_randomise_b == 5) { binarySolarMasses = 0.6; binarySolarRadius = 0.011; }
				if (roll1d10_whitedwarf_randomise_b == 6) { binarySolarMasses = 0.55; binarySolarRadius = 0.012; }
				if (roll1d10_whitedwarf_randomise_b == 7) { binarySolarMasses = 0.5; binarySolarRadius = 0.013; }
				if (roll1d10_whitedwarf_randomise_b == 8) { binarySolarMasses = 0.45; binarySolarRadius = 0.014; }
				if (roll1d10_whitedwarf_randomise_b == 9) { binarySolarMasses = 0.4; binarySolarRadius = 0.015; }
				if (roll1d10_whitedwarf_randomise_b == 10) { binarySolarMasses = 0.35; binarySolarRadius = 0.016; }
				
			}
			if (trinaryBasicType == "White Dwarf")
			{
				if (roll1d10_whitedwarf_randomise_t == 1) { trinarySolarMasses = 1.3; trinarySolarRadius = 0.004; }
				if (roll1d10_whitedwarf_randomise_t == 2) { trinarySolarMasses = 1.1; trinarySolarRadius = 0.007; }
				if (roll1d10_whitedwarf_randomise_t == 3) { trinarySolarMasses = 0.9; trinarySolarRadius = 0.009; }
				if (roll1d10_whitedwarf_randomise_t == 4) { trinarySolarMasses = 0.7; trinarySolarRadius = 0.010; }
				if (roll1d10_whitedwarf_randomise_t == 5) { trinarySolarMasses = 0.6; trinarySolarRadius = 0.011; }
				if (roll1d10_whitedwarf_randomise_t == 6) { trinarySolarMasses = 0.55; trinarySolarRadius = 0.012; }
				if (roll1d10_whitedwarf_randomise_t == 7) { trinarySolarMasses = 0.5; trinarySolarRadius = 0.013; }
				if (roll1d10_whitedwarf_randomise_t == 8) { trinarySolarMasses = 0.45; trinarySolarRadius = 0.014; }
				if (roll1d10_whitedwarf_randomise_t == 9) { trinarySolarMasses = 0.4; trinarySolarRadius = 0.015; }
				if (roll1d10_whitedwarf_randomise_t == 10) { trinarySolarMasses = 0.35; trinarySolarRadius = 0.016; }
			}
			if (quaternaryBasicType == "White Dwarf")
			{
				if (roll1d10_whitedwarf_randomise_q == 1) { quaternarySolarMasses = 1.3; quaternarySolarRadius = 0.004; }
				if (roll1d10_whitedwarf_randomise_q == 2) { quaternarySolarMasses = 1.1; quaternarySolarRadius = 0.007; }
				if (roll1d10_whitedwarf_randomise_q == 3) { quaternarySolarMasses = 0.9; quaternarySolarRadius = 0.009; }
				if (roll1d10_whitedwarf_randomise_q == 4) { quaternarySolarMasses = 0.7; quaternarySolarRadius = 0.010; }
				if (roll1d10_whitedwarf_randomise_q == 5) { quaternarySolarMasses = 0.6; quaternarySolarRadius = 0.011; }
				if (roll1d10_whitedwarf_randomise_q == 6) { quaternarySolarMasses = 0.55; quaternarySolarRadius = 0.012; }
				if (roll1d10_whitedwarf_randomise_q == 7) { quaternarySolarMasses = 0.5; quaternarySolarRadius = 0.013; }
				if (roll1d10_whitedwarf_randomise_q == 8) { quaternarySolarMasses = 0.45; quaternarySolarRadius = 0.014; }
				if (roll1d10_whitedwarf_randomise_q == 9) { quaternarySolarMasses = 0.4; quaternarySolarRadius = 0.015; }
				if (roll1d10_whitedwarf_randomise_q == 10) { quaternarySolarMasses = 0.35; quaternarySolarRadius = 0.016; }
			}


			if (primaryBasicType == "White Dwarf")
			{
				if (roll1d10_whitedwarf_randomise_p_temp == 1) { primaryTempKelvin = 30000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 2) { primaryTempKelvin = 25000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 3) { primaryTempKelvin = 20000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 4) { primaryTempKelvin = 16000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 5) { primaryTempKelvin = 14000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 6) { primaryTempKelvin = 12000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 7) { primaryTempKelvin = 10000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 8) { primaryTempKelvin = 8000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 9) { primaryTempKelvin = 6000; }
				if (roll1d10_whitedwarf_randomise_p_temp == 10) { primaryTempKelvin = 4000; }
				primarySolarLuminosity = Math.Pow(primarySolarRadius, 2) * ((Math.Pow(primaryTempKelvin, 4)) / Math.Pow(5800, 4));

			}
			if (binaryBasicType == "White Dwarf")
			{
				if (roll1d10_whitedwarf_randomise_b_temp == 1) { binaryTempKelvin = 30000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 2) { binaryTempKelvin = 25000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 3) { binaryTempKelvin = 20000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 4) { binaryTempKelvin = 16000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 5) { binaryTempKelvin = 14000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 6) { binaryTempKelvin = 12000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 7) { binaryTempKelvin = 10000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 8) { binaryTempKelvin = 8000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 9) { binaryTempKelvin = 6000; }
				if (roll1d10_whitedwarf_randomise_b_temp == 10) { binaryTempKelvin = 4000; }
				binarySolarLuminosity = Math.Pow(binarySolarRadius, 2) * ((Math.Pow(binaryTempKelvin, 4)) / Math.Pow(5800, 4));
			}
			if (trinaryBasicType == "White Dwarf")
			{

				if (roll1d10_whitedwarf_randomise_t_temp == 1) { trinaryTempKelvin = 30000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 2) { trinaryTempKelvin = 25000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 3) { trinaryTempKelvin = 20000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 4) { trinaryTempKelvin = 16000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 5) { trinaryTempKelvin = 14000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 6) { trinaryTempKelvin = 12000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 7) { trinaryTempKelvin = 10000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 8) { trinaryTempKelvin = 8000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 9) { trinaryTempKelvin = 6000; }
				if (roll1d10_whitedwarf_randomise_t_temp == 10) { trinaryTempKelvin = 4000; }
				trinarySolarLuminosity = Math.Pow(trinarySolarRadius, 2) * ((Math.Pow(trinaryTempKelvin, 4)) / Math.Pow(5800, 4));
			}
			if (quaternaryBasicType == "White Dwarf")
			{
				if (roll1d10_whitedwarf_randomise_q_temp == 1) { quaternaryTempKelvin = 30000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 2) { quaternaryTempKelvin = 25000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 3) { quaternaryTempKelvin = 20000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 4) { quaternaryTempKelvin = 16000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 5) { quaternaryTempKelvin = 14000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 6) { quaternaryTempKelvin = 12000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 7) { quaternaryTempKelvin = 10000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 8) { quaternaryTempKelvin = 8000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 9) { quaternaryTempKelvin = 6000; }
				if (roll1d10_whitedwarf_randomise_q_temp == 10) { quaternaryTempKelvin = 4000; }
				quaternarySolarLuminosity = Math.Pow(quaternarySolarRadius, 2) * ((Math.Pow(quaternaryTempKelvin, 4)) / Math.Pow(5800, 4));
			}



			// now randomise the brown dwarfs
			int roll1d10_browndwarf_randomise_p = seed.Next(1, 11);
			int roll1d10_browndwarf_randomise_b = seed.Next(1, 11);
			int roll1d10_browndwarf_randomise_t = seed.Next(1, 11);
			int roll1d10_browndwarf_randomise_q = seed.Next(1, 11);
			int roll1d10_browndwarf_randomise_p_temp = seed.Next(1, 11);
			int roll1d10_browndwarf_randomise_b_temp = seed.Next(1, 11);
			int roll1d10_browndwarf_randomise_t_temp = seed.Next(1, 11);
			int roll1d10_browndwarf_randomise_q_temp = seed.Next(1, 11);

			if (primaryBasicType == "Brown Dwarf")
			{
				if (roll1d10_browndwarf_randomise_p == 1) { primarySolarMasses = 0.07; primarySolarRadius = 0.07; }
				if (roll1d10_browndwarf_randomise_p == 2) { primarySolarMasses = 0.064; primarySolarRadius = 0.08; }
				if (roll1d10_browndwarf_randomise_p == 3) { primarySolarMasses = 0.058; primarySolarRadius = 0.09; }
				if (roll1d10_browndwarf_randomise_p == 4) { primarySolarMasses = 0.052; primarySolarRadius = 0.10; }
				if (roll1d10_browndwarf_randomise_p == 5) { primarySolarMasses = 0.046; primarySolarRadius = 0.11; }
				if (roll1d10_browndwarf_randomise_p == 6) { primarySolarMasses = 0.040; primarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_p == 7) { primarySolarMasses = 0.034; primarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_p == 8) { primarySolarMasses = 0.026; primarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_p == 9) { primarySolarMasses = 0.020; primarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_p == 10) { primarySolarMasses = 0.014; primarySolarRadius = 0.12; }
			}
			if (binaryBasicType == "Brown Dwarf")
			{
				if (roll1d10_browndwarf_randomise_b == 1) { binarySolarMasses = 0.07; binarySolarRadius = 0.07; }
				if (roll1d10_browndwarf_randomise_b == 2) { binarySolarMasses = 0.064; binarySolarRadius = 0.08; }
				if (roll1d10_browndwarf_randomise_b == 3) { binarySolarMasses = 0.058; binarySolarRadius = 0.09; }
				if (roll1d10_browndwarf_randomise_b == 4) { binarySolarMasses = 0.052; binarySolarRadius = 0.10; }
				if (roll1d10_browndwarf_randomise_b == 5) { binarySolarMasses = 0.046; binarySolarRadius = 0.11; }
				if (roll1d10_browndwarf_randomise_b == 6) { binarySolarMasses = 0.040; binarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_b == 7) { binarySolarMasses = 0.034; binarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_b == 8) { binarySolarMasses = 0.026; binarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_b == 9) { binarySolarMasses = 0.020; binarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_b == 10) { binarySolarMasses = 0.014; binarySolarRadius = 0.12; }
			}
			if (trinaryBasicType == "Brown Dwarf")
			{
				if (roll1d10_browndwarf_randomise_t == 1) { trinarySolarMasses = 0.07; trinarySolarRadius = 0.07; }
				if (roll1d10_browndwarf_randomise_t == 2) { trinarySolarMasses = 0.064; trinarySolarRadius = 0.08; }
				if (roll1d10_browndwarf_randomise_t == 3) { trinarySolarMasses = 0.058; trinarySolarRadius = 0.09; }
				if (roll1d10_browndwarf_randomise_t == 4) { trinarySolarMasses = 0.052; trinarySolarRadius = 0.10; }
				if (roll1d10_browndwarf_randomise_t == 5) { trinarySolarMasses = 0.046; trinarySolarRadius = 0.11; }
				if (roll1d10_browndwarf_randomise_t == 6) { trinarySolarMasses = 0.040; trinarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_t == 7) { trinarySolarMasses = 0.034; trinarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_t == 8) { trinarySolarMasses = 0.026; trinarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_t == 9) { trinarySolarMasses = 0.020; trinarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_t == 10) { trinarySolarMasses = 0.014; trinarySolarRadius = 0.12; }
			}
			if (quaternaryBasicType == "Brown Dwarf")
			{
				if (roll1d10_browndwarf_randomise_q == 1) { quaternarySolarMasses = 0.07; quaternarySolarRadius = 0.07; }
				if (roll1d10_browndwarf_randomise_q == 2) { quaternarySolarMasses = 0.064; quaternarySolarRadius = 0.08; }
				if (roll1d10_browndwarf_randomise_q == 3) { quaternarySolarMasses = 0.058; quaternarySolarRadius = 0.09; }
				if (roll1d10_browndwarf_randomise_q == 4) { quaternarySolarMasses = 0.052; quaternarySolarRadius = 0.10; }
				if (roll1d10_browndwarf_randomise_q == 5) { quaternarySolarMasses = 0.046; quaternarySolarRadius = 0.11; }
				if (roll1d10_browndwarf_randomise_q == 6) { quaternarySolarMasses = 0.040; quaternarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_q == 7) { quaternarySolarMasses = 0.034; quaternarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_q == 8) { quaternarySolarMasses = 0.026; quaternarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_q == 9) { quaternarySolarMasses = 0.020; quaternarySolarRadius = 0.12; }
				if (roll1d10_browndwarf_randomise_q == 10) { quaternarySolarMasses = 0.014; quaternarySolarRadius = 0.12; }
			}


			if (primaryBasicType == "Brown Dwarf")
			{
				if (roll1d10_browndwarf_randomise_p_temp == 1) { primaryTempKelvin = 2200; }
				if (roll1d10_browndwarf_randomise_p_temp == 2) { primaryTempKelvin = 2000; }
				if (roll1d10_browndwarf_randomise_p_temp == 3) { primaryTempKelvin = 1800; }
				if (roll1d10_browndwarf_randomise_p_temp == 4) { primaryTempKelvin = 1600; }
				if (roll1d10_browndwarf_randomise_p_temp == 5) { primaryTempKelvin = 1400; }
				if (roll1d10_browndwarf_randomise_p_temp == 6) { primaryTempKelvin = 1200; }
				if (roll1d10_browndwarf_randomise_p_temp == 7) { primaryTempKelvin = 1000; }
				if (roll1d10_browndwarf_randomise_p_temp == 8) { primaryTempKelvin = 900; }
				if (roll1d10_browndwarf_randomise_p_temp == 9) { primaryTempKelvin = 800; }
				if (roll1d10_browndwarf_randomise_p_temp == 10) { primaryTempKelvin = 700; }
				primarySolarLuminosity = Math.Pow(primarySolarRadius, 2) * ((Math.Pow(primaryTempKelvin, 4)) / Math.Pow(5800, 4));
			}
			if (binaryBasicType == "Brown Dwarf")
			{
				if (roll1d10_browndwarf_randomise_b_temp == 1) { binaryTempKelvin = 2200; }
				if (roll1d10_browndwarf_randomise_b_temp == 2) { binaryTempKelvin = 2000; }
				if (roll1d10_browndwarf_randomise_b_temp == 3) { binaryTempKelvin = 1800; }
				if (roll1d10_browndwarf_randomise_b_temp == 4) { binaryTempKelvin = 1600; }
				if (roll1d10_browndwarf_randomise_b_temp == 5) { binaryTempKelvin = 1400; }
				if (roll1d10_browndwarf_randomise_b_temp == 6) { binaryTempKelvin = 1200; }
				if (roll1d10_browndwarf_randomise_b_temp == 7) { binaryTempKelvin = 1000; }
				if (roll1d10_browndwarf_randomise_b_temp == 8) { binaryTempKelvin = 900; }
				if (roll1d10_browndwarf_randomise_b_temp == 9) { binaryTempKelvin = 800; }
				if (roll1d10_browndwarf_randomise_b_temp == 10) { binaryTempKelvin = 700; }
				binarySolarLuminosity = Math.Pow(binarySolarRadius, 2) * ((Math.Pow(binaryTempKelvin, 4)) / Math.Pow(5800, 4));
				}
			if (trinaryBasicType == "Brown Dwarf")
			{

				if (roll1d10_browndwarf_randomise_t_temp == 1) { trinaryTempKelvin = 2200; }
				if (roll1d10_browndwarf_randomise_t_temp == 2) { trinaryTempKelvin = 2000; }
				if (roll1d10_browndwarf_randomise_t_temp == 3) { trinaryTempKelvin = 1800; }
				if (roll1d10_browndwarf_randomise_t_temp == 4) { trinaryTempKelvin = 1600; }
				if (roll1d10_browndwarf_randomise_t_temp == 5) { trinaryTempKelvin = 1400; }
				if (roll1d10_browndwarf_randomise_t_temp == 6) { trinaryTempKelvin = 1200; }
				if (roll1d10_browndwarf_randomise_t_temp == 7) { trinaryTempKelvin = 1000; }
				if (roll1d10_browndwarf_randomise_t_temp == 8) { trinaryTempKelvin = 900; }
				if (roll1d10_browndwarf_randomise_t_temp == 9) { trinaryTempKelvin = 800; }
				if (roll1d10_browndwarf_randomise_t_temp == 10) { trinaryTempKelvin = 700; }
				trinarySolarLuminosity = Math.Pow(trinarySolarRadius, 2) * ((Math.Pow(trinaryTempKelvin, 4)) / Math.Pow(5800, 4));
				}
			if (quaternaryBasicType == "Brown Dwarf")
			{
				if (roll1d10_browndwarf_randomise_q_temp == 1) { quaternaryTempKelvin = 2200; }
				if (roll1d10_browndwarf_randomise_q_temp == 2) { quaternaryTempKelvin = 2000; }
				if (roll1d10_browndwarf_randomise_q_temp == 3) { quaternaryTempKelvin = 1800; }
				if (roll1d10_browndwarf_randomise_q_temp == 4) { quaternaryTempKelvin = 1600; }
				if (roll1d10_browndwarf_randomise_q_temp == 5) { quaternaryTempKelvin = 1400; }
				if (roll1d10_browndwarf_randomise_q_temp == 6) { quaternaryTempKelvin = 1200; }
				if (roll1d10_browndwarf_randomise_q_temp == 7) { quaternaryTempKelvin = 1000; }
				if (roll1d10_browndwarf_randomise_q_temp == 8) { quaternaryTempKelvin = 900; }
				if (roll1d10_browndwarf_randomise_q_temp == 9) { quaternaryTempKelvin = 800; }
				if (roll1d10_browndwarf_randomise_q_temp == 10) { quaternaryTempKelvin = 700; }
				quaternarySolarLuminosity = Math.Pow(quaternarySolarRadius, 2) * ((Math.Pow(quaternaryTempKelvin, 4)) / Math.Pow(5800, 4));
				}


			// now moving to 
			// PART I SYSTEM DATA 8
			// ONE / 2 SYSTEM AGE AND ABUNDANCE

			double primaryAgeInGY = 0;
			double primaryLifeInGY = 0;
			double primaryLuminosityModFromAge = 1;

			int roll1d10_primary_age_roll = seed.Next(1, 11);

			if (primaryBasicType == "Spectral Class B") primaryLifeInGY = 0.1;

			if (primaryBasicType == "Spectral Class A" && primarySpectralClassNum <= 4) {
				primaryLifeInGY = 0.6;
				if (roll1d10_primary_age_roll <= 2) primaryAgeInGY = 0.1;
				else if (roll1d10_primary_age_roll <= 4) primaryAgeInGY = 0.2;
				else if (roll1d10_primary_age_roll <= 6) primaryAgeInGY = 0.3;
				else if (roll1d10_primary_age_roll <= 8) primaryAgeInGY = 0.4;
				else if (roll1d10_primary_age_roll == 9) primaryAgeInGY = 0.5;
				else if (roll1d10_primary_age_roll == 10) primaryAgeInGY = 0.6;
			}
			else if (primaryBasicType == "Spectral Class A" && primarySpectralClassNum <= 9) // a5-a9
			{
				primaryLifeInGY = 1.3;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 0.2;
					primaryLuminosityModFromAge = 0.8;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 0.4;
					primaryLuminosityModFromAge = 0.8;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 0.5;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 0.6;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 0.7;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 0.8;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 0.9;
					primaryLuminosityModFromAge = 1.1;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 1;
					primaryLuminosityModFromAge = 1.1;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 1.1;
					primaryLuminosityModFromAge = 1.2;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 1.2;
					primaryLuminosityModFromAge = 1.2;
				}
			}
			else if (primaryBasicType == "Spectral Class F" && primarySpectralClassNum <= 4) // f0-f4
			{
				primaryLifeInGY = 3.2;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 0.3;
					primaryLuminosityModFromAge = 0.6;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 0.6;
					primaryLuminosityModFromAge = 0.7;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 1;
					primaryLuminosityModFromAge = 0.8;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 1.3;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 1.6;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 2.0;
					primaryLuminosityModFromAge = 1.1;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 2.3;
					primaryLuminosityModFromAge = 1.2;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 2.6;
					primaryLuminosityModFromAge = 1.3;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 2.9;
					primaryLuminosityModFromAge = 1.4;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 3.2;
					primaryLuminosityModFromAge = 1.5;
				}
			}
			else if (primaryBasicType == "Spectral Class F" && primarySpectralClassNum >= 5 && primarySpectralClassNum <= 9) // f5-f9
			{
				primaryLifeInGY = 5.6;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 0.5;
					primaryLuminosityModFromAge = 0.6;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 1;
					primaryLuminosityModFromAge = 0.7;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 1.5;
					primaryLuminosityModFromAge = 0.8;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 2;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 2.5;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 3.0;
					primaryLuminosityModFromAge = 1.1;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 3.5;
					primaryLuminosityModFromAge = 1.2;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 4.0;
					primaryLuminosityModFromAge = 1.3;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 4.5;
					primaryLuminosityModFromAge = 1.4;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 5;
					primaryLuminosityModFromAge = 1.5;
				}
			}

			else if (primaryBasicType == "Spectral Class G" && primarySpectralClassNum >= 0 && primarySpectralClassNum <= 4) // g0-g4
			{
				primaryLifeInGY = 10;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 1.0;
					primaryLuminosityModFromAge = 0.6;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 2.0;
					primaryLuminosityModFromAge = 0.7;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 3.0;
					primaryLuminosityModFromAge = 0.8;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 4.0;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 5.0;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 6.0;
					primaryLuminosityModFromAge = 1.1;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 7.0;
					primaryLuminosityModFromAge = 1.2;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 8.0;
					primaryLuminosityModFromAge = 1.3;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 9.0;
					primaryLuminosityModFromAge = 1.4;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 10.0;
					primaryLuminosityModFromAge = 1.5;
				}
			}
			else if (primaryBasicType == "Spectral Class G" && primarySpectralClassNum >= 5 && primarySpectralClassNum <= 9) // g5-g9
			{
				primaryLifeInGY = 14.0;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 1.0;
					primaryLuminosityModFromAge = 0.6;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 2.0;
					primaryLuminosityModFromAge = 0.7;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 3.0;
					primaryLuminosityModFromAge = 0.8;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 4.0;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 5.0;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 6.0;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 7.0;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 8.0;
					primaryLuminosityModFromAge = 1.1;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 9.0;
					primaryLuminosityModFromAge = 1.2;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 10.0;
					primaryLuminosityModFromAge = 1.3;
				}
			}
			else if (primaryBasicType == "Spectral Class K" && primarySpectralClassNum >= 0 && primarySpectralClassNum <= 5) // k0-k5
			{
				primaryLifeInGY = 23.0;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 1.0;
					primaryLuminosityModFromAge = 0.8;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 2.0;
					primaryLuminosityModFromAge = 0.85;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 3.0;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 4.0;
					primaryLuminosityModFromAge = 0.95;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 5.0;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 6.0;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 7.0;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 8.0;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 9.0;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 10.0;
					primaryLuminosityModFromAge = 1.05;
				}
			}
			else if (primaryBasicType == "Spectral Class K" && primarySpectralClassNum >= 5 && primarySpectralClassNum <= 9) // k5-k9
			{
				primaryLifeInGY = 42.0;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 1.0;
					primaryLuminosityModFromAge = 0.9;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 2.0;
					primaryLuminosityModFromAge = 0.95;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 3.0;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 4.0;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 5.0;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 6.0;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 7.0;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 8.0;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 9.0;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 10.0;
				}
			}
			else if (primaryBasicType == "Spectral Class M" && primarySpectralClassNum >= 0 && primarySpectralClassNum <= 9) // m0-m9
			{
				primaryLifeInGY = 50;
				if (roll1d10_primary_age_roll == 1)
				{
					primaryAgeInGY = 1.0;
					primaryLuminosityModFromAge = 1.1;
				}
				else if (roll1d10_primary_age_roll == 2)
				{
					primaryAgeInGY = 2.0;
				}
				else if (roll1d10_primary_age_roll == 3)
				{
					primaryAgeInGY = 3.0;
				}
				else if (roll1d10_primary_age_roll == 4)
				{
					primaryAgeInGY = 4.0;
				}
				else if (roll1d10_primary_age_roll == 5)
				{
					primaryAgeInGY = 5.0;
				}
				else if (roll1d10_primary_age_roll == 6)
				{
					primaryAgeInGY = 6.0;
				}
				else if (roll1d10_primary_age_roll == 7)
				{
					primaryAgeInGY = 7.0;
				}
				else if (roll1d10_primary_age_roll == 8)
				{
					primaryAgeInGY = 8.0;
				}
				else if (roll1d10_primary_age_roll == 9)
				{
					primaryAgeInGY = 9.0;
				}
				else if (roll1d10_primary_age_roll == 10)
				{
					primaryAgeInGY = 10.0; // ten billion years ago; the entire universe today is 13.7
				}
			}
			primarySolarLuminosity *= primaryLuminosityModFromAge;

			

			// TODO - just freestyling this bit, representing the dwarf and giant rows of this table
			//if (binaryBasicType == "White Dwarf" || binaryBasicType == "Brown Dwarf")
			if (primaryAgeInGY == 0) 
			{
				primaryAgeInGY = roll1d10_primary_age_roll + 3;
				primaryLifeInGY = 12 + (roll1d10_primary_age_roll * 4);
			}

			// 1.2.2  lifespans
			if (primaryLifeInGY==0) { primaryLifeInGY = (10 / (primarySolarMasses / primarySolarLuminosity)); }
			



			// 1.2.3 abundance
			int roll2d10_abundance = seed.Next(1, 11) + seed.Next(1, 11);
			string AbundanceDesc = string.Empty;
			int AbundanceInt = 0;
			if (roll2d10_abundance <= 8) { AbundanceInt = 5; AbundanceDesc = "Exceptional"; }
			else if (roll2d10_abundance <= 11) { AbundanceInt = 4; AbundanceDesc = "High"; }
			else if (roll2d10_abundance <= 17) { AbundanceInt = 3; AbundanceDesc = "Normal"; }
			else if (roll2d10_abundance <= 20) { AbundanceInt = 2; AbundanceDesc = "Poor"; }
			else { AbundanceInt = 1; AbundanceDesc = "Depleted"; }



			// PART I SYSTEM DATA 10
			// ONE / 3 MULTIPLE STARS


			//after we've finished implementing every step of the pdf
			Star newPrimaryStar = new Star(newStarSystem);
			newStarSystem.PrimaryStar = newPrimaryStar;
			newStarSystem.Stars.Add(newPrimaryStar);

			newStarSystem.AbundanceDesc = AbundanceDesc;
			newStarSystem.AbundanceInt = AbundanceInt;
			newPrimaryStar.BasicSpectralType = primaryBasicType;
			newPrimaryStar.SizeCode = primarySizeCode;
			newPrimaryStar.SizeCodeNum = primarySizeCodeNum;
			newPrimaryStar.SolarLuminosity = primarySolarLuminosity;
			newPrimaryStar.TempKelvin = primaryTempKelvin;
			newPrimaryStar.SolarMasses = primarySolarMasses;
			newPrimaryStar.SolarRadius = primarySolarRadius;
			newPrimaryStar.SpectralClassNum = primarySpectralClassNum;

			// the guide says they are all the same age in the system?
			// so we set binary/ies to this also
			newPrimaryStar.AgeInGY = primaryAgeInGY;
			// similarly the expected lifespan of the primary is also the expected lifespan of the whole star system
			newPrimaryStar.LifeInGY = primaryLifeInGY;


			newStarSystem.hasBinary = hasBinary;
			newPrimaryStar.IsParentToBinaries = hasBinary;

			if (hasBinary)
			{
				Star newBinaryStar = new Star(newStarSystem);
				newStarSystem.binaryStarElement = newBinaryStar;
				newStarSystem.Stars.Add(newBinaryStar);
				newPrimaryStar.ChildStars.Add(newBinaryStar);
				newBinaryStar.ParentStar = newPrimaryStar;
				newBinaryStar.IsChildStar = true;

				newBinaryStar.BasicSpectralType = binaryBasicType;
				newBinaryStar.SizeCode = binarySizeCode;
				newBinaryStar.SizeCodeNum = binarySizeCodeNum;
				newBinaryStar.SolarLuminosity = binarySolarLuminosity;
				newBinaryStar.TempKelvin = binaryTempKelvin;
				newBinaryStar.SolarMasses = binarySolarMasses;
				newBinaryStar.SolarRadius = binarySolarRadius;
				newBinaryStar.SpectralClassNum = binarySpectralClassNum;
				newBinaryStar.AgeInGY = primaryAgeInGY;
				newBinaryStar.LifeInGY = primaryLifeInGY;

			}
			newStarSystem.hasTrinary = hasTrinary;
			if (hasTrinary)
			{
				Star newTrinaryStar = new Star(newStarSystem);
				newStarSystem.trinaryStarElement = newTrinaryStar;
				newStarSystem.Stars.Add(newTrinaryStar);
				newPrimaryStar.ChildStars.Add(newTrinaryStar);
				newTrinaryStar.ParentStar = newPrimaryStar;
				newTrinaryStar.IsChildStar = true;

				newTrinaryStar.BasicSpectralType = trinaryBasicType;
				newTrinaryStar.SizeCode = trinarySizeCode;
				newTrinaryStar.SizeCodeNum = trinarySizeCodeNum;
				newTrinaryStar.SolarLuminosity = trinarySolarLuminosity;
				newTrinaryStar.TempKelvin = trinaryTempKelvin;
				newTrinaryStar.SolarMasses = trinarySolarMasses;
				newTrinaryStar.SolarRadius = trinarySolarRadius;
				newTrinaryStar.SpectralClassNum = trinarySpectralClassNum;
				newTrinaryStar.AgeInGY = primaryAgeInGY;
				newTrinaryStar.LifeInGY = primaryLifeInGY;

			}
			newStarSystem.hasQuaternary = hasQuaternary;
			if (hasQuaternary)
			{
				Star newQuaternaryStar = new Star(newStarSystem);
				newStarSystem.quaternaryStarElement = newQuaternaryStar;
				newStarSystem.Stars.Add(newQuaternaryStar);
				newPrimaryStar.ChildStars.Add(newQuaternaryStar);
				newQuaternaryStar.ParentStar = newPrimaryStar;
				newQuaternaryStar.IsChildStar = true;

				newQuaternaryStar.BasicSpectralType = quaternaryBasicType;
				newQuaternaryStar.SizeCode = quaternarySizeCode;
				newQuaternaryStar.SizeCodeNum = quaternarySizeCodeNum;
				newQuaternaryStar.SolarLuminosity = quaternarySolarLuminosity;
				newQuaternaryStar.TempKelvin = quaternaryTempKelvin;
				newQuaternaryStar.SolarMasses = quaternarySolarMasses;
				newQuaternaryStar.SolarRadius = quaternarySolarRadius;
				newQuaternaryStar.SpectralClassNum = quaternarySpectralClassNum;
				newQuaternaryStar.AgeInGY = primaryAgeInGY;
				newQuaternaryStar.LifeInGY = primaryLifeInGY;

			}


			// rearrange the stars so the most massive is primary and least massive is furthest out
			List<Star> sortedStars = newStarSystem.Stars.OrderByDescending(star => star.SolarMasses).ToList();
			newStarSystem.PrimaryStar = sortedStars[0];
			newStarSystem.PrimaryStar.ChildStars = new List<Star>();
			newStarSystem.PrimaryStar.IsChildStar = false;
			if (newStarSystem.hasBinary) { 
				newStarSystem.binaryStarElement = sortedStars[1];
				newStarSystem.binaryStarElement.ParentStar = newStarSystem.PrimaryStar;
				newStarSystem.binaryStarElement.IsChildStar = true;
				newStarSystem.PrimaryStar.IsParentToBinaries = true;
				newStarSystem.PrimaryStar.ChildStars.Add(newStarSystem.binaryStarElement);
			}
			if (newStarSystem.hasTrinary)
			{
				newStarSystem.trinaryStarElement = sortedStars[2];
				newStarSystem.trinaryStarElement.ParentStar = newStarSystem.PrimaryStar;
				newStarSystem.trinaryStarElement.IsChildStar = true;
				newStarSystem.PrimaryStar.ChildStars.Add(newStarSystem.trinaryStarElement);
			}
			if (newStarSystem.hasQuaternary)
			{
				newStarSystem.quaternaryStarElement = sortedStars[3];
				newStarSystem.quaternaryStarElement.ParentStar = newStarSystem.PrimaryStar;
				newStarSystem.quaternaryStarElement.IsChildStar = true;
				newStarSystem.PrimaryStar.ChildStars.Add(newStarSystem.quaternaryStarElement);

			}




			// galaxy geometry
			int numArms = galaxy.GameSettings.GalaxyArms;
			float armSeparationDistance = (float)(2 * Math.PI / numArms);
			float armOffsetMax = 0.5f;
			
			float angle = (float)(seed.NextDouble() * 2 * Math.PI);
			float dist = (float)(seed.NextDouble() * galaxyRadius);
			float armIndex = angle / armSeparationDistance;
			float armAngle = (float)Math.Floor(armIndex) * armSeparationDistance;
			float armOffset = (armIndex - (float)Math.Floor(armIndex) - 0.5f) * armSeparationDistance * armOffsetMax;
			float spiralFactor = (float)Math.Pow(dist / galaxyRadius, 0.5);
			float rotatedAngle = armAngle + armOffset + spiralFactor * (float)Math.PI * 0.5f;
			float zStdDev = galaxyRadius * 0.05f * (1.0f - dist / galaxyRadius);
			
			newStarSystem.GalacticPosition = new Vector3
			{
				X = dist * (float)Math.Cos(rotatedAngle),
				Y = dist * (float)Math.Sin(rotatedAngle),
				Z = (float)Core.Utils.GameFunctions.SampleGaussian(seed, 0, zStdDev)
			};







			int numJumpSites = 6; //per sys
								  // now generate jump sites
			for (int i = 0; i < numJumpSites; i++)
			{
				JumpSite newJumpSite = JumpSiteGenerator.GenerateJumpSite(newStarSystem, seed);
			}






			//now generate planets

			for (int i = 0; i <= 3; i++)
			{
				Planet p = PlanetGenerator.GeneratePlanet(newPrimaryStar, seed);
				p.Radius = 100 * i;
			}


			return newStarSystem;
		}


		// find luminsity mass etc numbers in the big table 1.1.3
		private static (float SolLuminosity, float SolMass, float TempKelvin, float SolRadius)? GetStarDetails(string basicType, int spectralClassNum, string sizeCode)
		{
			if (StarDetailsMap.TryGetValue((basicType, spectralClassNum, sizeCode), out var details))
			{
				return details;
			}
			return null; 
		}


		private static readonly Dictionary<(string BasicType, int SpectralClassNum, string SizeCode), (float SolLuminosity, float SolMass, float TempKelvin, float SolRadius)> StarDetailsMap = new() {
				//Table 1.1.3 Basic Luminosity & Mass
				//transcribed in aistudio.goog
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
