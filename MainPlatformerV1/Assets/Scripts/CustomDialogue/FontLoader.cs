using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainGame {
    public static class FontLoader {
        //This is the order that the characters should be in the characterSheet
        // private static char[] chars = "abcdefghijklmnopqrstuvwxyzæABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~><!?'\"#%&/\\()[]{}@£$*^+-.,:;_=".ToCharArray();
        // private static char[] chars = " !\"#$%&'()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[\\]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~".ToCharArray();
        private static readonly char[] chars =
            "!\"#$%&'()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[\\]^_ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~"
                .ToCharArray();

        private static List<Dictionary<char, CharData>> loadedFonts = new List<Dictionary<char, CharData>>();
        private static readonly List<Texture2D>
            loadedFontResources =
                new List<Texture2D>(); //This is to keep track of all the loaded fonts and only load them once

        /// <summary>
        ///     Property with dictionary of font characters and char data
        /// </summary>
        public static List<Dictionary<char, CharData>> LoadedFonts => loadedFonts;

        /// <summary>
        ///     Loads font resources from character sheet array into dictionary
        /// </summary>
        public static List<Dictionary<char, CharData>> LoadFontResources(params Texture2D[] characterSheet){
            return characterSheet != null && characterSheet.Any() ?
                characterSheet.Select(x => LoadFontResource(x)).ToList() :
                new List<Dictionary<char, CharData>>();
        }

        /// <summary>
        ///     Reload font resources from character sheet array into dictionary
        /// </summary>
        public static List<Dictionary<char, CharData>> ReloadFontResources(params Texture2D[] characterSheet){
            return characterSheet != null && characterSheet.Any() ?
                characterSheet.Select(x => ReloadFontResource(x)).ToList() :
                new List<Dictionary<char, CharData>>();
        }

        /// <summary>
        ///     Loads font resources from character sheet into dictionary
        /// </summary>
        public static Dictionary<char, CharData> LoadFontResource(Texture2D characterSheet){
            return LoadFontResource(characterSheet, true);
        }

        /// <summary>
        ///     Loads font resource from character sheet and adds it to existing loaded font if applicable
        /// </summary>
        private static Dictionary<char, CharData> LoadFontResource(Texture2D characterSheet, bool addToLoaded){
            //If we already have this loaded then we just return the loaded one
            if (IsFontLoaded(characterSheet)) return LoadedFonts[loadedFontResources.IndexOf(characterSheet)];

            var subsprites = Resources.LoadAll<Sprite>(characterSheet.name);
            int spriteSize = (int)subsprites.Max(x => x.rect.width);
            // int spriteSize = (int)subsprites[0].rect.width; //characterSheet.width / subsprites.Length; //OLD

            // Debug.Log(subsprites.Length);
            // Debug.Log(chars.Length);

            var loadedFontDictionary = GenerateCharFontDictionary(characterSheet, spriteSize, subsprites);


            if (!addToLoaded) return loadedFontDictionary;

            LoadedFonts.Add(loadedFontDictionary);
            loadedFontResources.Add(characterSheet);

            return loadedFontDictionary;
        }

        /// <summary>
        ///     Reloads font resource from character sheet into dictionary
        /// </summary>
        public static Dictionary<char, CharData> ReloadFontResource(Texture2D characterSheet){
            if (IsFontLoaded(characterSheet)) {
                var loadedFontResource = LoadFontResource(characterSheet, false);
                loadedFonts[loadedFontResources.IndexOf(characterSheet)] = loadedFontResource;
                return loadedFontResource;
            }
            else {
                Debug.Log("Font in Texture2D: " + characterSheet.name + " hasn't previously been loaded, Loading normally. Please use LoadFontResource/LoadFontResources if this behaviour is not desired.");
                return LoadFontResource(characterSheet);
            }

            // Debug.Log("Font in Texture2D: " + characterSheet.name +
            //           " hasn't previously been loaded, Loading normally. Please use LoadFontResource/LoadFontResources if this behaviour is not desired.");
            // return LoadFontResource(characterSheet);
        }

        /// <summary>
        ///     Checks if loaded font resources contains the character sheet
        /// </summary>
        public static bool IsFontLoaded(Texture2D characterSheet){
            return loadedFontResources.Contains(characterSheet);
        }

        /// <summary>
        ///     Generates font dictionary by performing a vertical scan on each font sprite and stores character width
        /// </summary>
        private static Dictionary<char, CharData> GenerateCharFontDictionary(Texture2D characterSheet, int spriteSize, Sprite[] characterSprites){
            int height = characterSheet.height; // We might need this if we ever use a text image that is on more than one line
            int width = characterSheet.width;

            var charIndex = 0;

            var charData = new Dictionary<char, CharData>();

            // Perform vertical scan on each sprite to find the widths

            //Y Texture Coordinate
            for (var texCoordY = height - spriteSize;
                texCoordY >= 0 && charIndex < chars.Length;
                texCoordY -= spriteSize) {
                var maxY = texCoordY + spriteSize;

                //X Texture Coordinate
                for (var texCoordX = 0; texCoordX < width && charIndex < chars.Length; texCoordX += spriteSize) {
                    var maxX = texCoordX + (spriteSize - 1);
                    var edgeFound = false;

                    //right edge
                    var rightEdge = 0;
                    for (var currentX = maxX; currentX >= texCoordX; currentX--) {
                        for (var currentY = texCoordY; currentY < maxY; currentY++) {
                            edgeFound = characterSheet.GetPixel(currentX, currentY).a != 0;
                            if (edgeFound) break;
                        }

                        if (edgeFound) break;
                        rightEdge++;
                    }

                    edgeFound = false;


                    //left edge
                    var leftEdge = 0;
                    for (var currentX = texCoordX; currentX <= maxX; currentX++) {
                        //X
                        for (var currentY = texCoordY; currentY < maxY; currentY++) {
                            edgeFound = characterSheet.GetPixel(currentX, currentY).a != 0;
                            if (edgeFound) break;
                        }

                        if (edgeFound) break;
                        leftEdge++;
                    }

                    //Store current sprite width
                    // int currentSpriteWidth = Mathf.Max(spriteSize - (leftEdge + rightEdge), 1);
                    var currentSpriteWidth = spriteSize - (leftEdge + rightEdge);

                    if (currentSpriteWidth < 0) {
                        // Debug.Log($"{chars[charIndex]} width {currentSpriteWidth} {spriteSize} {leftEdge} {rightEdge}");
                        // vape fix, just manually set width for "!" and "B" because its setting the width to < 0
                        // CharData temp = charData['A'];
                        charData.Add(chars[charIndex], new CharData(14, characterSprites[charIndex], 3, 3));
                    }
                    else {
                        //Determine center offsets
                        var halfWidth = spriteSize / 2;
                        var leftOffset = halfWidth - leftEdge;
                        var rightOffset = halfWidth - rightEdge;
                        Debug.Log(spriteSize + leftOffset + rightOffset);

                        charData.Add(chars[charIndex],
                            new CharData(currentSpriteWidth, characterSprites[charIndex], leftOffset, rightOffset));
                    }

                    charIndex++;
                }
            }

            return charData;
        }
    }

    /// <summary>
    ///     Struct holding char data of width, left offset, right offset, and sprite data
    /// </summary>
    public struct CharData {
        private int Width;
        public readonly int LeftOffset;
        public readonly int RightOffset;

        public readonly Sprite Sprite;

        public CharData(int width, Sprite sprite, int leftOffset, int rightOffset){
            Width = width;
            Sprite = sprite;
            LeftOffset = leftOffset;
            RightOffset = rightOffset;
        }
    }
}
