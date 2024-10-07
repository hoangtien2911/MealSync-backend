using MealSync.Application.Common.Abstractions.Messaging;
using MealSync.Application.Shared;
using Microsoft.AspNetCore.Http;

namespace MealSync.Application.UseCases.Storages.Commands.UploadFile;

public class UploadFileCommand: ICommand<Result>
{
    public IFormFile File { get; set; }
}