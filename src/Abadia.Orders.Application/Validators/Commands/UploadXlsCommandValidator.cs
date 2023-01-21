using Abadia.Orders.Application.Commands.Upload;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Abadia.Orders.Application.Validators.Commands;

public class UploadXlsCommandValidator : AbstractValidator<UploadXlsCommand>
{
	public UploadXlsCommandValidator()
	{
		RuleFor(u => u.UploadFile)
			.NotNull()
			.Must(AllowedExtension);
		//TODO validate file size range
	}

	private bool AllowedExtension(IFormFile formFile)
	{
		var allowedExtensions = new string[2] { ".xlsx", ".xlsm" };

		return allowedExtensions.Contains(Path.GetExtension(formFile.FileName).ToLowerInvariant());
	}
}