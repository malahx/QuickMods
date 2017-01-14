/**
 * Language Patches Framework
 * Translates the game into different Languages
 * Copyright (c) 2016 Thomas P.
 * Licensed under the terms of the MIT License
 * Modders should copy this file into their mods for easy translation
 */

using System;
using System.Linq;
using System.Reflection;

namespace LanguagePatches
{
    /// <summary>
    /// API Wrapper for the Language Patches translation database,
    /// to support mod translations
    /// </summary>
    public static class LanguageAPI
    {
        public delegate String TranslationDelegate(String text, String context);

        /// <summary>
        /// The function in the LanguagePatches Assembly that is used for translating
        /// </summary>
        private static TranslationDelegate translate { get; set; }

        /// <summary>
        /// Whether the Language Patches Assembly is there
        /// </summary>
        internal static Boolean hasLanguagePatches { get; private set; }

        static LanguageAPI()
        {
			Type[] types = AssemblyLoader.loadedAssemblies.SelectMany (a => a.assembly.GetTypes ()).ToArray ();
            Type languagePatches = types.FirstOrDefault(t => t.Name == "LanguagePatches");
            if (languagePatches != null)
            {
                hasLanguagePatches = true;
                MethodInfo translateInfo = languagePatches.GetMethod("Translate", BindingFlags.Public | BindingFlags.Static);
                translate = (TranslationDelegate) Delegate.CreateDelegate(typeof(TranslationDelegate), null, translateInfo);
            }
            else
                hasLanguagePatches = false;
        }
        
        /// <summary>
        /// Translates a string into the language loaded by the Language Patches Framework.
        /// The translation has to exist in the correct context, otherwise it will be english.
        /// Context is the Name of the Assembly that calls the function
        /// </summary>
        /// <param name="text">The text that is being translated</param>
        public static String Translate(String text)
        {
            return Translate(text, Assembly.GetCallingAssembly().GetName().Name);
        }

        /// <summary>
        /// Translates a string into the language loaded by the Language Patches Framework.
        /// The translation has to exist in the correct context, otherwise it will be english.
        /// </summary>
        /// <param name="text">The text that is being translated</param>
        /// <param name="context">The context of the translation</param>
        public static String Translate(String text, String context)
        {
            if (!hasLanguagePatches)
                return text;
            if (translate == null)
                return text;
            return translate(text, context);
        }
    }
}