﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Consyzer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class CmdArgsParser
    {
        public static (string AnalysisDirectory, IEnumerable<string> FileExtensions) GetAnalysisParams()
        {
            return (GetAnalysisDirectory(), GetFileExtensions());
        }

        private static string GetAnalysisDirectory()
        {
            string analysisDirectory = Environment.GetCommandLineArgs().ElementAtOrDefault(1) 
                ?? throw new ArgumentException("No command line parameter was passed that contains the location of the catalog to analyze.");

            if (!Directory.Exists(analysisDirectory))
            {
                throw new DirectoryNotFoundException("The directory path for the analysis does not exist or is incorrect.");
            }

            return analysisDirectory;
        }

        private static IEnumerable<string> GetFileExtensions()
        {
            string fileExtensions = Environment.GetCommandLineArgs().ElementAtOrDefault(2) 
                ?? throw new ArgumentException("No command line parameter containing binary file extensions for analysis was passed.");

            var extensions = fileExtensions.Split(',').Select(e => e.Trim());
            if (extensions.Any(e => !Path.HasExtension(e)))
            {
                throw new ArgumentException("One or more names are not extensions.");
            }

            return extensions;
        }
    }
}