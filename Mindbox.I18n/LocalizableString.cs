﻿using System;
using System.Linq;

namespace Mindbox.I18n
{
	public abstract class LocalizableString
	{
		public abstract string Key { get; }

		public static LocalizableString LocaleIndependent(string localeIndependentString)
		{
			return new LocaleIndependentString(localeIndependentString);
		}

		public override string ToString()
		{
			return ToStringCore();
		}

		public abstract string Render(LocalizationProvider localizationProvider, Locale locale);

		protected abstract string ToStringCore();

		public static implicit operator LocalizableString(string key)
		{
			// Strictly speaking, this is illegal and will result in ArgumentNullException later.
			if (key == null)
				return null;

			return new LocaleDependentString(key);
		}

		public static LocalizableString ForKey([LocalizationKey]string key)
		{
			return new LocaleDependentString(key);
		}

		private object context = null;

		private Func<object> getContext = null;
		
		public LocalizableString WithContext<TContext>(TContext aContext) where TContext : class
		{
			if (context != null || getContext != null)
				throw new InvalidOperationException($"Context is already set");

			context = aContext ?? throw new ArgumentNullException(nameof(aContext));

			return this;
		}

		public LocalizableString WithContext<TContext>(Func<TContext> contextGetter) where TContext : class
		{
			if (context != null || getContext != null)
				throw new InvalidOperationException($"Context is already set");
			
			getContext = contextGetter ?? throw new ArgumentNullException(nameof(contextGetter));

			return this;
		}
		
		public TContext GetContext<TContext>() where TContext : class
		{
			var targetContext = new[] { context, getContext?.Invoke() }.SingleOrDefault(c => c != null);
			
			if (targetContext == null)
				return null;

			var result = targetContext as TContext;
			if (result == null)
				throw new InvalidOperationException(
					$"Context is not empty, but can't cast it's value of type {targetContext.GetType()} to {typeof(TContext)}");

			return targetContext as TContext;
		}
	}
}
