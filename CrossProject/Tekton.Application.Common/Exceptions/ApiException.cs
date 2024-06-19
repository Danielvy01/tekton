using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tekton.Application.Common.Exceptions
{
	/// <summary>
	/// ApiException
	/// </summary>
	[ExcludeFromCodeCoverage]
	[Serializable]
	public class ApiException : Exception
	{
		/// <summary>
		/// ApiException
		/// </summary>
		public ApiException()
		{
		} 

		/// <summary>
		/// ApiException
		/// </summary>
		/// <param name="message"></param>
		public ApiException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// ApiException
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ApiException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// ApiException
		/// </summary>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public ApiException(string message, params object[] args)
			: base(String.Format(CultureInfo.CurrentCulture, message, args))
		{
		}

		/// <summary>
		/// ApiException
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
