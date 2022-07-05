using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Mindbox.I18n.Analyzers.Test;

[TestClass]
public class OnlyStringLiteralsCanBeUsedAsKeysTests : MindboxI18nAnalyzerTests
{
	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_CorrectKey_NoDiagnosticsAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				LocalizableString s = ""Namespace:Key_Key"";
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		VerifyCSharpDiagnostic(test);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_CorrectKeyUsingConditionalExpression_NoDiagnosticsAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				LocalizableString s = true ? ""Namespace:Key_Key1"" : ""Namespace:Key_Key2"";
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		VerifyCSharpDiagnostic(test);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_StringInterpolation_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				LocalizableString s = $""{DateTime.Now}"";
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 10, 27)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_StringFormat_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				LocalizableString s = string.Format(""{0}"", ""text"");
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 10, 27)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_ExplicitConversion_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;
    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				string key = ""text"";
				var str = (LocalizableString)key;
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 10, 34)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_LocalStringVariable_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;
    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				string key = ""text"";
				LocalizableString s = key;
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 10, 27)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_MethodArgumentIsLocalizableString_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod2(LocalizableString s) 
			{
				
			}

			void TestMethod() 
			{
				string key = ""text"";
				TestMethod2(key);
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 16, 17)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_StringMemberAccess_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				LocalizableString s = TestingClass.Key;
			}

			private const string Key = ""text"";
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 10, 27)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_StringConditionalMemberAccess_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				(string, string)? tuple = (""a"", ""b"");
				LocalizableString s = tuple?.Item1;
			}

			private object object;
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 11, 27)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_StringConcatenation_ErrorAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{
				LocalizableString s = ""pupa"" + ""lupa"";
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var expected = new DiagnosticResult
		{
			Id = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.Id,
			Message = Diagnostics.OnlyStringLiteralsCanBeUsedAsKeys.MessageFormat.ToString(),
			Severity = DiagnosticSeverity.Error,
			Locations =
				new[] {
					new DiagnosticResultLocation("Test0.cs", 10, 27)
				}
		};
		VerifyCSharpDiagnostic(test, expected);
	}

	[TestMethod]
	public void OnlyStringLiteralsCanBeUsed_LocalizableStringAssignedToLocalizableString_NoDiagnosticsAsync()
	{
#pragma warning disable Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		var test = @"
	using Mindbox.I18n;

    namespace ConsoleApplication1
    {
		class TestingClass 
		{
			void TestMethod() 
			{				
				LocalizableString s = ""Namespace:Key_Key"";
				LocalizableString s2 = s;
			}
		}
    }";
#pragma warning restore Mindbox1002 // Отступы должны формироваться только с помощью табуляции
		VerifyCSharpDiagnostic(test);
	}

	protected override MindboxI18nAnalyzer CreateAnalyzer()
	{
		return new MindboxI18nAnalyzer();
	}
}