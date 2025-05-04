using System.Data;

namespace Neb25.Core.Galaxy.Generators
{
	public class StarGenerator
	{



		public static Star GenerateStar(StarSystem parentStarSystem, Random seed)
		{
			Star output = new(parentStarSystem);
			return output;
			// we are following along with the Tyge Sjöstrand world gen pdf
			// version 2000-08-06
			/*
			i dont think this page is used at all 
			Star newStar = new Star(parentStarSystem);



			int roll_1d100_for_spectral = seed.Next(1, 101);
			int roll_1d10_for_binary = seed.Next(1, 11);
			int roll_1d10_for_specification = seed.Next(1, 11);

			// the result of this particular dice roll is compared to down the line, when generating binary/trinaries.
			newStar.SpecDiceRoll = roll_1d10_for_specification;

			if (roll_1d10_for_binary >= 7) { newStar.IsParentToBinaries = true; }
			int roll_1d10_for_spectral_class_num = seed.Next(1, 11);


			string spectralType = String.Empty;
			string sizeCode = String.Empty;
			int? spectralSizeNum = null;


			if (roll_1d100_for_spectral <= 1)
			{ // 1%   (Roll 1) Spectral Class A V or A IV
				spectralType = "A";
				sizeCode = "V";
				if (roll_1d10_for_specification >= 7)
				{
					sizeCode = "IV";
				}
			}

			else if (roll_1d100_for_spectral <= 4)
			{     // 3%   (Roll 2-4) Spectral Class F V or F IV
				spectralType = "F";
				sizeCode = "V";
				if (roll_1d10_for_specification >= 9)
				{
					sizeCode = "IV";
				}

			}
			else if (roll_1d100_for_spectral <= 12)
			{     // 8%   (Roll 5-12) Spectral Class G V or G IV
				spectralType = "G";
				sizeCode = "V";
				if (roll_1d10_for_specification >= 10)
				{
					sizeCode = "IV";
				}
			}
			else if (roll_1d100_for_spectral <= 26)
			{ // 14%  (Roll 13-26) for Spectral Class K V
				spectralType = "K"; 
				sizeCode = "V";
			}   
			else if (roll_1d100_for_spectral <= 36)
			{    // 10%  (Roll 27-36) for White Dwarf VII
				spectralType = "White Dwarf"; 
				sizeCode = "VII";
			}
			else if (roll_1d100_for_spectral <= 85) {
				// 49%  (Roll 37-85) for Spectral Class M V
				spectralType = "M";
				sizeCode = "V";	
			}   
			else if (roll_1d100_for_spectral <= 98) spectralType = "Brown Dwarf"; // 13%  (Roll 86-98)
			
			else if (roll_1d100_for_spectral <= 99)
			{ // 1%  (Roll 99) for the various Giant type stars
				spectralType = "Giant";
				sizeCode = "III";
				if (roll_1d10_for_specification == 1)
				{
					spectralType = "F";
				}
				else if (roll_1d10_for_specification <= 2)
				{
					spectralType = "G";
				}
				else if (roll_1d10_for_specification <= 7)
				{
					spectralType = "K";
				}
				else if (roll_1d10_for_specification >= 8)
				{
					spectralType = "K";
					sizeCode = "IV";
				}
			}
			else if (roll_1d100_for_spectral == 100)
			{
				// 1% (Roll 100) for a black hole
				spectralType = "Black Hole";
			}
			
			newStar.BasicSpectralType = spectralType;
			newStar.SizeCode = sizeCode;


			if (spectralType != "Brown Dwarf" && spectralType != "White Dwarf") {
				if (roll_1d10_for_spectral_class_num == 10) roll_1d10_for_spectral_class_num = 0;
				spectralSizeNum = roll_1d10_for_spectral_class_num;
			}

			newStar.SpectralSizeNum = spectralSizeNum;
			return newStar;
			*/

		}
	}
}
