﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mindbox.I18n.Analyzers
{
	// https://stackoverflow.com/questions/707107/parsing-visual-studio-solution-files
	// тут очень печальный код, но по-другому нельзя было
	public static class SolutionFileParser
	{
		private static readonly Regex ProjectLineRegex = new Regex(
			"Project\\(\"(?<ParentProjectGuid>{[A-F0-9-]+})\"\\) = \"(?<ProjectName>.*?)\", \"(?<RelativePath>.*?)\", \"(?<ProjectGuid>{[A-F0-9-]+})");

		public static IEnumerable<string> GetProjectRelativePaths(string solutionFileName)
		{
			var projectRelativePaths = new List<string>();
			var solutionFileLines = File.ReadAllLines(solutionFileName);

			foreach (var line in solutionFileLines)
			{
				var projectMatch = ProjectLineRegex.Match(line);
				if (!projectMatch.Success)
					continue;

				var projectFileRelativePath = projectMatch.Groups["RelativePath"].Value;
				if (!projectFileRelativePath.EndsWith("csproj"))
					continue;

				projectRelativePaths.Add(projectFileRelativePath);
			}

			return projectRelativePaths;
		}
	}
}