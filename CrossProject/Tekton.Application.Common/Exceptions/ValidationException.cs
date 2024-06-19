using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Tekton.Application.Common.Exceptions
{
	/// <summary>
	/// ValidationException
	/// </summary>
	[ExcludeFromCodeCoverage]
    [Serializable]
    public class ValidationException : Exception
    {
		/// <summary>
		/// Errors
		/// </summary>
		public List<string> Errors { get; }
		/// <summary>
		/// ValidationException
		/// </summary>
		public ValidationException()
        {
            Errors = new List<string>();
        } 
		/// <summary>
		/// ValidationException
		/// </summary>
		/// <param name="failures"></param>
		public ValidationException(IEnumerable<ValidationFailure> failures)
		: this()
		{
			foreach (var failure in failures)
			{
				Errors.Add(failure.ErrorMessage);
			}
		}
		/// <summary>
		/// ValidationException
		/// </summary>
		/// <param name="message"></param>
		public ValidationException(string message)
			: base(message)
		{
			Errors = new List<string>();
		}
		/// <summary>
		/// ValidationException
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>

		public ValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
			Errors = new List<string>();
		}

		/// <summary>
		/// ValidationException
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Errors = new List<string>();
		}
	}
}
