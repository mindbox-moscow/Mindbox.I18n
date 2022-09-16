// Copyright 2022 Mindbox Ltd
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Mindbox.I18n.Abstractions;

public abstract class LocalizableString
{
	private static ILogger? Logger { get; set; }

	public static void InitializeLogger(ILogger logger)
	{
		Logger = logger;
	}

	public static LocalizableString ForKey([LocalizationKey] string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException(nameof(key));
		}
		return new LocaleDependentString(key);
	}

	public static LocalizableString LocaleIndependent(string localeIndependentString)
	{
		return new LocaleIndependentString(localeIndependentString);
	}

#pragma warning disable CA2225
	public static implicit operator LocalizableString(string key)
#pragma warning restore CA2225
	{
		return new LocaleDependentString(key);
	}

	public abstract string Key { get; }

	public override string ToString()
	{
		Logger?.LogInvalidOperation($"ToString() called on LocalizableString with key {Key}");
		return Key;
	}

	public abstract string Render(ILocalizationProvider localizationProvider, ILocale locale);

	public LocalizableString WithContext<TContext>(TContext context) where TContext : class
	{
		if (_context != null)
			throw new InvalidOperationException($"Context has already been set");

		_context = context ?? throw new ArgumentNullException(nameof(context));

		return this;
	}

	public TContext? GetContext<TContext>() where TContext : class
	{
		if (_context == null)
			return null;
		if (_context is not TContext context)
			throw new InvalidOperationException(
				$"Context is not empty, but can't cast it's value of type {_context.GetType()} to {typeof(TContext)}");

		return context;
	}

	private object? _context;
}